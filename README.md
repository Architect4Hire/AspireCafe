# Aspire Café: A Modern .NET 9 Microservices Architecture

The Aspire Café solution showcases a cutting-edge approach to building cloud-native applications using .NET 9 and the new .NET Aspire application stack. This blog post breaks down the architecture, components, and design patterns that make this solution both robust and scalable.

## Microservices Architecture

The solution follows a **microservices architecture**, with several specialized APIs working together:

- **Product API**: Manages the product catalog and inventory, supporting full CRUD operations
- **Counter API**: Handles order processing, payments, and order state management
- **Kitchen API**: Manages food preparation workflows and kitchen-specific business rules
- **Barista API**: Handles beverage preparation workflows and beverage-specific business rules
- **Order Summary API**: Aggregates order information across services for reporting and analytics

Each service has its own responsibility, following the **Single Responsibility Principle**, allowing for independent scaling and deployment. The services communicate through well-defined APIs and asynchronous messaging via Azure Service Bus.

## Key Technical Components

### .NET Aspire Application Host(s)

The solution features two distinct Aspire Application Host projects:

1. **Aspirecafe.Apphost**: Development environment configured with emulators:
   
```csharp
   var keyvault = builder.AddAzureKeyVaultEmulator("keyvault", ContainerLifetime.Persistent);
   var cosmos = builder.AddAzureCosmosDB("cosmos").RunAsEmulator(...);
   var cache = builder.AddRedis("cache").WithLifetime(ContainerLifetime.Persistent);
   var servicebus = builder.AddAzureServiceBus("servicebus").RunAsEmulator(...);
   
```

2. **AppHostAzure**: Production-ready configuration using connection strings:
   
```csharp
   var keyvault = builder.AddConnectionString("keyvault");
   var cosmos = builder.AddConnectionString("cosmos");
   var cache = builder.AddConnectionString("cache");
   var servicebus = builder.AddConnectionString("servicebus");
   
```

This dual-host approach provides a seamless transition from development to production environments, with consistent dependencies and configurations.

### Infrastructure Components

The solution leverages several cloud-native services, all managed through Aspire:

- **Azure Cosmos DB**: Document database for storing product and order information, with dedicated containers for different document types 
- **Redis Cache**: Distributed caching for improved performance, with Redis Insight and Redis Commander for monitoring
- **Azure Service Bus**: Message broker with topics and subscriptions for asynchronous communication between services
- **Azure Key Vault**: Secure credential storage (using an emulator for development)

Each of these components is containerized for local development, ensuring consistency across environments.

## Design Patterns and Practices

### Layered (Onion Style) Architecture

Each microservice implements a **clean layered architecture**:

1. **API Layer**: Controllers handling HTTP requests (e.g., `ProductController`, `CatalogController`)
2. **Facade Layer**: Orchestrates business logic and validation (e.g., `IFacade`, `ICatalogFacade`)
3. **Business Layer**: Implements core business rules (e.g., `IBusiness`, `ICatalogBusiness`)
4. **Data Layer**: Manages data access and persistence (e.g., `IData`, `ICatalogData`)

This separation of concerns improves maintainability and testability, with each layer having clear responsibilities and dependencies flowing inward.

### Dependency Injection

The solution extensively uses dependency injection for loose coupling:


```csharp
// Service registration
builder.Services.AddScoped<IFacade, Facade>();
builder.Services.AddScoped<IBusiness, Business>();
builder.Services.AddScoped<IData, Data>();

// Constructor injection
public Facade(IBusiness business, IDistributedCache cache)
{
    _business = business;
    _validator = new ProductViewModelValidator();
}

```

This approach enhances testability and makes the system more modular and maintainable.

### Facade Pattern

The facade pattern is used extensively to provide a simplified interface to complex subsystems. For example, in the `Facade.cs` files:


```csharp
public class Facade : IFacade
{
    private readonly IBusiness _business;
    private readonly ProductViewModelValidator _validator;
    
    public async Task<Result<ProductServiceModel>> CreateProductAsync(ProductViewModel product)
    {
        // Validation logic
        var validationResult = await _validator.ValidateAsync(product);
        if (!validationResult.IsValid)
        {
            return Result<ProductServiceModel>.Failure(Error.InvalidInput, 
                validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }
        // Business logic delegation
        var data = await _business.CreateProductAsync(product);
        return Result<ProductServiceModel>.Success(data);
    }
}

```

This pattern decouples the client (controller) from the implementation details of the business logic, providing a clean separation of concerns.

### Result Pattern

A custom `Result<T>` type is used throughout the application to handle operation outcomes in a consistent way:


```csharp
public class Result<T> where T : ServiceBaseModel
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<string> Messages { get; set; }
    public Error Error { get; }
    public T? Data { get; set; }

    // Factory methods
    public static Result<T> Success(T data) => new(true, Error.None, null, data);
    public static Result<T> Failure(Error error, List<string>? messages) => 
        new(false, error, messages, null);
}

```

Controllers use functional-style processing of these results:


```csharp
return result.Match(
    onSuccess: () => result,
    onFailure: error => Result<ProductServiceModel>.Failure(error, result.Messages)
);

```

This functional approach avoids exception-based flow control and provides clear error information.

### Validation Pattern

**FluentValidation** is used for model validation, separating validation rules from the models themselves:


```csharp
var validationResult = await _validator.ValidateAsync(product);
if (!validationResult.IsValid)
{
    return Result<ProductServiceModel>.Failure(Error.InvalidInput, 
        validationResult.Errors.Select(x => x.ErrorMessage).ToList());
}

```

Validators are registered using assembly scanning:


```csharp
builder.Services.AddValidatorsFromAssemblyContaining<ProductViewModelValidator>();

```

This ensures robust input validation before business operations.

### API Documentation

The controllers use **XML Documentation** and **ProducesResponseType** attributes to generate comprehensive API documentation:


```csharp
/// <summary>
/// Creates a new product.
/// </summary>
/// <param name="product">The product details to be created.</param>
/// <returns>
/// A <see cref="Result{T}"/> containing a <see cref="ProductServiceModel"/> if the operation is successful.
/// Returns a failure result with error details if the operation fails.
/// </returns>
/// <response code="201">Indicates that the product was created successfully.</response>
/// <response code="400">Indicates a bad request due to invalid input.</response>
/// <response code="500">Indicates an internal server error.</response>
[HttpPost("create")]
[ProducesResponseType(typeof(Result<ProductServiceModel>), 201)]

```

This makes the API self-documenting and integrates well with OpenApi and Scalar.net (an excellent replacement for Swashbuckle).

### Frontend Integration

The solution includes an Angular 19 frontend:


```json
"dependencies": {
  "@angular/animations": "^19.0.0",
  "@angular/common": "^19.0.0",
  "@angular/core": "^19.0.0",
  // Other Angular dependencies
}

```

This provides a modern single-page application experience that communicates with the backend APIs.

## Modern C# and .NET Features

The solution leverages the latest features of C# 13 and .NET 9:

- **Async/await** patterns for non-blocking operations
- **Pattern matching** with the `Match` method for functional-style error handling
- **Extension methods** for cleaner code organization
- **Middleware** for cross-cutting concerns like exception handling
- **.NET Aspire** for distributed application orchestration

## Conclusion

The Aspire Café solution demonstrates how modern .NET applications can be built using a microservices architecture with the new .NET Aspire stack. By leveraging clean design patterns, cloud-native infrastructure, and the latest language features, the application achieves both scalability and maintainability.

The dual host approach (development vs. production) provides a seamless path to cloud deployment, while the consistent layering and separation of concerns ensure that each service can be developed, tested, and deployed independently.

This architecture provides a solid foundation for building complex, distributed systems that can evolve independently while working together seamlessly, setting the stage for future enhancements and scaling as the application grows.