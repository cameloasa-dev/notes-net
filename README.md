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

- user authentication

- API key generation

- secure access to protected endpoints

- CRUD operations on a chosen resource

- clean architecture (services, repositories, DTOs, middleware)

- database integration using Entity Framework Core and SQL Server

- API documentation using Swagger / OpenAPI

The chosen resource for this project is Notes.
Each note contains a title, content, and is linked to the authenticated user.

## 🧱 Full Stack / Technologies Used

- ASP.NET Core 9 Web API
- Entity Framework Core 9
- SQL Server
- ASP.NET Core Identity (email + password)
- API Key Authentication (custom)
- FluentValidation (request validation)
- Swashbuckle / Swagger (API documentation)

```text
MyOwnApi/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── UserController.cs
│   ├── ApiKeyController.cs
│   └── NotesController.cs
│
├── Models/
│   ├── User.cs
│   ├── ApiKey.cs
│   └── Note.cs
│
├── DTOs/
│   │── RegisterDto.cs
│   │── LoginDto.cs
│   │── CreateNoteDto.cs
│   │── UpdateNoteDto.cs
│   │── NoteResponseDto.cs
│   │── ApiKeyResponseDto.cs
│   │── AuthResponseDto.cs
│
├── Validators/
│   ├── RegisterDtoValidator.cs
│   ├── LoginDtoValidator.cs
│   └── ApiKeyValidator.cs
│
├── Services/
│   ├── AuthService.cs
│   ├── ApiKeyService.cs
│   └── NotesService.cs
│
├── Repositories/
│   ├── UserRepository.cs
│   ├── ApiKeyRepository.cs
│   └── NotesRepository.cs
│
├── Middleware/
│   └── ApiKeyMiddleware.cs
│
├── Context/
│   └── AppDbContext.cs
│
├── appsettings.json
├── appsettings.Development.json
├── .env.development
├── .env.production
├── Program.cs
├── MyOwnApi.csproj
└── README.md
```

## 🔐 Authentication Flow

1. Register
User registers with email + password.

2. Login
User logs in and receives access to the API Key endpoint.

3. Request API Key
The API key is:

- generated server-side

- stored in the database

- linked to the user

- required for all protected endpoints

4. Use API Key

Every protected request must include:

```text
X-Api-Key: your_api_key_here
```

## 🚀 How to Run the Project

Clone the repository

Restore dependencies

```bash
dotnet restore
```

Create database migrations

```bash
dotnet ef migrations add <MigrationName>
```

Apply database migrations

```bash
dotnet ef database update
```

Run the API

```bash
dotnet run
```

## 🔒 Swagger Security Scheme

Swagger UI is configured to support API Key authentication.

1. Click Authorize

2. Enter your API Key

3. Swagger will automatically send:

```text
X-Api-Key: your_api_key_here
```

for all protected endpoints.

[http://localhost:5027/swagger/index.html]

## 📝 Notes Resource (CRUD)

Each note contains:

- Title

- Content

- UserId

- CreatedAt

- UpdatedAt

All CRUD operations require a valid API Key.

### 🧪 Example Requests

Examples will include:

- Register

```json
POST /api/auth/register
{
  "email": "test@example.com",
  "password": "Test123!"
}
```

- Login

```json
POST /api/auth/login
{
  "email": "test@example.com",
  "password": "Test123!"
}
```

- Request API key

POST /api/apikey
Response:

```json
{
  "apiKey": "xxxxxxxxxxxxxxxxxxxx",
  "createdAt": "...",
  "expiresAt": "...",
  "userId": "..."
}
```

- Create Note

POST /api/notes
X-Api-Key: your_api_key

```json
{
  "title": "My note",
  "content": "Hello world"
}
```

## 🧪 API Testing – Postman

Postman is used to verify:

- registration

- login

- API key generation

- CRUD operations

- authorization

- error handling

- status codes

Base URL:
All requests are sent to:

[http://localhost:5027]

## 🧭 Future Improvements

- pagination

- search

- soft delete

- refresh API keys

- tags/categories for notes
