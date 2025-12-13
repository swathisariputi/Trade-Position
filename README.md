# Trade & Position Management Service

## Overview
This project is a **RESTful Trade and Position Management service** built using **ASP.NET Core** and **SQL Server**.  
It allows clients to submit trades, persist them in a database, and calculate positions at the **Account–Asset** level.

Service has endpoint to send trade details to the service.
Service stores trades info and calculates position.
Service has endpoints to:
- retrieve trades
- retrieve position details

The primary goal of this project is to demonstrate:
- Clean REST API design
- Proper backend layering (Controller, Service, Repository)
- Domain-driven trade and position calculations
- Database storage (no in-memory storage)

This solution was developed as part of a **coding assessment** with a strong emphasis on **REST services, C#, SQL, and business logic**.

## Key Features
- Create, update, and retrieve trades
- Automatic position calculation based on trades
- Position aggregation by **Account + Asset**
- SQL Server database for storage.
- RESTful endpoints with clear HTTP semantics
- Swagger support for API exploration

## Tech Stack
- **.NET / ASP.NET Core**
- **Rest Services**
- **SQL Server**
- **Swagger (Swashbuckle)**
- **C#**


## Domain Concepts

### Trade
A trade represents a single execution in the market.

**Key attributes:**
- TradeId (Identity, auto-generated)
- Account
- Asset
- TradeType (BUY / SELL)
- Quantity
- Price
- TradeTimestamp


### Position
A position represents the **aggregated state of holdings** for an Account–Asset pair, derived from all trades.

**Position is NOT directly input by the user** — it is calculated based on trades.

**Key attributes:**
- PositionId (Identity, auto-generated)
- Account
- Asset
- NetQuantity
- AveragePrice
- RealisedPnL
- NotionalValue
- Position Status
- LastUpdated

## Position Calculation Logic
Positions are calculated by aggregating all trades for a given **Account + Asset**:
- BUY trades increase quantity
- SELL trades decrease quantity
- NetQuantity = TotalBuyQuantity − TotalSellQuantity
- AveragePrice is calculated using a weighted average for BUY trades
- Market price is intentionally excluded (not available)

This ensures:
- Idempotent position calculation
- Accurate recalculation when trades are updated


## API Endpoints

### Trade APIs
| Method | Endpoint | Description |
|------|---------|-------------|
| POST | `/api/v1/add/trade` | Insert a new trade |
| PUT | `/api/v1/update/trade` | Update an existing trade |
| GET | `api/v1/trades` | Retrieve all trades |

Sample PostMan Requests:
<img width="1773" height="702" alt="image" src="https://github.com/user-attachments/assets/3ae5cda6-f6cd-4e70-92c0-209b4cd111df" />
<img width="1783" height="722" alt="image" src="https://github.com/user-attachments/assets/bd7cd0da-e7d0-4600-8a2f-da97aef8d85e" />
<img width="1787" height="865" alt="image" src="https://github.com/user-attachments/assets/29438e85-970f-4d48-b909-bd0c0738ac91" />


### Position APIs
| Method | Endpoint | Description |
|------|---------|-------------|
| GET | `/api/v1/positions` | Retrieve all positions |
| GET | `/api/v1/positions/{account}/{asset}` | Retrieve position for an account and asset |
<img width="1797" height="892" alt="image" src="https://github.com/user-attachments/assets/e5b4d10a-1738-4671-91da-2b699dc01474" />
<img width="1819" height="863" alt="image" src="https://github.com/user-attachments/assets/fbe4b670-ce54-4875-85b1-c3470ffce81f" />


## Project Structure
Trade-Position
│
├── Contants
│ └── DBConstants.cs
│
├── Controllers
│ └── TradeController.cs
│
├── Interfaces
│ └── ITradeRepository.cs
│
├── Models
│ ├── Trade.cs
│ └── Position.cs
│
├── Repositories
│ └── TradeRepository.cs
│
├── Scripts
│ └── .sql files
│
├── Services
│ └── TradeService.cs
│
├── Repositories
│ └── TradeRepository.cs
│
└── Program.cs

## How to Run

1. Clone the repository: git clone https://github.com/swathisariputi/Trade-Position.git
2. Checkout to develop-2025-dec branch
3. Update SQL Server connection string in TradeRepository
4. Run Database scripts on your server
5. Run the application
6. It launches the local host UI in the browser: https://localhost:{port}/swagger
7. Now hit the APIs to validate
<img width="1918" height="996" alt="image" src="https://github.com/user-attachments/assets/e563aa97-5747-4613-ab9b-d59dcc4b9ca6" />
