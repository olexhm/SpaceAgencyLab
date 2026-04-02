# DevOps Space Agency (Lab)

The lab requirement was to build one app with:
- an external API
- CRUD
- a frontend
- a database

## Lab includes

- Frontend: Blazor WebAssembly (`DevOpsSpaceAgency.Web`)
- Backend: ASP.NET Core Web API (`DevOpsSpaceAgency.Api`)
- Business layer: service/use-case logic (`DevOpsSpaceAgency.BLL`)
- Data layer: EF Core + SQL Server (`DevOpsSpaceAgency.DAL`)
- Shared contracts: DTOs used by API and frontend (`DevOpsSpaceAgency.Shared`)
- Tests: xUnit tests (`DevOpsSpaceAgency.Test`)

Main implemented features:
- Astronaut CRUD (create, read, update, delete)
- ISS crew sync from an external API
- ISS current position lookup
- NASA Astronomy Picture of the Day
- Simulated module status checks

## External APIs used

- `http://api.open-notify.org/` (ISS crew and ISS position)
- `https://api.nasa.gov/` (APOD)
- `https://httpstat.us/` (simulated module health states)

The frontend calls only the local API, not external services directly.

## Stack

- .NET 10
- ASP.NET Core Web API
- Blazor WebAssembly
- Entity Framework Core
- SQL Server (Docker)
- MudBlazor

## Run locally

Requirements:
- .NET 10 SDK
- Docker

Start SQL Server:
```bash
docker compose up -d
```

Run the API:
```bash
dotnet run --project DevOpsSpaceAgency.Api --launch-profile https
```

Run the frontend:
```bash
dotnet run --project DevOpsSpaceAgency.Web --launch-profile https
```

Default local URLs:
- API: `https://localhost:7193` (OpenAPI UI at `/scalar` in Development)
- Web: `https://localhost:7273`
