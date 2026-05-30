[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/fwqWTTOB)
# Inlämningsuppgift

Detta projekt är startpunkten för din inlämingsuppgift. Ni behöver skapa ett eget api som är skyddat med hjälp av api-nycklar. Ni skall definiera resurser (data) som skall kunna skickas från ert api efter att en användare gör ett korrekt anrop och inkluderar sin api-nyckel. 

## Användarregistrering
- En användare ska kunna registrera sig i systemet.
- Användaren ska kunna logga in.
- Du väljer inloggningsstrategi:
  - Individuella konton (med e-post och lösenord)

## API-nyckel
- Efter registrering och inloggning ska användaren kunna begära en API-nyckel.
- API-nyckeln ska sparas i databasen och kopplas till användaren.

## Skyddade API-slutpunkter med CRUD-funktionalitet
- Du ska skapa minst en resurs (t.ex. recept, sportresultat, speldata, personliga anteckningar) som användaren kan hantera via CRUD:
  - Create – Lägga till data.
  - Read – Hämta data.
  - Update – Ändra befintlig data.
  - Delete – Ta bort data.
- Alla CRUD-operationer ska kräva giltig API-nyckel.
- API-nyckeln ska skickas med i anropet och valideras innan data returneras eller ändras.

## Databas
- Du väljer:
  - Entity Framework + SQL

## Betygsättning
Denna uppgift bedöms med IG (icke godkänd), G (godkänd) och VG (Väl Godkänt).

### För godkänt (G) krävs:
- Användare kan registrera sig och logga in.
- Användare kan begära och få en API-nyckel.
- CRUD-funktionalitet finns för vald resurs och är skyddad med API-nyckel.
- API-nyckeln valideras korrekt vid varje anrop.
- Databasen fungerar enligt vald lösning (SQL med EF).
- Ni använder kontroller som endpoints och hanterar logiken i dessa.
- Korrekta svarskoder skickas från ditt API.

### För Väl godkänt (VG) krävs:
- Samtliga punkter från G
- Ni har valt en komplex struktur av data att returnera och använder er av DTO:er för att begränsa informationen.
- Ni använder designmönster med tjänster och repositories
- Ni använder korrekt validering och har skapat minst en egen validering (custom validation).
- En fungerande Swagger

## Inlämning
- En länk till ett GitHub-repo på itslearning
- Bifoga en README.md som beskriver:
  - Hur projektet startas.
  - Hur man registrerar en användare och får en API-nyckel.
  - Exempel på anrop till de skyddade CRUD-slutpunkterna.

## 📌 Project Overview – Notes API

This project is a secure Notes API built with ASP.NET Core 9.
Users can register, log in, request an API key, and use that key to access protected CRUD endpoints for managing personal notes.

The purpose of the assignment is to demonstrate:

user authentication

API key generation

secure access to protected endpoints

CRUD operations on a chosen resource

clean architecture using services, repositories, DTOs, and validation

database integration using Entity Framework Core and SQL Server

The chosen resource for this project is Notes.
Each note contains a title, content, and is linked to the authenticated user.

## 🧱 Full Stack / Technologies Used

- ASP.NET Core 9 Web API
- Entity Framework Core 9
- SQL Server LocalDB
- ASP.NET Core Identity (email + password)
- API Key Authentication (custom)
- AutoMapper (DTO mapping)
- FluentValidation (request validation)
- Swashbuckle / Swagger (API documentation)

```text
MyOwnApi/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── ApiKeyController.cs
│   └── NotesController.cs
│
├── Models/
│   ├── ApplicationUser.cs
│   ├── ApiKey.cs
│   └── Note.cs
│
├── DTOs/
│   ├── Auth/
│   │   ├── RegisterDto.cs
│   │   └── LoginDto.cs
│   ├── Notes/
│   │   ├── NoteCreateDto.cs
│   │   ├── NoteUpdateDto.cs
│   │   └── NoteResponseDto.cs
│   └── ApiKey/
│       └── ApiKeyResponseDto.cs
│
├── Validators/
│   ├── RegisterDtoValidator.cs
│   ├── LoginDtoValidator.cs
│   └── NoteDtoValidator.cs
│
├── Services/
│   ├── Interfaces/
│   │   ├── IAuthService.cs
│   │   ├── IApiKeyService.cs
│   │   └── INotesService.cs
│   ├── AuthService.cs
│   ├── ApiKeyService.cs
│   └── NotesService.cs
│
├── Repositories/
│   ├── Interfaces/
│   │   ├── IApiKeyRepository.cs
│   │   └── INotesRepository.cs
│   ├── ApiKeyRepository.cs
│   └── NotesRepository.cs
│
├── Middleware/
│   └── ApiKeyMiddleware.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Mappings/
│   └── AutoMapperProfile.cs
│
├── appsettings.json
├── appsettings.Development.json
├── .env.development
├── .env.production
├── Program.cs
├── MyOwnApi.csproj
└── README.md
```

Architecture

Controllers

Services

Repositories

DTOs

Validators

Middleware (API key validation)

## Tools

EF Core Migrations

Swagger / OpenAPI

GitHub Classroom

## 🚀 How to Run the Project

Clone the repository

Restore dependencies

```bash
dotnet restore
```

Apply database migrations

```bash
dotnet ef database update
```

Run the API

```bash
dotnet run
```

Open Swagger UI in the browser

[http://localhost:5027/swagger/index.html]

### 🔐 User Registration & Login

1. Register
A user can register with email and password.
The account is stored using ASP.NET Core Identity.

2. Login
The user logs in with email and password.
After login, the user can request an API key.

🔑 Requesting an API Key
After authentication, the user can request an API key.
The API key is:

generated server-side

stored in the database

linked to the user

required for all CRUD operations

The API key must be included in the request header:

X-Api-Key: your_api_key_here

### 📝 Notes Resource (CRUD)

The protected resource in this project is Notes.

Each note contains:

Title

Content

UserId (automatically linked to the authenticated user)

CRUD operations:
Create – Add a new note

Read – Retrieve notes

Update – Modify an existing note

Delete – Remove a note

All operations require a valid API key.

### 🧪 Example Requests

Examples will include:

Register

Login

Request API key

CRUD operations for Notes

(These will be added after implementation and testing.)

### 🧪 API Testing – Postman

All API functionality in this project is tested using Postman.
Postman is used to verify:

user registration

user login

API key generation

validation of API keys

CRUD operations for Notes

correct HTTP status codes

error handling

authorization flow

Postman Base URL
All requests are sent to:

[http://localhost:5027]
