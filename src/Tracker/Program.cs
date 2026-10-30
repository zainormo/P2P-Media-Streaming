using Tracker.Core;
using Tracker.Infrastructure;
using Tracker.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddModules(builder.Configuration);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();

app.UseAuthentication();
app.UseAuthorization();
app.MapModules();
app.Run();
