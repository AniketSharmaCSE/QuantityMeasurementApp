# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and restore
COPY . .
RUN dotnet restore "QuantityMeasurement.Api/QuantityMeasurement.Api.csproj"

# Build and publish
RUN dotnet publish "QuantityMeasurement.Api/QuantityMeasurement.Api.csproj" \
    -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "QuantityMeasurement.Api.dll"]