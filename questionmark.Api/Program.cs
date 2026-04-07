using questionmark.Api.Filters;
using questionmark.Application;
using questionmark.Infrastructure;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
//dotnet run --project questionmark.Api
//dotnet ef migrations add Initial --project ./questionmark.Domain --startup-project ./questionmark.Api --verbose
//dotnet ef database update --project ./questionmark.Domain --startup-project ./questionmark.Api
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.OverrideDefaultResultFactoryWith<ValidationBehaviour>();
});
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();