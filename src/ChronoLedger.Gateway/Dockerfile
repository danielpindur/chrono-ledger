FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Echo the build configuration
RUN echo "Build configuration: $BUILD_CONFIGURATION"

# Copy sln and csproj files to restore dependencies
WORKDIR /build
COPY *.sln .
COPY src/ChronoLedger.Gateway/*.csproj src/ChronoLedger.Gateway/
COPY src/ChronoLedger.Schema/*.csproj src/ChronoLedger.Schema/
COPY src/ChronoLedger/*.csproj src/ChronoLedger/
COPY src/ChronoLedger.Common/*.csproj src/ChronoLedger.Common/
# TODO: SchemaSync is a seprarate thing, so shouldn't probably be a part of the Gateway docker image
COPY src/ChronoLedger.SchemaSync/*.csproj src/ChronoLedger.SchemaSync/

RUN dotnet restore

# Copy the rest of the files and build
COPY src/. src/.

RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChronoLedger.Gateway.dll"]
