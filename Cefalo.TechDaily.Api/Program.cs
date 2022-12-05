
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
using Cefalo.TechDaily.Service.Utils.Services;
using Cefalo.TechDaily.Service.Utils.Contracts;
using Cefalo.TechDaily.Api.GlobalExceptionHandler;
using Microsoft.Extensions.Logging;
using Cefalo.TechDaily.Service.DtoValidators;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Api.CustomOutputFormatter.StoryOutputFormatter;
using Cefalo.TechDaily.Api.CustomOutputFormatter.UserOutputFormatter;
using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using HealthChecks.UI.Client;

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHealthChecks().
    AddSqlServer("Data Source=tcp:cefalotechdailyapidbserver.database.windows.net,1433;Initial Catalog=Cefalo.TechDaily.Api_db;User Id=sajid@cefalotechdailyapidbserver;Password=sajid")
    .AddUrlGroup(new Uri("https://localhost:7010/api/stories"));
builder.Services.AddHealthChecksUI().AddInMemoryStorage();
builder.Services.AddHttpClient();
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
        builder.WithOrigins("http://localhost:3001", "https://localhost:3001", "https://techdaily2022.netlify.app/")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

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

//Interfaces
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<IStoryService, StoryService>();

builder.Services.AddScoped<IPasswordHandler, PasswordHandler>();
builder.Services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();

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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
    {
        AllowCachingResponses = false,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    endpoints.MapHealthChecksUI(options =>
    {
        options.UIPath = "/healthchecks-ui";
        options.ApiPath = "/health-ui-api";
        
    });
    endpoints.MapRazorPages();

});
Task HealthCheckResponseWriter(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";
    JObject obj = new JObject(
        new JProperty("Overall status", report.Status.ToString()),
        new JProperty("Time took", report.TotalDuration.TotalSeconds.ToString("0:0:00")),
        new JProperty("Dependency Health Checks", new JObject(
            report.Entries.Select(item =>
                new JProperty(item.Key,
                new JObject(
                    new JProperty("Status", item.Value.Status.ToString()),
                    new JProperty("Data", item.Value.Data.ToString()),
                    new JProperty("Exception", item.Value.Exception?.ToString()),
                    new JProperty("Tags", item.Value.Tags.ToString()),
                    new JProperty("Duration", item.Value.Duration.TotalSeconds.ToString("0:0:00"))
                    ))
                ))
        ));
    return context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
}
app.MapControllers();

app.Run();
