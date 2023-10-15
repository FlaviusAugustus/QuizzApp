using QuizApi;
using QuizApi.Constants;
using QuizApi.Extensions;
using QuizApi.Services;
using QuizApi.Services.DateTimeProvider;
using QuizApi.Services.QuizService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureControllers();

builder.Services.ConfigureIdentity();

builder.Services.ConfigureDb(builder.Configuration);

builder.Services.ConfigureJwtAuthentication(builder.Configuration);

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy(nameof(Policy.CanAccessSecureController), policy => policy.RequireRole(nameof(Role.Admin)));
    opts.AddPolicy(nameof(Policy.CanManageRoles), policy => policy.RequireRole(nameof(Role.Admin)));
});

await builder.Services.AddRoles();

builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddScoped<IQuizService, QuizService>();

builder.Services.AddScoped<IParserFactory<FlashCardSet>, ParserFactory<FlashCardSet>>();

builder.Services.ConfigureSwagger();

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