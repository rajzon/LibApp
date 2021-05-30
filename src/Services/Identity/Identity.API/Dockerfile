FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

ENV ASPNETCORE_ENVIRONMENT=Development

COPY ["Services/Identity/Identity.API/Identity.API.csproj", "Services/Identity/Identity.API/"]
RUN dotnet restore "Services/Identity/Identity.API/"
COPY [ "Services/Identity/Identity.API/", "Services/Identity/Identity.API/"]
WORKDIR "Services/Identity/Identity.API"
RUN dotnet build "Identity.API.csproj" -c Release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
WORKDIR /app
ENTRYPOINT ["dotnet", "Identity.API.dll"] 
