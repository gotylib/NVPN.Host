using Infrastructure.AnswerObjects;
using Infrastructure.Dto;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Infrastructure.Services;

public class ServerCommandService : IServerCommandService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ServerCommandService> _logger;

    public ServerCommandService(IHttpClientFactory httpClientFactory, ILogger<ServerCommandService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    public Task<ResultModel<string, Exception>> AddPersonToServer(ServerModel serverDto, UserModel userModel, DateTime? date = null)
    {
        throw new NotImplementedException();
    }

    public Task<ResultModel<object, Exception>> AddVpnServer(ServerModel serverDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResultModel<ICollection<UserModel>, Exception>> GetUsers(ServerModel serverDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResultModel<object, Exception>> GetVpnServer(ServerModel serverDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultModel<string, Exception>> Login(ServerModel serverDto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{serverDto.ServerName}/login");

            // Set headers
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("ru"));
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.9));
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-GB", 0.8));
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US", 0.7));
            request.Headers.Connection.Add("keep-alive");
            request.Headers.Add("Origin", "http://193.233.253.104:51047");
            request.Headers.Referrer = new Uri("http://193.233.253.104:51047/1cx8lkkU8sJVyYF/");
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36 Edg/134.0.0.0");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            // Set cookies
            request.Headers.Add("Cookie", "lang=en-US; SL_G_WPT_TO=ru; SL_GWPT_Show_Hide_tmp=1; SL_wptGlobTipTmp=1");

            // Set content
            var content = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("username", $"{serverDto.Login}"),
                new KeyValuePair<string, string>("password", $"{serverDto.Password}")
            ]);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            request.Content = content;

            // Send request
            var response = await client.SendAsync(request);

            // Log response
            _logger.LogInformation("Response status code: " + response.StatusCode);

            // Read response body
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseBody);

            // Check success status
            var success = jsonResponse.Value<bool>("success");

            if (!success)
            {
                return ResultModel<string, Exception>.CreateFailedResult();
            }

            // Extract cookies
            var cookies = response.Headers.GetValues("Set-Cookie");
            var cookiesHeader = string.Join("; ", cookies);

            return ResultModel<string, Exception>.CreateSuccessfulResult(cookiesHeader);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "��������� �������������� ������.");

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

    public Task<ResultModel<object, Exception>> UpdatePersonFromServer(ServerModel serverDto, UserModel userModel, DateTime? date = null)
    {
        throw new NotImplementedException();
    }
}