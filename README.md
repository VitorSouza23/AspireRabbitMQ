# About

This is a little project to demonstrate how to use RabbitMQ with .NET Aspire.
The solution contains four projects:
- AspireRabbitMQ.AppHost: The Aspire host application that will orchestrate all the other projects.
- AspireRabbitMQ.ServiceDefault: The Aspire Service Default methods collection.
- AspireRabbitMQ.Sender: The ASP.NET Core Web API that have a POST endpoint to send a message to RabbitMQ.
- AspireRabbitMQ.Receiver: The ASP.NET Core project that have a background service to receive messages from RabbitMQ.

## How to run

- Pre requirements:
  - Docker installed
  - .NET 8 SDK installed
  - Aspire Workload installed

Visual Studio approach: Open the solution in Visual Studio and run the AspireRabbitMQ.AppHost project. This will start the Aspire host and all the other projects.

Dotnet CLI approach: Run the following commands in the root folder of the solution:
```bash
dotnet run --project AspireRabbitMQ.AppHost
```

## How to test

After running the solution, you can test the RabbitMQ integration by sending a POST request to the Sender project. The endpoint is:

```bash
curl -X 'POST' \
  'http://localhost:5155/send?message=test' \
  -H 'accept: */*' \
  -d ''
```

You can verify if the message was received by checking the logs of the Receiver project.

## Dashboards and frontends

It's possible to access the RabbitMQ management dashboard by accessing the following URL:

```bash
http://localhost:36567/
```

To get the username and password, check the Aspire portal under the RabbitMQ service environment variables.

It's also possible to access the Aspire portal to see the logs and metrics of the services. The URL is:

```bash
http://localhost:15021/
```

It's possible access AspireRaabbitMQ.Sender Swagger UI by accessing the following URL:

```bash
http://localhost:5155/swagger/index.html
```