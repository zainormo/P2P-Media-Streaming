using Microsoft.Extensions.Hosting;
using Peer.Core;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddModules(builder.Configuration);
await builder.Build().RunAsync();
