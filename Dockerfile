# ----------------------------
# Stage 1: Build the application
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0.102 AS build

WORKDIR /src

# Copy everything and restore
COPY . .
RUN dotnet restore

# Publish the app
RUN dotnet publish -c Release -o /app/publish

# ----------------------------
# Stage 2: Run the application
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Run the app
ENTRYPOINT ["dotnet", "SportsStore.dll"]
