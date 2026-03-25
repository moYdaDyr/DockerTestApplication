# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
# FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

FROM node:23.8.0-alpine AS build

WORKDIR /app

COPY testapplicationfrontend/package*.json ./
RUN npm ci
COPY testapplicationfrontend/ .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
