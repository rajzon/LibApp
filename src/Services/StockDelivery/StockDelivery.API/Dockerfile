FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

ENV ASPNETCORE_ENVIRONMENT=Development

COPY ["Services/StockDelivery/StockDelivery.API/StockDelivery.API.csproj", "Services/StockDelivery/StockDelivery.API/"]
RUN dotnet restore "Services/StockDelivery/StockDelivery.API/"
COPY [ "Services/StockDelivery/StockDelivery.API/", "Services/StockDelivery/StockDelivery.API/"]
WORKDIR "Services/StockDelivery/StockDelivery.API"
RUN dotnet build "StockDelivery.API.csproj" -c Release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
WORKDIR /app
ENTRYPOINT ["dotnet", "StockDelivery.API.dll"] 