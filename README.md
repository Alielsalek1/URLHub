# URL Hub - A Feature-Rich URL Shortener

## Overview

URL Hub is a URL shortening service built using **ASP.NET Core** and **SQL Server**, with additional features including:
- **User Authentication** (Register, Login, Refresh Tokens)
- **Friend System** (Users can add friends)
- **URL Management** (Each user can create, view, and delete their own shortened URLs)
- **Shared Visibility** (Users can see other users' shortened URLs via pagination)
- **Entity Framework Core** for ORM

## API Endpoints (Postman Collection)

A Postman collection has been created to interact with the API. Below is a summary of the key API endpoints:

### Authentication
- **Register**: `POST /auth/register` - Create a new user
- **Login**: `POST /auth/login` - Authenticate and obtain a token
- **Refresh Token**: `POST /auth/refresh` - Refresh the authentication token

### User Management
- **Get My Data**: `GET /user/me` - Retrieve user profile data
- **Update Profile**: `PUT /user/me` - Update username

### URL Management
- **Create Shortened URL**: `POST /url/me` - Shorten a new URL
- **Delete URL**: `DELETE /url/me` - Remove a shortened URL
- **Get User URLs**: `GET /url/user/{id}` - Fetch paginated URLs of a user
- **Redirect**: `GET /url/{shortCode}` - Redirect to the original URL

### Friends System
- **Add Friend**: `POST /friend/{userId}` - Add another user as a friend
- **List Friends**: `GET /friend` - Retrieve a list of friends

## Installation & Setup

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server
- [Postman](https://www.postman.com/) (optional for API testing)

### Security Configuration

Before running the API, you must configure two critical security settings using .NET Core User Secrets (or your preferred configuration method):

1. **Database Connection String**:  
   Configure your SQL Server connection string. For example:
  ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER;Database=URLHubDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
  }
  ```

2. **JWT Secret Key**:
  Configure a secure JWT secret key. This key is used to sign and validate JWT tokens for authentication. For example:
  ```json
  {
      "JwtSettings": {
        "SecretKey": "YOUR_SECURE_JWT_SECRET_KEY",
        "Issuer": "https://localhost:7005/",
        "Audience": "https://localhost:7005/api",
        "DurationInMinutes": 30
      }
  }
  ```
  **Important**: Use a strong, randomly generated secret key. Do not expose this key in public repositories.

## Steps to Run the API

  1. **Clone the Repository**:
      ```git
        git clone <repository-url>
        cd URL-Hub
      ```

  2. **Configure User Secrets**:
     
      Use the .NET Core User Secrets tool to add the above configurations for both the connection string and JWT secret key.

  3. **Apply Migrations**
      ```sh
        dotnet ef database update
      ```

  4. **Run the API**
      ```sh
        dotnet run
      ```

## Testing the API
- Import the provided Postman collection and test the endpoints.
- Use the Bearer Token for authenticated endpoints.
