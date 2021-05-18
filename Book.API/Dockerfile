FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

ENV ASPNETCORE_ENVIRONMENT=Development

COPY ["Book.API/Book.API.csproj", "Services/Book/Book.API/"]
RUN dotnet restore "Services/Book/Book.API/"
COPY [ "Book.API/", "Services/Book/Book.API/"]
WORKDIR "Services/Book/Book.API"
RUN dotnet build "Book.API.csproj" -c Release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Book.API.dll"] 

