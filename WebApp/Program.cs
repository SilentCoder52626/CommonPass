using DomainModule.Entity;
using InfrastructureModule.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;
using NToastNotify;
using System;
using System.Configuration;
using System.Text;
using WebApp.ActionFilters;
using WebApp.AuthenticationAuthorization;
using WebApp.CustomTokenProvider;
using WebApp.DefaultDataSeeder;
using WebApp.DiConfig;
using WebApp.Extensions;


var builder = WebApplication.CreateBuilder(args);

var CustomEmailTokenProposeString = "Email_Token_Provider";
// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {

        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    })
    .AddRazorRuntimeCompilation();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<CustomEmailTokenProvider<User>>(CustomEmailTokenProposeString);

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.SignIn.RequireConfirmedEmail = false;
    options.Tokens.EmailConfirmationTokenProvider = CustomEmailTokenProposeString;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.MaxValue; // lock out forever until the user is unlocked manually by admin
}
);
//Token lifespan config
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(1);
});


//Custome Token provider config -- for token the lifespan is 1 hour but for email token its 20 minute
builder.Services.Configure<CustomEmailTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(20));

builder.Services.AddMvc()
     .AddMvcOptions(options =>
     {
         var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
         options.Filters.Add(new AuthorizeFilter(policy));
         options.Filters.Add(typeof(ActivityLogFilters));

     })
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        ProgressBar = true,
        TimeOut = 1500,
        PositionClass = ToastPositions.TopRight
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                       ValidateIssuerSigningKey = true
                   };
               });

builder.Services.ConfigureAuthentication();
builder.Services.UseDIConfig();


builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<ActivityLogFilters>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<CookiePolicyOptions>(
                   options =>
                   {
                       options.MinimumSameSitePolicy = SameSiteMode.Lax;
                   }
               );
builder.Services.AddCookiePolicy(options => options.Secure = CookieSecurePolicy.None);
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();
//app.UseHangfireDashboard("/mydashboard");
app.UseStatusCodePagesWithReExecute("/Error/Error/{0}");

ServiceActivator.Configure(builder.Services.BuildServiceProvider());
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await DataSeeder.SeedUsersAndRolesAsync(app);

app.Run();
