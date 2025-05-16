# AspireCafe - Technical Documentation

## 1. System Overview

AspireCafe is a cloud-native, microservices-based point-of-sale system designed for cafes. The solution follows a modern distributed architecture that leverages .NET 9 Aspire for cloud-native capabilities and Angular 19 for the frontend user interface.

## 2. Architectural Overview

### 2.1 System Architecture

AspireCafe follows a microservices architecture with the following core components:

- **Frontend**: Angular 19-based UI application
- **Backend Microservices**:
  - Product API: Manages product catalog and inventory
  - Counter API: Handles customer orders and payments
  - Kitchen API: Manages food preparation workflow
  - Barista API: Manages beverage preparation workflow
  - Order Summary API: Provides order analytics and reporting

  ![System Architecture](https://github.com/Architect4Hire/AspireCafe/blob/dev/images/system.png)

### 2.2 Deployment Architecture

The solution is designed to be cloud-native and can be deployed in the following ways:

- **Azure Cloud Deployment**: Using AppHost.Azure
- **Local Development**:
  - Intel64 systems: Using AppHost.Intel64
  - ARM64 systems (Apple Silicon/Mx Chipset): Using AppHost.Arm64

### 2.3 Communication Patterns

The system uses a combination of:
- **Synchronous Communication**: RESTful APIs for direct service-to-service communication
- **Asynchronous Communication**: Azure Service Bus for event-driven messaging between services

## 3. Layer Architecture

Each microservice follows a consistent layered architecture:

### 3.1 API Layer

- **Responsibility**: Exposes HTTP endpoints, handles request/response formatting, and applies API versioning
- **Components**: API Controllers, API versioning configuration, Swagger/OpenAPI documentation
- **Key Implementation**: Controllers inherit from `ControllerBase` and use attributes like `[ApiController]` and `[Route]`

### 3.2 Facade Layer

- **Responsibility**: Orchestrates multiple business operations and simplifies the API layer interface
- **Pattern**: Implements the Facade design pattern
- **Components**: Interface (e.g., `IFacade`, `ICatalogFacade`) and implementation classes

### 3.3 Business Layer

- **Responsibility**: Contains business logic and business rules
- **Components**: Business managers and services (e.g., `IBusiness`, `ICatalogBusiness`)
- **Key Implementation**: Business classes contain the application's core logic

### 3.4 Data Access Layer

- **Responsibility**: Handles data persistence and retrieval from databases
- **Pattern**: Repository pattern
- **Components**: Data access interfaces (e.g., `IData`, `ICatalogData`) and implementations
- **Database Technology**: Azure Cosmos DB, accessed via Entity Framework Core

### 3.5 Domain Layer

- **Responsibility**: Contains domain entities, value objects, and business models
- **Components**: Various model types:
  - Domain Models (e.g., `ProductDomainModel`) - database POCO with light events if needed
  - Service Models (e.g., `ProductServiceModel`) - POCO for responses from ActionMethods 
  - View Models (e.g., `ProductViewModel`) - incoming POCO for requests for ActionMethods

## 4. Design Patterns and Practices

### 4.1 Repository Pattern

Applied in the data access layer to abstract database operations:


```csharp
public interface IData
{
    Task<ProductDomainModel> FetchProductByIdAsync(Guid productId);
    Task<ProductDomainModel> CreateProductAsync(ProductDomainModel product);
    Task<ProductDomainModel> UpdateProductAsync(ProductDomainModel product);
    Task<ProductDomainModel> DeleteProductAsync(Guid productId);
}

```

### 4.2 Facade Pattern

Used to simplify complex subsystems and provide a unified interface:


```csharp
public class Facade : IFacade
{
    private readonly IBusiness _business;
    
    public Facade(IBusiness business)
    {
        _business = business;
    }
    
    // Facade methods that delegate to business layer
}

```

### 4.3 Dependency Injection

Consistently used throughout the system for loose coupling:


```csharp
// In Program.cs
void AddScopes(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IFacade, Facade>();
    builder.Services.AddScoped<IBusiness, Business>();
    builder.Services.AddScoped<IData, Data>();
}

```

### 4.4 Model Mapping Pattern

Systematic transformation between different model types:


```csharp
// Business layer - converting domain models to service models
public async Task<ProductServiceModel> FetchProductByIdAsync(Guid productId)
{
    var data = await _data.FetchProductByIdAsync(productId);
    return data.MapToServiceModel();
}

```

### 4.5 Result Pattern

Used for consistent error handling and operation results:


```csharp
// Controller returning a Result object
[HttpGet("fetch")]
public async Task<Result<CatalogServiceModel>> FetchCatalog()
{
    var result = await _facade.FetchCatalog();
    return result.Match(
        onSuccess: () => result,
        onFailure: error => Result<CatalogServiceModel>.Failure(error, result.Messages)
    );
}

```

## 5. API Structure

### 5.1 RESTful API Design

All APIs follow RESTful design principles with standardized patterns:

- **Route Structure**: `api/[controller]/[action]`
- **HTTP Methods**: GET, POST, PUT, DELETE for appropriate operations
- **Response Format**: Consistent use of the Result pattern for all responses
- **Versioning**: API versioning through URL segment, header, or query string

### 5.2 API Documentation

- OpenAPI (Swagger) integration for all services
- XML comments for API controllers and methods

## 6. Data Models

### 6.1 Model Hierarchy

Each service maintains a consistent model hierarchy:

- **Domain Models**: Core internal representation (e.g., `ProductDomainModel`)
- **Service Models**: For outgoing communication (e.g., `ProductServiceModel`)
- **View Models**: For incoming communication (e.g., `ProductViewModel`)

### 6.2 Base Models

- `DomainBaseModel`: Common properties for domain entities
- `ServiceBaseModel`: Common properties for service layer models
- `ViewModelBase`: Common properties for API models

## 7. Cloud-Native Features

### 7.1 .NET Aspire Integration

AspireCafe leverages .NET Aspire for cloud-native capabilities:


```csharp
// AppHostAzure/Program.cs
var builder = DistributedApplication.CreateBuilder(args);
//resources
var keyvault = builder.AddConnectionString("keyvault");
var cosmos = builder.AddConnectionString("cosmos");
var cache = builder.AddConnectionString("cache");
var servicebus = builder.AddConnectionString("servicebus");

```

### 7.2 Azure Service Integration

- **Azure Cosmos DB**: Primary data store for all services
- **Azure Redis Cache**: Distributed caching
- **Azure Service Bus**: Messaging and event propagation
- **Azure Key Vault**: Secure configuration and secrets management

### 7.3 Observability

- Distributed tracing through .NET Aspire
- Service health endpoints
- Standardized logging and metrics

## 8. Frontend Architecture (Angular)

### 8.1 Component Structure

- Feature-based organization
- Shared components for reusability
- Smart and presentation components pattern

### 8.2 State Management

- RxJS for reactive state management
- Services for data fetching and manipulation

### 8.3 API Integration


```typescript
// Example order service implementation
loadOrder(id: string): void {
  this.orderService.getOrder(id).subscribe({
    next: (order) => {
      this.order = order;
      this.isLoading = false;
    },
    error: (error) => {
      console.error('Error loading order', error);
      this.errorMessage = 'Failed to load order details';
      this.isLoading = false;
    }
  });
}

```

## 9. Validation and Error Handling

### 9.1 Input Validation

- FluentValidation for request validation:


```csharp
// Program.cs
builder.Services.AddValidatorsFromAssemblyContaining<OrderViewModelValidator>();

```

### 9.2 Global Exception Handling

- Centralized exception middleware:


```csharp
// Program.cs
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

```

## 10. Security Considerations

- HTTPS enforcement for all services
- Authentication and authorization framework
- Azure Key Vault integration for secrets management

## 11. Development Environment Setup

### 11.1 Prerequisites

- .NET 9 SDK
- Node.js (latest LTS)
- Angular CLI 19.x
- Visual Studio 2022 or Visual Studio Code
- Docker Desktop (for containerized development)

### 11.2 Running the Solution Locally

1. Clone the repository
2. Open the solution in Visual Studio 2022
3. Set the appropriate AppHost project as startup (Intel64/Arm64)
4. Press F5 to build and run
5. Access the Aspire dashboard for service monitoring

## 12. Common Development Tasks

### 12.1 Adding a New Endpoint

1. Define view and service models
2. Implement business logic in the appropriate business class
3. Add controller method with proper attributes
4. Update API documentation

### 12.2 Modifying Data Models

1. Update domain model
2. Update corresponding service and view models
3. Update mapping extensions
4. Update database context if schema changes are required
5. Update validation rules if necessary

## 13. Testing Strategy

- Unit tests for business logic
- Integration tests for API endpoints
- End-to-end tests for complete flows

## 14. Deployment Procedures

### 14.1 Azure Deployment

- CI/CD pipeline integration
- Infrastructure as Code using Azure Resource Manager templates
- Blue-green deployment strategy

### 14.2 Configuration Management

- Environment-specific configuration
- Secret management through Azure Key Vault

## 15. Maintenance and Troubleshooting

- Aspire dashboard for service monitoring
- Logging and diagnostics configuration
- Health check endpoints

---

This documentation provides a comprehensive overview of the AspireCafe solution's architecture, design patterns, and implementation details. Developers and support engineers can use this as a reference for understanding, maintaining, and extending the system.