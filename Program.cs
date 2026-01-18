using Microsoft.EntityFrameworkCore;
using CSharpApi.Context;
using CSharpApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseSnakeCaseNamingConvention()
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["AppSettings:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
        ValidateIssuerSigningKey = true,
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CSharpApi",
        Version = "v1",
        Description = "API de gerenciamento de usuários com autenticação JWT"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddHttpClient<JsonPlaceholderService>(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "CSharpApi API Documentation";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CSharpApi v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => new { message = "CSharpApi - API de Gerenciamento de Usuários", version = "1.0", swagger = "/swagger" });

app.MapControllers();

app.Run();