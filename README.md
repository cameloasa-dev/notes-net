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
