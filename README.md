# Tax Calculator Web Application

The Tax Calculator Web Application provides a user interface to calculate taxes based on postal codes and annual incomes. It works in conjunction with the Tax Calculator API.

## Features

- Calculate Tax: Users can enter their postal code and annual income to calculate the corresponding tax amount.
- View Tax Records: The application displays a table that lists previously calculated tax records, including the postal code, annual income, and tax amount.
- Edit and Delete Tax Records: Users can edit and delete existing tax records directly from the application.
- Error Handling: The application handles errors during the tax calculation process and displays appropriate error messages to the user.

## Technologies Used

- ASP.NET Core: The web application is built using the ASP.NET Core framework, which provides a robust and scalable platform for web development.
- C#: The backend logic and calculations are implemented using the C# programming language.
- Razor Pages: The user interface is created using Razor Pages, a lightweight and intuitive web programming model in ASP.NET Core.
- HTML/CSS: The application's frontend is built using HTML for structure and CSS for styling.
- JavaScript: JavaScript is used to handle user interactions, perform API requests, and update the UI dynamically.
- Entity Framework Core: The application uses Entity Framework Core, an object-relational mapper, to interact with the database and manage tax records.
- MSSQL: The database is used to store tax records.

## Prerequisites

Before running the Tax Calculator Web Application, make sure you have the following installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Getting Started

Follow these steps to run the Tax Calculator Web Application:

Clone the repository:
git clone https://github.com/matth1010/TaxCalculator.git
Navigate to the project directory:

Configuration

Locate the TaxCalculator.mdf file in the repository.
Attach the database file to your local SQL Server instance.
Build the solution:

Run the TaxCalculator.API project first:

Open the solution in Visual Studio.
Set the TaxCalculator.API as the startup project.
Build and run the project.
The API documentation can be accessed at https://localhost:7032/swagger/index.html.

The application relies on a configuration file to specify the API base URL. By default, the appsettings.json file in the TaxCalculator.Web project contains the following configuration:

Example
{
  "ApiBaseUrl": "https://localhost:7032"
}

Run the TaxCalculator.Web project:

dotnet run --project TaxCalculator.Web
Open your web browser and access the application at https://localhost:7172/.

Usage
Enter a postal code and annual income in the provided form fields.
Click the "Calculate" button to calculate the tax amount.
The tax record will be displayed in the table below.
To edit or delete a tax record, use the respective buttons in the Actions column.
