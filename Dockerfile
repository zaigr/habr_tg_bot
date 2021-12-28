# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /source

COPY *.sln .
COPY src/HabrTelegramBot.Service/*.csproj src/HabrTelegramBot.Service/
RUN dotnet restore -r linux-musl-x64

COPY . .
RUN dotnet publish -c Release -o /app -r linux-musl-x64 --self-contained false

FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine-amd64
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT [ "./HabrTelegramBot.Service" ]
