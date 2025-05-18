using AzureKeyVaultEmulator.Aspire.Hosting;
using System.Net.Sockets;


var builder = DistributedApplication.CreateBuilder(args);

//key vault - https://jamesgould.dev/posts/Azure-Key-Vault-Emulator/
var keyvault = builder.AddAzureKeyVaultEmulator("keyvault", new KeyVaultEmulatorOptions
{
    Lifetime = ContainerLifetime.Persistent
});

//database - https://learn.microsoft.com/en-us/dotnet/aspire/database/azure-cosmos-db-integration?tabs=dotnet-cli
#pragma warning disable CS0618 // Suppress warning for evaluation-only API
#pragma warning disable ASPIRECOSMOSDB001 // Suppress warning for evaluation-only API
var cosmos = builder.AddAzureCosmosDB("cosmos").RunAsEmulator(c =>
{
    c.WithLifetime(ContainerLifetime.Persistent);
    c.WithGatewayPort(8081);
});
var db = cosmos.AddCosmosDatabase("AspireCafe");
var container = db.AddContainer("orders", "/DocumentType");
var container3 = db.AddContainer("barista", "/DocumentType");
var container4 = db.AddContainer("kitchen", "/DocumentType");
var container2 = db.AddContainer("products", "/DocumentType");
#pragma warning restore CS0618 // Restore warning
#pragma warning restore ASPIRECOSMOSDB001 // Restore warning

//redis cache - https://learn.microsoft.com/en-us/dotnet/aspire/caching/stackexchange-redis-integration?tabs=dotnet-cli&pivots=redis
var cache = builder.AddRedis("cache").WithLifetime(ContainerLifetime.Persistent).WithRedisInsight().WithRedisCommander();

//service bus - https://learn.microsoft.com/en-us/dotnet/aspire/messaging/azure-service-bus-integration?tabs=dotnet-cli
var servicebus = builder.AddAzureServiceBus("servicebus").RunAsEmulator(e =>
{
    e.WithLifetime(ContainerLifetime.Persistent);
    e.WithHostPort(8080);
});
var topic = servicebus.AddServiceBusTopic("purchased-orders");
topic.AddServiceBusSubscription("barista-orders").WithProperties(p =>
{
    p.MaxDeliveryCount = 5;
});
topic.AddServiceBusSubscription("kitchen-orders").WithProperties(p =>
{
    p.MaxDeliveryCount = 5;
});

var seq = builder.AddSeq("seq")
                 .WithAnnotation(new EndpointAnnotation(ProtocolType.Tcp, uriScheme: "http", name: "seq", port: 61955, targetPort: 61956))
                 .ExcludeFromManifest()
                 .WithLifetime(ContainerLifetime.Persistent)
                 .WithEnvironment("ACCEPT_EULA", "Y");

//services
var baristaapi = builder.AddProject<Projects.AspireCafe_BaristaApi>("aspirecafe-baristaapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus).WithReference(seq).WaitFor(seq);
var counterapi = builder.AddProject<Projects.AspireCafe_CounterApi>("aspirecafe-counterapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus).WithReference(seq).WaitFor(seq);
var kitchenapi = builder.AddProject<Projects.AspireCafe_KitchenApi>("aspirecafe-kitchenapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus).WithReference(seq).WaitFor(seq);
var productapi = builder.AddProject<Projects.AspireCafe_ProductApi>("aspirecafe-productapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus).WithReference(seq).WaitFor(seq);
var ordersummaryapi = builder.AddProject<Projects.AspireCafe_OrderSummaryApi>("aspirecafe-odersummaryapi").WithReference(keyvault).WaitFor(keyvault).WithReference(cosmos).WaitFor(cosmos).WithReference(cache).WaitFor(cache).WithReference(servicebus).WaitFor(servicebus).WithReference(seq).WaitFor(seq);
var angular = builder.AddNpmApp("aspirecafe-ui", "../AspireCafe.UI/")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();
builder.AddProject<Projects.AspireCafe_AuthenticationApi>("aspirecafe-authenticationapi");
builder.AddProject<Projects.AspireCafe_Proxy>("aspirecafe-proxy");
builder.Build().Run();


