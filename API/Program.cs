using API.Errors;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddApplicationService(builder.Configuration);


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();


// <<<<<<<  This piece of code apply any pending migratio, and seed some data  >>>>>>
//using var scope = app.Services.CreateScope();
//var services = scope.ServiceProvider;
//var context = services.GetRequiredService<StoreContext>();
//var logger = services.GetRequiredService<ILogger<Program>>();

//try
//{
//    await context.Database.MigrateAsync();
//    await SeedDataContext.SeedAsync(context);
//}
//catch (Exception ex)
//{
//    logger.LogError(ex, "Error occured");
//}
// <<<<<<<<<<<<<<<<<<<<<<<<  End of Seeding   >>>>>>>>>>>>>>>>>>>>>>>>

app.Run();
