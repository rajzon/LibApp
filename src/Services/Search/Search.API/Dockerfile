FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

ENV ASPNETCORE_ENVIRONMENT=Development

COPY ["Services/Search/Search.API/Search.API.csproj", "Services/Search/Search.API/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["BuildingBlocks/EventBus.Messages/", "BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "Services/Search/Search.API/"
COPY [ "Services/Search/Search.API/", "Services/Search/Search.API/"]
WORKDIR "Services/Search/Search.API"
RUN dotnet build "Search.API.csproj" -c Release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
WORKDIR /app
ENTRYPOINT ["dotnet", "Search.API.dll"] 