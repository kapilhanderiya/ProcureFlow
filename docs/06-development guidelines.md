# Development Guidelines

## Purpose

This document defines the coding standards, development workflow, and engineering conventions for ProcureFlow.

All contributors should follow these guidelines to ensure consistency, maintainability, and code quality.

---

# General Principles

- Write readable code before clever code.
- Prefer composition over inheritance.
- Keep methods small and focused.
- Follow SOLID principles.
- Avoid premature optimization.
- Favor explicit code over hidden magic.

---

# Naming Conventions

## Classes

Use PascalCase.

Examples

```csharp
PurchaseRequest
PurchaseOrderService
CreateVendorCommand
```

---

## Methods

Use PascalCase.

Methods should describe an action.

```csharp
CreatePurchaseRequest()
ApproveRequest()
GeneratePurchaseOrder()
```

---

## Variables

Use camelCase.

```csharp
purchaseRequest
totalAmount
currentUser
```

---

## Interfaces

Prefix with `I`.

```csharp
IApplicationDbContext
ITokenService
IEmailService
```

---

## Enums

Use PascalCase.

```csharp
PurchaseRequestStatus
ApprovalStatus
Priority
```

---

# API Conventions

- Use nouns for resource names.
- Use plural endpoints.

Good

```text
/api/v1/purchase-requests
```

Bad

```text
/api/v1/createPurchaseRequest
```

---

# Validation

Use FluentValidation.

Never place validation logic inside controllers.

---

# Business Logic

Business rules belong in the Application layer.

Controllers should only:

- Receive requests
- Call application services
- Return responses

---

# Database

- Never write raw SQL unless necessary.
- Prefer LINQ.
- Use transactions for multi-step operations.
- Always use async methods.

---

# Error Handling

Use global exception middleware.

Do not return exception messages directly to clients.

---

# Logging

Log:

- Authentication events
- Approval actions
- Purchase Order generation
- Vendor updates
- Unexpected exceptions

Never log:

- Passwords
- JWT tokens
- Sensitive personal information

---

# Git Workflow

Main branches:

```text
main
develop
```

Feature branches:

```text
feature/authentication
feature/purchase-requests
feature/vendor-management
```

Bug fixes:

```text
bugfix/login-refresh-token
```

---

# Commit Message Convention

Examples

```text
feat: add purchase request creation

fix: resolve approval workflow bug

refactor: simplify notification service

docs: update architecture guide

test: add purchase request unit tests
```

---

# Code Reviews

Every pull request should verify:

- Business rules are correct.
- Validation exists.
- Authorization is enforced.
- Logging is adequate.
- No duplicated code.
- Tests pass.

---

# Testing Strategy

### Unit Tests

Focus on:

- Business rules
- Approval engine
- Validation
- Services

### Integration Tests

Focus on:

- Database
- Authentication
- API endpoints

### Future

- End-to-end testing
- Load testing
- Security testing

---

# Definition of Done

A feature is complete only if:

- Code is implemented.
- Validation exists.
- Authorization exists.
- Tests pass.
- Swagger is updated.
- Documentation is updated.
- Logging is implemented.
- No critical warnings remain.

---

# Engineering Philosophy

ProcureFlow is built as if it were a real enterprise SaaS product.

Every design decision should prioritize:

- Maintainability
- Scalability
- Security
- Readability
- Testability