FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

# solve issue with libdl.so needed by OpenTelemetry library (due to gRPC dependency)
RUN apt-get update && apt-get install -y libc-dev

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "webapi-sample.dll"]