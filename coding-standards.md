# ASP.NET Core Coding Standards and Best Practices

## 1. Architecture Overview

### Clean Layered Architecture

The Aspire Cafe solution implements a well-structured layered architecture that promotes separation of concerns, maintainability, and testability:

#### API Layer
- Contains controllers, middleware, and API configurations
- Acts as the entry point for all external requests
- Responsible only for routing requests to the appropriate domain layer

#### Domain Layer
- Separated into discrete projects (e.g., `Aspire Cafe.ProductApiDomainLayer`)
- Contains business logic, validation, data access, and domain models
- Organized into sublayers:
  - **Facade**: Acts as the application service layer, coordinating operations
  - **Business**: Implements business logic and rules
  - **Data**: Handles data persistence and retrieval

#### Shared Layer
- Common code, utilities, and cross-cutting concerns
- Extensions, middleware, enums, and result patterns used across projects

### Key Characteristics

- **Microservices Architecture**: Each functional area (Product, Kitchen, Barista, etc.) is implemented as a separate API
- **Cloud-Native**: Built on .NET Aspire with integrated Azure services
- **Domain-Driven Design**: Clear separation of domain models, business rules, and infrastructure

## 2. Project Structure and Organization

### API Project Structure
```
Aspire Cafe.[ServiceName]Api/
├── Controllers/         # REST API endpoints
├── Program.cs           # Application configuration and startup
├── Dockerfile           # Containerization setup
```

### Domain Layer Structure
```
Aspire Cafe.[ServiceName]ApiDomainLayer/
├── Business/            # Business logic implementation
├── Data/                # Data access layer implementation
├── Facade/              # Application service layer
├── Managers/
│   ├── Context/         # Database contexts
│   ├── Exceptions/      # Custom exceptions
│   ├── Models/          # Domain, service, and view models
│   │   ├── Domain/      # Internal domain models
│   │   ├── Service/     # Service models returned to API
│   │   └── View/        # DTO models accepted from API
│   └── Validators/      # Validation rules
```

### Shared Project Structure
```
Aspire Cafe.Shared/
├── Enums/               # Common enumerations
├── Extensions/          # Extension methods
├── Middleware/          # Shared middleware components
├── Models/              # Shared models
└── Results/             # Result pattern implementation
```

## 3. Coding Standards

### General C# Standards

1. **Naming Conventions**
   - **Classes/Interfaces**: PascalCase (e.g., `ProductController`, `IFacade`)
   - **Methods**: PascalCase (e.g., `FetchProductById`)
   - **Parameters/Variables**: camelCase (e.g., `productId`)
   - **Private Fields**: Use underscore prefix (e.g., `_facade`)
   - **Interfaces**: Prefix with 'I' (e.g., `IData`, `IBusiness`)

2. **File Organization**
   - One class per file
   - Filename matches class name
   - Group related files in appropriate folders

3. **Code Style**
   - Use `var` when type is obvious
   - Enable nullable reference types (`<Nullable>enable</Nullable>`)
   - Use implicit using directives (`<ImplicitUsings>enable</ImplicitUsings>`)
   - Follow consistent indentation and spacing

### API Design Standards

1. **Controller Standards**
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

2. **Route Conventions**
   - Use lowercase routes (configured in `PipelineExtensions.cs`)
   - Include API versioning in route path (`api/v{v:apiVersion}/[controller]`)
   - Use appropriate HTTP verbs (GET, POST, PUT, DELETE)
   - Use meaningful route names and parameters

3. **Response Conventions**
   - Always return the Result pattern object
   - Document responses with `ProducesResponseType` attributes
   - Include comprehensive XML documentation for API endpoints

### API Layer Implementation

1. **Thin Controllers**
   - Controllers should only:
     - Receive requests
     - Pass data to the facade layer
     - Return appropriate HTTP responses
   - No business logic in controllers
   - Use dependency injection for services

2. **API Configuration in Program.cs**
```csharp
var builder = WebApplication.CreateBuilder(args);
SetUpBuilder(builder);
var app = builder.Build();
SetUpApp(app);
app.Run();

void SetUpBuilder(WebApplicationBuilder builder)
{
    builder.AddServiceDefaults();
    builder.AddRedisDistributedCache("cache");
    AddDatabases(builder); 
    AddScopes(builder); 
    AddFluentValidation(builder);
    builder.AddVersioning(1);
    builder.AddExceptionHandling();
    builder.AddUniversalConfigurations();
       builder.AddSeq();
       AddAuthentication(builder);
   }
```

3. **Security and Authentication**
   - Always use JWT authentication
   - Configure authorization policies
   - Secure sensitive endpoints with `[Authorize]` attribute

## 4. Domain Layer Patterns

### Facade Pattern

The Facade layer acts as an entry point to the domain layer, handling:
- Input validation
- Error handling and wrapping results
- Orchestrating business layer calls

```csharp
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
            return Result<ProductServiceModel>.Failure(Error.InvalidInput, 
                validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }
        var data = await _business.CreateProductAsync(product);
        return Result<ProductServiceModel>.Success(data);
    }
}
```

#### Key Responsibilities:
- Input validation using FluentValidation
- Wrapping responses in the Result pattern
- Error handling and conversion
- No direct data access

### Business Layer Pattern

The Business layer implements core business logic:

```csharp
public class Business : IBusiness
{
    private readonly IData _data;

    public Business(IData data)
    {
        _data = data;
    }

    public async Task<ProductServiceModel> CreateProductAsync(ProductViewModel product)
    {
        var data = await _data.CreateProductAsync(product.MapToDomainModel());
        return data.MapToServiceModel();
    }
}
```

#### Key Responsibilities:
- Implementing business rules and logic
- Orchestrating data operations
- Model mapping between layers
- No direct HTTP or external concerns

### Data Layer Pattern

The Data layer handles persistence concerns:

```csharp
public class Data : IData
{
    private readonly ProductContext _context;
    
    public Data(ProductContext context)
    {
        _context = context;
    }
    
    public async Task<ProductDomainModel> CreateProductAsync(ProductDomainModel product)
    {
        product.Id = Guid.NewGuid();
        product.CreatedDate = DateTime.UtcNow;
        product.ModifiedDate = DateTime.UtcNow;
        product.DocumentType = DocumentType.Product.ToString();
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }
}
```

#### Key Responsibilities:
- Database interactions
- CRUD operations
- Query execution
- No business logic

## 5. Key Design Patterns and Practices

### Result Pattern

The solution uses a Result pattern for consistent error handling:

```csharp
public class Result<T> where T : ServiceBaseModel
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<string> Messages { get; set; }
    public Error Error { get; }
    public T? Data { get; set; }
    
    public static Result<T> Success(T data) => new(true, Error.None, null, data);
    public static Result<T> Failure(Error error, List<string>? messages) => 
        new(false, error, messages, null);
}
```

Usage in controllers via extension method:
```csharp
public async Task<IActionResult> FetchProductById(Guid productId)
{
    var result = await _facade.FetchProductByIdAsync(productId);
    return result.Match();
}
```

The `Match()` extension method translates Result objects to appropriate HTTP responses:
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

### Validation Pattern

The solution uses FluentValidation for consistent input validation:

```csharp
public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
{
    public ProductViewModelValidator()
    {
        RuleFor(x => x.ProductType).IsInEnum();
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .Length(1, 100)
            .WithMessage("Name must be between 1 and 100 characters.");
        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Price is required.")
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }
}
```

Registration:

```csharp
void AddFluentValidation(WebApplicationBuilder builder)
{
    builder.Services.AddValidatorsFromAssemblyContaining<ProductViewModelValidator>();
}
```

### Exception Handling Pattern

Global exception handling with a consistent approach:

```csharp
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, 
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();
        problemDetails.Instance = httpContext.Request.Path;
        problemDetails.Title = exception.Message;
        logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);
        problemDetails.Status = httpContext.Response.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
```

Registration:

```csharp
public static void AddExceptionHandling(this WebApplicationBuilder builder)
{
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
}
```

### Dependency Injection Pattern

The solution follows consistent dependency injection practices:

```csharp
void AddScopes(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IFacade, Facade>();
    builder.Services.AddScoped<IBusiness, Business>();
    builder.Services.AddScoped<IData, Data>();
}
```

## 6. API Versioning Strategy

The solution implements a consistent API versioning approach:

```csharp
public static void AddVersioning(this WebApplicationBuilder builder, int primaryVersion)
{
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(primaryVersion, 0);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("x-api-version"),
            new QueryStringApiVersionReader("api-version")
        );
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = false;
    });
    builder.Services.AddOpenApi();
    builder.Services.AddOpenApi($"v{primaryVersion}");
}
```

## 7. Authentication and Authorization

JWT authentication is implemented consistently across services:

```csharp
void AddAuthentication(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? 
            "super-secret-scary-password-a4h-aspire");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // In production, set to true with a valid issuer
            ValidateAudience = false, // In production, set to true with a valid audience
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAuthenticatedUser", policy =>
            policy.RequireAuthenticatedUser());
    });
}
```

Authorization is applied at the controller or method level:
```csharp
[Authorize]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    // Controller implementation
}
```

## 8. Cloud-Native Considerations

The solution is built with cloud-native principles:

1. **Aspire Integration**
   - Service defaults and health checks
   - Distributed tracing and logging
   - Service discovery

2. **Azure Service Integration**
```csharp
void AddDatabases(WebApplicationBuilder builder)
{
    builder.AddCosmosDbContext<ProductContext>("Aspire Cafe", "Aspire Cafe");
}

void AddServiceBus(WebApplicationBuilder builder)
   {
       builder.AddAzureServiceBusClient("serviceBusConnection");
   }
```

3. **Distributed Caching**
```csharp
builder.AddRedisDistributedCache("cache");
```

## 9. Model Mapping Strategy

The solution follows a consistent approach to model mapping:

1. **Model Types**
   - **View Models**: DTOs for incoming API interactions
   - **Domain Models**: Internal representation within the domain layer
   - **Service Models**: Models returned from the business layer to the facade (outbound DTO)

2. **Mapping Approach**
```csharp
public async Task<ProductServiceModel> CreateProductAsync(ProductViewModel product)
{
    var data = await _data.CreateProductAsync(product.MapToDomainModel());
    return data.MapToServiceModel();
   }
```

## 10. General Coding Principles

### SOLID Principles
- **Single Responsibility**: Each class should have only one reason to change
- **Open/Closed**: Classes should be open for extension but closed for modification
- **Liskov Substitution**: Derived classes must be substitutable for their base classes
- **Interface Segregation**: Clients shouldn't depend on interfaces they don't use
- **Dependency Inversion**: Depend on abstractions, not concretions

### Self-documenting Code
- Write code that clearly expresses intent without requiring extensive comments
- Use meaningful method and variable names that describe their purpose
- Break complex logic into well-named helper methods

```csharp
// Avoid this
public bool Check(Order o)
{
    return o.Items.Any(i => i.Price > 50);
}

// Prefer this
public bool ContainsHighValueItems(Order order)
{
    return order.Items.Any(item => item.Price > Constants.HighValueThreshold);
}
```

### Method Design
- Keep methods short and focused on a single responsibility
- Methods should ideally be less than 30 lines
- Extract complex conditionals into readable predicate methods

## 11. Code Structure & Formatting

### Naming Conventions (Enhanced)
- Use descriptive, intention-revealing names
- Avoid abbreviations unless they're widely understood
- Be consistent with existing codebase conventions

### Constants and Magic Numbers
- Replace magic numbers with named constants
- Define constants in a logical location (class-specific or application-wide)

```csharp
// Avoid
if (product.Price > 50)

// Prefer
private const decimal HighValueThreshold = 50m;

if (product.Price > HighValueThreshold)
```

### Code Grouping
- Group related methods and properties together
- Maintain a consistent order of members:
  1. Private fields
  2. Properties
  3. Constructors
  4. Public methods
  5. Private methods

## 12. Error Handling & Logging (Enhanced)

### Exception Handling
- Never silently catch exceptions without appropriate handling
- Use specific exception types rather than generic exceptions
- Implement retry logic for transient failures using Polly

```csharp
// Implement retry policy
var retryPolicy = Policy
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(3, retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

await retryPolicy.ExecuteAsync(async () => await httpClient.GetAsync(url));
```

### Structured Logging
- Use structured logging for better searchability
- Include contextual information in log messages
- Log appropriate levels (Debug, Info, Warning, Error)

```csharp
// Structured logging with context
logger.LogInformation(
    "Order {OrderId} created for customer {CustomerId} with {ItemCount} items",
    order.Id, order.CustomerId, order.Items.Count);
```

## 13. Performance & Security (Enhanced)

### Asynchronous Programming
- Use async/await for I/O-bound operations
- Avoid blocking calls in async methods
- Use ConfigureAwait(false) when appropriate in library code

```csharp
// Proper async implementation
public async Task<OrderStatus> CheckOrderStatusAsync(Guid orderId)
{
    var order = await _repository.GetOrderByIdAsync(orderId)
        .ConfigureAwait(false);
    return order?.Status ?? OrderStatus.NotFound;
}
```

### Caching Strategy
- Implement caching for frequently accessed, rarely changing data
- Use appropriate cache invalidation strategies
- Consider distributed caching for multi-instance environments

```csharp
public async Task<List<ProductCategory>> GetCategoriesAsync()
{
    return await _cache.GetOrCreateAsync("product-categories", async entry => {
        entry.SetAbsoluteExpiration(TimeSpan.FromHours(24));
        return await _repository.GetAllCategoriesAsync();
    });
}
```

### Input Validation and Security
- Validate all input at the system boundaries
- Use Anti-XSRF tokens for forms
- Implement proper authentication and authorization

## 14. Testing Best Practices

### Unit Testing
- Write unit tests for all business logic
- Follow Arrange-Act-Assert pattern
- Use mocking frameworks for dependencies

```csharp
[Fact]
public async Task CreateOrder_WithValidItems_ReturnsSuccess()
{
    // Arrange
    var orderViewModel = new OrderViewModel { /* test data */ };
    _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<OrderViewModel>(), default))
        .ReturnsAsync(new ValidationResult());
    
    // Act
    var result = await _facade.CreateOrderAsync(orderViewModel);
    
    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
    Assert.NotEqual(Guid.Empty, result.Data.Id);
}
```

### Test Coverage
- Aim for high test coverage of business-critical code
- Focus on testing behavior, not implementation details
- Include edge cases and error conditions in tests

### CI/CD Integration
- Run tests automatically in CI/CD pipeline
- Maintain a quality gate based on test pass rate
- Include static code analysis with tools like SonarQube

## 15. API Design Principles (Enhanced)

### REST Best Practices
- Use appropriate HTTP methods for operations
- Return meaningful HTTP status codes
- Design resource-focused endpoints

```csharp
// Resource-based endpoint design
[HttpGet]                 // GET /api/v1/products - List all products
[HttpGet("{id}")]         // GET /api/v1/products/123 - Get one product
[HttpPost]                // POST /api/v1/products - Create product
[HttpPut("{id}")]         // PUT /api/v1/products/123 - Update product
[HttpDelete("{id}")]      // DELETE /api/v1/products/123 - Delete product
```

### API Versioning (Enhanced)
- Support multiple versions simultaneously during transitions
- Document deprecation schedules for older versions
- Use Swagger/OpenAPI to document all versions

## Conclusion

These enhanced coding standards build on the existing foundation to promote maintainable, secure, and efficient code. By following these practices consistently across all services in the Aspire Cafe solution, teams can ensure a cohesive codebase that's easier to maintain and extend.

Remember that coding standards should evolve with the project and technology landscape. Regularly review and update these standards as the team identifies new best practices or challenges.

## Implementation Checklist

When implementing a new API endpoint, follow these steps:

1. **Define Models**
   - Create view model (input DTO)
   - Create domain model (if not exists)
   - Create service model (output DTO)

2. **Implement Data Layer**
   - Add interface method to `IData`
   - Implement method in `Data` class
   - Focus on persistence operations only

3. **Implement Business Layer**
   - Add interface method to `IBusiness`
   - Implement method in `Business` class
   - Add business logic and model mapping

4. **Implement Facade Layer**
   - Add interface method to `IFacade`
   - Implement method in `Facade` class
   - Add validation and result wrapping

5. **Implement Controller Endpoint**
   - Add method to appropriate controller
   - Document with XML comments
   - Specify response types with `ProducesResponseType`
   - Use Result pattern with `Match()` extension

6. **Add Validation Rules**
   - Create or update validator class for view model
   - Implement FluentValidation rules

