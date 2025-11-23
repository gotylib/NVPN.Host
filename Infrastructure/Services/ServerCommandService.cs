using Infrastructure.AnswerObjects;
using Infrastructure.Dto;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Infrastructure.Services;

public class ServerCommandService(IHttpClientFactory httpClientFactory, ILogger<ServerCommandService> logger)
    : IServerCommandService
{
    public async Task<ResultModel<string, Exception>> AddPersonToServer(ServerModel serverDto, UserModel userModel, DateTime? date = null)
    {
        try
        {
            // First, login to get authentication cookies
            var loginResult = await Login(serverDto);
            if (!loginResult.IsSuccessful || string.IsNullOrEmpty(loginResult.Value))
            {
                logger.LogError("Failed to login to server {ServerName}", serverDto.ServerName);
                return ResultModel<string, Exception>.CreateFailedResult();
            }

            var cookies = loginResult.Value;
            var httpClient = httpClientFactory.CreateClient();

            // Get list of inbounds to find the first inbound ID
            var baseUrl = serverDto.ServerName.TrimEnd('/');
            var listUrl = $"{baseUrl}/panel/api/inbounds/list";
            
            var listRequest = new HttpRequestMessage(HttpMethod.Get, listUrl);
            listRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            listRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            listRequest.Headers.Add("Cookie", cookies);

            var listResponse = await httpClient.SendAsync(listRequest);
            if (!listResponse.IsSuccessStatusCode)
            {
                logger.LogError("Failed to get inbounds list, status code: {StatusCode}", listResponse.StatusCode);
                return ResultModel<string, Exception>.CreateFailedResult();
            }

            var listBody = await listResponse.Content.ReadAsStringAsync();
            var inbounds = JArray.Parse(listBody);
            
            if (inbounds.Count == 0)
            {
                logger.LogError("No inbounds found on server {ServerName}", serverDto.ServerName);
                return ResultModel<string, Exception>.CreateFailedResult();
            }

            // Use first inbound ID (or could be configurable)
            var firstInbound = inbounds[0] as JObject;
            var inboundId = firstInbound?["id"]?.ToString() ?? "1";

            // Generate client data
            var clientId = Guid.NewGuid().ToString();
            var subId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
            
            // Calculate expiryTime from date parameter (Unix timestamp in milliseconds)
            var expiryTime = date.HasValue 
                ? (long)(date.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds 
                : 0L;

            // Create client settings object
            var clientSettings = new
            {
                id = clientId,
                flow = "",
                email = userModel.Email,
                limitIp = 0,
                totalGB = 0,
                expiryTime,
                enable = true,
                tgId = "",
                subId,
                comment = userModel.Username,
                reset = 0
            };

            var settings = new
            {
                clients = new[] { clientSettings }
            };

            var settingsJson = JsonConvert.SerializeObject(settings);

            // Construct API URL
            var apiUrl = $"{baseUrl}/panel/api/inbounds/addClient";

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            // Set minimal required headers
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Cookie", cookies);

            // Set content
            var formData = new List<KeyValuePair<string, string>>
            {
                new("id", inboundId),
                new("settings", settingsJson)
            };

            var content = new FormUrlEncodedContent(formData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            request.Content = content;

            // Send request
            var response = await httpClient.SendAsync(request);

            // Log response
            logger.LogInformation("AddPersonToServer response status code: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                logger.LogError("AddPersonToServer request failed with status code: {StatusCode}, response: {Response}", 
                    response.StatusCode, errorBody);
                return ResultModel<string, Exception>.CreateFailedResult();
            }

            // Read response
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseBody);

            // Check success status
            var success = jsonResponse.Value<bool>("success");
            if (!success)
            {
                var message = jsonResponse["msg"]?.ToString() ?? "Unknown error";
                logger.LogError("AddPersonToServer failed: {Message}", message);
                return ResultModel<string, Exception>.CreateFailedResult();
            }

            return ResultModel<string, Exception>.CreateSuccessfulResult("Client added successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при добавлении пользователя на сервер {ServerName}", serverDto.ServerName);
            return ResultModel<string, Exception>.CreateFailedResult(new Exception($"Ошибка при добавлении пользователя: {ex.Message}", ex));
        }
    }

    public Task<ResultModel<object, Exception>> AddVpnServer(ServerModel serverDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultModel<ICollection<UserModel>, Exception>> GetUsers(ServerModel serverDto)
    {
        try
        {
            // First, login to get authentication cookies
            var loginResult = await Login(serverDto);
            if (!loginResult.IsSuccessful || string.IsNullOrEmpty(loginResult.Value))
            {
                logger.LogError("Failed to login to server {ServerName}", serverDto.ServerName);
                return ResultModel<ICollection<UserModel>, Exception>.CreateFailedResult();
            }

            var cookies = loginResult.Value;
            var httpClient = httpClientFactory.CreateClient();

            // Construct API URL - append /panel/api/inbounds/list to ServerName
            var baseUrl = serverDto.ServerName.TrimEnd('/');
            var apiUrl = $"{baseUrl}/panel/api/inbounds/list";

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

            // Set minimal required headers
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            // Set cookies from login
            request.Headers.Add("Cookie", cookies);

            // Send request
            var response = await httpClient.SendAsync(request);

            // Log response
            logger.LogInformation("GetUsers response status code: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("GetUsers request failed with status code: {StatusCode}", response.StatusCode);
                return ResultModel<ICollection<UserModel>, Exception>.CreateFailedResult();
            }

            // Read and parse response
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JArray.Parse(responseBody);

            // Extract users from inbounds
            var users = jsonResponse.OfType<JObject>()
                .Select(inboundObj => inboundObj["clients"])
                .OfType<JArray>()
                .SelectMany(clients => clients)
                .OfType<JObject>()
                .Select(clientObj => new UserModel
                {
                    Username = clientObj["email"]?.ToString() ?? clientObj["id"]?.ToString() ?? string.Empty,
                    Password = string.Empty, // Password is not returned by API
                    Email = clientObj["email"]?.ToString() ?? string.Empty,
                    VpnLinks = null, // Will be populated separately if needed
                    ServerName = serverDto.ServerName
                })
                .ToList();

            return ResultModel<ICollection<UserModel>, Exception>.CreateSuccessfulResult(users);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении пользователей с сервера {ServerName}", serverDto.ServerName);
            return ResultModel<ICollection<UserModel>, Exception>.CreateFailedResult(new Exception($"Ошибка при получении пользователей: {ex.Message}", ex));
        }
    }

    public Task<ResultModel<object, Exception>> GetVpnServer(ServerModel serverDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultModel<string, Exception>> Login(ServerModel serverDto)
    {
        try
        {
            var client = httpClientFactory.CreateClient();

            var baseUrl = serverDto.ServerName.TrimEnd('/');
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/login");

            // Set minimal required headers
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            // Set content
            var content = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("username", serverDto.Login),
                new KeyValuePair<string, string>("password", serverDto.Password)
            ]);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            request.Content = content;

            // Send request
            var response = await client.SendAsync(request);

            // Log response
            logger.LogInformation("Login response status code: {StatusCode}", response.StatusCode);

            // Read response body
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseBody);

            // Check success status
            var success = jsonResponse.Value<bool>("success");

            if (!success)
            {
                logger.LogError("Login failed for server {ServerName}", serverDto.ServerName);
                return ResultModel<string, Exception>.CreateFailedResult();
            }

            // Extract cookies
            var cookies = response.Headers.GetValues("Set-Cookie");
            var cookiesHeader = string.Join("; ", cookies);

            return ResultModel<string, Exception>.CreateSuccessfulResult(cookiesHeader);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Error logging in to server {ServerName}", serverDto.ServerName);

            return ResultModel<string, Exception>.CreateFailedResult();
        }
        
    }

    public Task<ResultModel<object, Exception>> RemovePersonFromServer(ServerModel serverDto, UserModel userModel)
    {
        throw new NotImplementedException();
    }

    public Task<ResultModel<object, Exception>> RemoveVpnServer(ServerModel serverDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultModel<object, Exception>> UpdatePersonFromServer(ServerModel serverDto, UserModel userModel, DateTime? date = null)
    {
        try
        {
            // First, login to get authentication cookies
            var loginResult = await Login(serverDto);
            if (!loginResult.IsSuccessful || string.IsNullOrEmpty(loginResult.Value))
            {
                logger.LogError("Failed to login to server {ServerName}", serverDto.ServerName);
                return ResultModel<object, Exception>.CreateFailedResult();
            }

            var cookies = loginResult.Value;
            var httpClient = httpClientFactory.CreateClient();

            // Get list of inbounds to find the client
            var baseUrl = serverDto.ServerName.TrimEnd('/');
            var listUrl = $"{baseUrl}/panel/api/inbounds/list";
            
            var listRequest = new HttpRequestMessage(HttpMethod.Get, listUrl);
            listRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            listRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            listRequest.Headers.Add("Cookie", cookies);

            var listResponse = await httpClient.SendAsync(listRequest);
            if (!listResponse.IsSuccessStatusCode)
            {
                logger.LogError("Failed to get inbounds list, status code: {StatusCode}", listResponse.StatusCode);
                return ResultModel<object, Exception>.CreateFailedResult();
            }

            var listBody = await listResponse.Content.ReadAsStringAsync();
            var inbounds = JArray.Parse(listBody);
            
            if (inbounds.Count == 0)
            {
                logger.LogError("No inbounds found on server {ServerName}", serverDto.ServerName);
                return ResultModel<object, Exception>.CreateFailedResult();
            }

            // Find client by email across all inbounds
            JObject? foundClient = null;
            JObject? foundInbound = null;
            string? inboundId = null;

            foreach (var inbound in inbounds.OfType<JObject>())
            {
                if (inbound["clients"] is not JArray clients) continue;

                foreach (var client in clients.OfType<JObject>())
                {
                    var clientEmail = client["email"]?.ToString();
                    if (clientEmail != userModel.Email) continue;
                    foundClient = client;
                    foundInbound = inbound;
                    inboundId = inbound["id"]?.ToString() ?? "1";
                    break;
                }

                if (foundClient != null) break;
            }

            if (foundClient == null || foundInbound == null || string.IsNullOrEmpty(inboundId))
            {
                logger.LogError("Client with email {Email} not found on server {ServerName}", userModel.Email, serverDto.ServerName);
                return ResultModel<object, Exception>.CreateFailedResult();
            }

            // Get existing client data
            var clientId = foundClient["id"]?.ToString() ?? Guid.NewGuid().ToString();
            var existingSubId = foundClient["subId"]?.ToString();
            var existingCreatedAt = foundClient["created_at"]?.Value<long>() ?? 0L;
            _ = foundClient["updated_at"]?.Value<long>() ?? 0L;

            // Generate new subId if not exists, otherwise keep existing
            var subId = !string.IsNullOrEmpty(existingSubId) 
                ? existingSubId 
                : Guid.NewGuid().ToString().Replace("-", "")[..16];

            // Calculate expiryTime from date parameter (Unix timestamp in milliseconds)
            var expiryTime = date.HasValue 
                ? (long)(date.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds 
                : foundClient["expiryTime"]?.Value<long>() ?? 0L;

            // Get current timestamp for updated_at
            var currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            // Create updated client settings object
            var clientSettings = new
            {
                id = clientId,
                flow = foundClient["flow"]?.ToString() ?? "",
                email = userModel.Email,
                limitIp = foundClient["limitIp"]?.Value<int>() ?? 0,
                totalGB = foundClient["totalGB"]?.Value<long>() ?? 0L,
                expiryTime,
                enable = foundClient["enable"]?.Value<bool>() ?? true,
                tgId = foundClient["tgId"]?.ToString() ?? "",
                subId,
                comment = userModel.Username,
                reset = foundClient["reset"]?.Value<long>() ?? 0L,
                created_at = existingCreatedAt > 0 ? existingCreatedAt : currentTimestamp,
                updated_at = currentTimestamp
            };

            var settings = new
            {
                clients = new[] { clientSettings }
            };

            var settingsJson = JsonConvert.SerializeObject(settings);

            // Construct API URL with client ID
            var apiUrl = $"{baseUrl}/panel/api/inbounds/updateClient/{clientId}";

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            // Set minimal required headers
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Cookie", cookies);

            // Set content
            var formData = new List<KeyValuePair<string, string>>
            {
                new("id", inboundId),
                new("settings", settingsJson)
            };

            var content = new FormUrlEncodedContent(formData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            request.Content = content;

            // Send request
            var response = await httpClient.SendAsync(request);

            // Log response
            logger.LogInformation("UpdatePersonFromServer response status code: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                logger.LogError("UpdatePersonFromServer request failed with status code: {StatusCode}, response: {Response}", 
                    response.StatusCode, errorBody);
                return ResultModel<object, Exception>.CreateFailedResult();
            }

            // Read response
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseBody);

            // Check success status
            var success = jsonResponse.Value<bool>("success");
            if (!success)
            {
                var message = jsonResponse["msg"]?.ToString() ?? "Unknown error";
                logger.LogError("UpdatePersonFromServer failed: {Message}", message);
                return ResultModel<object, Exception>.CreateFailedResult();
            }

            return ResultModel<object, Exception>.CreateSuccessfulResult(new { message = "Client updated successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обновлении пользователя на сервере {ServerName}", serverDto.ServerName);
            return ResultModel<object, Exception>.CreateFailedResult(new Exception($"Ошибка при обновлении пользователя: {ex.Message}", ex));
        }
    }
}