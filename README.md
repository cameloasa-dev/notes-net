
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
│   ├── UpdateNoteDtovalidator.cs
│   └── CreateNoteDtoValidator.cs
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
│   └── ErrorHandlingMiddleware.cs
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
├── agent.md
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

- IsDone

- DueDate

- Priority (Low, Medium, High)

- IsPinned

- IsDeleted (soft delete)

- IsOverdue

All CRUD operations require a valid API Key.

### 🧪 Example Requests

Examples will include:

- Register

```json
POST /api/auth/register
{
  "firstName": "First",
  "lastName": "Last",
  "email": "test@example.com",
  "password": "Example123!",
  "confirmPassword":"Example123!"
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
  "userId": "..."
}
```

- Create Note

POST /api/notes
X-Api-Key: your_api_key

```json
{
  "title": "My note",
  "content": "Hello world",
  "dueDate": "2026-06-08T12:07:41.498Z",
  "priority": "string", //Low, Medium, High
  "isPinned": true
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

- tags/categories for notes

- add cors
