
## The “TaekwondoRanking” Project

**TaekwondoRanking** is a web application for managing and ranking Taekwondo athletes and competitions. It is built using **ASP.NET Core MVC** as a comprehensive platform for event tracking, athlete performance analysis, and live rankings.

## :pencil2: Overview

**TaekwondoRanking** allows users to manage athlete data, tournament structures, and ranking filters with a robust backend and responsive frontend.

* **All users** can view rankings and athlete histories.
* **Authorized users** (non-admins) can view more detailed information and access their own competition data.
* **Admins** have the ability to manage athlete records, handle temporarily deleted athletes, manage regions, competitions, and view all data.
* The app supports **live search**, filtering by country, continent, and world level.
* AJAX functionality is implemented for dynamic, client-side updates without full page reloads.
* Rankings and history pages provide detailed statistics on athlete performance.

## :performing_arts: User Types

**Administrator**
* Manage all athlete and tournament records.
* View temporarily deleted athletes.
* Access administrative pages for region and competition management.

**Authorized User**
* View rankings and competition details.
* Search athletes and view their histories.
* Cannot modify data.

**Public User**
* View public pages like home, about, and general rankings.
* Can view tournament summaries and basic athlete info.

## :hammer: Built With

* ASP.NET [Core MVC]
    - **6 controllers** (including API controllers for athlete and tournament AJAX)
    - **15+ entity models** for Athletes, Competitions, Categories, Results, Regions, etc.
    - **Razor Views** for all major pages (Index, Rankings, Details, Error pages)
    - **Services layer** to encapsulate business logic (AthleteService, RegionService, CompetitionService)
    - ViewModels and helper classes for structured data transfer
* ASP.NET Core Identity
    - Used for authentication and role management
    - Admin seeding and account login/register
* Entity Framework Core
    - Using code-first migrations
    - Seed data for Identity setup
* AutoMapper
    - Mapping between domain models and ViewModels
* Razor Pages and MVC mixed architecture
* Bootstrap + jQuery for frontend
    - Responsive layout
    - Modal forms and interactive UI components
* AJAX support
    - API endpoints for dynamic athlete and tournament interactions
* Custom error pages
    - Including 401 Unauthorized and access denied pages
* MVC Areas
    - Identity area for account management
* In-memory handling of temporary deletions
* TempData for notification messages
* Dependency Injection used throughout for service layers

## :clipboard: Tests Coverage

* **xUnit**
    - Unit tests cover athlete retrieval logic, region filtering, and temporary deletion restoration
    - Tested using mocked data sets and service method assertions

## :wrench: DB Diagram

* **ApplicationDbContext** defines relationships across:
    - `Athlete`, `Competition`, `Result`, `Country`, `Region`, `Category`, `ApplicationUser`
* Relationships are handled via EF navigation properties
* Migrations are stored in the `Data/Migrations/` folder

## :rocket: Getting Started

1. Clone the repo and open in Visual Studio or your preferred IDE.
2. Run the following commands:
   ```bash
   dotnet restore
   dotnet ef database update
   dotnet run
   ```
3. Navigate to `https://localhost:5001` in your browser.

## :test_tube: Running Tests

```bash
cd TaekwondoRanking.Tests
dotnet test
```

---
