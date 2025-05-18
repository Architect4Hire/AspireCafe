# Aspire Cafe - Technical Documentation

## 1. System Overview

Aspire Cafe is a comprehensive cloud-native, microservices-based point-of-sale system designed for cafe operations. Built on .NET 9 and utilizing Microsoft's Aspire for cloud-native application development, this system provides a complete solution for managing cafe operations, from order taking to food preparation, payment processing, and analytics.

The solution follows a clean, multi-service architecture with a clear separation of concerns. Each microservice focuses on a specific business domain (product management, order processing, food preparation, etc.) and communicates with other services through well-defined REST APIs and message queues.

## 2. Solution Architecture

### 2.1 High-Level Architecture

Aspire Cafe implements a microservices architecture with the following components:

1. **Frontend (Angular UI)**
   - Single-page application built with Angular 19
   - Communicates with backend services via RESTful APIs

2. **Backend Services**
   - Authentication Service: Handles user authentication and JWT token management
   - Product API: Manages the product catalog and inventory
   - Counter API: Processes customer orders and payments
   - Kitchen API: Manages food preparation workflow
   - Barista API: Manages beverage preparation workflow
   - Order Summary API: Provides order analytics and reporting

3. **Cross-Cutting Concerns**
   - Service Defaults: Shared configurations and middleware
   - Azure Service Bus: Used for asynchronous messaging between services
   - Azure Cosmos DB: Document database for storing service-specific data
   - Redis Cache: Distributed caching for performance optimization
   - Seq: Centralized logging and diagnostic data

4. **Deployment Options**
   - AppHost.Azure: Configuration for Azure deployment
   - AppHost.Arm64: Configuration for local ARM (Mac Silicon/Mx Chipset) deployment
   - AppHost.Intel64: Configuration for local Intel64 deployment

### 2.2 Component Diagram

```
┌─────────────────┐                ┌──────────────────┐
│                 │                │                  │
│  Aspire Cafe.UI  │◄───REST API────┤ Authentication   │
│  (Angular 19)   │                │     Service      │
│                 │                │                  │
└────────┬────────┘                └──────────────────┘
         │                                  ▲
         │                                  │
         │                                  │ JWT Validation
         ▼                                  │
┌─────────────────┐                ┌───────┴──────────┐
│                 │                │                  │
│ API Gateway     │◄───────────────┤   Product API    │
│                 │                │                  │
└────────┬────────┘                └──────────────────┘
         │                                  ▲
         │                                  │
         ▼                                  │
┌─────────────────┐    Orders      ┌───────┴──────────┐
│                 │◄───────────────┤                  │
│   Counter API   │                │   Kitchen API    │
│                 │─────Service────►                  │
└────────┬────────┘     Bus        └──────────────────┘
         │                                  ▲
         │                                  │
         ▼                                  │ Service Bus
┌─────────────────┐                ┌───────┴──────────┐
│                 │                │                  │
│   Barista API   │◄───────────────┤ Order Summary    │
│                 │                │      API         │
└─────────────────┘                └──────────────────┘
```

## 3. Design Patterns and Principles

### 3.1 Architectural Patterns

#### Microservices Architecture
The solution is divided into independent services, each with its own data store and business logic, communicating through well-defined APIs and message queues.

#### API Gateway Pattern
All client requests are routed through a single entry point, which then forwards them to the appropriate internal services.

#### Event-Driven Architecture
Services communicate asynchronously using Azure Service Bus to decouple components and improve resilience.

### 3.2 Design Patterns

#### Repository Pattern
The Data layer implements the Repository pattern to abstract data access logic from business logic. Each service has its own data repository that handles database operations.

```csharp
// Example from ProductApiDomainLayer\Data\Data.cs
public class Data : IData
{
    private readonly ProductContext _context;
    
    public Data(ProductContext context)
    {
        _context = context;
    }
    
    public async Task<ProductDomainModel> FetchProductByIdAsync(Guid productId)
    {
        return await _context.Products.FirstOrDefaultAsync(w => w.Id == productId && w.IsAvailable && w.IsActive);
    }
    
    // Other repository methods...
}
```

#### Facade Pattern
Each service implements a Facade pattern to provide a simplified interface to the complex subsystems. The Facade layer handles validation, error handling, and communication with the business layer.

```csharp
// Example from ProductApiDomainLayer\Facade\Facade.cs
public class Facade : IFacade
{
    private readonly IBusiness _business;
    private readonly ProductViewModelValidator _validator;

    public Facade(IBusiness business, IDistributedCache cache)
    {
        _business = business;
        _validator = new ProductViewModelValidator();
    }

    public async Task<Result<ProductServiceModel>> CreateProductAsync(ProductViewModel product)
    {
        var validationResult = await _validator.ValidateAsync(product);
        if (!validationResult.IsValid)
        {
            return Result<ProductServiceModel>.Failure(Error.InvalidInput, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }
        var data = await _business.CreateProductAsync(product);
        return Result<ProductServiceModel>.Success(data);
    }
    
    // Other facade methods...
}
```

#### Dependency Injection
The solution heavily uses ASP.NET Core's built-in dependency injection to manage service lifetime and dependencies, promoting loose coupling and testability.

```csharp
// Example from Aspire Cafe.ProductApi\Program.cs
void AddScopes(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IFacade, Facade>();
    builder.Services.AddScoped<IBusiness, Business>();
    builder.Services.AddScoped<IData, Data>();
}
```

#### Result Pattern
The system uses a custom Result pattern to handle the outcome of operations, providing a consistent way to handle success and failure scenarios.

```csharp
// From ProductController.cs
[HttpGet("{productId:guid}")]
public async Task<IActionResult> FetchProductById(Guid productId)
{
    var result = await _facade.FetchProductByIdAsync(productId);
    return result.Match();
}
```

#### Observer Pattern (via Service Bus)
The Counter API publishes order events to Azure Service Bus, which the Kitchen and Barista APIs subscribe to for asynchronous processing.

### 3.3 SOLID Principles

- **Single Responsibility Principle**: Each class has a single responsibility (e.g., Controllers handle HTTP requests, Business classes handle business logic)
- **Open/Closed Principle**: The system is designed to be extended without modifying existing code
- **Liskov Substitution Principle**: Interfaces are used throughout the system to ensure behavioral consistency
- **Interface Segregation**: Interfaces are kept focused and specific to their purpose
- **Dependency Inversion**: The system depends on abstractions (interfaces) rather than concrete implementations

## 4. Layer Architecture

Each microservice in the Aspire Cafe solution follows a layered architecture:

### 4.1 Layer Structure

1. **API Layer (Controllers)**
   - Handles HTTP requests and responses
   - Routes requests to the appropriate Facade methods
   - Manages API versioning and documentation
   - Example: `ProductController`, `AuthenticationController`

2. **Facade Layer**
   - Provides a simplified interface to the business layer
   - Handles input validation using FluentValidation
   - Transforms business layer responses into standardized API responses
   - Error handling and logging
   - Example: `ProductApiDomainLayer.Facade.Facade`, `AuthenticationDomainLayer.Facade.Facade`

3. **Business Layer**
   - Contains core business logic
   - Orchestrates operations between different components
   - Maps between domain models and service models
   - Example: `ProductApiDomainLayer.Business.Business`, `CounterApiDomainLayer.Business.Business`

4. **Data Layer**
   - Implements the Repository pattern
   - Handles database operations
   - Manages domain entities and data persistence
   - Example: `ProductApiDomainLayer.Data.Data`, `BaristaApiDomainLayer.Data.Data`

5. **Domain Models**
   - Represents business entities and their relationships
   - Example: `ProductDomainModel`, `OrderDomainModel`

6. **Service Models**
   - Represents data returned from the API (outbound DTO)
   - Used for communication between layers
   - Example: `ProductServiceModel`, `AuthenticationServiceModel`

7. **View Models**
   - Represents data sent to the API (inbound DTO)
   - Example: `ProductViewModel`, `OrderViewModel`

### 4.2 Flow of Control

The typical flow through the layers:

1. Client sends HTTP request to API endpoint
2. Controller receives request and forwards to Facade
3. Facade validates input and calls Business layer
4. Business layer processes the request, calling Data layer as needed
5. Data layer interacts with the database
6. Results propagate back up through the layers
7. Controller returns HTTP response to client

## 5. Service Details

### 5.1 Authentication Service

**Purpose**: Manages user authentication and JWT token generation/validation.

**Key Components**:
- JWT-based authentication
- Token generation and validation
- Authorization policies

```csharp
// Example JWT token generation from AuthenticationDomainLayer\Business\Business.cs
public async Task<AuthenticationServiceModel> GenerateTokenAsync(string username, string password)
{
    // Authentication logic
    if (username == "admin" && password == "password")
    {
        var key = Encoding.UTF8.GetBytes("super-secret-scary-password-a4h-aspire");
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return new AuthenticationServiceModel { UserName = username, Token = tokenString };
    }
    return null;
}
```

### 5.2 Product API

**Purpose**: Manages the cafe's product catalog and inventory.

**Key Components**:
- Product CRUD operations
- Product categorization
- Inventory management
- Product metadata for order routing

**Endpoints**:
- `GET /api/v1/product/{productId}`: Retrieve product details
- `POST /api/v1/product/create`: Create new product
- `PUT /api/v1/product/update`: Update existing product
- `DELETE /api/v1/product/delete/{productId}`: Delete product

### 5.3 Counter API

**Purpose**: Handles customer orders and payment processing.

**Key Components**:
- Order creation and management
- Payment processing
- Order routing to Kitchen and Barista services via Service Bus

**Endpoints**:
- `POST /api/v1/counter/order`: Submit new order
- `PUT /api/v1/counter/order`: Update existing order
- `GET /api/v1/counter/order/{orderId}`: Retrieve order details
- `POST /api/v1/counter/order/payment`: Process payment for order

### 5.4 Kitchen API

**Purpose**: Manages food preparation workflow.

**Key Components**:
- Order processing queue management
- Food preparation status tracking
- Integration with Counter API

**Endpoints**:
- `GET /api/v1/kitchen/orders`: Retrieve active orders
- `PUT /api/v1/kitchen/order/{orderId}/status`: Update food preparation status

### 5.5 Barista API

**Purpose**: Manages beverage preparation workflow.

**Key Components**:
- Beverage preparation queue management
- Beverage preparation status tracking
- Integration with Counter API

**Endpoints**:
- `GET /api/v1/barista/orders`: Retrieve active orders
- `PUT /api/v1/barista/order/{orderId}/status`: Update beverage preparation status

### 5.6 Order Summary API

**Purpose**: Provides order analytics and reporting.

**Key Components**:
- Order history and analytics
- Sales reporting
- Performance metrics

## 6. Infrastructure

### 6.1 Data Storage

- **Cosmos DB**: Primary data store for each microservice
- **Redis Cache**: Used for distributed caching to improve performance

### 6.2 Messaging

- **Azure Service Bus**: Used for asynchronous communication between services
- Topics: "purchased-orders" with subscriptions for "kitchen" and "barista"

### 6.3 Logging and Monitoring

- **Seq**: Centralized logging and diagnostic data
- Distributed tracing through .NET Aspire

### 6.4 Authentication and Authorization

- **JWT Authentication**: All microservices use JWT tokens for authentication
- **Authorization Policies**: Access control based on claims and roles

## 7. Development and Testing

### 7.1 Development Environment

- **.NET 9 SDK**: Core development platform
- **Visual Studio 2022**: Primary IDE
- **Angular CLI 19.x**: Frontend development

### 7.2 Testing Approach

The solution includes unit tests using MSTest with Moq for mocking:

```csharp
// Example test from CounterApi.TestLayer\BusinessTests.cs
[TestMethod]
[DataRow(true, DisplayName = "GetOrderAsync - Success")]
[DataRow(false, DisplayName = "GetOrderAsync - Failure")]
public async Task GetOrderAsync_ShouldHandleResultCorrectly(bool isSuccess)
{
    // Arrange
    var orderId = Guid.NewGuid();
    if (isSuccess)
    {
        _dataMock.Setup(d => d.GetOrderAsync(orderId)).ReturnsAsync(new OrderDomainModel());
    }
    else
    {
        _dataMock.Setup(d => d.GetOrderAsync(orderId)).ReturnsAsync((OrderDomainModel)null);
    }

    // Act
    var result = await _business.GetOrderAsync(orderId);

    // Assert
    if (isSuccess)
    {
        Assert.IsNotNull(result);
    }
    else
    {
        Assert.IsNull(result);
    }
    _dataMock.Verify(d => d.GetOrderAsync(orderId), Times.Once);
}
```

## 8. Deployment

### 8.1 Local Development

- **Intel64 AppHost**: For standard Windows/Linux development environments
- **Arm64 AppHost**: For Mac Silicon/M-series chip development

### 8.2 Azure Deployment

- **Azure AppHost**: Configuration for deploying to Azure
- **Managed Identity**: Used for secure access to Azure resources
- **Azure App Configuration**: Used to manage application settings

## 9. Security Considerations

- **JWT Authentication**: All API calls require valid JWT tokens
- **Secure Configuration**: Sensitive configuration data should be stored in Azure Key Vault
- **HTTPS**: All services should be configured to use HTTPS in production
- **Input Validation**: Facade layer validates all incoming requests using FluentValidation

## 10. Performance Considerations

- **Caching**: Redis cache is used to improve performance for frequently accessed data
- **Asynchronous Operations**: All operations use async/await pattern for non-blocking I/O
- **Microservices Scaling**: Each service can be scaled independently based on load

## 11. Known Limitations and Future Enhancements

- **Hardcoded JWT Secret**: The current implementation uses a hardcoded JWT secret; this should be moved to a secure configuration
- **Limited Authentication**: Authentication is currently a basic implementation and should be enhanced for production use
- **Payment Processing**: The payment processing is simulated; integration with real payment providers is needed for production

## 12. Troubleshooting

### 12.1 Common Issues

- **Service Bus Connection**: Ensure the connection string for Azure Service Bus is correctly configured
- **Cosmos DB Connection**: Verify Cosmos DB connection strings and database/container names
- **JWT Token Validation**: Ensure all services use the same JWT configuration for token validation

### 12.2 Logging

- All services use Seq for centralized logging
- Check Seq dashboard for error messages and diagnostic information
