<p align="center">
  <h1 align="center">🚀 DevCourseHub API</h1>
  <p align="center">
    Modern online learning platform backend built with <b>.NET 8</b> and <b>Clean Architecture</b>
  </p>
</p>

<p align="center">
  <img alt=".NET" src="https://img.shields.io/badge/.NET-8.0-purple" />
  <img alt="Architecture" src="https://img.shields.io/badge/Architecture-Clean-blue" />
  <img alt="Database" src="https://img.shields.io/badge/Database-PostgreSQL-336791" />
  <img alt="ORM" src="https://img.shields.io/badge/ORM-EF%20Core-green" />
  <img alt="Mapper" src="https://img.shields.io/badge/Mapper-AutoMapper-orange" />
  <img alt="Auth" src="https://img.shields.io/badge/Auth-JWT-red" />
</p>

---

## 📖 Overview

DevCourseHub API is a scalable backend for an online learning platform built with .NET 8 and Clean Architecture.

---

## ✨ Features

- 🔐 JWT Authentication & Authorization  
- 📚 Course Management  
- 🧩 Section & Lesson Structure  
- 🧑‍🎓 Enrollment System  
- 📈 Progress Tracking  
- ⭐ Review & Rating System  
- 🔍 Pagination & Filtering  
- 🧼 Clean Architecture  

---

## 🛠️ Tech Stack

- .NET 8  
- Entity Framework Core  
- PostgreSQL  
- AutoMapper  

---

## 🏗️ Architecture

```bash
DevCourseHub/
├── Domain
├── Application
├── Infrastructure
├── API
```

---

## 🔗 API Endpoints

- /api/auth  
- /api/courses  
- /api/sections  
- /api/lessons  
- /api/enrollments  
- /api/progress  
- /api/reviews  

---

## ⚡ Getting Started

### 1. Clone repository

```bash
git clone https://github.com/your-username/devcoursehub.git
cd devcoursehub
```

---

### 2. Configure Database

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=DevCourseHub;Username=postgres;Password=YOUR_PASSWORD"
}
```

---

### 3. Apply migrations

```bash
dotnet ef database update
```

---

### 4. Run the project

```bash
dotnet run
```

---

### 5. Swagger

```
https://localhost:5001/swagger
```

---

## 🔐 JWT Authentication

This API uses JWT (JSON Web Token) based authentication.

After login, a token is returned and must be included in requests:

```http
Authorization: Bearer YOUR_TOKEN
```

---

## ⚙️ JWT Configuration

```json
"Jwt": {
  "Key": "YOUR_SECRET_KEY",
  "Issuer": "DevCourseHub",
  "Audience": "DevCourseHubUsers",
  "ExpiryMinutes": 60
}
```

---

## ⚠️ Security Notice

- Never store real secrets in the repository  
- Replace `YOUR_SECRET_KEY` with your own secure key  
- Use local config or environment variables  

---

## 🔑 Using .NET User Secrets

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "your_real_secret_key"
```

---

## 👥 Default Users

```
Admin
admin@devcoursehub.com / Admin123*

Instructor
instructor@devcoursehub.com / Instructor123*

Student
student@devcoursehub.com / Student123*
```

---

## 🧠 Key Concepts

- Clean Architecture  
- Repository Pattern  
- Unit of Work  
- DTO & AutoMapper  
- JWT Authentication  

---

## 🤝 Contributing

1. Fork  
2. Create branch  
3. Commit  
4. Open Pull Request  

---

## 📄 License

MIT

---

## 👨‍💻 Author

Olguhan Hünerli

---

## ⭐ Support

If you like this project, consider giving it a star ⭐
