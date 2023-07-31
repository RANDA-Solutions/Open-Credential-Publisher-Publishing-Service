#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["OpenCredentialPublisher.PublishingService.Api/OpenCredentialPublisher.PublishingService.Api.csproj", "OpenCredentialPublisher.PublishingService.Api/"]
COPY ["OpenCredentialPublisher.PublishingService.Data/OpenCredentialPublisher.PublishingService.Data.csproj", "src/OpenCredentialPublisher.PublishingService.Data/"]
COPY ["OpenCredentialPublisher.PublishingService.Shared/OpenCredentialPublisher.PublishingService.Shared.csproj", "src/OpenCredentialPublisher.PublishingService.Shared/"]
COPY ["OpenCredentialPublisher.PublishingService.Services/OpenCredentialPublisher.PublishingService.Services.csproj", "src/OpenCredentialPublisher.PublishingService.Services/"]
COPY ["OpenCredentialPublisher.Credentials.VerifiableCredentials/OpenCredentialPublisher.Credentials.VerifiableCredentials.csproj", "src/OpenCredentialPublisher.Credentials.VerifiableCredentials/"]
COPY ["OpenCredentialPublisher.Credentials.Cryptography/OpenCredentialPublisher.Credentials.Cryptography.csproj", "src/OpenCredentialPublisher.Credentials.Cryptography/"]
COPY ["OpenCredentialPublisher.Credentials.Clrs.v1_0/OpenCredentialPublisher.Credentials.Clrs.v1_0.csproj", "src/OpenCredentialPublisher.Credentials.Clrs.v1_0/"]
COPY ["OpenCredentialPublisher.Credentials.Drawing/OpenCredentialPublisher.Credentials.Drawing.csproj", "src/OpenCredentialPublisher.Credentials.Drawing/"]
RUN dotnet restore "OpenCredentialPublisher.PublishingService.Api/OpenCredentialPublisher.PublishingService.Api.csproj"
COPY . .
WORKDIR "/src/OpenCredentialPublisher.PublishingService.Api"
RUN dotnet build "OpenCredentialPublisher.PublishingService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenCredentialPublisher.PublishingService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenCredentialPublisher.PublishingService.Api.dll"]