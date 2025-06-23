Employee & Department Management Web Application
This is a simple ASP.NET Core MVC + Razor Pages web application for managing departments and employees in an organization. It demonstrates basic CRUD operations, model validation, partial views, view components, and in-memory data repositories without a database.

Features
Manage Departments: List, Create, Edit, Delete.

Manage Employees: List, Create, Edit, Delete.

Filter employees by department or search by name.

Departments and Employees stored in in-memory static repositories.

Razor Pages with form validation and partial views.

Simple confirmation dialogs for delete actions.

Clean UI with Bootstrap styling.

Technologies Used
ASP.NET Core MVC & Razor Pages

C# 11 / .NET 7 (or compatible)

Bootstrap 5 (for UI styling)

In-memory static collections for data storage

Project Structure
Models: Department, Employee, and their repositories (DepartmentRepository, EmployeeRepository) storing static data.

Controllers: DepartmentsController, HomeController handling requests and returning views.

Pages/Employees: Razor Pages to Create, Edit, List, and filter employees.

Views: Razor Views and Partial Views for UI rendering (_EmployeeDetail, employee and department lists).

Helpers: Utility classes for model validation errors.

Getting Started
Prerequisites
.NET SDK 7+

IDE such as Visual Studio 2022, Visual Studio Code, or JetBrains Rider

Running the Application
Clone the repository or copy the source code.

Open the solution/project in your IDE.

Build the solution to restore NuGet packages.

Run the project (F5 or dotnet run).

Navigate to:

/departments for department management

/employees for employee management

Code Highlights
Models
Department with Id, Name, and Description.

Employee with Id, Name, Position, Salary, and Department reference.

Repositories
Static in-memory lists holding departments and employees.

CRUD operations implemented using LINQ on static lists.

Filtering and searching enabled by name or department.

Razor Pages (Employees)
CreateModel: Create new employee with validation.

EditModel: Edit or delete employee with confirmation.

IndexModel: List employees with filter/search functionality.

Uses partial views (_EmployeeDetail) for employee form inputs.

Controllers (Departments)
CRUD operations using DepartmentRepository.

Use of ModelState validation and error handling.

Usage
Add Department: Navigate to /departments/create.

Edit Department: From the department list, choose a department and edit.

Delete Department: Delete department from the department list view.

List Employees: Go to /employees to see all employees.

Filter Employees: Search by name or filter by department.

Add Employee: Go to /employees/create.

Edit Employee: From the employee list, click edit to update or delete.

Future Enhancements
Replace in-memory repositories with a persistent database (e.g., EF Core with SQL Server).

Add authentication and role-based authorization.

Improve UI/UX with better navigation and validation feedback.

Add API endpoints for external integrations.

Implement paging for large lists.

License
This project is open-source and free to use for learning and demonstration purposes.
