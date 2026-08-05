# Folder Structure

## Purpose

This document defines the repository structure for ProcureFlow.

The goal is to keep the solution organized, scalable, and easy to navigate as the application grows.

---

# Repository Structure

```text
ProcureFlow/
│
├── docs/
│   ├── 01-prd.md
│   ├── 02-domain-model.md
│   ├── 03-database-design.md
│   ├── 04-api-specification.md
│   ├── 05-architecture.md
│   ├── 06-development-guidelines.md
│   ├── 07-folder-structure.md
│   └── 08-roadmap.md
│
├── src/
│   ├── ProcureFlow.Api/
│   ├── ProcureFlow.Application/
│   ├── ProcureFlow.Domain/
│   ├── ProcureFlow.Infrastructure/
│   └── ProcureFlow.Shared/
│
├── tests/
│   ├── ProcureFlow.UnitTests/
│   ├── ProcureFlow.IntegrationTests/
│   └── ProcureFlow.ArchitectureTests/
│
├── frontend/
│   ├── public/
│   ├── src/
│   ├── package.json
│   └── vite.config.ts
│
├── docker/
│   ├── api/
│   ├── database/
│   └── docker-compose.yml
│
├── .github/
│   └── workflows/
│
├── README.md
├── LICENSE
├── .gitignore
└── ProcureFlow.sln
```

---

# Backend Structure

## ProcureFlow.Api

```text
ProcureFlow.Api/
│
├── Controllers/
├── Middleware/
├── Extensions/
├── Filters/
├── Configurations/
├── Authorization/
├── appsettings.json
├── Program.cs
└── ProcureFlow.Api.csproj
```

---

## ProcureFlow.Application

```text
Application/
│
├── Features/
│
│   ├── Authentication/
│   │
│   ├── Departments/
│   │
│   ├── Users/
│   │
│   ├── PurchaseRequests/
│   │
│   ├── Approval/
│   │
│   ├── Vendors/
│   │
│   ├── PurchaseOrders/
│   │
│   ├── Notifications/
│   │
│   └── Dashboard/
│
├── Behaviors/
├── Interfaces/
├── Common/
├── DTOs/
├── Exceptions/
├── Mappings/
└── DependencyInjection.cs
```

Each feature follows the same structure.

Example:

```text
PurchaseRequests/
│
├── Commands/
│
├── Queries/
│
├── DTOs/
│
├── Validators/
│
├── Services/
│
└── Mapping/
```

---

## ProcureFlow.Domain

```text
Domain/
│
├── Entities/
├── Enums/
├── ValueObjects/
├── Events/
├── Specifications/
├── Exceptions/
└── Interfaces/
```

---

## ProcureFlow.Infrastructure

```text
Infrastructure/
│
├── Persistence/
│   ├── Configurations/
│   ├── Migrations/
│   └── Seed/
│
├── Identity/
│
├── Repositories/
│
├── Services/
│
├── Logging/
│
├── Email/
│
├── Storage/
│
└── DependencyInjection.cs
```

---

## ProcureFlow.Shared

```text
Shared/
│
├── Constants/
├── Helpers/
├── Results/
├── Pagination/
└── Extensions/
```

---

# Test Structure

```text
tests/
│
├── ProcureFlow.UnitTests/
│
├── ProcureFlow.IntegrationTests/
│
└── ProcureFlow.ArchitectureTests/
```

---

# Frontend Structure

```text
frontend/
│
├── public/
│
├── src/
│   │
│   ├── api/
│   ├── assets/
│   ├── components/
│   ├── features/
│   ├── hooks/
│   ├── layouts/
│   ├── pages/
│   ├── routes/
│   ├── services/
│   ├── stores/
│   ├── styles/
│   ├── types/
│   ├── utils/
│   └── App.tsx
│
├── package.json
└── vite.config.ts
```

---

# Documentation Structure

```text
docs/
│
├── 01-prd.md
├── 02-domain-model.md
├── 03-database-design.md
├── 04-api-specification.md
├── 05-architecture.md
├── 06-development-guidelines.md
├── 07-folder-structure.md
└── 08-roadmap.md
```

---

# Future Expansion

As ProcureFlow grows, additional modules may include:

- Budget Management
- Cost Centers
- Contracts
- Assets
- Workflow Designer
- Vendor Portal
- Analytics
- AI Services
- Multi-Tenancy