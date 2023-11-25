using apiWeb_MVC.Services;
using Datos;
using Configuracion;
using Datos.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Datos.Services;
using Datos.Schemas;
using Datos.Validate;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

//DI
builder.Services.AddScoped<IDaoBD,DaoBD>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IValidateMethodes, ValidateMethodes>();
builder.Services.AddScoped<IDaoBDBook, DaoBDBook>();
builder.Services.AddScoped<IBookServices, BookServices>();
builder.Services.AddScoped<IDaoBDRentedBook, DaoBDRentedBook>();
builder.Services.AddScoped<IRentedBook, RentedBook>();
builder.Services.AddScoped<IRentedServices, RentedServices>();
builder.Services.AddScoped<IDaoBDAuthor, DaoBDAuthor>();
builder.Services.AddScoped<IAuthorServices, AuthorServices>();
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<BDConfiguration>(builder.Configuration.GetSection("BD"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("*");
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
