using identityUser.Configuration;
using identityUser.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationUserContext>(option =>
{
	option.UseSqlServer(builder.Configuration
		.GetConnectionString("DefaultSQLConnection"));
});

builder.Services
	.AddIdentityCore<IdentityUser>(options => {
		options.SignIn.RequireConfirmedAccount = false;
		options.User.RequireUniqueEmail = true;
		options.Password.RequireDigit = false;
		options.Password.RequiredLength = 6;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireLowercase = false;
	})
	.AddEntityFrameworkStores<ApplicationUserContext>();

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters()
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidAudience = builder.Configuration["Jwt:Audience"],
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
			)
		};
	});


builder.Services.AddScoped<JwtService>();

// Add services to the container.

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

app.Run();
