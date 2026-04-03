using Church4Site.Models;
using Church4Site.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/*connection strings= IonosServerDataBase SqlDataBase*/
builder.Services.AddDbContext<Church4DbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDataBase"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });

    // Logging and Detailed Errors
    options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
           .EnableDetailedErrors();
           /*.EnableSensitiveDataLogging(); // Useful for seeing exactly what ID is failing*/
});


builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IMainServices, MainServices>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["AppSettings:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("AuthToken"))
            {
                context.Token = context.Request.Cookies["AuthToken"];
            }
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();


// 1. ERROR HANDLING FIRST
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Main/Error");
    app.UseHsts(); // Tells browsers: "Only talk to me over HTTPS for the next year"
}

// 2. STATIC FILES SECOND (Fixes CSS/Image loading issues)
app.UseStaticFiles();

// 3. THE REDIRECT FIX
// Only redirect inside your code if NOT on IONOS (Production)
/*
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
*/

//to tell the app to trust ionos https redirect
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

if (!app.Environment.IsDevelopment())
{
    // --- METHOD 3: Security for Browsers ---
    app.UseHsts();
}

//now this is safe to use because of "app.UseForwardedHeaders"
app.UseHttpsRedirection();


app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets(); //for catching elements in the dom

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=MainPage}/{id?}")
    .WithStaticAssets();

//app.UseStaticFiles(); 
//disable this for better dom caching of elements

app.Run();
