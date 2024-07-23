using Microsoft.EntityFrameworkCore;
using Project.API.Interfaces;
using Project.API.Entities;
using Task_Management_System.Data;
using Project.API.Repositories;
using Project.API.Services;
using Project.API.Utils;
using Project.API.Data;
using Microsoft.AspNetCore.Authentication;
using Project.API.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Project.API.Middlewares;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Binding configurations from user secrets
SendGridSettings sendGridSettings = new SendGridSettings();
SMSSettings smsSettings = new SMSSettings();
JwtSettings jwtSettings = new JwtSettings();
DatabaseSettings databaseSettings = new DatabaseSettings();
databaseSettings.DatabaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Configuration.GetSection("Jwt").Bind(jwtSettings);
sendGridSettings.APIKey = builder.Configuration["SendGridAPIKey"]!;
smsSettings.APIKey = builder.Configuration["SMSAPIKey"]!;
BearerAuthenticationOptions bearerAuthenticationOptions = new BearerAuthenticationOptions(jwtSettings);

// Register Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    return ConnectionMultiplexer.Connect("localhost:6379");
});

// Authentication Schemes
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => 
bearerAuthenticationOptions.ConfigureJwtAuthentictionOptions(options));


// Registering classes in DI container
builder.Services.AddSingleton(smsSettings);
builder.Services.AddSingleton(sendGridSettings);
builder.Services.AddScoped<BufferingBodyMiddleware>();
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(databaseSettings);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddSingleton<IAuthorizationPolicy, AuthorityManager>();
builder.Services.AddScoped<ApplicationService>();
builder.Services.AddScoped<IUserProjectService, UserProjectService>();
builder.Services.AddScoped<IRepository<Project.API.Entities.Project>, ProjectRepository>();
builder.Services.AddScoped<IJunctionRepository<UserProjects>, UserProjectsRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Todo>, TodoRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserTodosService, UserTodosService>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IJunctionRepository<UserTodos>, UserTodosRepository>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ICacheService, CacheService>();

// Registering database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(databaseSettings.DatabaseConnectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<BufferingBodyMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
