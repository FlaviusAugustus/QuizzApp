using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApi;
using QuizApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<QuizContext>(options => 
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("database"));
    } 
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = new MapperConfiguration(config =>
{
    config.CreateMap<FlashCardDto, FlashCard>();
    config.CreateMap<FlashCardSetDto, FlashCardSet>();
});
var mapper = configuration.CreateMapper();

builder.Services.AddSingleton(mapper);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();