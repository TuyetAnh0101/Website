<div align="center">

# Used Books & Tutor Platform

### A Web Application for Used Books and Tutor Services

A web application for buying and renting used books, combined with tutor search and rental features. The project was developed to practice building a database-driven web system with practical business workflows.

<p>
  <img src="https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
</p>

</div>

---

## Overview

The platform supports the buying, selling, and renting of used books, while also providing features for searching and renting tutors.

The project focuses on developing a practical web application with a relational database and common business operations such as account management, shopping, orders, reviews, and user authorization.

---

## Tech Stack

| Category         | Technologies             |
| ---------------- | ------------------------ |
| Backend          | C#, ASP.NET Core         |
| ORM              | Entity Framework Core    |
| Database         | SQL Server               |
| Authentication   | ASP.NET Core Identity    |
| Frontend         | HTML, CSS, JavaScript    |
| Web Technologies | Razor Pages, MVC, Blazor |

---

## Main Features

| Feature            | Description                                 |
| ------------------ | ------------------------------------------- |
| Used Books         | Manage, buy, sell, and rent used books      |
| Shopping Cart      | Add books to cart and manage selected items |
| Order Management   | Create and manage customer orders           |
| Account Management | Registration, login, and account management |
| Reviews            | Allow users to provide ratings and reviews  |
| Tutor Services     | Search for and rent tutors                  |
| Authentication     | User authentication and authorization       |
| Data Management    | Manage and review system data               |

---

## Architecture

The application follows a layered approach connecting the web application, business logic, ORM, and relational database.

```text
User
  |
  v
ASP.NET Core
  |
  v
Controllers / Services
  |
  v
Entity Framework Core
  |
  v
SQL Server
```

---

## Project Highlights

* Designed and managed a relational database for a practical web application.
* Implemented business workflows for buying, selling, renting, and order management.
* Integrated authentication and user authorization using ASP.NET Core Identity.
* Applied Entity Framework Core for database operations with SQL Server.
* Organized the application structure to support maintainability and future development.

---

## Getting Started

### Requirements

* .NET 9 SDK
* SQL Server
* Visual Studio 2022 or later

### Installation

Clone the repository:

```bash
git clone https://github.com/TuyetAnh0101/Website.git
cd Website
```

Restore the project dependencies:

```bash
dotnet restore
```

Run the application:

```bash
dotnet run
```

Before running the application, configure the required connection string and authentication settings in `appsettings.json`.

---

## Repository

<p align="center">
  <a href="https://github.com/TuyetAnh0101/Website">
    <img src="https://img.shields.io/badge/View%20Repository-181717?style=for-the-badge&logo=github&logoColor=white" />
  </a>
</p>

---

## Author

<div align="center">

**Tuyết Anh**

Information Technology Student

</div>
