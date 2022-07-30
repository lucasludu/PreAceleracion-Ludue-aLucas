using Disney.Data;
using Disney.Mapper;
using Disney.Services.Implements;
using Disney.Services.Interfaces;
using Disney.UOWork;
using Disney.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region BBDD
builder.Services.AddDbContext<ApplicationDbContext>(Options => {
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

#region Scoped and Mapper
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddSingleton<IFileStorage, FileStorage>();
builder.Services.AddHttpContextAccessor();
#endregion

#region JWT
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option => {
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("ApiUserDisney", new OpenApiInfo
    {
        Title = "Api User",
        Version = "1",
        Description = "Backend Disney"
    });
    c.SwaggerDoc("ApiMovieDisney", new OpenApiInfo
    {
        Title = "Api Movie",
        Version = "1",
        Description = "Backend Disney"
    });
    c.SwaggerDoc("ApiCharacterDisney", new OpenApiInfo
    {
        Title = "Api Character",
        Version = "1",
        Description = "Backend Disney"
    });
    c.SwaggerDoc("ApiGenreDisney", new OpenApiInfo
    {
        Title = "Api User",
        Version = "1",
        Description = "Backend Disney"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new string[]{}
        }
    });
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/ApiUserDisney/swagger.json", "Api User");
        c.SwaggerEndpoint("/swagger/ApiMovieDisney/swagger.json", "Api Movie");
        c.SwaggerEndpoint("/swagger/ApiCharacterDisney/swagger.json", "Api Character");
        c.SwaggerEndpoint("/swagger/ApiGenreDisney/swagger.json", "Api Genre");
        c.DefaultModelsExpandDepth(-1);
    });

}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
