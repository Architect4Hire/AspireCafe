# AspireCafe.UI

## Overview
AspireCafe.UI is the frontend component of the AspireCafe application, a cloud-native, microservices-based point-of-sale system for cafes. Built using Angular 19, this UI provides a modern and responsive interface for cafe staff to manage orders, products, and customer interactions.

## Technology Stack
- **Framework**: Angular 19
- **Language**: TypeScript
- **Styling**: SCSS
- **Testing**: Jasmine
- **Build Tool**: Angular CLI
- **Integration**: Integrated with .NET 9 Aspire backend services

## Architecture

### Frontend Architecture
AspireCafe.UI follows a component-based architecture with:
- **Component-Based Structure**: UI elements encapsulated in reusable components
- **Reactive Programming**: Using RxJS for reactive state management
- **Routing**: Angular Router for navigation between different views
- **Services**: API communication layer to interact with backend microservices

### Backend Integration
The UI connects to multiple microservices through RESTful APIs:
- **Product API**: Manages product catalog and inventory
- **Counter API**: Handles customer orders and payments
- **Kitchen API**: Manages food preparation workflow
- **Barista API**: Manages beverage preparation workflow
- **Order Summary API**: Provides order analytics and reporting

## Design Patterns
- **Repository Pattern**: Data access abstraction for API calls
- **Facade Pattern**: Simplifies interface to complex subsystems
- **Dependency Injection**: Angular's built-in DI for loose coupling
- **Observable Pattern**: Reactive programming for state management and asynchronous operations

## Solution Structure
The AspireCafe solution follows a clean, multi-project architecture:
- **AspireCafe.UI**: Angular frontend
- **AspireCafe.ProductApi**: Product catalog and inventory management
- **AspireCafe.CounterApi**: Order processing and payment handling
- **AspireCafe.KitchenApi**: Food preparation workflow
- **AspireCafe.BaristaApi**: Beverage preparation workflow
- **AspireCafe.OrderSummaryApi**: Order analytics and reporting
- **AspireCafe.ServiceDefaults**: Shared service defaults and configurations
- **AppHost.Azure**: .NET Aspire application host for Azure deployment
- **AppHost.Arm64**: .NET Aspire application host for local ARM (Mac Silicon/Mx Chipset) deployment
- **AppHost.Intel64**: .NET Aspire application host for local Intel64 deployment

## Cloud-Native Features
- **.NET Aspire Integration**: Seamless integration with Microsoft's cloud-native app stack
- **Azure Resources**: Configuration for various Azure services (CosmosDB, Redis, Service Bus)
- **Distributed Tracing**: Built-in observability and diagnostics
- **Service Discovery**: Automatic service registration and discovery

## Development Setup

### Prerequisites
- Node.js (latest LTS version)
- npm or yarn package manager
- Angular CLI 19.x
- .NET 9 SDK
- Visual Studio 2022 or Visual Studio Code

### Running the UI Locally
1. Clone the repository
2. Navigate to the AspireCafe.UI directory
3. Install dependencies:
   
```
   npm install
   
```
4. Start the development server:
   
```
   npm start
   
```
5. The application will be available at http://localhost:4200

### Running with Backend Services
For a complete local development experience, run the entire application using the .NET Aspire AppHost:

1. Open the solution in Visual Studio 2022
2. Set AppHostAzure as the startup project
3. Press F5 to build and run

The Aspire dashboard will launch showing all services, and the UI will automatically connect to the locally running backend services.

## Testing
- Unit tests: `npm test`
- End-to-end tests: `npm run e2e`

## Building for Production

```
npm run build

```
This will generate production-ready files in the `dist/AspireCafe.UI/browser/` directory.

## Contribution Guidelines
- Follow Angular style guide for code formatting
- Write unit tests for new components and services
- Use feature branches and pull requests for changes

## License
[License information]
