# Database Design

## Purpose

This document defines the relational database design for ProcureFlow.

The schema is normalized to reduce redundancy while remaining practical for enterprise application development.

The design aims to provide:

- Data integrity
- Scalability
- Auditability
- Configurable workflows
- Efficient querying

---

# Design Principles

- Third Normal Form (3NF)
- Soft deletes where appropriate
- Immutable audit records
- GUID primary keys
- Foreign key constraints
- Optimistic concurrency support
- UTC timestamps
- Flexible Role-Based Access Control (RBAC)
- Configurable approval workflows

---

# High-Level Entity Relationship Diagram

```text
                        Department
                             │
                             │
                        ┌────▼────┐
                        │  Users  │
                        └────┬────┘
                             │
              ┌──────────────┼───────────────┐
              │              │               │
              ▼              ▼               ▼
      PurchaseRequest    Notifications   AuditEvents
              │
     ┌────────┴────────┐
     ▼                 ▼
PurchaseRequestItem ApprovalInstance
                           │
                           ▼
                    ApprovalStep
                           │
                           ▼
                    PurchaseOrder
                           │
             ┌─────────────┴─────────────┐
             ▼                           ▼
       GoodsReceipt                  Invoice

Vendor ─────────────► PurchaseOrder

Users ─────► UserRoles ◄──── Roles
                           │
                           ▼
                  RolePermissions
                           │
                           ▼
                     Permissions
```

---

# Core Tables

---

# Departments

Represents business departments.

| Column | Type | Notes |
|---------|------|------|
| Id | UNIQUEIDENTIFIER | PK |
| Name | NVARCHAR(100) | Unique |
| Description | NVARCHAR(500) | Nullable |
| ManagerId | UNIQUEIDENTIFIER | FK → Users |
| IsActive | BIT | |
| CreatedAt | DATETIME2 | UTC |
| UpdatedAt | DATETIME2 | UTC |

---

# Users

Represents employees and system users.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| FirstName | NVARCHAR(100) |
| LastName | NVARCHAR(100) |
| Email | NVARCHAR(255) |
| PasswordHash | NVARCHAR(MAX) |
| DepartmentId | UNIQUEIDENTIFIER |
| ManagerId | UNIQUEIDENTIFIER |
| IsActive | BIT |
| CreatedAt | DATETIME2 |
| UpdatedAt | DATETIME2 |

### Relationships

- Many Users belong to one Department.
- One User may manage many Users.
- One User may create many Purchase Requests.

> **Users do not contain a RoleId. Authorization is handled through RBAC.**

---

# Role-Based Access Control (RBAC)

## Roles

Represents business roles.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| Name | NVARCHAR(100) |
| Description | NVARCHAR(500) |

Example Roles

- Employee
- Manager
- Finance Officer
- Procurement Officer
- Administrator

---

## Permissions

Represents fine-grained system permissions.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| Name | NVARCHAR(100) |
| Description | NVARCHAR(500) |

Example Permissions

```text
PurchaseRequest.Create
PurchaseRequest.Edit
PurchaseRequest.View

Approval.Approve
Approval.Reject

Vendor.Create
Vendor.Update
Vendor.Delete

PurchaseOrder.Generate

Workflow.Configure

Audit.View
```

Permission Naming Convention

```text
<Resource>.<Action>
```

Examples

```text
Vendor.Create
PurchaseRequest.Approve
Workflow.Configure
Department.Manage
```

---

## UserRoles

Many-to-many relationship between Users and Roles.

| Column | Type |
|---------|------|
| UserId | UNIQUEIDENTIFIER |
| RoleId | UNIQUEIDENTIFIER |

Composite Primary Key

- UserId
- RoleId

---

## RolePermissions

Many-to-many relationship between Roles and Permissions.

| Column | Type |
|---------|------|
| RoleId | UNIQUEIDENTIFIER |
| PermissionId | UNIQUEIDENTIFIER |

Composite Primary Key

- RoleId
- PermissionId

---

# PurchaseRequests

Central business entity.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| RequestNumber | NVARCHAR(30) |
| RequestedById | UNIQUEIDENTIFIER |
| DepartmentId | UNIQUEIDENTIFIER |
| Status | INT |
| Priority | INT |
| TotalEstimatedCost | DECIMAL(18,2) |
| Justification | NVARCHAR(MAX) |
| RequiredByDate | DATE |
| SubmittedAt | DATETIME2 |
| CreatedAt | DATETIME2 |
| UpdatedAt | DATETIME2 |

---

# PurchaseRequestItems

Represents individual items within a Purchase Request.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| PurchaseRequestId | UNIQUEIDENTIFIER |
| ItemName | NVARCHAR(255) |
| Description | NVARCHAR(MAX) |
| Quantity | INT |
| UnitPrice | DECIMAL(18,2) |
| EstimatedTotal | DECIMAL(18,2) |

Relationship

- One Purchase Request has many Purchase Request Items.

---

# Vendors

Represents suppliers.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| Name | NVARCHAR(255) |
| Email | NVARCHAR(255) |
| Phone | NVARCHAR(50) |
| Address | NVARCHAR(MAX) |
| Rating | DECIMAL(2,1) |
| IsActive | BIT |
| CreatedAt | DATETIME2 |

---

# VendorCategories

Lookup table.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| Name | NVARCHAR(100) |

---

# VendorCategoryMappings

Many-to-many relationship.

| Column | Type |
|---------|------|
| VendorId | UNIQUEIDENTIFIER |
| CategoryId | UNIQUEIDENTIFIER |

Composite Primary Key

- VendorId
- CategoryId

---

# Configurable Approval Engine

The approval engine is entirely data-driven.

Approval chains are **never hardcoded**.

---

## ApprovalWorkflows

Workflow templates.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| Name | NVARCHAR(200) |
| DepartmentId | UNIQUEIDENTIFIER (Nullable) |
| MinimumAmount | DECIMAL(18,2) |
| MaximumAmount | DECIMAL(18,2) |
| IsActive | BIT |

Example

Engineering purchases

₹0 – ₹50,000

↓

Manager

↓

Finance

↓

Procurement

---

## ApprovalWorkflowSteps

Workflow template steps.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| WorkflowId | UNIQUEIDENTIFIER |
| StepOrder | INT |
| RoleId | UNIQUEIDENTIFIER |
| IsRequired | BIT |

---

## ApprovalInstances

Created when a Purchase Request is submitted.

Represents one execution of a workflow.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| PurchaseRequestId | UNIQUEIDENTIFIER |
| WorkflowId | UNIQUEIDENTIFIER |
| Status | INT |
| StartedAt | DATETIME2 |
| CompletedAt | DATETIME2 |

---

## ApprovalSteps

Actual approval records.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| ApprovalInstanceId | UNIQUEIDENTIFIER |
| WorkflowStepId | UNIQUEIDENTIFIER |
| ApproverId | UNIQUEIDENTIFIER |
| Status | INT |
| Comments | NVARCHAR(MAX) |
| ActionedAt | DATETIME2 |

---

# PurchaseOrders

Automatically generated after successful approval.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| PONumber | NVARCHAR(50) |
| PurchaseRequestId | UNIQUEIDENTIFIER |
| VendorId | UNIQUEIDENTIFIER |
| Status | INT |
| TotalAmount | DECIMAL(18,2) |
| CreatedAt | DATETIME2 |

---

# GoodsReceipts

Tracks received goods.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| PurchaseOrderId | UNIQUEIDENTIFIER |
| ReceivedById | UNIQUEIDENTIFIER |
| ReceivedDate | DATETIME2 |
| Notes | NVARCHAR(MAX) |

---

# Invoices

Vendor invoices.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| PurchaseOrderId | UNIQUEIDENTIFIER |
| InvoiceNumber | NVARCHAR(100) |
| InvoiceDate | DATE |
| Amount | DECIMAL(18,2) |
| PaymentStatus | INT |

---

# Notifications

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| UserId | UNIQUEIDENTIFIER |
| Title | NVARCHAR(255) |
| Message | NVARCHAR(MAX) |
| IsRead | BIT |
| CreatedAt | DATETIME2 |

---

# Audit Model

Instead of storing large JSON blobs in a single table, ProcureFlow separates business events from field-level changes.

---

## AuditEvents

Represents immutable business events.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| UserId | UNIQUEIDENTIFIER |
| EntityName | NVARCHAR(100) |
| EntityId | UNIQUEIDENTIFIER |
| Action | NVARCHAR(100) |
| Timestamp | DATETIME2 |
| IpAddress | NVARCHAR(45) |
| UserAgent | NVARCHAR(500) |

Examples

- Purchase Request Submitted
- Approval Granted
- Vendor Updated
- Purchase Order Generated

---

## EntityChanges

Stores property-level changes linked to an Audit Event.

| Column | Type |
|---------|------|
| Id | UNIQUEIDENTIFIER |
| AuditEventId | UNIQUEIDENTIFIER |
| PropertyName | NVARCHAR(100) |
| OldValue | NVARCHAR(MAX) |
| NewValue | NVARCHAR(MAX) |

Example

| Property | Old | New |
|----------|-----|-----|
| EstimatedCost | 45000 | 48000 |
| Priority | Medium | High |

---

# Enumerations

## PurchaseRequestStatus

- Draft
- Submitted
- PendingApproval
- Approved
- Rejected
- Cancelled
- Completed

---

## ApprovalStatus

- Pending
- Approved
- Rejected
- Skipped

---

## PurchaseOrderStatus

- Draft
- Issued
- Sent
- PartiallyReceived
- Completed
- Cancelled

---

## Priority

- Low
- Medium
- High
- Critical

---

# Indexing Strategy

## Unique Indexes

- Users.Email
- Roles.Name
- Permissions.Name
- PurchaseRequests.RequestNumber
- PurchaseOrders.PONumber

## Performance Indexes

- PurchaseRequests.Status
- PurchaseRequests.DepartmentId
- PurchaseRequests.RequestedById
- ApprovalSteps.ApproverId
- Notifications.UserId
- AuditEvents.EntityId
- AuditEvents.Timestamp

---

# Business Constraints

## Purchase Requests

- Must contain at least one item.
- Only the creator can edit drafts.
- Submitted requests become read-only.
- Cancelled requests cannot be approved.

---

## Approval Engine

- Approval order is configurable.
- Users cannot approve their own requests.
- Rejected requests terminate the workflow.
- Returned requests may be edited and resubmitted.
- Purchase Orders are generated only after successful completion of all required approval steps.

---

## Vendors

- Vendors may belong to multiple categories.
- Inactive vendors cannot receive new Purchase Orders.

---

## Purchase Orders

- One Purchase Order belongs to exactly one Purchase Request.
- Purchase Orders cannot exist without an approved Purchase Request.
- Purchase Order numbers must be unique.
- Purchase Orders are never physically deleted.

---

## Audit

- Audit events are immutable.
- Entity changes must always reference an Audit Event.
- Every approval, rejection, modification, and login action must generate an audit record.

---

# Future Database Extensions

The schema is intentionally designed for future growth.

Potential additions include:

- Multi-tenancy
- Cost Centers
- Budget Management
- File Attachments
- Contracts
- Invoice OCR
- Email Templates
- Currency Support
- Workflow Designer
- Spend Analytics
- AI Vendor Recommendations
- Forecasting