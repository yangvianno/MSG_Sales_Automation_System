# 🏥 MSG Sales Automation System

This project is a full-stack **Sales Automation Platform** developed for **Medical Saigon Group (MSG)** to assist with healthcare-related CRM, hospital management, lead tracking, reporting, and doctor/patient scheduling. Built using **ASP.NET Core**, **Blazor WebAssembly**, **SQL Server**, and layered libraries, the system enables seamless communication between users, hospitals, and sales operators.

---

## 💼 Project Overview

- 🔧 **Backend**: ASP.NET Core Web API (Entity Framework, Repository-Service pattern)
- 🌐 **Frontend**: Blazor WebAssembly (SPA)
- 🧠 **Business Logic**: Modular libraries (`LeadLib`, `HelperLib`, `DataAccessLibrary`)
- 🏥 **Healthcare Integration**: Supports doctor/patient scheduling, HIS/ABR systems
- 📊 **Reporting Engine**: Daily, weekly, and monthly KPI reports with Excel output
- 🔐 **Authentication**: JWT-based secure login with user role management

---

## 📁 Project Structure

```
MSG_Sales_Automation_System/
├── SalesAuto.Api/           # ASP.NET Core Web API (controllers, services, DB)
├── SalesAuto.Wasm/          # Blazor WebAssembly frontend
├── LeadLib/                 # Lead conversion and tracking logic
├── HelperLib/               # Utility helpers and general services
├── DataAccessLibrary/       # EF Core models and DB contexts
├── DB/                      # SQL scripts, schemas, migrations
├── .sln                     # Visual Studio solution file
```

---

## 🚀 Key Features

### 🔗 CRM Integration
- Syncs product, store, and order data with external CRM via HttpClient (`CRMClientRepo`)
- RESTful endpoints for product listings, order filtering, and store lookup

### 📅 Scheduling & Management
- APIs for **ABR (doctor/nurse)** and **HIS (hospital information system)**
- Supports hospital data ingestion and scheduling logic

### 📈 Reporting Tools
- Generate Excel reports: **daily, weekly, monthly KPIs**
- File outputs supported via APIs (`ReportExcelController`, `DailyReportController`)

### 🛠️ Backend Architecture
- Cleanly structured using:
  - `Controllers` → Endpoints
  - `Services` → Business logic
  - `Repositories` → Data access
- Configured via `Startup.cs` with **Entity Framework Core** and **SQL Server**

---

## 🔒 Authentication

- JWT Bearer Token Authentication
- Integrated middleware for protected API access
- Role-based logic for admin, user, operator

---

## 📸 Screenshots / Visuals (coming soon)

- Blazor frontend UI with login, dashboards, and data forms
- Exported Excel sample reports
- API usage and CRM integration flow

---

## 🏛️ Acknowledgements

Developed during internship at **Medical Saigon Group**  
Special thanks to the MSG IT Department for architectural guidance and enterprise exposure.

---

## 📌 License

This repository is for **educational and showcase purposes only** and may contain internal APIs or integrations not suitable for production reuse without permission.
