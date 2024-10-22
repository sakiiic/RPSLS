
# Rock, Paper, Scissors, Lizard, Spock - Game Application

<p  align="center">

![Rock, Paper, Scissors, Lizard, Spock](./img/RPSLS.webp)

</p>

### A modern twist on the classic game of Rock, Paper, Scissors with two new moves: Lizard and Spock!

## Table of Contents

- [About the Game](#about-the-game)
- [How the Game Works](#how-the-game-works)
- [API Endpoints](#api-endpoints)
- [Technologies Used](#technologies-used)
- [Running the Application](#running-the-application)
- [Adding Migrations](#adding-migrations)

## About the Game

This application is a web-based version of the famous Rock, Paper, Scissors, Lizard, Spock game. The game adds "Lizard" and "Spock" to the traditional Rock, Paper, Scissors game, making it more interesting and strategic!

-  **Rock** crushes Scissors.
-  **Scissors** cuts Paper.
-  **Paper** covers Rock.
-  **Rock** crushes Lizard.
-  **Lizard** poisons Spock.
-  **Spock** smashes Scissors.
-  **Scissors** decapitates Lizard.
-  **Lizard** eats Paper.
-  **Paper** disproves Spock.
-  **Spock** vaporizes Rock.

---

## How the Game Works

### Game Logic Overview

1. The player selects one of the five available options: Rock, Paper, Scissors, Lizard, or Spock.
2. The computer generates a random choice.
3. The game logic determines the winner based on predefined rules.
4. The result is displayed as a win, loss, or draw for the player.

The game is structured using the `GameService` class that handles the core logic. The service fetches all available choices from the database and uses a random generator to simulate the computer's choice.

---

## API Endpoints

The application exposes three main API endpoints:

### 1. `GET /choices`
-  **Description**: Retrieves the list of all possible choices (Rock, Paper, Scissors, Lizard, Spock).
-  **Response**: A list of choices in the game.

Example:
```bash
curl  http://localhost:5000/choices
```

---

### 2. `GET /choices`
-  **Description**: Retrieves the list of all possible choices (Rock, Paper, Scissors, Lizard, Spock).
-  **Response**: A list of available choices.

```bash
curl  http://localhost:5000/choice
```

### 3. POST /play
**Description**: Plays a round based on the player's selected choice (Rock, Paper, Scissors, Lizard, Spock).

**Request**: The user's choice ID (1-5) must be passed in the request body.

**Response**: The result of the game, including whether the player won, lost, or tied against the computer.

**Example**:
```bash
curl  -X  POST  http://localhost:5000/play  -H  "Content-Type: application/json"  -d  "3"
```

---

## Technologies Used

-  **ASP.NET Core 8.0**: A modern, high-performance web framework for building APIs.
-  **Entity Framework Core**: An ORM for interacting with the database.
-  **SQL Server**: The database system used for storing game choices and rules.
-  **AutoMapper**: To map between entities and data transfer objects (DTOs).
-  **FluentValidation**: For input validation.
-  **Dependency Injection (DI)**: To manage service lifetimes and inject dependencies where needed.
-  **Docker**: Used to containerize the application and database.

---
## Running the Application

### Prerequisites

- .NET 8.0 SDK
- Docker
- SQL Server

### Steps for Local Development

#### 1. Clone the Repository:
```bash
git  clone  https://github.com/sakiiic/RPSLS.git
```

#### 2. Navigate to the Project Directory:
```bash
cd  rpsls-game
```

#### 3. Restore Dependencies:
```bash
dotnet  restore
```

#### 4. Run the Application:
```bash
dotnet  run
```

#### 5. Access the API:
The application will be available at `http://localhost:5000`.

---

## Adding Migrations

Entity Framework Core allows you to handle database migrations. Follow these steps to create and apply migrations to keep your database schema up to date.

### Prerequisites

Ensure you have the following tools installed:

- **.NET Core CLI**
- **SQL Server** running either in Docker or locally.

### Steps to Add Migrations

1. **Open Terminal in the Project Directory**:

   Ensure you are in the root of the project where your `.csproj` file is located.

   ```bash
   cd rpsls-game
   ```

2. **Add a Migration**:

   Use the following command to create a new migration based on the changes in your Entity Framework models:

   ```bash
   dotnet ef migrations add <MigrationName>
   ```

   Replace `<MigrationName>` with a descriptive name for the migration (e.g., `InitialCreate`, `AddPlayerStats`).

3. **Update the Database**:

   After generating the migration, apply it to the database by running:

   ```bash
   dotnet ef database update
   ```

   This will apply all pending migrations to your database, ensuring that the schema is up to date.

### Common Commands

- **Remove a Migration**: If you created a migration by mistake, you can remove it with:

   ```bash
   dotnet ef migrations remove
   ```

- **View Migration Status**: You can check the status of migrations in your project:

   ```bash
   dotnet ef migrations list
   ```

