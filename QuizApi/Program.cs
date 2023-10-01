using QuizApi.Extensions;
using QuizApi.Services.DateTimeProvider;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureControllers();

builder.Services.ConfigureIdentity();

builder.Services.ConfigureDb(builder.Configuration);

builder.Services.ConfigureJwtAuthentication(builder.Configuration);

builder.Services.ConfigureMapper();

builder.Services.AddSwaggerGen();

await builder.Services.AddRoles();

builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();

app.AddAdmin();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();