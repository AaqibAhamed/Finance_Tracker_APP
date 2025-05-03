# Personal Finance Tracker Demo Application

## Overview

This is a simple personal finance tracker application built as a full-stack solution for demonstration purposes. It allows users to manage their income, expenses, and view a basic financial summary.

## Technology Choices and Reasoning

* **Frontend:** Blazor WebAssembly (.NET 8)
    * Reasoning: Enables building interactive web UIs using C# instead of JavaScript, leveraging existing .NET skills. Blazor WebAssembly allows for client-side execution, providing a responsive user experience.
* **UI Library:** MudBlazor
    * Reasoning: A comprehensive and well-documented UI component library for Blazor, offering a rich set of pre-built components that follow Material Design principles. This accelerates UI development and provides a clean, modern look and feel with built-in responsiveness.
* **Backend:** ASP.NET Core Web API (.NET 8)
    * Reasoning: A robust and scalable framework for building RESTful APIs using C#. It integrates well with Entity Framework Core and provides excellent performance.
* **ORM:** Entity Framework Core (EF Core)
    * Reasoning: A modern object-relational mapper for .NET, simplifying database interactions by allowing developers to work with .NET objects instead of raw SQL.
* **Database:** SQLite
    * Reasoning: A lightweight, file-based database that requires no separate server installation. It's ideal for demos and small applications, making setup easy.

## Architecture

The application follows a standard three-tier architecture:

1.  **Presentation Tier (Frontend - Blazor WebAssembly):** Handles the user interface and user interactions. It consumes the API exposed by the backend.
2.  **Application Tier (Backend - ASP.NET Core Web API):** Contains the business logic and API endpoints. It interacts with the data access tier.
3.  **Data Tier (Database - SQLite, accessed via EF Core):** Manages the storage and retrieval of data.
3.  **Shared .NET Class Library (DTOs Models):** Transfer data between the backend and frontend.

## Setup Instructions

1.  **Prerequisites:**
    * .NET SDK 8.0 or later. You can download it from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).
    * A code editor like Visual Studio or Visual Studio Code.

2.  **Backend Setup:**
    * Navigate to the `FinanceTrackerAPI` directory in your terminal.
    * Run the following commands to create and apply the database migrations:
        ```bash
        dotnet ef database update
        ```
    * Run the backend API:
        ```bash
        dotnet run
        ```
        The API will typically run on `https://localhost:<port>`. Note the port number.

3.  **Frontend Setup:**
    * Navigate to the `FinanceTrackerUI` directory in your terminal.
    * Open the `FinanceTrackerUI/Program.cs` file and ensure the `HttpClient.BaseAddress` is set to the correct URL of your backend API (including the port number). For example:
        ```csharp
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:<your_api_port>") });
        ```
    * Run the Blazor WebAssembly application:
        ```bash
        dotnet run
        ```
        The frontend will typically run on `https://localhost:<another_port>`.

## Assumptions

* This is a basic demo application and does not include user authentication or authorization.
* Error handling is basic and primarily focuses on HTTP status codes and simple alerts.
* Data validation is primarily done using Data Annotations.
* For simplicity, we are loading all data for the dashboard summary. In a real application, this would likely be optimized.

## Code Quality

The code is structured with:

* Clear and descriptive naming conventions.
* Separation of concerns (models, controllers/components, services, data context).
* Basic adherence to SOLID principles (e.g., single responsibility of services).

## Data Handling (Entity Framework Core)

* EF Core is used as the ORM to interact with the SQLite database.
* Data models (`Income`, `Expense`) are defined as C# classes.
* `FinanceTrackerDbContext` manages the database connection and provides `DbSet` properties for querying and saving data.
* Migrations are used to manage the database schema.

## Testing

* Basic unit tests are included for the backend API controllers to verify the logic of the endpoints. More comprehensive testing would be required for a production application.

## Pagination and Filtering

* The backend API `GET` endpoints for income and expenses support pagination using `pageNumber` and `pageSize` query parameters.
* Basic sorting is also implemented using `sortBy` and `sortDirection` query parameters.

## Error Handling

* The backend API uses `ModelState.IsValid` for request validation and returns appropriate HTTP status codes (e.g., 400 for bad requests, 404 for not found).
* A global exception handler middleware is implemented to catch unhandled exceptions and return a generic 500 error.
* The Blazor frontend displays basic success and error alerts based on the API responses.

## Responsive UI Design

* MudBlazor components and the `MudGrid` system provide a responsive layout that adapts to different screen sizes.

This `README.md` provides a good overview of the application for anyone looking at the project.

With these steps, we should have a functional Personal Finance Tracker demo application that meets the requirements. You can now run both the backend and frontend projects and see it in action!

Do you have any specific aspects you'd like to review or any further questions?