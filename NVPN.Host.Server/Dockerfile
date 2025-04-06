FROM mcr.microsoft.com/dotnet/nightly/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build
WORKDIR /src
COPY ["NVPN.Host.Server/NVPN.Host.Server.csproj", "NVPN.Host.Server/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["DAL/DAL.csproj", "DAL/"]
RUN dotnet restore "NVPN.Host.Server/NVPN.Host.Server.csproj"
COPY . .
WORKDIR "/src/NVPN.Host.Server"
RUN dotnet build "NVPN.Host.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NVPN.Host.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "NVPN.Host.Server.dll"]