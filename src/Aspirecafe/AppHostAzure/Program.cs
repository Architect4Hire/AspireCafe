var builder = DistributedApplication.CreateBuilder(args);
//resources
var keyvault = builder.AddConnectionString("keyvault");
var cosmos = builder.AddConnectionString("cosmos");
var cache = builder.AddConnectionString("cache");
var servicebus = builder.AddConnectionString("servicebus");
//services
builder.AddProject<Projects.AspireCafe_BaristaApi>("aspirecafe-baristaapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus);
builder.AddProject<Projects.AspireCafe_CounterApi>("aspirecafe-counterapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus);
builder.AddProject<Projects.AspireCafe_KitchenApi>("aspirecafe-kitchenapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus);
builder.AddProject<Projects.AspireCafe_ProductApi>("aspirecafe-productapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus);
builder.AddProject<Projects.AspireCafe_OrderSummaryApi>("aspirecafe-ordersummaryapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus);
builder.Build().Run();