# OnyxStore Product Service

A simple ASP.NET Core Web API project built with .NET 8+ to manage products with JWT-based authentication and SQLite persistence.

---
## Features

- **HealthCheckController**
    - Anonymous endpoint to verify the API health (`GET /api/healthcheck`).

- **AuthController**
    - User registration and login with JWT token generation.
    - Passwords securely hashed.
    - Tokens contain claims for authorization.

- **ProductsController**
    - Secured endpoints requiring JWT authentication with an audience claim.
    - Create products.
    - Retrieve all products.
    - Retrieve products filtered by color.

---

## Possible Architecture Enhancements
    - Added Architecture_With_EDA.pdf
    - A simple architecture diagram showing how this product service could form part of a distributed or microservices event-driven architecture with a few other components shown (e.g. orders, payments)

## Technologies Used

- .NET 8 Web API
- SQLite (via Entity Framework Core)
- JWT Authentication
- MSTest, Moq, In-MemoryDB & Shouldly for unit testing

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQLite](https://www.sqlite.org/index.html) (optional, the DB file will be created automatically)
- [Git](https://git-scm.com/)

### Clone the repository

```bash
git clone https://github.com/rathnagiri/OnyxStore.git
cd OnyxStore
