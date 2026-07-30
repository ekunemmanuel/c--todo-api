# Todo API (C# & ASP.NET Core Minimal APIs)

A lightweight RESTful To-Do Management API built with **C# (.NET 10)**, **ASP.NET Core Minimal APIs**, **Entity Framework Core (PostgreSQL)**, **AutoMapper**, and **Scalar API Reference**.

---

## 📝 Developer Journal: Learning C# & Minimal APIs (A TypeScript Developer's Perspective)

*Reflections and key insights from building my first REST API using C# and .NET.*

---

### 1. Dependency Injection, Parameter Injection & Managing Parameter Bloat
> *"In TypeScript, I used to import modules/functions directly or resolve them from singletons. In C#, I noticed heavy use of framework-level Dependency Injection. In Minimal APIs, calling database context or mappers is clean—you pass the service type as a parameter to the handler. However, as an API grows, injecting multiple services (`IMapper`, `ApplicationDbContext`, `ILogger`, `IConfiguration`...) makes parameter lists very long."*

- **TypeScript Approach**: Importing functions/modules directly (`import { db } from './db'`) or using DI containers (like NestJS or `tsyringe`).
- **C# Minimal API Approach**: Injected via parameter lists `(IMapper mapper, ApplicationDbContext db, ILogger<Program> logger)`.
- **Flutter / Dart Parallel**: Similar to Service Locators (`GetIt`) or `Provider.of<T>(context)` in Flutter, where a central container provides dependencies.
- **Handling Long Parameter Lists in C#**: 
  - Using the **`[AsParameters]`** attribute in ASP.NET Core to group multiple injected services into a single container struct/class (e.g. `([AsParameters] TodoServices services)`).
  - Encapsulating logic inside dedicated Service/Handler classes or MediatR patterns.

---

### 2. Minimal APIs vs Controllers (No Controller Classes Required!)
> *"I thought I was going to be using Controllers for everything like traditional ASP.NET Core tutorials showed. But Minimal APIs (`app.MapGet`, `app.MapPost`) mean I don't necessarily need Controller classes at all—I can map routes directly!"*

- **Traditional Controller Approach**: Creating `TodoController : ControllerBase` files with attributes (`[HttpGet]`, `[HttpPost]`, `[Route]`).
- **Minimal API Approach**: Expressive top-level route mapping directly in `Program.cs`, reducing boilerplate while keeping the API lightweight.

---

### 3. Database Configuration & Secret Management
> *"Configuring a database wasn't as simple as placing a database string inside a `.env` file like I was used to in TypeScript / Node.js. Visual Studio assisted with `UserSecrets` (`secrets.json`). Learning how .NET separates configuration (`appsettings.json`) from local secrets was a new experience."*

- Managing sensitive connection strings outside source control using `UserSecretsId` and `builder.Configuration`.
- Entity Framework Core Code-First approach: mapping C# classes to PostgreSQL relational database tables via migrations.

---

### 4. Data Transfer Objects (DTOs) & AutoMapper vs. TS Interfaces
> *"Coming from TypeScript, I'm used to strong typing using `interface` or `type` aliases. In TypeScript, to strip sensitive response fields, you might manually destructure or pick properties. In C#, industry standard calls for DTO classes (`TodoCreateDTO`, `TodoUpdateDTO`, `TodoDTO`) paired with AutoMapper to separate domain entity models from public API contracts."*

- **Domain Model**: `Todo.cs` (contains database metadata, constraints).
- **Data Transfer Objects**: `TodoCreateDTO` (with strict validation like `[Required(AllowEmptyStrings = false)]`), `TodoUpdateDTO`, `TodoDTO`.
- **AutoMapper**: Maps between entities and DTOs seamlessly without repetitive manual assignment code.

---

### 5. Route Metadata & Response Declarations (`Produces`, `Accepts`)
> *"Declarative metadata like `.Produces<TodoDTO>(201)`, `.ProducesProblem(400)`, and `.Accepts<DTO>()` felt a bit verbose at first. However, unlike compile-time-only TypeScript types, these C# annotations stay intact at runtime to feed OpenAPI/Scalar for automatic, interactive documentation."*

- OpenAPI integration via Scalar (`app.MapScalarApiReference()`).
- Strong typing for request inputs and response schemas available at runtime via reflection.

---

### 6. HTTP Results & C# Pattern Matching (`is null`)
> *"In TypeScript, null checks are written as `if (!todo)` or `if (todo === null)`. In C#, writing readable pattern matching like `if (todo is null)` felt very natural and English-like. Also, Minimal APIs return typed `IResult` objects like `Results.Ok()` and `Results.NotFound()` for clean response handling."*

- Expressive C# pattern matching: `if (todo is null) return Results.NotFound();`
- Consistent HTTP status responses using `Results.*` factories.

---

### 7. Embracing OOP, C# Classes, & Access Modifiers
> *"Even though TypeScript supports OOP, I often stayed away from class-heavy patterns in web dev. Writing C# forced me to embrace Object-Oriented Programming (OOP), access modifiers (`public`, `protected`, `static`), and class-based schema definitions. Learning C# class design and EF Core ORM mappings provides strong runtime and compile-time safety."*

---

## 🚀 Tech Stack

- **Framework**: ASP.NET Core Minimal APIs (.NET 10)
- **Database**: PostgreSQL (Npgsql Entity Framework Core)
- **ORM & Migrations**: Entity Framework Core
- **Validation**: `MinimalApis.Extensions` (DataAnnotations parameter validation)
- **Object Mapper**: AutoMapper
- **API Documentation**: Scalar.AspNetCore & Microsoft.AspNetCore.OpenApi

---

## 💻 API Endpoints

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/` | Retrieve all todo items |
| `GET` | `/{id}` | Retrieve a single todo item by ID |
| `POST` | `/` | Create a new todo item (Requires non-empty title) |
| `PATCH` | `/{id}` | Update an existing todo item |
| `DELETE` | `/{id}` | Delete a todo item by ID |
| `DELETE` | `/` | Delete all todo items |

---

## 🛠️ How to Run Locally

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd todo-api
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Configure Database Connection**:
   Update `ConnectionStrings:DatabaseConnection` in `appsettings.json` or via User Secrets:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DatabaseConnection" "Host=localhost;Database=tododb;Username=postgres;Password=yourpassword"
   ```

4. **Run Database Migrations**:
   ```bash
   dotnet ef database update
   ```

5. **Start the API**:
   ```bash
   dotnet run
   ```
   Open `http://localhost:5088/scalar/` (or `https://localhost:7102/scalar/`) to view interactive Scalar API docs!
