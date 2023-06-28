FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["gcp.api.integrations.poc/gcp.api.integrations.poc.csproj", "gcp.api.integrations.poc/"]
RUN dotnet restore "gcp.api.integrations.poc/gcp.api.integrations.poc.csproj"
COPY . .
WORKDIR "/src/gcp.api.integrations.poc"
RUN dotnet build "gcp.api.integrations.poc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gcp.api.integrations.poc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gcp.api.integrations.poc.dll"]
