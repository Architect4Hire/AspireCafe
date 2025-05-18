using System.Net.Sockets;

var builder = DistributedApplication.CreateBuilder(args);
//resources
var keyvault = builder.AddConnectionString("keyvault");
var cosmos = builder.AddConnectionString("cosmos");
var cache = builder.AddConnectionString("cache");
var servicebus = builder.AddConnectionString("servicebus");

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
 .WithHttpEndpoint(env: "PORT", port: 4200)
 .WithExternalHttpEndpoints();
builder.AddProject<Projects.AspireCafe_AuthenticationApi>("aspirecafe-authenticationapi");
builder.AddProject<Projects.AspireCafe_Proxy>("aspirecafe-proxy");
builder.Build().Run();