# Fiap Tech Challenge

This is a .NET 8 project that provides a RESTful API for managing contacts. The project is written in C# and uses several technologies and frameworks.

[![Build](https://github.com/lucasfm95/fiap-tech-challenge-fase2/actions/workflows/continuous-integration.yml/badge.svg)](https://github.com/lucasfm95/fiap-tech-challenge-fase2/actions/workflows/continuous-integration.yml)

## Technologies Used

- **.NET 8**: The latest version of the .NET framework, used for building high-performance, cross-platform applications.
- **C#**: The primary programming language used in this project.
- **ASP.NET Core**: A framework for building web applications.
- **Entity Framework Core**: An object-relational mapper (ORM) that simplifies data access by letting you work with relational data using domain-specific objects.
- **PostgreSQL**: The database used for persisting data.
- **FluentValidation**: A library for building strongly-typed validation rules.
- **Prometheus**: is an open-source systems monitoring and alerting toolkit, used to get metrics about the app.
- **Grafana**: is an open-source plataform for monitoring and observability, used to create the dashboards with Prometheus metrics. 

## Project Structure

- `src/Fiap.TechChallenge.Domain`: This project contains the domain entities and repositories interfaces.
- `src/Fiap.TechChallenge.Application`: This project contains the application services and DTOs.
- `src/Fiap.TechChallenge.Api`: This project is the API layer that exposes endpoints to interact with the application.
- `src/Fiap.TechChallenge.Infrastructue`: This project contains the implementation of the repositories and the database context.
- `tests/Fiap.TechChallenge.Tests`: This project contains the unit tests for the application services.
- `tests/Fiap.TechChallenge.IntegrationTests`: This project contains the integrations tests for the application services.

## Getting Started With Docker Compose
1. Clone the repository.
2. Navigate to the root directory.
3. Ensure Docker is installed on your machine.
4. Run command `docker-compose up -d` in the terminal
5. The API be avaible at `http://localhost:8888`
6. The Grafana be avaible at `http://localhost:3000` and use the user admin and password admin.


## Getting Started With local app
1. Clone the repository.
2. Navigate to the `src/Fiap.TechChallenge.Api` directory.
3. Run `dotnet restore` to restore the NuGet packages.
4. Update the connection string in the `launchSettings.json` file.
5. Run `dotnet run` to start the application.

The API will be available at `http://localhost:5010` and `https://localhost:7010`.

## Running only the app With Docker
1. Navigate to the root directory.
2. Run `docker build -t tech-challenge-api .` to start the application.
3. Run `docker run --name tech-challenge-api -p 5010:80 -d tech-challenge-api` to start the container.
4. The API will be available at `http://localhost:5010`.

## Running Migrations
1. Navigate to the `src/Fiap.TechChallenge.Infrastructure` directory.
2. Add the following code in the `ContactDbContext`:
3. `protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123456");
    }`
4. Run the following commands to create the database and run the migrations.
5. `docker-compose up -d`
6. `cd .\src\Fiap.TechChallenge.Infrastructure\`
7. `dotnet ef migrations add InitialMigration`
8. `dotnet ef database update `
9. The database will be created and the tables will be created.
10. The API will be available at `http://localhost:5010` and `https://localhost:7010`.

## Running the Tests
1. Navigate to the `src/Fiap.TechChallenge.Tests` directory.
2. Run `dotnet test` to run the tests.
3. The test results will be displayed in the console.

## API Endpoints

- `GET /api/contact`: Get all contacts.
- `GET /api/contact/{id}`: Get a contact by ID.
- `GET /api/contact/ddd/{dddNumber}`: Get all contacts by DDD number.
- `POST /api/contact`: Create a new contact.
- `DELETE /api/contact/{id}`: Delete a contact by ID.
- `PUT /api/contact/{id}`: Update a contact by ID.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.