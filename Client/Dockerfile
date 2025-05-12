FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
USER app
# Stage 1: Build Angular app
FROM node:20-alpine AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

# Stage 2: Serve with nginx
FROM nginx:stable-alpine AS production
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/dist/Client/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
