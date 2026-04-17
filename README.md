# E-Commerce Backend API

A .NET 8.0 Web API for e-commerce functionality with JWT authentication and Entity Framework Core.

## Tech Stack

- **Framework:** .NET 8.0
- **Database:** SQL Server with Entity Framework Core
- **Authentication:** JWT Bearer Tokens
- **API Documentation:** Swagger (Swashbuckle)

## Project Structure

```
E-commerce Endpoints/
├── Controllers/          # API endpoints
│   ├── AuthController.cs
│   ├── UserController.cs
│   ├── ProductController.cs
│   ├── OrderController.cs
│   ├── CartController.cs
│   ├── CategoryController.cs
│   ├── BrandController.cs
│   └── ...
├── DTO/                  # Data Transfer Objects
│   ├── User/
│   ├── Product/
│   ├── Order/
│   └── ...
├── Helper/               # Utilities (JWT, Validation, etc.)
├── Services/             # Business logic interfaces & implementations
│   ├── Interfaces/
│   └── Implementation/
├── Data/                 # DbContext
└── appsettings.json      # Configuration
```

## Configuration

Update `appsettings.json` with your database connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=Ecommerce;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JWT": {
    "Secret": "your-256-bit-secret-key-here",
    "Issuer": "EcommerceAPI",
    "Audience": "EcommerceClient"
  }
}
```

## Getting Started

1. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

2. **Update database (if using migrations):**
   ```bash
   dotnet ef database update
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Access Swagger UI:**
   - Navigate to `https://localhost:7000/swagger` (or the port shown in terminal)

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token
- `POST /api/auth/change-password` - Change password (requires auth)

### Products
- `GET /api/product` - Get all products
- `GET /api/product/{id}` - Get product by ID
- `POST /api/product` - Create product (admin only)
- `PUT /api/product/{id}` - Update product (admin only)
- `DELETE /api/product/{id}` - Delete product (admin only)

### Categories & Brands
- `GET /api/category` - Get all categories
- `GET /api/brand` - Get all brands
- `GET /api/subcategory` - Get subcategories

### Cart & Orders
- `GET /api/cart` - Get user cart
- `POST /api/cart/add` - Add item to cart
- `POST /api/order` - Place order
- `GET /api/order` - Get user orders

### Favorites
- `GET /api/favorite` - Get user favorites
- `POST /api/favorite` - Add to favorites
- `DELETE /api/favorite/{id}` - Remove from favorites

## Authentication

Include JWT token in requests:

```
Authorization: Bearer <token>
```

## CORS

Currently configured to allow requests from `http://127.0.0.1:5500`. Update in `Program.cs` for different origins.