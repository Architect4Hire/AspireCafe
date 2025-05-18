# Aspire Cafe Solution Overview

## Functional and Technical Requirements

### Functional Requirements
Aspire Cafe implements a comprehensive café point-of-sale system with the following key capabilities:

1. **Product Management**
   - Complete product catalog with food/beverage categorization
   - Product routing to specific preparation stations
   - Full CRUD operations and availability tracking

2. **Order Processing**
   - Customer order creation with details (customer, table, items)
   - Order calculation with subtotal, tax, and tip
   - Order routing and lifecycle tracking
   - Multi-payment method support with validation

3. **Kitchen & Barista Operations**
   - Specialized displays for kitchen and barista staff
   - Preparation status tracking
   - Multiple preparation station support (Grill, Fry, Pantry)
   - Item readiness tracking

4. **Reporting & Analytics**
   - Order analytics and summaries
   - Historical data querying
   - Business insights generation

5. **Security**
   - User authentication
   - Token validation
   - Secure login mechanisms

### Technical Requirements
The solution is built on modern cloud-native technologies:

1. **Architecture**
   - Microservices-based design using .NET 9 and Aspire
   - Multi-target deployment (Azure, ARM64, Intel64)
   - Service discovery for communication

2. **Data Storage**
   - CosmosDB for persistent storage
   - Redis for distributed caching
   - Service-isolated data contexts

3. **Communication**
   - Azure Service Bus for async messaging
   - RESTful API design with versioning
   - Result pattern for error handling

4. **Frontend**
   - Angular 19 with component architecture
   - Reactive programming (RxJS)
   - HTTP clients for API consumption

5. **Cross-Cutting Concerns**
   - FluentValidation for input validation
   - Distributed tracing and centralized logging (Seq)
   - Health monitoring endpoints
   - Secure authentication

## Architecture Overview

Aspire Cafe is built as a cloud-native, microservices-based application using .NET 9 Aspire, with an Angular 19 frontend. The solution follows a clean multi-layered architecture with a clear separation of concerns:

### Solution Structure

The solution is divided into multiple microservices, each responsible for a specific business domain:

```
Aspire Cafe
├── Aspire Cafe.UI                  # Angular frontend
├── Aspire Cafe.ProductApi          # Product catalog management
├── Aspire Cafe.CounterApi          # Order processing and payment
├── Aspire Cafe.KitchenApi          # Food preparation workflow
├── Aspire Cafe.BaristaApi          # Beverage preparation workflow
├── Aspire Cafe.OrderSummaryApi     # Order analytics and reporting
├── Aspire Cafe.ServiceDefaults     # Shared service configurations
├── Aspire Cafe.Shared              # Shared models and utilities
├── AppHost.Azure                  # Azure deployment host
├── AppHost.Arm64                  # Local ARM deployment host
└── AppHost.Intel64                # Local Intel64 deployment host
```

### Layer Architecture (Per Microservice)

Each microservice follows a layered architecture:

1. **API Layer**: Controllers handling HTTP requests and responses
2. **Domain Layer**: Business logic and domain models
   - **Facade**: Simplifies client interaction with complex subsystems
   - **Business**: Contains core business logic
   - **Data**: Data access and repository implementation
3. **Shared Layer**: Cross-cutting concerns, common models, and utilities

## Design Patterns and Implementation

### 1. Repository Pattern

The Repository Pattern abstracts data access logic and provides a clean API for working with domain models.

**Example implementation (interfaces):**

```csharp
// Interface definition
public interface IData
{
    Task<ProcessingOrderDomainModel> AddOrderAsync(ProcessingOrderDomainModel order);
    Task<ProcessingOrderDomainModel> UpdateOrderStatusAsync(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus);
    Task<List<ProcessingOrderDomainModel>> FetchActiveOrdersAsync();
}
```

**Value**: This pattern decouples the application from specific data storage implementations, making it easier to change data sources or add caching mechanisms without affecting business logic.

### 2. Facade Pattern

The Facade Pattern provides a simplified interface to a complex subsystem, hiding implementation details from clients.

**Example implementation:**

```csharp
public class Facade : IFacade
{
    private readonly IBusiness _business;

    public Facade(IBusiness business)
    {
        _business = business;
    }

    public async Task<Result<OrderUpdateServiceModel>> UpdateOrderStatusAsync(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus)
    {
        if (orderId == Guid.Empty)
        {
            return Result<OrderUpdateServiceModel>.Failure(Error.InvalidInput, new List<string>() { "Invalid Metadata" });
        }
        var result = await _business.UpdateOrderStatusAsync(orderId, orderProcessStation, orderProcessStatus);
        if (result == null)
        {
            return Result<OrderUpdateServiceModel>.Failure(Error.NotFound, new List<string>() { "Order not found." });
        }
        return Result<OrderUpdateServiceModel>.Success(result);
    }
}
```

**Value**: The Facade pattern simplifies client code by providing a unified interface to complex subsystems. It handles validation, error conditions, and wraps business logic results in a consistent Result<T> pattern.

### 3. Dependency Injection

The solution uses built-in .NET dependency injection to manage service lifetimes and dependencies.

**Example configuration:**

```csharp
void AddScopes(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IFacade, Facade>();
    builder.Services.AddScoped<IBusiness, Business>();
    builder.Services.AddScoped<IData, Data>();
}
```

**Value**: DI promotes loose coupling, makes testing easier through mocking, and facilitates configuration of service lifetimes.

### 4. Result Pattern

A custom Result<T> pattern handles operation outcomes, providing a consistent approach to success and error handling.

```csharp
public class Result<T> where T : ServiceBaseModel
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<string> Messages { get; set; }
    public Error Error { get; }
    public T? Data { get; set; }

    public static Result<T> Success(T data) => new(true, Error.None, null, data);
    public static Result<T> Failure(Error error, List<string>? messages) => new(false, error, messages, null);
}
```

Extension methods for consistent API responses:

```csharp
public static IActionResult Match<TModel>(this Result<TModel> result) where TModel : ServiceBaseModel
{
    if (result.IsSuccess)
    {
        return new OkObjectResult(result);
    }

    return result.Error switch
    {
        Error.NotFound => new NotFoundObjectResult(result),
        Error.InvalidInput => new BadRequestObjectResult(result),
        Error.Unauthorized => new UnauthorizedObjectResult(result),
        Error.Forbidden => new ForbidResult(),
        Error.InternalServerError => new ObjectResult(result) { StatusCode = 500 },
        _ => new ObjectResult(result) { StatusCode = 500 }
    };
}
```

**Value**: This pattern provides consistent error handling and response formatting across all APIs, improving developer experience and client code predictability.

### 5. Model Segregation

The solution uses different model types for different layers:

- **DomainModel**: Core business entities used within domain layer
- **ViewModel**: DTOs for API inputs 
- **ServiceModel**: DTOs for API responses

**Example:**

```csharp
// Domain model (internal use)
public class OrderProcessingLineItem
{
    public string ProductName { get; set; }
    public string Notes { get; set; }
}

// ViewModel (API input)
public interface IFacade
{
    Task<Result<ProductServiceModel>> CreateProductAsync(ProductViewModel product);
}

// ServiceModel (API output)
public interface IBusiness
{
    Task<ProductServiceModel> FetchProductByIdAsync(Guid productId);
}
```

**Value**: This separation prevents leaking implementation details, makes API contracts clearer, and allows each layer to have models specifically tailored to its needs.

### 6. API Controller Design

Controllers are kept thin, delegating logic to the underlying layers:

```csharp
[Authorize]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IFacade _facade;

    public ProductController(IFacade facade)
    {
        _facade = facade;
    }

    [HttpGet("{productId:guid}")]
    [ProducesResponseType(typeof(Result<ProductServiceModel>), 200)]
    [ProducesResponseType(typeof(Result<ProductServiceModel>), 404)]
    [ProducesResponseType(typeof(Result<ProductServiceModel>), 500)]
    public async Task<IActionResult> FetchProductById(Guid productId)
    {
        var result = await _facade.FetchProductByIdAsync(productId);
        return result.Match();
    }
}
```

**Value**: This approach keeps controllers focused on HTTP concerns while delegating business logic to appropriate layers, improving maintainability and testability.

## Cloud-Native Features

### 1. .NET Aspire Integration

Aspire Cafe leverages .NET Aspire for managing cloud-native application components:

- **Service Discovery**: Automatic registration and discovery of services
- **Configuration Management**: Centralized configuration through Aspire
- **Resource Management**: Simplified deployment of cloud resources

### 2. Distributed Tracing and Observability

The application integrates with Seq for logging and distributed tracing:

```csharp
public static void AddSeq(this WebApplicationBuilder builder)
{
    builder.AddSeqEndpoint("seq");
}
```

### 3. Azure Integration

Dedicated AppHost projects for different deployment environments:

- **AppHost.Azure**: Azure-specific configuration
- **AppHost.Arm64**: Optimized for ARM architecture (Mac/Apple Silicon)
- **AppHost.Intel64**: Optimized for Intel-based systems

### 4. Authentication and Authorization

JWT-based authentication is implemented across all microservices:

```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "super-secret-scary-password-a4h-aspire");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
```

## Frontend Architecture (Angular)

The frontend follows component-based architecture using Angular 19:

- **Reactive Programming**: RxJS for state management
- **Routing**: Angular Router for navigation
- **Services**: Communication with backend APIs
- **Component Structure**: Reusable UI components

## Testing Strategy

The solution includes various testing approaches:

- **Unit Tests**: Testing individual components with mocking
- **Integration Tests**: Using .NET Aspire's DistributedApplicationTestingBuilder to test service interactions
- **End-to-End Tests**: Testing full user scenarios

## Conclusion

Aspire Cafe demonstrates a modern, cloud-native application architecture using .NET 9 Aspire and Angular 19. By applying established design patterns like Repository, Facade, and Dependency Injection, the solution achieves:

1. **High Modularity**: Easy to extend or modify individual services
2. **Scalability**: Services can scale independently based on demand
3. **Maintainability**: Clear separation of concerns and consistent patterns
4. **Testability**: Design supports effective unit and integration testing
5. **Cloud-Ready**: Designed for deployment in various environments

This architecture provides a solid foundation for building complex, distributed applications that can evolve with changing business requirements.