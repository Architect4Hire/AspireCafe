# AspireCafe System - Technical Documentation

## 1. Solution Overview

AspireCafe is a distributed microservices-based application designed to support cafe operations. It is built on .NET 9 with C# 13 and follows modern architectural patterns such as clean architecture, distributed systems design, and containerized deployment. The solution includes multiple APIs and a front-end UI, orchestrated through a centralized application host.

## 2. Architecture

### 2.1 High-Level Architecture

The system is composed of the following components:

1. **AppHostAzure**: The central orchestrator for all services, managing dependencies and startup order.
2. **Microservices**:
   - **ProductApi**: Manages product catalog and metadata.
   - **CounterApi**: Handles order processing and payment.
   - **KitchenApi**: Manages kitchen operations like order preparation.
   - **OrderSummaryApi**: Provides aggregated order summaries.
   - **BaristaApi**: Manages barista-specific operations.
3. **UI**: A front-end Angular application for user interaction.

### 2.2 Design Patterns and Practices

#### 2.2.1 Clean Architecture

The solution follows clean architecture principles, ensuring separation of concerns:

- **Presentation Layer**: API controllers and Angular UI.
- **Domain Layer**: Business logic, validation, and domain models.
- **Data Layer**: Data access and persistence logic.

#### 2.2.2 Distributed Application Pattern

The `AppHostAzure` project uses the `DistributedApplication` builder to orchestrate the startup of all services and manage their dependencies.

#### 2.2.3 Dependency Injection

All services use dependency injection to manage dependencies, ensuring loose coupling and testability.

#### 2.2.4 Result Pattern

A generic `Result<T>` class is used across all services to handle success and failure scenarios consistently.

#### 2.2.5 Facade Pattern

The Facade pattern is used to simplify interactions with complex subsystems, providing a unified interface for controllers.

## 3. AppHostAzure

The `AppHostAzure` project is the entry point for the entire system. It uses the `DistributedApplication` builder to configure and start all services.

### 3.1 Key Features

- **Centralized Configuration**: Manages connection strings for shared resources like KeyVault, CosmosDB, Redis Cache, and Service Bus.
- **Service Orchestration**: Ensures services start in the correct order and with the required dependencies.
- **Scalability**: Designed to support horizontal scaling of services.

### 3.2 Code Example


```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Resources
var keyvault = builder.AddConnectionString("keyvault");
var cosmos = builder.AddConnectionString("cosmos");
var cache = builder.AddConnectionString("cache");
var servicebus = builder.AddConnectionString("servicebus");

// Services
var productapi = builder.AddProject<Projects.AspireCafe_ProductApi>("aspirecafe-productapi")
    .WithReference(keyvault).WaitFor(keyvault)
    .WithReference(cosmos).WaitFor(cosmos)
    .WithReference(cache).WaitFor(cache)
    .WithReference(servicebus).WaitFor(servicebus);

var counterapi = builder.AddProject<Projects.AspireCafe_CounterApi>("aspirecafe-counterapi")
    .WithReference(keyvault).WaitFor(keyvault)
    .WithReference(cosmos).WaitFor(cosmos)
    .WithReference(cache).WaitFor(cache)
    .WithReference(servicebus).WaitFor(servicebus);

builder.Build().Run();

```

### 3.3 Key Responsibilities

- **Dependency Management**: Ensures services like `ProductApi` and `CounterApi` have access to shared resources (e.g., KeyVault, CosmosDB).
- **Service Startup Order**: Uses `.WaitFor()` to enforce startup dependencies.

## 4. Microservices

### 4.1 ProductApi

#### Responsibilities

- Manages product catalog and metadata.
- Provides CRUD operations for products.

#### Key Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/product/{productId:guid}` | GET | Fetches a product by ID. |
| `/api/product/create` | POST | Creates a new product. |
| `/api/product/update` | PUT | Updates an existing product. |
| `/api/product/delete/{productId:guid}` | DELETE | Deletes a product. |

#### Key Patterns

- **Facade Pattern**: Simplifies interactions with the business and data layers.
- **Validation**: Uses FluentValidation for input validation.

#### Example Code


```csharp
[HttpPost("create")]
public async Task<Result<ProductServiceModel>> CreateProduct(ProductViewModel product)
{
    var result = await _facade.CreateProductAsync(product);
    return result.Match(
        onSuccess: () => result,
        onFailure: error => Result<ProductServiceModel>.Failure(error, result.Messages)
    );
}

```

### 4.2 CounterApi

#### Responsibilities

- Handles order processing and payment.
- Manages customer orders at the counter.

#### Key Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/counter/submitorder` | POST | Submits a new order. |
| `/api/counter/getorder/{orderId:guid}` | GET | Retrieves an order by ID. |
| `/api/counter/updateorder` | PUT | Updates an existing order. |
| `/api/counter/payorder` | POST | Processes payment for an order. |

#### Example Code


```csharp
[HttpPost("SubmitOrder")]
public async Task<Result<OrderServiceModel>> SubmitOrder(OrderViewModel order)
{
    var result = await _facade.SubmitOrderAsync(order);
    return result.Match(
        onSuccess: () => result,
        onFailure: error => Result<OrderServiceModel>.Failure(error, result.Messages)
    );
}

```

### 4.3 KitchenApi

#### Responsibilities

- Manages kitchen operations like order preparation and cooking status.
- Tracks kitchen inventory.

#### Planned Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/kitchen/prepareorder` | POST | Marks an order as being prepared. |
| `/api/kitchen/orderstatus/{orderId:guid}` | GET | Retrieves the status of an order. |

### 4.4 OrderSummaryApi

#### Responsibilities

- Provides aggregated order summaries for reporting and analytics.

#### Planned Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/ordersummary/daily` | GET | Retrieves daily order summaries. |
| `/api/ordersummary/weekly` | GET | Retrieves weekly order summaries. |

### 4.5 BaristaApi

#### Responsibilities

- Manages barista-specific operations like drink preparation and queue management.

#### Planned Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/barista/preparedrink` | POST | Marks a drink as being prepared. |
| `/api/barista/drinkstatus/{drinkId:guid}` | GET | Retrieves the status of a drink. |

## 5. UI (Angular)

The front-end Angular application provides a user-friendly interface for interacting with the system.

### Key Features

- **Responsive Design**: Optimized for both desktop and mobile devices.
- **Integration**: Communicates with the back-end APIs using RESTful endpoints.
- **Modular Structure**: Organized into feature modules for scalability.

### Key Files

- `angular.json`: Configures the Angular build process.
- `package.json`: Manages dependencies for the Angular application.

## 6. Shared Resources

### 6.1 KeyVault

Used for secure storage of secrets and configuration settings.

### 6.2 CosmosDB

The primary database for all services, providing scalable and distributed data storage.

### 6.3 Redis Cache

Used for caching frequently accessed data to improve performance.

### 6.4 Service Bus

Facilitates asynchronous communication between services.

## 7. Deployment

The solution is containerized and deployed using .NET Aspire. Each service is independently deployable and scalable.

### Deployment Steps

1. Build the solution using the `.NET 9` SDK.
2. Deploy containers for each service.
3. Configure shared resources (KeyVault, CosmosDB, Redis, Service Bus).
4. Start the `AppHostAzure` project to orchestrate the services.

## 8. Development Workflow

### Adding a New Service

1. Create a new project for the service.
2. Define the service's responsibilities and endpoints.
3. Implement the Facade, Business, and Data layers.
4. Add the service to `AppHostAzure` with the required dependencies.

### Debugging

- Use logs to trace issues in the `AppHostAzure` startup process.
- Validate API requests and responses using Swagger/OpenAPI.
- Check shared resource configurations (KeyVault, CosmosDB, etc.).

## 9. Security Considerations

- All APIs enforce HTTPS.
- Secrets are stored securely in KeyVault.
- Input validation prevents injection attacks.
- Error messages are sanitized in production.

## 10. Conclusion

AspireCafe is a robust, scalable, and maintainable system designed to support modern cafe operations. Its modular architecture and use of best practices make it easy to extend and support.