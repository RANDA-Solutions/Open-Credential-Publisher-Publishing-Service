#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["OpenCredentialsPublisher.PublishingService.Functions/OpenCredentialsPublisher.PublishingService.Functions.csproj", "OpenCredentialsPublisher.PublishingService.Functions/"]
COPY ["src/OpenCredentialsPublisher.PublishingService.Services/OpenCredentialsPublisher.PublishingService.Services.csproj", "src/OpenCredentialsPublisher.PublishingService.Services/"]
COPY ["src/Clrs/OpenCredentialsPublisher.Credentials.Clrs.csproj", "src/Clrs/"]
COPY ["src/Cryptography/OpenCredentialsPublisher.Credentials.Cryptography.csproj", "src/Cryptography/"]
COPY ["src/OpenCredentialsPublisher.PublishingService.Data/OpenCredentialsPublisher.PublishingService.Data.csproj", "src/OpenCredentialsPublisher.PublishingService.Data/"]
COPY ["src/OpenCredentialsPublisher.Credentials.Drawing/OpenCredentialsPublisher.Credentials.Drawing.csproj", "src/OpenCredentialsPublisher.Credentials.Drawing/"]
COPY ["src/VerifiableCredentials/OpenCredentialsPublisher.Credentials.VerifiableCredentials.csproj", "src/VerifiableCredentials/"]
RUN dotnet restore "OpenCredentialsPublisher.PublishingService.Functions/OpenCredentialsPublisher.PublishingService.Functions.csproj"
COPY . .
WORKDIR "/src/OpenCredentialsPublisher.PublishingService.Functions"
RUN dotnet build "OpenCredentialsPublisher.PublishingService.Functions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenCredentialsPublisher.PublishingService.Functions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true