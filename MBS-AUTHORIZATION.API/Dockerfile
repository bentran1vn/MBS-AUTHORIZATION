FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MBS-AUTHORIZATION.API/MBS-AUTHORIZATION.API.csproj", "MBS-AUTHORIZATION.API/"]
COPY ["MBS-AUTHORIZATION.Application/MBS-AUTHORIZATION.Application.csproj", "MBS-AUTHORIZATION.Application/"]
COPY ["MBS_AUTHORIZATION.Contract/MBS_AUTHORIZATION.Contract.csproj", "MBS_AUTHORIZATION.Contract/"]
COPY ["MBS_AUTHORIZATION.Infrastructure/MBS_AUTHORIZATION.Infrastructure.csproj", "MBS_AUTHORIZATION.Infrastructure/"]
COPY ["MBS_AUTHORIZATION.Presentation/MBS_AUTHORIZATION.Presentation.csproj", "MBS_AUTHORIZATION.Presentation/"]
RUN dotnet restore "MBS-AUTHORIZATION.API/MBS-AUTHORIZATION.API.csproj"
COPY . .
WORKDIR "/src/MBS-AUTHORIZATION.API"
RUN dotnet build "MBS-AUTHORIZATION.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MBS-AUTHORIZATION.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MBS-AUTHORIZATION.API.dll"]
