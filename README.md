# Meeting Manager System

An integrated system for managing meeting requests between students, secretaries, and managers, built using the latest .NET technologies.

## 🚀 Key Features

- **Clean Architecture**: Project divided into layers (Domain, Application, Infrastructure, MVC) for maintainability and scalability.
- **Role Management**: Comprehensive permission system (Admin, Manager, Secretary).
- **Real-Time Updates**: Uses SignalR to send notifications and update tables instantly when requests are created or status changes.
- **Arabic Interface**: Full support for Arabic language and RTL layout.
- **Modern Design**: Attractive user interfaces using Bootstrap 5 with advanced CSS customizations (Glassmorphism, Gradients).

## 🛠️ Tech Stack

- **Backend**: .NET 9, ASP.NET Core MVC, Entity Framework Core.
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5 (RTL).
- **Real-Time**: SignalR.
- **Database**: SQL Server LocalDB.
- **Architecture**: Clean Architecture, Repository Pattern, Dependency Injection.

## 📂 Project Structure

- **MeetingManager.Domain**: Contains Entities, Interfaces, and Enums.
- **MeetingManager.Application**: Contains Business Logic (Services), DTOs, and Interfaces.
- **MeetingManager.Infrastructure**: Contains Data Access implementation (Repositories), DbContext, and external services.
- **MeetingManager.Mvc**: Presentation Layer containing Controllers and Views.

## ⚙️ Setup and Run

1.  **Prerequisites**:

    - .NET 9 SDK
    - SQL Server (or LocalDB)

2.  **Database Setup**:

    - Check the connection string in `appsettings.json` if necessary.
    - The application will automatically create the database and seed initial data on first startup.

3.  **Running the Application**:
    - Open the project in Visual Studio or VS Code.
    - Run the `MeetingManager.Mvc` project.
    - Or use the command:
      ```bash
      dotnet run --project MeetingManager.Mvc
      ```
