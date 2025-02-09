using BlazingQuiz.Api.Data;
using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Api.Endpoints;
using BlazingQuiz.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IPasswordHasher<User>,PasswordHasher<User>>();

builder.Services.AddDbContext<QuizContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlazingQuiz"));
});

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(option =>
{
    var secretKey = builder.Configuration.GetValue<string>("JWT:secret");
    var symmetrickey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
    option.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = symmetrickey,
        ValidIssuer = builder.Configuration.GetValue<string>("JWT:issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("JWT:audience"),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
    };

});

builder.Services.AddCors(options=>
{
    options.AddDefaultPolicy(builderCors =>
    {
        var AllowedOriginsStr = builder.Configuration.GetValue<string>("AllowedOrigins");
        var AllowedOrigins = AllowedOriginsStr.Split(",",StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        builderCors
        .WithOrigins(AllowedOrigins)
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddTransient<AuthService>();



var app = builder.Build();


#if DEBUG
ApplayMigrations(app.Services);
#endif

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors();

app.MapAuthEndpoints();

app.Run();

static void ApplayMigrations(IServiceProvider sp)
{
    var scope= sp.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<QuizContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}