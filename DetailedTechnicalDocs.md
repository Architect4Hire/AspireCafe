# AspireCafe System - Technical Documentation

## 1. Solution Overview

AspireCafe is a modern .NET 9 microservices-based application built with C# 13, designed to support cafe operations through a well-architected, distributed system. The solution follows clean architecture principles with clearly defined separation of concerns across multiple specialized APIs.

## 2. Architecture

### 2.1 High-Level Architecture

The solution is structured as a distributed system with multiple microservices, each responsible for a specific functional domain:

- **ProductApi**: Manages the product catalog, menu items, and product information
- **CounterApi**: Handles order processing, payment handling, and counter operations
- **KitchenApi**: Manages food preparation and kitchen operations

The system is containerized and orchestrated with .NET Aspire for reliable deployment and service management.

### 2.2 Design Patterns and Practices

#### 2.2.1 Clean Architecture

The system implements a layered architecture across all services:

1. **Presentation Layer**: API controllers exposing RESTful endpoints
2. **Domain Layer**: Business logic, validation, and domain models
3. **Data Layer**: Data access and persistence logic

#### 2.2.2 Facade Pattern

The Facade pattern is used consistently across all services to provide a simplified interface to complex subsystems:


```csharp
// Example from ProductApi
public interface IFacade
{
    Task<Result<ProductServiceModel>> FetchProductByIdAsync(Guid productId);
    Task<Result<ProductServiceModel>> CreateProductAsync(ProductViewModel product);
    Task<Result<ProductServiceModel>> UpdateProductAsync(ProductViewModel product);
    Task<Result<ProductServiceModel>> DeleteProductAsync(Guid productId);
}

```

#### 2.2.3 Result Pattern

A generic `Result<T>` class provides consistent error handling and success/failure responses:


```csharp
public class Result<T> where T : ServiceBaseModel
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public List<string> Messages { get; set; }
    public Error Error { get; }
    public T? Data { get; set; }
    
    // Factory methods
    public static Result<T> Success(T data);
    public static Result<T> Failure(Error error, List<string>? messages);
}

```

#### 2.2.4 Repository Pattern

Data access is abstracted through interfaces that follow the Repository pattern.

#### 2.2.5 Validation

FluentValidation is used for request validation across all services.

## 3. API Controllers

### 3.1 ProductApi Controllers

#### 3.1.1 ProductController

Responsible for CRUD operations on individual product entities.

| Endpoint | Method | Description | Status Codes |
|----------|--------|-------------|-------------|
| `api/product/{productId:guid}` | GET | Fetches a product by ID | 200, 404, 500 |
| `api/product/create` | POST | Creates a new product | 201, 400, 500 |
| `api/product/update` | PUT | Updates an existing product | 200, 400, 404, 500 |
| `api/product/delete/{productId:guid}` | DELETE | Deletes a product | 200, 404, 500 |

**Key Implementation Details:**


```csharp
// Dependency Injection
private readonly IFacade _facade;

public ProductController(IFacade facade)
{
    _facade = facade;
}

// Example Endpoint
[HttpPost("create")]
[ProducesResponseType(typeof(Result<ProductServiceModel>), 201)]
[ProducesResponseType(typeof(Result<ProductServiceModel>), 400)]
[ProducesResponseType(typeof(Result<ProductServiceModel>), 500)]
public async Task<Result<ProductServiceModel>> CreateProduct(ProductViewModel product)
{
    var result = await _facade.CreateProductAsync(product);
    return result.Match(
        onSuccess: () => result,
        onFailure: error => Result<ProductServiceModel>.Failure(error, result.Messages)
    );
}

```

#### 3.1.2 CatalogController

Manages the product catalog and product metadata.

| Endpoint | Method | Description | Status Codes |
|----------|--------|-------------|-------------|
| `api/catlog/fetch` | GET | Retrieves the full product catalog | 200, 400, 500 |
| `api/catlog/fetch/metadata` | POST | Fetches metadata for specific products | 200, 400, 500 |

**Key Implementation Details:**


```csharp
// Dependency Injection
private readonly ICatalogFacade _facade;

public CatlogController(ICatalogFacade facade)
{
    _facade = facade;
}

// Example Endpoint
[HttpGet("fetch")]
[ProducesResponseType(typeof(Result<CatalogServiceModel>), 200)]
[ProducesResponseType(typeof(Result<CatalogServiceModel>), 400)]
[ProducesResponseType(typeof(Result<CatalogServiceModel>), 500)]
public async Task<Result<CatalogServiceModel>> FetchCatalog()
{
    var result = await _facade.FetchCatalog();
    return result.Match(
        onSuccess: () => result,
        onFailure: error => Result<CatalogServiceModel>.Failure(error, result.Messages)
    );
}

```

**Data Models:**

- `CatalogServiceModel`: Contains a dictionary mapping category names to lists of catalog items
- `CatalogItemServiceModel`: Represents a simplified product view for catalog display
- `ProductMetaDataViewModel`: Contains a list of product IDs for metadata lookup
- `ProductMetaDataServiceModel`: Maps product IDs to their routing information

### 3.2 CounterApi Controllers

#### 3.2.1 CounterController

Manages order processing and payment handling at the counter.

| Endpoint | Method | Description | Status Codes |
|----------|--------|-------------|-------------|
| `api/counter/submitorder` | POST | Creates a new order | 200, 400, 500 |
| `api/counter/getorder/{orderId:guid}` | GET | Retrieves an order by ID | 200, 500 |
| `api/counter/updateorder` | PUT | Updates an existing order | 200, 400, 500 |
| `api/counter/payorder` | POST | Processes payment for an order | 200, 500 |

**Key Implementation Details:**


```csharp
// Dependency Injection
private readonly IFacade _facade;

public CounterController(IFacade facade)
{
    _facade = facade;
}

// Example Endpoint
[HttpPost("SubmitOrder")]
[ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status500InternalServerError)]
public async Task<Result<OrderServiceModel>> SubmitOrder(OrderViewModel order)
{
    var result = await _facade.SubmitOrderAsync(order);
    return result.Match(
        onSuccess: () => result,
        onFailure: error => Result<OrderServiceModel>.Failure(error, result.Messages)
    );
}

```

**Data Models:**

- `OrderViewModel`: Contains order details, line items, and payment information
- `OrderServiceModel`: Service-layer representation of an order with header, line items, and footer
- `OrderPaymentViewModel`: Contains payment details for processing order payment

### 3.3 KitchenApi Controllers

The KitchenApi project structure exists but currently does not have implemented controllers. This is likely planned for future development to manage kitchen operations like order preparation, cooking status, and kitchen inventory.

## 4. Model Structure

### 4.1 Model Layers

Each API follows a consistent three-layer model approach:

1. **View Models**: Used for API requests/responses
   
```csharp
   public class ProductViewModel : ViewModelBase
   {
       public Guid ProductId { get; set; }
       public string Name { get; set; } = string.Empty;
       public string Description { get; set; } = string.Empty;
       public decimal Price { get; set; }
       // Other properties
   }
   
```

2. **Service Models**: Used within business logic
   
```csharp
   public class ProductServiceModel : ServiceBaseModel
   {
       public string ProductId { get; set; } = string.Empty;
       public string Name { get; set; } = string.Empty;
       public string Description { get; set; } = string.Empty;
       public decimal Price { get; set; }
       // Other properties
   }
   
```

3. **Domain Models**: Used for persistence operations

### 4.2 Key Enumerations

The system uses several enumerations to maintain data consistency:

- **ProductType**: Food, Beverage, Dessert, Snack
- **ProductCategory**: Appetizer, MainCourse
- **RouteType**: Used for product metadata routing
- **Error**: None, NotFound, InvalidInput, Unauthorized, Forbidden, InternalServerError
- **OrderType**: Used in the CounterApi for order classification
- **PaymentStatus**: Tracks payment state of orders
- **PaymentMethod**: Defines payment options

## 5. Infrastructure

### 5.1 Data Storage

Azure Cosmos DB is used for data persistence:


```csharp
builder.AddCosmosDbContext<ProductContext>("aspireCafe", "AspireCafe");
builder.AddCosmosDbContext<CounterContext>("aspireCafe", "AspireCafe");

```

### 5.2 Caching

Redis is used for distributed caching in the ProductApi:


```csharp
builder.AddRedisDistributedCache("cache");

```

### 5.3 Exception Handling

Global exception handling is consistent across all APIs:


```csharp
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

```

### 5.4 API Documentation

OpenAPI/Swagger is configured for all API services:


```csharp
builder.Services.AddOpenApi();

```

## 6. Cross-Cutting Concerns

### 6.1 Validation

FluentValidation is used across all services:


```csharp
builder.Services.AddValidatorsFromAssemblyContaining<ProductViewModelValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<OrderViewModelValidator>();

```

### 6.2 Result Handling

The `Match` pattern provides elegant result handling across all controllers:


```csharp
return result.Match(
    onSuccess: () => result,
    onFailure: error => Result<ProductServiceModel>.Failure(error, result.Messages)
);

```

### 6.3 URL Consistency

All APIs enforce lowercase URLs for consistency:


```csharp
builder.Services.AddRouting(options => options.LowercaseUrls = true);

```

## 7. Request Flow

### 7.1 ProductApi Request Flow

1. Client request arrives at the ProductController or CatalogController
2. Controller delegates to the appropriate Facade method
3. Facade performs validation using FluentValidation
4. Business layer executes business logic
5. Data layer handles persistence operations
6. Results flow back through the layers with appropriate success/error handling

### 7.2 CounterApi Request Flow

1. Client request arrives at the CounterController
2. Controller delegates to the appropriate Facade method
3. Facade performs validation and prepares the operation
4. Business layer processes the order or payment
5. Data layer performs persistence operations
6. Results flow back with the appropriate status codes and messages

## 8. Deployment

The solution leverages .NET Aspire for deployment orchestration:


```csharp
builder.AddServiceDefaults();
// Service configuration
app.MapDefaultEndpoints();

```

## 9. Development Workflow

### 9.1 Adding New Features

1. Define controller endpoints with proper documentation
2. Implement Facade methods with validation
3. Implement business logic in the appropriate Business class
4. Create or update data access methods
5. Test the full flow

### 9.2 Common Support Scenarios

- **Invalid Input**: Check validation rules and input data structure
- **Not Found Errors**: Verify entity existence in the database
- **Internal Server Errors**: Review application logs for exceptions

## 10. Security Considerations

- HTTPS is enforced across all endpoints
- Authorization middleware is in place
- Input validation protects against injection attacks
- Error messages are sanitized in production

## 11. Extensibility

The modular, service-oriented architecture allows for:
- Adding new controllers with minimal impact on existing ones
- Extending service capabilities through new endpoints
- Integrating additional services (like the planned KitchenApi)
- Adding new data models and business logic