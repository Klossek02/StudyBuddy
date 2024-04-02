using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Data;
using StudyBuddy.Models;
using StudyBuddy;
using Microsoft.OpenApi.Models;
using NETCore.MailKit.Core;
using StudyBuddy.Services;
using StudyBuddy.Resources;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API StudyBuddy", Version = "v1" });
});

// Register services and resources
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentResource, StudentResource>();
builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddScoped<ITutorResource, TutorResource>();
builder.Services.AddScoped<IAdminResource, AdminResource>();
builder.Services.AddScoped<IAdminService, AdminService>();

// adding services to the container
builder.Services.AddRazorPages();

// adding DbContext and Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// adding Authentication with JWT Tokens
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyHere")),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// configuring HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
