# Domain Model

## Purpose

The Domain Model defines the core business entities of ProcureFlow and how they interact.

Unlike the database design, this document focuses on business concepts rather than implementation details. It serves as the foundation for the database schema, API design, and application architecture.

---

# Core Domain

The procurement lifecycle revolves around a Purchase Request submitted by an employee and processed through configurable approval stages before resulting in a Purchase Order.

```text
Employee
    │
    ▼
Purchase Request
    │
    ▼
Approval Workflow
    │
    ▼
Approval Steps
    │
    ▼
Purchase Order
    │
    ▼
Goods Receipt
    │
    ▼
Invoice
```

---

# Domain Entities

## Organization

Represents the company using ProcureFlow.

### Responsibilities

- Owns departments
- Owns employees
- Owns vendors
- Owns approval workflows

---

## Department

Represents a business department.

Examples:

- Engineering
- Human Resources
- Finance
- Procurement
- Marketing

### Responsibilities

- Contains employees
- Has a department manager
- Owns purchase requests
- Tracks department spending

---

## User

Represents a system user.

### Responsibilities

- Authenticate
- Submit requests
- Approve requests
- Manage procurement activities
- Access dashboards

### Roles

- Employee
- Manager
- Finance
- Procurement
- Administrator

---

## Purchase Request

The central entity of the system.

Represents a request to purchase goods or services.

### Responsibilities

- Store requested items
- Store business justification
- Track lifecycle
- Track approvals
- Generate purchase order after approval

### Lifecycle

```text
Draft
Submitted
Under Review
Approved
Rejected
Cancelled
Completed
```

---

## Purchase Request Item

Represents an individual item inside a purchase request.

A request may contain multiple items.

Example:

Purchase Request

- Laptop
- Docking Station
- Mouse

---

## Approval Workflow

Defines how approvals should occur.

The workflow is configurable.

It is **not hardcoded**.

Examples

Engineering

Employee

↓

Manager

↓

Finance

↓

Procurement

Marketing

Employee

↓

Director

↓

Finance

↓

Procurement

---

## Approval Step

Represents one approval stage.

Examples

- Manager Approval
- Finance Validation
- Procurement Review

Each step records:

- Assigned approver
- Status
- Comments
- Decision date

---

## Vendor

Represents an external supplier.

### Responsibilities

- Supply goods
- Receive purchase orders
- Maintain ratings
- Maintain categories

---

## Vendor Category

Classifies vendors.

Examples

- Hardware
- Software
- Furniture
- Consulting
- Networking
- Cloud Services

---

## Purchase Order

Created automatically after approval.

Contains:

- PO Number
- Vendor
- Total Amount
- Status

Purchase Orders cannot exist without an approved Purchase Request.

---

## Goods Receipt

Represents confirmation that ordered goods have been received.

Records:

- Received By
- Quantity Received
- Delivery Date

---

## Invoice

Represents the supplier invoice.

Linked to

- Purchase Order
- Vendor

Stores

- Invoice Number
- Invoice Date
- Amount
- Payment Status

---

## Notification

Represents user notifications.

Examples

- Request approved
- Request rejected
- Reminder
- Escalation

Delivery Channels

- In-App
- Email (Future)

---

## Audit Log

Stores every important business action.

Examples

- User Login
- Request Created
- Request Submitted
- Approval Granted
- Vendor Updated
- Purchase Order Generated

Audit logs are immutable.

---

# Relationships

```text
Organization
│
├── Departments
│      │
│      └── Users
│
├── Vendors
│
└── Approval Workflows


Department
│
├── Users
└── Purchase Requests


User
│
├── Purchase Requests (Created)
├── Approval Steps (Approved)
└── Notifications


Purchase Request
│
├── Purchase Request Items
├── Approval Workflow
├── Purchase Order
└── Audit Logs


Approval Workflow
│
└── Approval Steps


Purchase Order
│
├── Vendor
├── Goods Receipt
└── Invoice
```

---

# Business Rules

## Purchase Requests

- A request must have at least one item.
- Only the creator may edit a draft.
- Submitted requests become read-only.
- Cancelled requests cannot be approved.
- Approved requests automatically generate a Purchase Order.

---

## Approval Rules

- Approval order is configurable.
- Users cannot approve their own requests.
- A rejected request ends the workflow.
- Returned requests may be edited and resubmitted.
- Every approval requires a timestamp.

---

## Vendor Rules

- Vendors may belong to multiple categories.
- Inactive vendors cannot receive new Purchase Orders.

---

## Purchase Orders

- Every Purchase Order belongs to exactly one Purchase Request.
- PO numbers must be unique.
- Purchase Orders cannot be deleted.

---

## Audit Rules

The following actions must always generate audit records:

- Login
- Logout
- Request Created
- Request Updated
- Request Submitted
- Approval
- Rejection
- Vendor Changes
- Purchase Order Generation
- Goods Receipt
- Invoice Verification

Audit records are immutable.

---

# Future Extensions

The current domain model is intentionally designed to support future enhancements without significant architectural changes.

Potential future capabilities include:

- Multi-company support
- Budget management
- Cost centers
- Workflow designer
- AI vendor recommendations
- OCR invoice processing
- Email integrations
- Contract management
- Procurement analytics
- Spend forecasting