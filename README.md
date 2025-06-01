URL Hub is a URL shortening service built using **ASP.NET Core** and **SQL Server**, with additional features including:
- **User Authentication** (Register, Login, Refresh Tokens, Email Activation)
- **Email Validation** with confirmation links
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
- **Activate Account**: `GET /auth/activate` - Validate email using activation token

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
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Postman](https://www.postman.com/) (optional for API testing)

### Configuration
Configure these settings in `appsettings.json`:

   ```json
   {
        "Jwt": {
          "Issuer": "https://localhost:7005/",
          "Audience": "https://localhost:7005/api",
          "DurationInMinutes": 30
        },
        "EmailSettings": {
          "From": "your-email@domain.com",
          "SmtpHost": "smtp.gmail.com",
          "SmtpPort": 587,
        },
        "Server": "https://localhost:7005",
   }
   ```

Use User Secrets for:

   ```json
      "Jwt:Key": "YOUR_SECURE_JWT_SECRET_KEY",
      "EmailSettings:SmtpUser": "YOUR_EMAIL_USERNAME",
      "EmailSettings:SmtpPass": "YOUR_EMAIL_APP_PASSWORD"
      "ConnectionStrings": {
         "DefaultConnection": "Server=YOUR_SERVER;Database=URLHubDb;Trusted_Connection=True;MultipleActiveResultSets=true"
        }
   ```

**Mandatory Configuration:**
1. **JWT Settings**:
   - Generate a strong random key for `Jwt:Key`
   - Update issuer and audience URLs according to your environment

2. **Email Settings**:
   - `SmtpUser`: Your email account username
   - `SmtpPass`: App password for email service
   - `From` must match your email account
   - For Gmail, enable [App Passwords](https://myaccount.google.com/apppasswords)

3. **Database**:
   - Configure SQL Server connection string in `DefaultConnection`

## Run the API
1. **Clone Repository**
```bash
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
<<<<<<< Updated upstream
- Use the Bearer Token for authenticated endpoints.
=======
- Use the Bearer Token for authenticated endpoints.
>>>>>>> Stashed changes
