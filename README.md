# 🌎 CityExplorerV2

City Explorer V2 is a full-stack web application built with ASP.NET Core. It allows users to explore city-specific data and weather insights in real time using external APIs, while supporting secure user authentication and admin-level data management.

---

## 📚 Overview

City Explorer V2 is a redesigned version of the original City Explorer project, now developed using C#, Razor Pages, and MongoDB. It includes user registration, login, admin dashboard functionality, and dynamic content loading through secure server-side API integration.

---

## 🧩 Key Features

- 🔍 **City & Weather Search** — fetch real-time data using RapidAPI services.
- 🧠 **Caching** — city and weather data are cached in MongoDB to reduce API calls.
- 👤 **User Authentication** — registration and login with secure password hashing.
- 🔐 **Admin Authorisation** — admin users can access a dashboard to manage data.
- 🗂️ **CRUD Operations** — full Create, Read, Update, and Delete on city and user data.
- ⚙️ **Dynamic UI** — JavaScript-powered dynamic content updates without reloads.
- 🌐 **Responsive Design** — styled using semantic HTML5 and CSS for accessibility.

---

## 📌 Technologies Used

- **Backend**: ASP.NET Core (C#) with Razor Pages
- **Frontend**: HTML, CSS, JavaScript
- **Database**: MongoDB
- **External APIs**: GeoDB Cities API, WeatherAPI (via RapidAPI)
- **Development Tools**: Rider IDE, .NET CLI, MongoDB Compass

---

## 🚀 Running the Project

1. **Clone the repo**:
   ```bash
   git clone https://github.com/Keano20/City-Explorer-V2.git

2. **Required Configuration**:
Before running the project, you must create two configuration files.
name the first one, appsettings.json

Create this file in the project root, and paste:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "MongoDbSettings": {
    "ConnectionString": "YOUR_MONGODB_CONNECTION_STRING",
    "DatabaseName": "CityExplorerDB"
  },
  "RapidApi": {
    "Key": "YOUR_RAPIDAPI_KEY"
  }
}
```

Replace the placeholders with your own MongoDB and RapidAPI credentials.
Need help getting keys?
visit MongoDB Atlas: https://www.mongodb.com/products/platform/atlas-database and RapidAPI (GeoDB) https://rapidapi.com/wirefreethought/api/geodb-cities
