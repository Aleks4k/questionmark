# Question Mark - Social Media API

A modern, scalable REST API for a social media platform built with .NET 8 and following clean architecture principles.

## What the Project Does

Question Mark is a **privacy-first** backend API that powers a decentralized social media application. The network is completely private with no usernames or personal identifiers exposed. Users are authenticated through an encrypted `auth` credential (generated on the client-side), ensuring anonymity and privacy by default.

The API is built using Clean Architecture with a layered approach (Domain, Application, Infrastructure) and implements modern best practices for security and privacy.

### Key Features

- **Privacy-First Architecture**: 
  - No usernames or email exposure
  - Users identified only by encrypted credentials
  - Anonymous post creation and interaction
- **User Management**: Secure user registration and authentication with encrypted credentials
- **Posts**: Create, retrieve, and manage anonymous posts
- **Comments**: Add comments to posts with full CRUD operations
- **Reactions**: Like/react to posts and comments
- **Session Management**: Token refresh and secure logout functionality
- **Enterprise-Grade Security**: 
  - JWT-based authentication with access and refresh tokens
  - Argon2 password hashing with salts
  - AES-GCM encryption for session data and sensitive information
  - Encrypted user IDs in database
- **Clean Architecture**: Domain-driven design with clear separation of concerns
- **API Documentation**: Built-in Swagger UI for interactive API exploration
- **Input Validation**: Comprehensive validation using FluentValidation
- **Error Handling**: Centralized exception handling with custom filters

## Why This Project is Useful

This project demonstrates:

- **Privacy by Design**: A real-world example of building a completely anonymous social network where users have no exposed identifiers
- **Production-Ready Architecture**: A well-structured foundation for building privacy-focused applications at scale
- **Learning Resource**: Best practices in .NET development including:
  - CQRS pattern with MediatR
  - Advanced encryption techniques (AES-GCM, Argon2)
  - JWT token management with refresh strategies
  - Entity Framework Core with MySQL
  - Secure session management
  - Auto-validation pipelines
- **Security Pattern Reference**: Demonstrates industry best practices for:
  - Encrypted storage of sensitive data
  - Secure authentication without usernames
  - Session integrity verification
  - Input validation and sanitization
- **Extensible Foundation**: Easy to add new features thanks to clean separation of concerns and privacy-first design patterns

## Getting Started

### Prerequisites

- .NET 8 SDK
- MySQL 8.0 or later
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/aleks4k/questionmark.git
   cd questionmark
   ```

2. **Configure environment variables**

   Create a `.env` file in the `questionmark.Api` directory with the following configuration:
   ```env
   MYSQL_CONNECTION=server=localhost;user=root;password=yourpassword;database=questionmark
   MYSQL_DATABASE=questionmark
   MYSQL_USER=questionmark
   MYSQL_PASSWORD=your_password
   MYSQL_ROOT_PASSWORD=your_root_password
   JWT_AccessTokenKey=your-access-token-key-uuid-format
   JWT_RefreshTokenKey=your-refresh-token-key-uuid-format
   JWT_Issuer=questionmark
   JWT_Audience=questionmark
   JWT_AccessTokenTTL=30000
   JWT_RefreshTokenTTL=10
   HASH_AuthSalt=your-auth-salt-base64-encoded-string
   HASH_CipherSalt=your-cipher-salt-base64-encoded-string
   AES_SessionKey=your-aes-session-key-hex-encoded-string
   ```

   **Configuration Details**:
   - `MYSQL_CONNECTION`: MySQL database connection string
   - `MYSQL_DATABASE`: Database name (used by Docker during initialization)
   - `MYSQL_USER`: MySQL user (used by Docker during initialization)
   - `MYSQL_PASSWORD`: MySQL password (used by Docker during initialization)
   - `MYSQL_ROOT_PASSWORD`: MySQL root password (required for Docker MySQL image)
   - `JWT_AccessTokenKey`: UUID format key for access token signing
   - `JWT_RefreshTokenKey`: UUID format key for refresh token signing
   - `JWT_Issuer` & `JWT_Audience`: Token identification (typically "questionmark")
   - `JWT_AccessTokenTTL`: Access token expiration in milliseconds (30000 = 30 seconds for testing)
   - `JWT_RefreshTokenTTL`: Refresh token expiration in days (10 = 10 days)
   - `HASH_AuthSalt`: Base64-encoded salt for authentication hashing (Argon2)
   - `HASH_CipherSalt`: Base64-encoded salt for cipher operations
   - `AES_SessionKey`: Hexadecimal-encoded AES encryption key for sessions (32 bytes)

3. **Set up the database**

   The database will be created automatically on first run using Entity Framework Core migrations. Ensure your MySQL server is running and the connection string in `.env` is correct.

4. **Build the solution**
   ```bash
   dotnet build
   ```

5. **Run the API**
   ```bash
   cd questionmark.Api
   dotnet run
   ```

   The API will start on `https://localhost:5001` (or the configured port).

### Usage Examples

#### 1. Register a New User (Anonymous Account)
```bash
curl -X POST "https://localhost:5001/api/user/register" \
  -H "Content-Type: application/json" \
  -d '{
    "auth": "generated-from-client-e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"
  }'
```

**Note**: The `auth` field is generated on the client-side by combining user credentials and hashing them. This ensures usernames and emails are never transmitted to or stored on the server.

#### 2. Login (Retrieve Access Tokens)
```bash
curl -X POST "https://localhost:5001/api/user/login" \
  -H "Content-Type: application/json" \
  -d '{
    "auth": "generated-from-client-e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"
  }'
```

Response includes:
```json
{
  "access": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refresh": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### 3. Create a Post (Anonymous)
```bash
curl -X POST "https://localhost:5001/api/post/create" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -d '{
    "text": "Hello, this is my anonymous post!"
  }'
```

Your identity is protected - only you can connect this post to your account using your encrypted credentials.

#### 4. Load All Posts
```bash
curl -X POST "https://localhost:5001/api/post/load" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

#### 5. Add an Anonymous Comment to a Post
```bash
curl -X POST "https://localhost:5001/api/post/comment/create" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -d '{
    "postId": 1,
    "text": "Great anonymous discussion!"
  }'
```

#### 6. React to a Post
```bash
curl -X POST "https://localhost:5001/api/post/react" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -d '{
    "postId": 1,
    "reactionType": "Like"
  }'
```

### Interactive API Documentation

Once the API is running, access the Swagger UI for interactive documentation:

```
https://localhost:5001/swagger
```

This provides an interactive interface to test all endpoints with automatic schema validation.

## Project Structure

### Privacy-First Authentication Model

Unlike traditional social networks, Question Mark uses an **anonymous authentication system**:

- **No Usernames**: Users are identified only by encrypted credentials
- **No Email Storage**: The server never knows your email address
- **Client-Side Hashing**: The `auth` credential is generated and hashed on the client-side before transmission
- **Encrypted Sessions**: User IDs are encrypted in the database using AES-GCM encryption
- **Fully Anonymous Posts**: All posts, comments, and reactions are completely detached from user identities on the network level

This design ensures that even the database doesn't contain easily readable user information.

### Architecture Layers

The project follows **Clean Architecture** with four main layers:

```
questionmark/
├── questionmark.Domain/              # Core business entities
│   ├── Entities/                     # Domain models (User, Post, Comment, Reaction)
│   └── Data/                         # DbContext and data configuration
├── questionmark.Application/         # Business logic layer
│   ├── Users/                        # User-related commands and queries
│   ├── Posts/                        # Post-related commands and queries
│   ├── Sessions/                     # Session management
│   └── Common/                       # Shared contracts and services
├── questionmark.Infrastructure/      # External services and data access
│   ├── Repository/                   # Data access layer
│   ├── Services/                     # JWT, encryption, hashing services
│   └── Settings/                     # Configuration classes
└── questionmark.Api/                 # REST API controllers
    ├── Controllers/                  # API endpoints
    └── Filters/                      # Exception handling and validation
```

## Technology Stack

- **Framework**: .NET 8
- **Web**: ASP.NET Core
- **Database**: MySQL with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI
- **Authentication**: JWT (JSON Web Tokens)
- **Security**: Argon2 (password hashing), custom encryption
- **Patterns**: CQRS (MediatR), Dependency Injection
- **Validation**: FluentValidation
- **Mapping**: Riok.Mapperly

## Configuration

### Environment Variables

The `.env` file supports the following variables:

| Variable | Description | Format |
|----------|-------------|--------|
| `MYSQL_CONNECTION` | MySQL database connection string | `server=host;user=username;password=pass;database=dbname` |
| `JWT_AccessTokenKey` | Secret key for signing access tokens | UUID format |
| `JWT_RefreshTokenKey` | Secret key for signing refresh tokens | UUID format |
| `JWT_Issuer` | Token issuer identifier | String (e.g., "questionmark") |
| `JWT_Audience` | Token audience identifier | String (e.g., "questionmark") |
| `JWT_AccessTokenTTL` | Access token expiration time | Milliseconds |
| `JWT_RefreshTokenTTL` | Refresh token expiration time | Days |
| `HASH_AuthSalt` | Salt for Argon2 authentication hashing | Base64-encoded string |
| `HASH_CipherSalt` | Salt for cipher operations | Base64-encoded string |
| `AES_SessionKey` | AES encryption key for session data | Hexadecimal-encoded (256-bit) |

## Support and Documentation

### Getting Help

- **Issues**: Report bugs or request features via [GitHub Issues](https://github.com/aleks4k/questionmark/issues)
- **Discussions**: Ask questions in [GitHub Discussions](https://github.com/aleks4k/questionmark/discussions)
- **API Docs**: Access the interactive Swagger documentation at `/swagger` when the API is running

### Additional Resources

- [Microsoft .NET Documentation](https://docs.microsoft.com/dotnet)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core)
- [JWT Best Practices](https://tools.ietf.org/html/rfc7519)

## Contributing

We welcome contributions! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

For detailed contribution guidelines, see [CONTRIBUTING.md](CONTRIBUTING.md) (if available).

### Development Workflow

1. Make sure the code builds: `dotnet build`
2. Run tests: `dotnet test` (if tests are available)
3. Format code to match existing style
4. Ensure no breaking changes to public APIs

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Maintainers

This project is maintained by the Question Mark development team. For questions or inquiries, please open an issue on GitHub.

---

**Built with ❤️ using .NET 8**