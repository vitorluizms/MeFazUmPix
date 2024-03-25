# APIx Hub

## Description

The MyWallet API is an application that allows you to manage your personal finances. It was developed using C# and .NET with Entity Framework.

## Features

- Management of bank accounts
- Create Pix keys
- Recording of financial transaction
- Generate a report of payments concilliations of archives to database data

## Technologies

- C#;
- ASP.NET Core (version 8.0.2);
- Entity Framework Core (ORM) (version 8.0.2);
- PostgreSQL;
- RabbitMQ;
- Prometheus;
- Grafana;
- Grafana K6;
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

## Queue Routes

To execute the routes `POST /payments` and `POST /concilliation`, you need to clone and execute the respective consumers and PSP Mock application.

-Payments Consumer: `https://github.com/vitorluizms/ConsumerPixHub`;
-Concilliation Consumer: `https://github.com/vitorluizms/ConcilliationConsumer`;
-PSP Mock: `https://github.com/vitorluizms/PSP-Mock`;

## Seeds

Create a .env file in `.k6/` folder, use the `.env.example` file and set the database connection.

To create a seed, follow the following steps:

-Open the project
-cd .k6 -`npm run seed`
WARNING - If your RAM is limited, don't use `npm run seed`, use:

- `npm run seed:pix-key`
- `npm run seed:payment`

## K6 Tests

In k6 folder, run `npm install` to install the dependencies.

To use k6 tests, follow the steps described at Seeds section. The base number data is 1M, but you can change in the seed archive.

- `npm run test:post-keys` - To run tests of POST /keys
- `npm run test:get-keys` - To run tests of GET /keys/:type/:value
- `npm run test:postpayments` - To run tests of POST /payments