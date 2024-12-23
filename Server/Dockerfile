# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# Install dependencies
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update && apt-get install -y curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs
RUN npm install -g @angular/cli

# Build stage
FROM with-node AS build
# Copy build files
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src"
COPY ["Server/Server.csproj", "Server/"]
COPY ["Client/Client.esproj", "Client/"]
RUN dotnet restore "./Server/Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "./Server.csproj" -c $BUILD_CONFIGURATION -o "/app/build"

# EF Database init
FROM build AS migrate
WORKDIR "/src/Server"
RUN dotnet tool install --global dotnet-ef --version 9.0.0-rc.2.24474.1
ENV PATH="$PATH:/root/.dotnet/tools"

# Publish .NET files
FROM migrate AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Server.csproj" -c $BUILD_CONFIGURATION -o "/app/publish"

# Copy to final destination
FROM base AS final
WORKDIR /app
COPY ["Server/Data", "Data/"]
COPY --from=publish "/app/publish" .
ENTRYPOINT ["dotnet", "Server.dll"]
