<div align="center">

# Used Books & Tutor Platform

A web application for buying and renting used books, combined with tutor search and rental features. Developed to practice building a database-driven web system with practical business workflows.

[![C#](https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet)
[![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)

[View Repository](https://github.com/TuyetAnh0101/Website) • [Report Bug](https://github.com/TuyetAnh0101/Website/issues)

</div>

---

## Overview

The platform supports the buying, selling, and renting of used books, while also providing features for searching and renting tutors. The project focuses on developing a practical web application with a relational database and common business operations such as account management, shopping, order processing, reviews, and user authorization.

---

## Application Preview

| Home Page | Statistics & Dashboard |
| :---: | :---: |
| <img src="docs/screenshots/trangchu.jpg" width="400" alt="Home Page"/> | <img src="docs/screenshots/thongke.jpg" width="400" alt="Statistics"/> |
| **Product Management** | **Tutor Management** |
| <img src="docs/screenshots/ql_sanpham.jpg" width="400" alt="Product Management"/> | <img src="docs/screenshots/ql_giasu.jpg" width="400" alt="Tutor Management"/> |
| **Order Management** | **Rental History** |
| <img src="docs/screenshots/ql_donhang.jpg" width="400" alt="Order Management"/> | <img src="docs/screenshots/lichsu_thue.jpg" width="400" alt="Rental History"/> |

---

## Tech Stack & Architecture

### Technologies

| Category | Technology |
|---|---|
| **Backend** | C#, ASP.NET Core |
| **ORM** | Entity Framework Core |
| **Database** | SQL Server |
| **Authentication** | ASP.NET Core Identity |
| **Frontend** | HTML, CSS, JavaScript |
| **Web Framework** | Razor Pages, MVC, Blazor |

### Architecture Flow

`Web Interface (ASP.NET Core)` ➔ `Application Layer (Controllers/Services)` ➔ `Data Access (EF Core)` ➔ `Database (SQL Server)`

---

## Main Features

- **Used Books Management:** Buy, sell, and rent used books through the platform.
- **Tutor Services:** Search for tutors and manage tutor rental services.
- **Shopping & Orders:** Add books to the cart, manage selected items, and process customer orders.
- **User Authentication:** Secure user registration, login, and account management using ASP.NET Core Identity.
- **Interactive Reviews:** Allow users to provide ratings and reviews for products and services.
- **Data Handling:** Robust relational database management utilizing Entity Framework Core.

---

## Getting Started

### Prerequisites

- **.NET SDK:** Version 9.0
- **Database:** SQL Server
- **IDE:** Visual Studio 2022 (or later)

### Installation & Setup

**1. Clone the repository**
```bash
git clone [https://github.com/TuyetAnh0101/Website.git](https://github.com/TuyetAnh0101/Website.git)
cd Website
