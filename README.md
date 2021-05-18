# LibApp

The application is designed to help the library in their daily tasks, such as: adding new books to the system, service 
borrowing / booking, assigning books to locations, etc. In addition, the application will include the ability to 
import and export data as csv. in order to speed up the operation. The administrator will be able to create new 
users, including giving them appropriate rights. The system is to be to some extent configurable both in terms of 
system operation and displaying the content on a public website. In addition, the project envisages in the future 
creation of a public version of the website (one that will be able to be viewed by customers), which will allow 
for: browsing all available books in the system, reviewing / rating them, booking a given book, adding a book to 
the list of favorites, etc.


# Prerequisite
If you are missing https dev certificate or have problem with existing dev cert to be found by docker, then you have to do:
1. Clear existing dev certs
```bash
dotnet dev-certs https --clean
```
2. Run command in PowerShell, that will create and trust certificate and export it to below location
```bash
dotnet dev-certs https --trust -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p SECRETPASSWORD
```

## Angular packages installation

Use the command on {workspace}/LibApp/Web/OrganisationSPA

```bash
npm install 
```

## Backend installation using Docker

Use the command on {workspace}/LibApp/

```bash
docker-compose build
```

## Running application

Use the command on {workspace}/LibApp/Web/OrganisationSPA for Angular SPA

```bash
ng serve
```

Use the command on {workspace}/LibApp/ for Backend

```bash
docker-compose up
```


