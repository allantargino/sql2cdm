FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION
WORKDIR /src
COPY ["src/Sql2Cdm.CLI/Sql2Cdm.CLI.csproj", "Sql2Cdm.CLI/"]
COPY ["src/Sql2Cdm.Library/Sql2Cdm.Library.csproj", "Sql2Cdm.Library/"]
RUN dotnet restore "Sql2Cdm.CLI"
COPY src/ .
WORKDIR "/src/Sql2Cdm.CLI"
RUN dotnet build -c ${BUILD_CONFIGURATION} -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION
RUN dotnet publish -c ${BUILD_CONFIGURATION} -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sql2Cdm.CLI.dll"]