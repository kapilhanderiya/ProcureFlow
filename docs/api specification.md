# REST API Specification

## Purpose

This document defines the REST API contract for ProcureFlow.

It specifies:

- Endpoints
- Request formats
- Response formats
- Authentication requirements
- Validation rules
- Error responses

The API follows RESTful principles and uses JSON for request and response bodies.

---

# API Standards

## Base URL

```text
/api/v1
```

---

## Authentication

Authentication is performed using JWT Bearer Tokens.

Example

```http
Authorization: Bearer <access_token>
```

---

## Response Format

### Success

```json
{
  "success": true,
  "message": "Purchase request created successfully.",
  "data": {}
}
```

---

### Error

```json
{
  "success": false,
  "message": "Validation failed.",
  "errors": [
    {
      "field": "estimatedCost",
      "message": "Estimated cost must be greater than zero."
    }
  ]
}
```

---

# Authentication Module

## Login

POST

```text
/api/v1/auth/login
```

Request

```json
{
  "email": "john@example.com",
  "password": "Password123!"
}
```

Response

```json
{
  "accessToken": "...",
  "refreshToken": "...",
  "expiresIn": 3600
}
```

---

## Refresh Token

POST

```text
/api/v1/auth/refresh
```

---

## Logout

POST

```text
/api/v1/auth/logout
```

---

## Current User

GET

```text
/api/v1/auth/me
```

---

# Department Module

## Get Departments

GET

```text
/api/v1/departments
```

---

## Get Department

GET

```text
/api/v1/departments/{id}
```

---

## Create Department

POST

```text
/api/v1/departments
```

---

## Update Department

PUT

```text
/api/v1/departments/{id}
```

---

## Delete Department

DELETE

```text
/api/v1/departments/{id}
```

Soft delete only.

---

# User Module

## Get Users

GET

```text
/api/v1/users
```

Supports

- Search
- Pagination
- Sorting
- Filtering

Example

```text
/api/v1/users?page=1&pageSize=20&search=john
```

---

## Get User

GET

```text
/api/v1/users/{id}
```

---

## Create User

POST

```text
/api/v1/users
```

---

## Update User

PUT

```text
/api/v1/users/{id}
```

---

## Activate User

PATCH

```text
/api/v1/users/{id}/activate
```

---

## Deactivate User

PATCH

```text
/api/v1/users/{id}/deactivate
```

---

# Purchase Request Module

## Get Requests

GET

```text
/api/v1/purchase-requests
```

Supports

- Pagination
- Status filter
- Priority filter
- Department filter
- Search

---

## Get Request

GET

```text
/api/v1/purchase-requests/{id}
```

---

## Create Request

POST

```text
/api/v1/purchase-requests
```

Request

```json
{
  "justification": "Development laptops",
  "priority": "High",
  "requiredByDate": "2026-10-15",
  "items": [
    {
      "itemName": "Laptop",
      "quantity": 5,
      "unitPrice": 65000
    }
  ]
}
```

---

## Update Draft

PUT

```text
/api/v1/purchase-requests/{id}
```

Only allowed while status is Draft.

---

## Submit Request

POST

```text
/api/v1/purchase-requests/{id}/submit
```

---

## Cancel Request

POST

```text
/api/v1/purchase-requests/{id}/cancel
```

---

## Get My Requests

GET

```text
/api/v1/purchase-requests/me
```

---

# Approval Module

## Get Pending Approvals

GET

```text
/api/v1/approvals/pending
```

---

## Approve Request

POST

```text
/api/v1/approvals/{id}/approve
```

Request

```json
{
  "comments": "Approved."
}
```

---

## Reject Request

POST

```text
/api/v1/approvals/{id}/reject
```

Request

```json
{
  "comments": "Budget exceeded."
}
```

---

## Return for Revision

POST

```text
/api/v1/approvals/{id}/return
```

---

# Vendor Module

## Get Vendors

GET

```text
/api/v1/vendors
```

---

## Get Vendor

GET

```text
/api/v1/vendors/{id}
```

---

## Create Vendor

POST

```text
/api/v1/vendors
```

---

## Update Vendor

PUT

```text
/api/v1/vendors/{id}
```

---

## Delete Vendor

DELETE

```text
/api/v1/vendors/{id}
```

Soft delete.

---

# Purchase Order Module

## Get Purchase Orders

GET

```text
/api/v1/purchase-orders
```

---

## Get Purchase Order

GET

```text
/api/v1/purchase-orders/{id}
```

---

## Generate Purchase Order

POST

```text
/api/v1/purchase-orders/{requestId}/generate
```

Normally generated automatically.

Manual generation is restricted to Procurement/Admin.

---

## Update Purchase Order Status

PATCH

```text
/api/v1/purchase-orders/{id}/status
```

---

# Dashboard Module

## Employee Dashboard

GET

```text
/api/v1/dashboard/employee
```

---

## Manager Dashboard

GET

```text
/api/v1/dashboard/manager
```

---

## Finance Dashboard

GET

```text
/api/v1/dashboard/finance
```

---

## Procurement Dashboard

GET

```text
/api/v1/dashboard/procurement
```

---

## Admin Dashboard

GET

```text
/api/v1/dashboard/admin
```

---

# Notification Module

## Get Notifications

GET

```text
/api/v1/notifications
```

---

## Mark as Read

PATCH

```text
/api/v1/notifications/{id}/read
```

---

## Mark All as Read

PATCH

```text
/api/v1/notifications/read-all
```

---

# Audit Module

## Get Audit Events

GET

```text
/api/v1/audit/events
```

Admin only.

Supports

- Entity
- User
- Date range

---

## Get Entity Changes

GET

```text
/api/v1/audit/events/{id}
```

---

# Common Query Parameters

Pagination

```text
?page=1&pageSize=20
```

Sorting

```text
?sort=createdAt
```

Descending

```text
?sort=-createdAt
```

Search

```text
?search=laptop
```

Filtering

```text
?status=Pending
```

Multiple filters

```text
?status=Pending&priority=High
```

---

# HTTP Status Codes

| Status | Meaning |
|---------|----------|
| 200 | Success |
| 201 | Created |
| 204 | No Content |
| 400 | Validation Error |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 409 | Conflict |
| 422 | Business Rule Violation |
| 500 | Internal Server Error |

---

# Validation Rules

Every endpoint must validate:

- Required fields
- String lengths
- Email format
- Numeric ranges
- Date ranges
- Duplicate values
- Business rules

Validation errors always return HTTP 400.

Business rule violations return HTTP 422.

---

# Versioning

The API is versioned using URL versioning.

```text
/api/v1
```

Future versions

```text
/api/v2
```

---

# OpenAPI

The REST API should expose Swagger/OpenAPI documentation.

Development

```text
/swagger
```

This document serves as the contract between the frontend and backend teams and should be updated whenever endpoints or payloads change.