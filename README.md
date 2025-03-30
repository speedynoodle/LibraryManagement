# Library Management System
Console application for managing a library of books.

## Assumptions
- ISBN is unique e.g. we can't have book title with duplicate ISBN
- Currently we can only have 1 of each book (no multiple copies of a book with the same title, author and ISBN)

## Future Improvements
- Allow the ability to store copies of a book e.g. a book  can have multiple copies of different types (1 title, multiple copies) e.g. hard copy or audiobook
- List all books: Add pagination
- Add logging

## Project Structure
The solution is organised based on the clean architecture concepts with layers separating concerns:

1. **LibraryManagement.Core**
    - Contains domain entities (currently only `Book`) and repository interfaces
    - Defines contracts for the application

2. **LibraryManagement.Data**
    - Implements the repository interfaces using Entity Framework Core
    - Uses EF Core's InMemoryDatabase provider for data persistence
    - Defines the DbContext and entity configurations

3. **LibraryManagement.Services**
    - Contains the business logic
    - Validates data and enforces business rules

4. **LibraryManagement.Console**
    - UI for interacting with the application
    - Sets up dependency injection

5. **LibraryManagement.Tests**
    - Contains unit tests for the repositories, services, and validators
    - Uses Moq for mocking dependencies

## Design Decisions
- Using Entity Framework Core with the InMemoryDatabase provider for simplicity. If we were to productionised this, we can easily replace with SQL Server, PostgreSQL etc by changing the DbContext configuration
- Using Dependency Injection to reduce coupling between components
- Using Service Layer that implements business logic and validation separate from the repositories and UI
- Use of specific interfaces that extend generic ones (Interface Segregation) for ease of future extension

## How to Run the Application

### Prerequisites
- .NET 8.0 SDK or later

### Building and Running

1. Clone the repository:
```
https://github.com/speedynoodle/LibraryManagement.git
cd LibraryManagement
```

2. Build the solution:
```
dotnet build
```

3. Run the console application:
```
cd LibraryManagement.Console
dotnet run
```

### Running Tests

```
dotnet test
```