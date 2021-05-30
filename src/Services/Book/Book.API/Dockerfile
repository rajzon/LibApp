FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

ENV ASPNETCORE_ENVIRONMENT=Development

COPY ["Services/Book/Book.API/Book.API.csproj", "Services/Book/Book.API/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["BuildingBlocks/EventBus.Messages/", "BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "Services/Book/Book.API/"
COPY [ "Services/Book/Book.API/", "Services/Book/Book.API/"]
WORKDIR "Services/Book/Book.API"
#WORKDIR "BuildingBlocks/EventBus.Messages/"
#RUN dotnet build "EventBus.Messages.csproj" -c Release -o /app --no-restore
#WORKDIR "../../Services/Book/Book.API"
RUN dotnet build "Book.API.csproj" -c Release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Book.API.dll"] 

