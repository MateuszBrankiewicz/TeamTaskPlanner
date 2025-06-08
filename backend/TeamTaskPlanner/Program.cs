using Microsoft.EntityFrameworkCore;
using TeamTaskPlanner.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  try
  {
   var context = services.GetRequiredService<AppDbContext>();
   DbInitializer.Initialize(context);
  }
  catch (Exception e)
  {
    Console.WriteLine(e);
    throw;
  }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapGet("/", () => "Hello World!");
app.Run();
