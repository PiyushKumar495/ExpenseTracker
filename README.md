# 💰 FinTrack – AI-Powered Expense Tracker

FinTrack is a **full-stack personal finance and expense tracking application** that I’m building using **ASP.NET Core Web API and Angular**.

The goal of FinTrack is to make it easier for users to understand and manage their spending by bringing expenses, bank statements, and financial insights into one place.

## 🚀 What I'm Building

FinTrack will allow users to:

* 🔐 Create an account and securely log in
* 💸 Add and manage income and expenses
* 🏦 Upload/import bank statements
* 📄 Extract transactions from bank statements
* 📊 Calculate total expenses and income
* 📈 View spending summaries and financial statistics
* 🗂️ Categorize transactions and expenses
* 🤖 Get AI-powered suggestions based on spending patterns
* 💡 Receive insights on where money is being spent and how expenses could potentially be reduced

## 🤖 AI-Powered Financial Insights

One of the main features I’m working towards is an **AI-based financial assistant**.

The application will analyze a user's transaction and expense data and provide useful suggestions such as:

* Identifying categories where the user spends the most
* Highlighting unusual or increased spending
* Suggesting areas where expenses could be reduced
* Providing monthly spending summaries
* Giving personalized insights based on spending patterns

> The AI feature is intended to provide financial insights and suggestions based on the user's data, not professional financial advice.

## 🛠️ Tech Stack

### Backend

* **C#**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **JWT Authentication**
* **REST APIs**

### Frontend

* **Angular**
* **TypeScript**
* **HTML**
* **CSS**

### Planned / Exploring

* **AI / LLM integration**
* Bank statement processing
* Transaction categorization
* Financial analytics and dashboards

## 🏗️ Project Architecture

The backend is being developed using a layered architecture to keep the application maintainable and scalable.

```text
FinTrack
│
├── FinTrack.API
│   └── Controllers
│
├── FinTrack.Application
│   ├── DTOs
│   ├── Services
│   └── Interfaces
│
├── FinTrack.Domain
│   ├── Entities
│   └── Enums
│
└── FinTrack.Infrastructure
    ├── Data
    ├── Repositories
    └── Configurations
```

## 📌 Current Progress

This project is **actively under development**.

### Completed / Working On

* [x] Project structure and backend architecture
* [x] Authentication and user registration
* [ ] Login and JWT access/refresh token flow
* [ ] Expense management
* [ ] Income management
* [ ] Bank statement upload
* [ ] Transaction extraction
* [ ] Expense categorization
* [ ] Expense dashboard and analytics
* [ ] AI-powered spending suggestions

## 🎯 Future Goals

I plan to expand FinTrack with features such as:

* 📅 Monthly and yearly expense reports
* 📊 Interactive financial dashboards
* 🔎 Advanced transaction filtering
* 🏷️ Automatic transaction categorization
* 🤖 More personalized AI insights
* 📱 Improved responsive UI
* 🔔 Budget and spending alerts

## 👨‍💻 About the Project

I’m building FinTrack as a practical **full-stack project** to improve my experience with **ASP.NET Core, Angular, SQL Server, authentication, API design, and AI integration**.

The project is continuously evolving as I implement and learn new technologies.

---

⭐ **This project is currently under development.**
