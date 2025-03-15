using HubService.API;
using HubService.Application.Interfaces.Repositories;
using HubService.Application.Interfaces.Services;
using HubService.Application.Interfaces.Services.HubService.Application.Interfaces.Services;
using HubService.Application.Services;
using HubService.Infrastructure;
using HubService.Infrastructure.Implementation;
using HubService.Application.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddThisLayer(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection
        ("ConnectionStrings:DefaultConnection").Value);

});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 5;

}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options=>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection
                ("Jwt:Key").Value))
    };
});

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddScoped<IBetRepository, BetRepository>();
builder.Services.AddScoped<IBetService, BetService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IActiveUserService, ActiveUserService>();

builder.Services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(builder.Configuration));
builder.Services.AddScoped<IMessageProducer, RabbitMqproducer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dataContext.Database.Migrate();
}

app.Run();
