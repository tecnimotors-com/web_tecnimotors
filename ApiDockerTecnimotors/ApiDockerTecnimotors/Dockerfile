FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiDockerTecnimotors.csproj", "."]
RUN dotnet restore "./ApiDockerTecnimotors.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ApiDockerTecnimotors.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ApiDockerTecnimotors.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiDockerTecnimotors.dll"]