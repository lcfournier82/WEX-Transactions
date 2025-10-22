# 🧾 WEX Transactions API

[![Build Status](https://github.com/your-org/wex-transactions-api/actions/workflows/build.yml/badge.svg)](https://github.com/your-org/wex-transactions-api/actions)
[![Test Coverage](https://img.shields.io/codecov/c/github/your-org/wex-transactions-api.svg)](https://codecov.io/gh/your-org/wex-transactions-api)
[![Docker Build](https://img.shields.io/docker/v/your-org/wex-transactions-api?label=docker)](https://hub.docker.com/r/your-org/wex-transactions-api)

## 📘 Overview

**WEX Transactions API** is a .NET 8 Web API designed to manage financial transactions and integrate with treasury exchange rate services.  
The system follows **Clean Architecture** principles and is organized into multiple layers:

- **Domain** — Core business logic and domain entities  
- **Application** — Use cases and service interfaces  
- **Infrastructure** — External integrations (e.g., database, APIs, logging)  
- **Web** — ASP.NET Core Web API layer  
- **Tests** — Integration and unit tests for service validation  

The solution leverages **PostgreSQL** as the database, **Elasticsearch** for logs, and **Docker Compose** for environment orchestration.

---

## 🧱 Project Structure

```
WEXTransactions.sln
├── WEX.TransactionAPI.Domain/
├── WEX.TransactionAPI.Application/
├── WEX.TransactionAPI.Infrastructure/
├── WEX.TransactionAPI.Web/                # ASP.NET Core API
├── WEX.TransactionAPI.Tests/              # Integration Tests
└── docker-compose.yml
```

---

## ⚙️ Prerequisites

Before running locally or via containers, make sure you have:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://docs.docker.com/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/)
- (Optional) [Postman](https://www.postman.com/) or `curl` for testing endpoints

---

## 🏗️ Build the Project

### Option 1 — Local Build

```bash
dotnet restore
dotnet build --configuration Release
```

### Option 2 — Build with Docker

```bash
docker compose build
```

---

## 🧪 Run Tests

### Run Tests Locally

```bash
dotnet test --configuration Release
```

> ⚠️ Note: Integration tests use **Testcontainers**, so make sure Docker is running locally.

### Run Tests Inside Docker

```bash
docker compose run --rm api dotnet test
```

---

## 🚀 Run the Application

### Option 1 — Run Locally

```bash
dotnet run --project ./wex-transactions-api/WEX.TransactionAPI.Web.csproj
```

Then open your browser or API client at:

```
http://localhost:8080/swagger
```

### Option 2 — Run via Docker Compose

```bash
docker compose up -d
```

| Service         | URL                           |
|-----------------|-------------------------------|
| API (Swagger)   | http://localhost:8080/swagger |
| PostgreSQL      | localhost:5432                |
| Elasticsearch   | http://localhost:9200         |

---

## 🧩 Environment Variables

| Variable | Description | Default Value |
|-----------|--------------|----------------|
| `ASPNETCORE_ENVIRONMENT` | Environment name | `Development` |
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string | `Host=postgres;Port=5432;Database=purchases_db;Username=wex_user;Password=wexApiTest@01235` |
| `ElasticConfiguration__Uri` | Elasticsearch endpoint | `http://elasticsearch:9200` |

---

## ☁️ Deployment

1. **Build and tag the image:**
   ```bash
   docker build -t wex-transactions-api:latest .
   ```

2. **Push to registry:**
   ```bash
   docker tag wex-transactions-api:latest <your-registry>/wex-transactions-api:latest
   docker push <your-registry>/wex-transactions-api:latest
   ```

3. **Deploy using your platform (Kubernetes, ECS, Azure, etc.)**

---

## 📄 License

This project is licensed under the **MIT License** — see the `LICENSE` file for details.
