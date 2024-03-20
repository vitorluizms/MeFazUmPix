# MyWallet API

## Description
The MyWallet API is an application that allows you to manage your personal finances. It was developed using C# and .NET with Entity Framework.

## Features
- Management of bank accounts
- Create Pix keys
- Recording of financial transactions

## Technologies
- C#;
- ASP.NET Core (version 8.0.2);
- Entity Framework Core (ORM) (version 8.0.2);
- PostgreSQL;
- RabbitMQ;
- Prometheus;
- Grafana;
- Grafana K6 with Node.js interface;
- Docker;
- Swagger;

## Configuration
1. Clone the repository
2. Open the `appsettings.json` file and configure the database connection string
3. Run the Entity Framework migrations to create the database:
    ```
    dotnet ef database update
    ```
4. Start the application:
    ```
    dotnet run
    ```

## Start with docker compose
Use `docker compose up` to start with all dependencies and start the project.

## API Routes
- `GET /health`: Health route
- `POST /keys`: Create a new pix key
- `GET /keys/:value/:type`: Get a pix key by value and type
- `POST /payments`: Create a financial transaction

You can use swagger documentation to see details of the routes.
Start the project and open `http:localhost:<PORT>/swagger/index.html`.