using System.Text;
using BestDealLib;
using CoreApi2.Controllers;
using CoreApi2.Data;
using CoreApi2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CoreApi2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(
	option => option.UseSqlServer(connectionString));

// Define identity options
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
	option =>
	{
		option.Password.RequireDigit = true;
		option.Password.RequiredLength = 8;
		option.Password.RequireNonAlphanumeric = false;
		option.Password.RequireUppercase = true;
		option.Password.RequireLowercase = true;
	}
).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add authentication to the build
builder.Services.AddAuthentication(option => {
  option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
  options.SaveToken = true;
  options.RequireHttpsMetadata = true;
  options.TokenValidationParameters = new TokenValidationParameters()
	{
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"]))
  };
});

// Add Cors
builder.Services.AddCors(o => o.AddPolicy("Policy", builder => {
  builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("Policy");

app.MapControllers();


/* This block runs a local DB update when the app is started up */

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(SQLConnectionService.GetConnectionString())
    .Options;
using var dbContext = new ApplicationDbContext(options);


// Creating cancellation token for updateScheduler.
CancellationTokenSource source = new CancellationTokenSource();
CancellationToken token = source.Token;

// Update scheduler with base api where /menu and /stores are appended deppending on which is needed.
UpdateScheduler updateScheduler = new UpdateScheduler(dbContext, BaseApiService.baseAPIs);
await updateScheduler.StartAsync(token);



app.Run();
