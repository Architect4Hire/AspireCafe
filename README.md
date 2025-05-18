# AspireCafe: A Modern Cloud-Native Architecture with .NET 9

The AspireCafe solution demonstrates a modern approach to building distributed, cloud-native applications using .NET 9 and the .NET Aspire application stack. This blog post explores the architecture, components, and design patterns that make this solution scalable, maintainable, and future-proof.

---
## **System and Functional Requirements**

## Functional Requirements

1. **Product Management**
   - Maintain a catalog of products with categories, subcategories, descriptions, images, and pricing
   - Support caching of catalog data to improve performance (30-minute cache validity)
   - Allow product metadata retrieval to determine routing to appropriate preparation stations

2. **Order Processing**
   - Enable customers to place orders with multiple items
   - Capture customer name and table number with each order
   - Calculate order totals correctly
   - Generate and display order confirmation with reference numbers
   - Route order items to appropriate preparation stations (Barista or Kitchen)

3. **Order Workflow Management**
   - Track order status through various stages (Pending, Processing, Complete)
   - Support separate workflows for beverage and food preparation
   - Maintain order history and processing status

4. **Reporting and Analytics**
   - Provide order summary and analytics functionality

## Technical Requirements

1. **Cloud-Native Architecture**
   - Implement microservices architecture with separate APIs for each functional domain
   - Use .NET 9 Aspire for cloud-native application development
   - Support deployment to Azure and local development environments (Intel64/ARM64)

2. **Event-Driven Communication**
   - Utilize Azure Service Bus with topics/subscriptions for asynchronous communication
   - Implement message retry mechanisms (5 max delivery attempts)
   - Use publisher-subscriber model for order routing

3. **Data Management**
   - Use CosmosDB for document storage with appropriate containers
   - Implement Redis caching with cache-aside pattern for performance optimization
   - Store different document types in separate containers (orders, barista, kitchen, products)

4. **Security**
   - Integrate with Azure Key Vault for secrets management

5. **Observability**
   - Implement comprehensive logging and tracing with OpenTelemetry
   - Export telemetry to Azure Monitor and OTLP endpoints when configured
   - Support Seq for local log aggregation and viewing

6. **UI Requirements**
   - Develop responsive Angular 19 frontend
   - Support real-time order status updates
   - Implement intuitive checkout flow
   - Follow component-based architecture with reactive programming

7. **Resilience**
   - Implement HTTP resilience with retry policies
   - Configure appropriate timeouts for service communications
   - Use health checks for service status monitoring

8. **Testing**
   - Support integration testing of distributed applications
   - Enable unit testing for components and services

## **Architecture Overview**

 ![System Architecture](https://github.com/Architect4Hire/AspireCafe/blob/dev/images/system.png)

AspireCafe is a **microservices-based architecture** designed to manage various aspects of a cafe's operations. Each microservice is independently deployable and responsible for a specific domain, ensuring modularity and scalability.

### **Core Microservices**

1. **Product API**: Manages the product catalog and inventory.
2. **Counter API**: Handles order creation, payment processing, and order state management.
3. **Kitchen API**: Manages food preparation workflows.
4. **Barista API**: Handles beverage preparation workflows.
5. **Order Summary API**: Aggregates and summarizes order data for reporting and analytics.

### **Frontend**

The **AspireCafe.UI** project is an Angular-based single-page application (SPA) that provides a user-friendly interface for interacting with the backend services. It is containerized and integrated into the overall solution using the `.WithNpmApp()` method in the application host.

---

## **Key Technical Components**

### **.NET Aspire Application Host**

The solution uses three application host projects for managing service orchestration and infrastructure:

1. **Aspirecafe.Intel64**:
   - Configures local development with emulators for Azure services.
   - Provides a seamless development experience with consistent dependencies.

2. **Aspirecafe.Azure**:
   - Configures production-ready services using Azure connection strings.
   - Ensures secure and scalable deployment in the cloud.

3. **Aspirecafe.AMD64**:
   - Configures local development with emulators for Azure services for AMD chipsets (Mac Silicon).
   - Provides a seamless development experience with consistent dependencies.

### **Cloud-Native Infrastructure**

The solution integrates with several Azure services for robust infrastructure:

- **Azure Cosmos DB**: Document database for storing product and order data.
- **Redis Cache**: Distributed caching for performance optimization.
- **Azure Service Bus**: Message broker for asynchronous communication between services.
- **Azure Key Vault**: Secure storage for credentials and configuration.

These services are containerized for local development, ensuring consistency across environments.

---

## **Design Patterns and Practices**

### **1. Layered Architecture**

Each microservice follows a clean, layered architecture:

- **API Layer**: Handles HTTP requests and responses (e.g., `CounterController`).
- **Facade Layer**: Orchestrates business logic and validation (e.g., `Facade.cs implementing IFacde.cs`).
- **Business Layer**: Implements core business rules (e.g., `Business.cs implementing IBusiness.cs`).
- **Data Layer**: Manages data access and persistence (e.g., `Data.cs implementing IData.cs`).

This separation of concerns ensures maintainability and testability.

---

### **2. Dependency Injection**

The solution uses dependency injection extensively to decouple components:


```csharp
builder.Services.AddScoped<IFacade, Facade>();
builder.Services.AddScoped<IBusiness, Business>();
builder.Services.AddScoped<IData, Data>();

```

This approach enhances modularity and makes the system easier to test.

---

### **3. Façade Pattern**

The Façade pattern simplifies the interaction between the API layer and the business logic:


```csharp
public class Facade : IFacade
{
    private readonly IBusiness _business;
    private readonly OrderViewModelValidator _validator;

    public async Task<Result<OrderServiceModel>> SubmitOrderAsync(OrderViewModel order)
    {
        var validationResult = await _validator.ValidateAsync(order);
        if (!validationResult.IsValid)
        {
            return Result<OrderServiceModel>.Failure(Error.InvalidInput, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }
        return Result<OrderServiceModel>.Success(await _business.SubmitOrderAsync(order));
    }
}

```

This pattern abstracts complex business logic and validation, providing a clean interface to the API layer.

---

### **4. Result Pattern**

A custom `Result<T>` type is used for consistent error handling:


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

This approach avoids exception-based flow control and provides clear error information.

---

### **5. Validation Pattern**

The solution uses **FluentValidation** for input validation:


```csharp
var validationResult = await _validator.ValidateAsync(product);
if (!validationResult.IsValid)
{
    return Result<ProductServiceModel>.Failure(Error.InvalidInput, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
}

```

This ensures robust input validation before business operations.

---

### **6. API Documentation**

Controllers use XML comments and `ProducesResponseType` attributes to generate comprehensive API documentation:


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
[ProducesResponseType(typeof(Result<ProductServiceModel>), 400)]
[ProducesResponseType(typeof(Result<ProductServiceModel>), 500)]

```

This integrates seamlessly with OpenAPI and Scalar.net for self-documenting APIs.

---

### **7. Frontend Integration**

The Angular-based frontend is integrated into the solution using `.WithNpmApp()`:


```csharp
var angular = builder.AddNpmApp("aspirecafe-ui", "../AspireCafe.UI/")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

```

This ensures the frontend is containerized and deployable alongside the backend services.

---

## **Modern C# and .NET Features**

The solution leverages the latest features of C# 13 and .NET 9:

- **Async/await** for non-blocking operations.
- **Pattern matching** for functional-style error handling.
- **Extension methods** for cleaner code organization.
- **Middleware** for cross-cutting concerns like exception handling.

---

## **Conclusion**

The AspireCafe solution is a prime example of how modern .NET applications can be built using a microservices architecture. By leveraging .NET Aspire, clean design patterns, and cloud-native infrastructure, the solution achieves scalability, maintainability, and ease of deployment.
