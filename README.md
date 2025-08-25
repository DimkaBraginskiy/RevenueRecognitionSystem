# Revenue Recognition System

A REST API for managing clients, contracts, and revenue recognition in compliance with financial regulations.

##  Table of Contents
- [Features](#-features)
- [API Endpoints](#-api-endpoints)
- [Authentication](#-authentication)
- [Installation](#-installation)
- [Usage Examples](#-usage-examples)
- [Business Rules](#-business-rules)

##  Features

- **JWT Authentication** - Secure user registration and login
- **Client Management** - CRUD operations for individual/corporate clients
- **Revenue Tracking** - Current and predicted revenue calculations
- **Contract Management** - Handle contracts and payments
- **Soft Delete** - Safe deletion of individual client records

##  API Endpoints

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Auth/register` | Register new user |
| POST | `/api/Auth/login` | Login and receive JWT token |

### Clients
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Clients` | Get all clients |
| GET | `/api/Clients/(id)` | Get specific client |
| POST | `/api/Clients` | Create new client |
| PUT | `/api/Clients/(id)` | Update client data |
| DELETE | `/api/Clients/(id)` | Delete client |

### Contracts & Revenue
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Contracts` | Create new contract |
| POST | `/api/Contracts/payments` | Register payment |
| GET | `/api/Revenue/current` | Get current revenue |
| GET | `/api/Revenue/predicted` | Get predicted revenue |

##  Authentication

!All endpoints except Auth ones require JWT token received during Login!
