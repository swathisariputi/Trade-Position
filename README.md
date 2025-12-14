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
- **API Versioning**
- **Fluent Validators**


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
| POST | `/api/V1/AddTrade` | Insert a new trade |
| PUT | `/api/v1/UpdateTrade/{TraceId}` | Update an existing trade |
| GET | `api/v1/trades` | Retrieve all trades |
| GET | `api/v1/trades/{Account}/{Asset}`  | Retrieve trades for an asset in account 

**Sample PostMan Requests:**

curl -X 'POST' \
  'https://localhost:7091/api/V1/AddTrade' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "tradeid": 9,
  "asset": "test",
  "account": "782973013029",
  "tradeType": 0,
  "quantity": 10,
  "price": 2574
}'

curl -X 'PUT' \
  'https://localhost:7091/api/V1/UpdateTrade/9' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "tradeid": 9,
  "asset": "test",
  "account": "782973013029",
  "tradeType": 1,
  "quantity": 5,
  "price": 6000
}'

curl -X 'GET' \
  'https://localhost:7091/api/V1/trades' \
  -H 'accept: text/plain'

curl -X 'GET' \
  'https://localhost:7091/api/V1/trades/782973013029/test' \
  -H 'accept: text/plain'

### Position APIs
| Method | Endpoint | Description |
|------|---------|-------------|
| GET | `/api/v1/positions` | Retrieve all positions |
| GET | `/api/v1/positions/{account}/{asset}` | Retrieve position for an account and asset |

**Sample PostMan Requests:**

curl -X 'GET' \
  'https://localhost:7091/api/V1/positions' \
  -H 'accept: text/plain'
  
curl -X 'GET' \
  'https://localhost:7091/api/V1/positions/782973013029/test' \
  -H 'accept: text/plain'


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
│ ├── ITradeRepository.cs
│ └── ITradeService.cs
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

## Installed libraries
1. Microsoft.Data.SqlClient
2. Swashbuckle.AspNetCore
3. FluentValidation
4. Asp.Versioning.Mvc

## How to Run

1. Clone the repository: git clone https://github.com/swathisariputi/Trade-Position.git
2. Checkout to develop-2025-dec branch
3. Update SQL Server connection string in TradeRepository
4. Run Database scripts on your server
5. Install the dependent nuget packages 
6. Run the application (in Visual Studio or dotnet run)
7. It launches the local host UI in the browser: https://localhost:{port}/swagger
8. Now hit the APIs to validate
<img width="1919" height="963" alt="image" src="https://github.com/user-attachments/assets/e92e8aec-7d49-43bf-83de-56c05f90e512" />

