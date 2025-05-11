var builder = DistributedApplication.CreateBuilder(args);
//resources
var keyvault = builder.AddConnectionString("keyvault");
var cosmos = builder.AddConnectionString("cosmos");
var cache = builder.AddConnectionString("cache");
var servicebus = builder.AddConnectionString("servicebus");
//services
var baristaapi = builder.AddProject<Projects.AspireCafe_BaristaApi>("aspirecafe-baristaapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus);
var counterapi = builder.AddProject<Projects.AspireCafe_CounterApi>("aspirecafe-counterapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus);
var kitchenapi = builder.AddProject<Projects.AspireCafe_KitchenApi>("aspirecafe-kitchenapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus);
var productapi = builder.AddProject<Projects.AspireCafe_ProductApi>("aspirecafe-productapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus);
var ordersummaryapi = builder.AddProject<Projects.AspireCafe_OrderSummaryApi>("aspirecafe-odersummaryapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus);
var angular = builder.AddProject<Projects.AspireCafe_UI>("aspirecafe-ui");
builder.Build().Run();