
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Cefalo.TechDaily.Database.Context;
using Microsoft.EntityFrameworkCore;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Services;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Repository.Repositories;
using Cefalo.TechDaily.Service.Utils.Contract;
using Cefalo.TechDaily.Service.Utils.Services;
using Cefalo.TechDaily.Service.Utils.Contracts;
using Cefalo.TechDaily.Api.GlobalExceptionHandler;
using Microsoft.Extensions.Logging;
using Cefalo.TechDaily.Service.DtoValidators;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Api.CustomOutputFormatter.StoryOutputFormatter;
using Cefalo.TechDaily.Api.CustomOutputFormatter.UserOutputFormatter;
using System;

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.WithOrigins("http://localhost:3001").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
//                      });
//});
// Add services to the container.

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
}).AddXmlDataContractSerializerFormatters()
            .AddMvcOptions(option =>
            {
                option.OutputFormatters.Add(new CsvStoryOutputFormatter());
                option.OutputFormatters.Add(new PlainTextStoryOutputFormatter());
                option.OutputFormatters.Add(new HtmlStoryOutputFormatter());
                option.OutputFormatters.Add(new CsvUserOutputFormatter());
                option.OutputFormatters.Add(new PlainTextUserOutputFormatter());
                option.OutputFormatters.Add(new HtmlUserOutputFormatter());
            });
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3001", "https://localhost:3001")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//builder.Services.AddSwaggerGen();

//DTO validators
builder.Services.AddScoped<BaseDtoValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddScoped<BaseDtoValidator<PostStoryDto>, PostStoryDtoValidator>();
builder.Services.AddScoped<BaseDtoValidator<SignupDto>, SignupDtoValidator>();
builder.Services.AddScoped<BaseDtoValidator<UpdateStoryDto>, UpdateStoryDtoValidator>();
builder.Services.AddScoped<BaseDtoValidator<UpdateUserDto>, UpdateUserDtoValidator>();
builder.Services.AddScoped<BaseDtoValidator<UserDto>, UserDtoValidator>();
builder.Services.AddScoped<BaseDtoValidator<UserWithToken>, UserWithTokenValidator>();

//Interfaces
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<IStoryService, StoryService>();

builder.Services.AddScoped<IPasswordHandler, PasswordHandler>();
builder.Services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
builder.Services.AddScoped<ICookieHandler, CookieHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ConfigureExceptionHandler();
app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
