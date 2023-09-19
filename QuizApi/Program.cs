using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizApi;
using QuizApi.Exceptions.Filters;
using QuizApi.Extensions;
using QuizApi.Repository;
using QuizApi.Services.UserService;
using QuizApi.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(config =>
{
    config.Filters.Add<ParsingExceptionFilter>();
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<QuizContext>();
builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.Password.RequireDigit = false;
    opts.Password.RequiredLength = 0;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequiredUniqueChars = 0;
    opts.Password.RequireNonAlphanumeric = false;
});
builder.Services.AddDbContext<QuizContext>(options => 
{
    options.UseSqlite(builder.Configuration.GetConnectionString("database"));
});

builder.Services.AddScoped<IRepositoryQuiz, QuizRepository>();
builder.Services.AddScoped<IRepositoryUser, UserRepository>();

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = false;
        opts.SaveToken = false;
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidIssuer = builder.Configuration["JWTConfig:Issuer"],
            ValidAudience = builder.Configuration["JWTConfig:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfig:Key"]))

        };
    });

builder.Services.Configure<JWT>(builder.Configuration.GetSection(JWT.JWTConfig));

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

app.AddAdmin();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();