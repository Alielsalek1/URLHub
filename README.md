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
- **Register**: `POST /api/auth/register` - Create a new user
- **Login**: `POST /api/auth/login` - Authenticate and obtain JWT + refresh token
- **Refresh Token**: `POST /api/auth/refresh` - Refresh the access token
- **External Login (OAuth2)**: `GET /api/auth/externallogin` - Redirects to Google for authentication
- **External Login Callback**: `GET /api/auth/externallogincallback` - Handle Google response and return tokens
- **Request Password Reset**: `POST /api/auth/request-reset-password` - Send a password-reset email link
- **Reset Password**: `POST /api/auth/reset-password` - Submit new password along with reset token
- **Request Activation Email**: `POST /api/auth/request-activate-email` - Send account activation link
- **Activate Account**: `POST /api/auth/activate-email?token={token}&email={email}` - Activate user via token

### User Management
- **Get My Profile**: `GET /api/user/me` - Retrieve current user profile
- **Update My Profile**: `PUT /api/user/me` - Update username or other profile details

### URL Management
- **Create Shortened URL**: `POST /api/url/me` - Shorten a new URL for the authenticated user
- **Delete URL**: `DELETE /api/url/me` - Remove a shortened URL (provide `url` in body)
- **Get User URLs**: `GET /api/url/user/{id}?pageNumber={n}&pageSize={m}` - Fetch paginated URLs for any user
- **Redirect**: `GET /api/url/{shortCode}` - Redirect to the original long URL

### Friends System
- **Add Friend**: `POST /api/friend/{id}` - Add another user as a friend
- **List My Friends**: `GET /api/friend/me?pageNumber={n}&pageSize={m}` - Retrieve paginated list of friends
- **Remove Friend**: `DELETE /api/friend/{id}` - Remove a friend by their user ID

## Installation & Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Postman](https://www.postman.com/) (optional for API testing)
- [Redis Server](https://github.com/redis/redis)

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
          "SmtpPort": 587
        },
        "Redis": {
          "ConnectionString": "localhost:6379"
        },
        "Server": "https://localhost:7005"
   }
   ```

Use User Secrets for:

   ```json
      "Jwt:Key": "YOUR_SECURE_JWT_SECRET_KEY",
      "EmailSettings:SmtpUser": "YOUR_EMAIL_USERNAME",
      "EmailSettings:SmtpPass": "YOUR_EMAIL_APP_PASSWORD",
      "Redis:ConnectionString": "localhost:6379",
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

4. **Redis**:
   - Set `Redis:ConnectionString` to your Redis server (e.g., `localhost:6379`)
   - Ensure Redis is running before starting the API

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
- Use the Bearer Token for authenticated endpoints.
