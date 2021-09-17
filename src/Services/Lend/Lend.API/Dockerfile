FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

ENV ASPNETCORE_ENVIRONMENT=Development

COPY ["Services/Lend/Lend.API/Lend.API.csproj", "Services/Lend/Lend.API/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["BuildingBlocks/EventBus.Messages/", "BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "Services/Lend/Lend.API/"
COPY [ "Services/Lend/Lend.API/", "Services/Lend/Lend.API/"]
WORKDIR "Services/Lend/Lend.API"
RUN dotnet build "Lend.API.csproj" -c Release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
WORKDIR /app
ENTRYPOINT ["dotnet", "Lend.API.dll"] 