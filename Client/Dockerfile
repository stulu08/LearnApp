FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
USER app
WORKDIR /app
EXPOSE 80

# Install dependencies
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update && apt-get install -y curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs
RUN npm install -g @angular/cli

FROM with-node AS build
WORKDIR "/app"
COPY . .
RUN npm install
# RUN ng build
ENTRYPOINT ["ng", "serve", "--host", "0.0.0.0"]

# FROM nginx:latest as final
# COPY nginx.conf /etc/nginx/nginx.conf
# COPY --from=build /app/dist/Client/browser /usr/share/nginx/html

