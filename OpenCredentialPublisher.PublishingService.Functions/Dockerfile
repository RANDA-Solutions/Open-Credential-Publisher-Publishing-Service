#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["OpenCredentialPublisher.PublishingService.Functions/OpenCredentialPublisher.PublishingService.Functions.csproj", "OpenCredentialPublisher.PublishingService.Functions/"]
COPY ["OpenCredentialPublisher.PublishingService.Services/OpenCredentialPublisher.PublishingService.Services.csproj", "src/OpenCredentialPublisher.PublishingService.Services/"]
COPY ["Clrs/OpenCredentialPublisher.Credentials.Clrs.v1_0.csproj", "src/Clrs/"]
COPY ["Cryptography/OpenCredentialPublisher.Credentials.Cryptography.csproj", "src/Cryptography/"]
COPY ["OpenCredentialPublisher.PublishingService.Data/OpenCredentialPublisher.PublishingService.Data.csproj", "src/OpenCredentialPublisher.PublishingService.Data/"]
COPY ["OpenCredentialPublisher.Credentials.Drawing/OpenCredentialPublisher.Credentials.Drawing.csproj", "src/OpenCredentialPublisher.Credentials.Drawing/"]
COPY ["VerifiableCredentials/OpenCredentialPublisher.Credentials.VerifiableCredentials.csproj", "src/VerifiableCredentials/"]
RUN dotnet restore "OpenCredentialPublisher.PublishingService.Functions/OpenCredentialPublisher.PublishingService.Functions.csproj"
COPY . .
WORKDIR "/src/OpenCredentialPublisher.PublishingService.Functions"
RUN dotnet build "OpenCredentialPublisher.PublishingService.Functions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenCredentialPublisher.PublishingService.Functions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true