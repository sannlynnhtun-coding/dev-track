# DevTrack Management System

DevTrack is a comprehensive management system designed to track training batches, developers, and attendance. It is built using a modern, decoupled architecture with .NET 8, following the **Result Pattern** and leveraging **Refit** for API-driven communication.

## Project Structure

The solution is divided into five main projects to ensure a clean separation of concerns:

- **DevTrack.Api**: The backend REST API. It handles all database interactions, business logic execution, and exposes endpoints for the WebApp.
- **DevTrack.WebApp**: An ASP.NET Core MVC application providing a premium user interface. It is completely decoupled from the database and consumes logic exclusively via the API.
- **DevTrack.Domain**: Contains the core business logic, feature-based services, and interfaces (`IBatchService`, `IDeveloperService`, `ITrainingService`).
- **DevTrack.Database**: Handles data persistence using Entity Framework Core (Database-First). Contains migrations, entities, and the `DevTrackDbContext`.
- **DevTrack.Shared**: A lightweight project containing shared models, DTOs, and the core `Result<T>` pattern used for consistent error handling across the solution.

## Architecture Highlights

- **API-First Design**: The WebApp does not have a database connection string. It uses **Refit** to generate type-safe API clients that communicate with the backend.
- **Service Pattern**: Domain logic is encapsulated in services that implement feature-based interfaces, allowing for easy swapping of implementations (e.g., calling a DB directly vs. calling an API).
- **Result Pattern**: All service and API operations return a `Result` or `Result<T>` object, ensuring that success/failure states and messages are handled uniformly.
- **Modern UI**: The WebApp features a premium design with smooth transitions and a focus on visual hierarchy.

## Features

- **Batch Management**: Create training batches, define mentors, and manage training durations.
- **Calendar Auto-Generation**: Automatically generates a training calendar based on specified start/end dates and training days.
- **Developer Tracking**: Manage developer profiles and assign them to specific batches.
- **Attendance System**: 
  - Bulk attendance marking for training dates.
  - Eligibility tracking based on attendance percentages.
  - Real-time summary views for mentors and administrators.

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server

### Configuration
1. **Database**: Update the connection string in `DevTrack.Api/appsettings.json`.
2. **API URL**: Update the `ApiSettings:BaseUrl` in `DevTrack.WebApp/appsettings.json` to match the API's hosting URL.

### Running the Application
1. Start the **DevTrack.Api** project.
2. Start the **DevTrack.WebApp** project.
3. Access the system via the WebApp URL.

## Technology Stack
- **Framework**: .NET 8 (C#)
- **Database**: SQL Server with EF Core
- **API Client**: Refit
- **Frontend**: ASP.NET Core MVC with modern CSS styling
- **Solution Format**: Visual Studio `.slnx`
