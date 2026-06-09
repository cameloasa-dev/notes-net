# Notes API Documentation

## Overview

The Notes API provides user authentication, API key–based access control, and full CRUD operations for managing personal notes.
It uses JWT authentication for user identity and API Keys for resource‑level authorization.

The API is designed for secure, multi‑user environments where each user can only access their own notes.

## Registration

1. Register
POST /api/auth/register

Register a user

- Request

```json
{
  "firstName": "User",
  "lastName": "Example",
  "email": "test@example.com",
  "password": "Example123!",
  "confirmPassword":"Example123!"
}
```

- Response

```json
{
   "message": "User registered successfully."
}

```

## Authentication

1. Login
POST /api/auth/login

Authenticates a user and returns a JWT token.

- Request

```json
{
  "email": "user@example.com",
  "password": "string"
}
```

- Response

```json
{
  "token": "jwt-token-here",
  "expiresAt": "2026-06-08T12:00:00Z",
  "userId": "uuid",
  "email": "testuser@test.com",
  "firstName": "string",
  "lastName": "string"
}
```

2. Get Current User
GET /api/auth/me

Requires a valid JWT.

Response

```json
{
  "id": "uuid",
  "email": "user@example.com",
  "firstName": "User",
  "lastName": "Example"
}
```

3. API Key Flow

3.1 Generate API Key
POST /api/apikey

Creates a new API key for the authenticated user.

Response

```json
{
  "apiKey": "generated-key",
  "createdAt": "2026-06-08T16:33:32Z",
  "userId": "uuid"
}
```json

3.2 Get My API Key
GET /api/apikey

Returns the existing API key for the authenticated user.

Response

```json
{
  "apiKey": "existing-key",
  "createdAt": "2026-06-08T16:33:32Z",
  "userId": "uuid"
}
```

If no key exists:

```json
{
  "success": false,
  "message": "No API key found.",
  "errors": null
}
```

4. Notes Module
All Notes endpoints require:

JWT in Authorization: Bearer [token]

API Key in X-Api-Key: [key]

4.1 Get All Notes
GET /api/notes

Returns all notes belonging to the authenticated user.

4.2 Get Note by ID
GET /api/notes/{id}

Returns a single note.

404 if the note does not exist or does not belong to the user.

4.3 Create Note
POST /api/notes

Request

```json
{
  "title": "Learning .NET",
  "content": "Finish the Notes project.",
  "dueDate": "2026-06-12T18:00:00Z",
  "priority": "High",
  "isPinned": true
}
```

Response

```json
{
  "id": 7,
  "title": "Learning .NET",
  "content": "Finish the Notes project.",
  "createdAt": "2026-06-08T16:44:48Z",
  "updatedAt": "2026-06-08T16:44:48Z",
  "isDone": false,
  "dueDate": "2026-06-12T18:00:00Z",
  "priority": "High",
  "isPinned": true,
  "isOverdue": false,
  "userId": "uuid"
}
```

4.4 Update Note
PUT /api/notes/{id}

Requires full UpdateNoteDto.

Request

```json
{
  "title": "Updated title",
  "content": "Updated content",
  "dueDate": "2026-06-15T18:00:00Z",
  "priority": "Medium",
  "isPinned": false,
  "isDone": true
}
```

404 if note does not exist.

4.5 Toggle Note Completion
PATCH /api/notes/{id}/toggle

Toggles isDone between true and false.

No request body.

404 if note does not exist.

4.6 Delete Note
DELETE /api/notes/{id}

Deletes the note.

404 if note does not exist.

5. DTO Definitions

5.1 CreateNoteDto

- title (string, required, 3–100 chars, no whitespace-only)

- content (string, required, no whitespace-only)

- dueDate (DateTime?, optional)

- priority (Low, Medium, High)

- isPinned (bool)

5.2 UpdateNoteDto
Same as CreateNoteDto, plus:

- isDone (bool)

6. Validation Rules

**Title**
Required
Min 3, max 100
Cannot be whitespace-only

**Content**
Required
Cannot be whitespace-only

**Priority**
Allowed values:Low,Medium,High

**DueDate**
Must be a valid ISO date
Optional

7. Error Handling

400 — Validation Error

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": ["Title is required."]
  }
}
```

401 — Unauthorized
Missing or invalid JWT.

403 — Forbidden
Missing or invalid API Key.

404 — Not Found
Note does not exist or does not belong to the user.

409 — Conflict
Used for business logic conflicts (e.g., email already exists).

8. Security Requirements
Every Notes endpoint requires:

- Authorization: Bearer [jwt]
- X-Api-Key: [api-key]
Both must be valid.

9. Example Workflow

- Login → receive JWT
- Generate API Key
- Create notes
- Update notes
- Toggle notes
- Delete notes
