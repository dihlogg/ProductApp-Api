# E-commerce API (ASP.NET Core Web API)

This is the backend API for an E-commerce system integrate AI and Automation, built with ASP.NET Core Web API. It powers the React Native + Expo mobile frontend, providing RESTful APIs for product management, user authentication, real-time cart synchronization, order processing, and AI-powered product recommendations. The backend integrates with PostgreSQL, SignalR, ML.NET, and n8n is used to automatically send notifications to user email.

## Technologies
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet)
[![Entity Framework Core](https://img.shields.io/badge/EF_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://docs.microsoft.com/ef/core)
[![SignalR](https://img.shields.io/badge/SignalR-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://docs.microsoft.com/aspnet/core/signalr)
[![ML.NET](https://img.shields.io/badge/ML.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet)
[![n8n](https://img.shields.io/badge/n8n-FF6F61?style=for-the-badge&logo=n8n&logoColor=white)](https://n8n.io)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org)

---

## 🔑 Key Features
### 🛠️ Backend Functions
- **RESTful API**: Endpoints for managing products, categories, users, and orders.
- **Product Management**: CRUD operations for products and categories.
- **Order Management**: Create, update, and track order statuses (New Order, Delivery, Completed).
- **Real-Time Cart Synchronization**: Sync each user’s cart data across multiple platforms (iOS, Android, and web) via SignalR.
- **Real-Time ChatBot**: Support real-time chatbot and customer support interactions via SignalR
- **AI-Powered Recommendations**: Product recommendations using K-Means clustering with ML.NET.
- **Automated Workflows**: Order confirmation emails sent via n8n workflows.
- **Database Management**: Seamless interaction with PostgreSQL using Entity Framework Core(ORM).

👤 Client Support
- Supports the React Native frontend for customer functions like product search, purchasing, and order history.
- Provides real-time updates for chatbot and cart management via SignalR.
- Supplies data for AI chatbot(N8N) and product recommendations(K-Means).
