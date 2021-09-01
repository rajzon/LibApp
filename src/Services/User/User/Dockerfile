FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

ENV ASPNETCORE_ENVIRONMENT=Development

COPY ["Services/User/User/User.csproj", "Services/User/User/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["BuildingBlocks/EventBus.Messages/", "BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "Services/User/User/"
COPY [ "Services/User/User/", "Services/User/User/"]
WORKDIR "Services/User/User"
RUN dotnet build "User.csproj" -c Release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
WORKDIR /app
ENTRYPOINT ["dotnet", "User.dll"] 