FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["LegalProvisionsBackend/LegalProvisionsBackend.csproj", "LegalProvisionsBackend/"]
COPY ["LegalProvisionsLib/LegalProvisionsLib.csproj", "LegalProvisionsLib/"]
RUN dotnet restore "LegalProvisionsBackend/LegalProvisionsBackend.csproj"
COPY . .
WORKDIR "/src/LegalProvisionsBackend"
RUN dotnet build "LegalProvisionsBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LegalProvisionsBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LegalProvisionsBackend.dll"]
