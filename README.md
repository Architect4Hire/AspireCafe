# AspireCafe: A Modern Cloud-Native Architecture with .NET 9

The AspireCafe solution demonstrates a modern approach to building distributed, cloud-native applications using .NET 9 and the .NET Aspire application stack. This document explores the architecture, components, and design patterns that make this solution scalable, maintainable, and future-proof.

I'll examine the available code to create functional and technical requirements for the AspireCafe system.
Based on my analysis of the AspireCafe solution code, I'll provide comprehensive functional and technical requirements.

## 1. Functional Requirements

### 1.1 Product Management
1. **Product Catalog**
   - System shall maintain a catalog of cafe products
   - System shall categorize products by type (food, beverage)
   - System shall support product subcategories
   - System shall provide metadata for routing products to appropriate preparation stations

2. **Product Operations**
   - System shall allow creating new products with details (name, description, price, image)
   - System shall support updating existing product information
   - System shall support product deletion
   - System shall track product availability status

3. **Product Querying**
   - System shall provide product metadata lookup by ID
   - System shall support retrieval of the full product catalog
   - System shall return appropriate error messages for invalid product requests

### 1.2 Order Processing
1. **Order Creation**
   - System shall support creating new customer orders
   - System shall capture order details (customer name, table number, items, quantities)
   - System shall calculate order totals (subtotal, tax, tip)
   - System shall assign unique identifiers to orders

2. **Order Workflow**
   - System shall track order status throughout its lifecycle
   - System shall route order items to appropriate preparation stations (Kitchen or Barista)
   - System shall support status updates at different preparation stations
   - System shall track order progress through states: Waiting, Received, Preparing, Ready, Delivered, Cancelled

3. **Payment Processing**
   - System shall support multiple payment methods (Cash, Card, Mobile Payment)
   - System shall track payment status for each order
   - System shall process payments with tip calculations
   - System shall validate payment information

### 1.3 Kitchen Operations
1. **Food Preparation Workflow**
   - System shall display active food orders to kitchen staff
   - System shall allow updating food preparation status
   - System shall support multiple preparation stations (Grill, Fry, Pantry, etc.)
   - System shall track food item readiness

### 1.4 Barista Operations
1. **Beverage Preparation Workflow**
   - System shall display active beverage orders to barista staff
   - System shall allow updating beverage preparation status
   - System shall track beverage item readiness

### 1.5 Order Summary and Reporting
1. **Order Analytics**
   - System shall provide summary views of orders
   - System shall support querying historical order data
   - System shall offer reporting capabilities for business insights

### 1.6 Authentication and Security
1. **User Authentication**
   - System shall authenticate users before granting access
   - System shall validate authentication tokens
   - System shall support secure login mechanisms

## 2. Technical Requirements

### 2.1 Architecture
1. **Microservices Architecture**
   - System shall implement a microservices-based architecture
   - System shall include the following services:
     - Product API: Managing product catalog
     - Counter API: Processing orders and payments
     - Kitchen API: Managing food preparation
     - Barista API: Managing beverage preparation
     - Order Summary API: Providing analytics
     - Authentication API: Handling user authentication
     - Proxy API: Routing client requests to appropriate services

2. **Cloud-Native Design**
   - System shall implement .NET Aspire cloud-native application framework
   - System shall support deployment to Azure cloud services
   - System shall include application hosts for different deployment targets (Azure, ARM64, Intel64)
   - System shall use service discovery for inter-service communication

### 2.2 Data Storage
1. **Database Requirements**
   - System shall use CosmosDB for persistent data storage
   - Each microservice shall maintain its own data context
   - System shall implement proper data models for each domain

2. **Caching**
   - System shall implement Redis distributed caching
   - Product catalog shall utilize caching for improved performance

### 2.3 Communication
1. **Messaging**
   - System shall use Azure Service Bus for asynchronous communication between services
   - System shall implement proper messaging patterns for order processing workflow

2. **API Design**
   - All APIs shall follow RESTful design principles
   - System shall implement API versioning
   - System shall provide appropriate error handling and response formatting
   - System shall use Result pattern for consistent response handling

### 2.4 Frontend
1. **UI Framework**
   - System shall implement Angular 19 for the frontend application
   - UI shall follow component-based architecture
   - UI shall implement reactive programming with RxJS

2. **Frontend-Backend Integration**
   - Frontend shall consume backend APIs through HTTP clients
   - System shall implement strongly-typed API clients using Refit

### 2.5 Cross-Cutting Concerns
1. **Validation**
   - System shall implement FluentValidation for input validation
   - Each API shall validate incoming requests before processing

2. **Observability**
   - System shall implement distributed tracing
   - System shall utilize Seq for logging
   - System shall provide health monitoring endpoints

3. **Security**
   - System shall implement secure authentication mechanisms
   - System shall validate authentication tokens for protected operations
   - System shall handle sensitive data securely

### 2.6 Development and Deployment
1. **Development Environment**
   - System shall support local development using .NET Aspire
   - System shall provide consistent development experience across platforms

2. **Deployment**
   - System shall support deployment to Azure cloud services
   - System shall include deployment configurations for different environments

3. **Testing**
   - System shall include unit testing with appropriate test frameworks
   - System shall support end-to-end testing of the application

## 3. Non-Functional Requirements

### 3.1 Performance
1. System shall respond to user interactions within acceptable time limits
2. System shall handle concurrent orders efficiently
3. System shall utilize caching for frequently accessed data

### 3.2 Scalability
1. System architecture shall support horizontal scaling of individual services
2. System shall handle increased load during peak cafe hours

### 3.3 Reliability
1. System shall maintain data consistency across services
2. System shall implement proper error handling and recovery
3. System shall provide appropriate fallback mechanisms

### 3.4 Usability
1. UI shall provide intuitive navigation for cafe staff
2. System shall support responsive design for various device sizes
3. System shall minimize steps required for common operations

### 3.5 Maintainability
1. Code shall follow clean architecture principles
2. System shall implement consistent design patterns across services
3. System shall include appropriate documentation

## **Architecture Overview**

 ![System Architecture](https://github.com/Architect4Hire/AspireCafe/blob/dev/images/system.png)

AspireCafe is a **microservices-based architecture** designed to manage various aspects of a cafe's operations. Each microservice is independently deployable and responsible for a specific domain, ensuring modularity and scalability.

### **Core Microservices**

1. **Product API**: Manages the product catalog and inventory.
2. **Counter API**: Handles order creation, payment processing, and order state management.
3. **Kitchen API**: Manages food preparation workflows.
4. **Barista API**: Handles beverage preparation workflows.
5. **Order Summary API**: Aggregates and summarizes order data for reporting and analytics.
6. **Authentication API**: Handles user identity and authorization using JWT token

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
- **API Gateway**: YARP reverse proxy routes requests to appropriate microservices
- **Seq**: Centralized logging and diagnostic service

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
