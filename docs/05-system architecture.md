# System Architecture

## Purpose

This document defines the architectural principles, project structure, and design decisions for ProcureFlow.

It serves as the blueprint for implementation and ensures that the application remains maintainable, scalable, testable, and extensible.

---

# Architecture Style

ProcureFlow follows **Clean Architecture** with **Domain-Driven Design (DDD)** principles.

The architecture separates business logic from infrastructure concerns, ensuring that business rules remain independent of frameworks, databases, and UI technologies.

Core principles:

- Separation of Concerns
- Dependency Inversion
- Single Responsibility
- Open/Closed Principle
- Explicit Boundaries
- Testability

---

# High-Level Architecture

```text
                   React Frontend
                          │
                          ▼
                  ASP.NET Core API
                          │
          ┌───────────────┴───────────────┐
          ▼                               ▼
    Application Layer               Infrastructure Layer
          │                               │
          ▼                               ▼
                  Domain Layer
                          │
                          ▼
                    SQL Server
```

---

# Solution Structure

```text
ProcureFlow.sln

src/
│
├── ProcureFlow.Api
│
├── ProcureFlow.Application
│
├── ProcureFlow.Domain
│
├── ProcureFlow.Infrastructure
│
└── ProcureFlow.Shared
```

---

# Project Responsibilities

## ProcureFlow.Api

Responsible for:

- Controllers
- Authentication
- Authorization
- Dependency Injection
- Swagger
- Middleware
- Request/Response Mapping

References:

- Application

Does NOT contain:

- Business logic
- SQL queries

---

## ProcureFlow.Application

Contains application use cases.

Responsible for:

- Commands
- Queries
- DTOs
- Validators
- Interfaces
- Services
- Mapping Profiles
- Business workflows

References:

- Domain

Does NOT reference Infrastructure.

---

## ProcureFlow.Domain

Contains pure business logic.

Responsible for:

- Entities
- Value Objects
- Domain Events (future)
- Enums
- Business Rules
- Interfaces (where appropriate)

Contains no EF Core, no ASP.NET Core, and no external dependencies.

---

## ProcureFlow.Infrastructure

Responsible for:

- Entity Framework Core
- SQL Server
- Authentication implementation
- File storage
- Email services
- Logging
- Repository implementations
- External integrations

References:

- Application
- Domain

---

## ProcureFlow.Shared

Contains shared components.

Examples:

- Constants
- Common Exceptions
- Result Wrapper
- Pagination Models
- Shared Utilities

---

# Request Lifecycle

```text
HTTP Request
      │
      ▼
Controller
      │
      ▼
Application Service
      │
      ▼
Domain Model
      │
      ▼
Repository
      │
      ▼
Database
```

---

# Dependency Rules

Dependencies only flow inward.

```text
API
 │
 ▼
Application
 │
 ▼
Domain

Infrastructure
 │
 ▼
Application
 │
 ▼
Domain
```

The Domain project must never depend on any other project.

---

# Design Patterns

## Repository Pattern

Repositories abstract persistence.

Example:

```csharp
IPurchaseRequestRepository
```

Implementation:

```text
Infrastructure
```

---

## Unit of Work

Coordinates multiple repository operations within a single transaction.

---

## Dependency Injection

All dependencies are injected through constructors.

No service locator pattern.

---

## Result Pattern

Application services return a standard result type.

Example

```csharp
Result<T>
```

Instead of throwing exceptions for expected business failures.

---

## FluentValidation

Validation occurs before business logic executes.

Example:

```
CreatePurchaseRequestValidator
```

---

## AutoMapper

Responsible only for mapping.

Never place business logic inside mapping profiles.

---

# Folder Structure

## API

```text
Controllers/
Middleware/
Extensions/
Configurations/
```

---

## Application

```text
Features/

    Authentication/

    Departments/

    Users/

    PurchaseRequests/

    Vendors/

    PurchaseOrders/

    Approval/

    Dashboard/

Common/

DTOs/

Interfaces/

Validators/

Mappings/
```

---

## Domain

```text
Entities/

Enums/

ValueObjects/

Events/

Interfaces/

Specifications/
```

---

## Infrastructure

```text
Persistence/

Configurations/

Repositories/

Identity/

Services/

Migrations/

Logging/

Email/
```

---

# Exception Handling

Global exception middleware handles:

- ValidationException
- UnauthorizedException
- ForbiddenException
- NotFoundException
- ConflictException
- Unexpected Exceptions

No controller should contain try/catch blocks for business logic.

---

# Logging

Serilog will be used.

Log levels:

- Information
- Warning
- Error
- Critical

Sensitive information must never be logged.

---

# Authentication

JWT Access Tokens

Refresh Tokens

Password Hashing

Authorization Policies

Permission-Based Authorization

---

# Authorization

The application uses Permission-Based Authorization.

Instead of checking:

```text
Role == Admin
```

The application checks:

```text
Permission = Vendor.Create
```

Roles merely group permissions.

---

# Entity Framework

Code-First Migrations

Fluent API configurations

Lazy Loading disabled

AsNoTracking() for read-only queries

Projection for DTOs

---

# Caching

Initial MVP

No caching

Future

Redis

---

# Background Processing

Future

Background jobs will handle:

- Notifications
- Email sending
- Escalations
- Scheduled reminders

Potential libraries

- Hangfire
- Quartz.NET

---

# File Storage

MVP

Local storage

Future

Azure Blob Storage

---

# API Documentation

Swagger/OpenAPI

Development only.

---

# Security

- HTTPS only
- JWT Authentication
- Password Hashing
- Input Validation
- SQL Injection Protection
- XSS Protection
- CSRF protection where applicable
- Audit Logging

---

# Performance Guidelines

- Pagination everywhere
- Async database access
- Database indexes
- DTO projection
- Avoid N+1 queries

---

# Coding Standards

- One class per file.
- One public type per file.
- Constructor injection only.
- Meaningful method names.
- Business logic never inside controllers.
- Business logic never inside EF configurations.
- Keep methods focused and small.

---

# Future Architecture Evolution

The architecture is designed to support:

- CQRS using MediatR
- Event-driven architecture
- Domain Events
- Redis caching
- Multi-tenancy
- Microservices (if required)
- Azure deployment
- Message queues
- AI integrations

---

# Architectural Decision Summary

| Decision | Choice |
|----------|--------|
| Architecture | Clean Architecture |
| Language | C# |
| Framework | ASP.NET Core .NET 8 |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Authentication | JWT + Refresh Tokens |
| Authorization | Permission-Based RBAC |
| Logging | Serilog |
| Validation | FluentValidation |
| Mapping | AutoMapper |
| API Documentation | Swagger |
| Frontend | React + TypeScript |
| Styling | Tailwind CSS |
| Data Fetching | TanStack Query |