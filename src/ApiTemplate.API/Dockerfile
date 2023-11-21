FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ApiTemplate.API/nuget.config", "src/ApiTemplate.API/"]
COPY ["src/ApiTemplate.API/ApiTemplate.API.csproj", "src/ApiTemplate.API/"]
COPY ["src/ApiTemplate.Application/ApiTemplate.Application.csproj", "src/ApiTemplate.Application/"]
COPY ["src/ApiTemplate.Domain/ApiTemplate.Domain.csproj", "src/ApiTemplate.Domain/"]
COPY ["src/ApiTemplate.Infrastructure/ApiTemplate.Infrastructure.csproj", "src/ApiTemplate.Infrastructure/"]
RUN dotnet restore "src/ApiTemplate.API/ApiTemplate.API.csproj" --configfile "src/ApiTemplate.API/nuget.config"
COPY . .
WORKDIR "/src/src/ApiTemplate.API"
RUN dotnet build "ApiTemplate.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiTemplate.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiTemplate.API.dll"]
