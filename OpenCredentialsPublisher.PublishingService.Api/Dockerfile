#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["OpenCredentialsPublisher.PublishingService.Api/OpenCredentialsPublisher.PublishingService.Api.csproj", "OpenCredentialsPublisher.PublishingService.Api/"]
COPY ["src/OpenCredentialsPublisher.PublishingService.Services/OpenCredentialsPublisher.PublishingService.Services.csproj", "src/OpenCredentialsPublisher.PublishingService.Services/"]
COPY ["src/Clrs/OpenCredentialsPublisher.Credentials.Clrs.csproj", "src/Clrs/"]
COPY ["src/Cryptography/OpenCredentialsPublisher.Credentials.Cryptography.csproj", "src/Cryptography/"]
COPY ["src/OpenCredentialsPublisher.PublishingService.Data/OpenCredentialsPublisher.PublishingService.Data.csproj", "src/OpenCredentialsPublisher.PublishingService.Data/"]
COPY ["src/OpenCredentialsPublisher.Credentials.Drawing/OpenCredentialsPublisher.Credentials.Drawing.csproj", "src/OpenCredentialsPublisher.Credentials.Drawing/"]
RUN dotnet restore "OpenCredentialsPublisher.PublishingService.Api/OpenCredentialsPublisher.PublishingService.Api.csproj"
COPY . .
WORKDIR "/src/OpenCredentialsPublisher.PublishingService.Api"
RUN dotnet build "OpenCredentialsPublisher.PublishingService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenCredentialsPublisher.PublishingService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OpenCredentialsPublisher.PublishingService.Api.dll"]