using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BusinessLogic;
using DataAccess;
using Azure.Storage.Blobs;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Net.Http.Headers;
using BursaryManagementAPI;
using System.Security.Claims;
using BusinessLogic.Models;

/// <summary>
/// The startup.
/// </summary>

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }



    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DatabaseConnection");

        //adding db connection services to the dependency injection container (Single object used in the applications lifetime)

        services.AddSingleton<SqlConnection>(_ => new SqlConnection(connectionString));
        
        services.AddScoped<UniversityDAL>();
        services.AddScoped<UserDAL>();
        services.AddScoped<UploadDocumentDAL>();
        services.AddScoped<StudentFundRequestDAL>();
        services.AddScoped<StudentFundRequestBLL>();
        services.AddScoped<UploadDocumentBLL>();
        services.AddScoped<UniversityDAL>();
        services.AddScoped<UniversityFundRequestBLL>();
        services.AddScoped<AdminBLL>();
        services.AddScoped<AdminDAL>();



        //adding Azure services to the dependency injection container (scoped to instantiate a new object when requested )
        services.AddScoped(provider =>
        {
            var storageConnectionString = Configuration.GetConnectionString("AzureStorageConnectionString");
            var blobServiceClient = new BlobServiceClient(storageConnectionString);
            return blobServiceClient;
        });


        _ = services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            
        }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {   
            options.TokenValidationParameters = new TokenValidationParameters
            {   
           
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = Configuration["AuthSettings:Audience"],
                ValidIssuer = Configuration["AuthSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"]) ),
                ValidateIssuerSigningKey = true,
            };
        });
        Console.WriteLine(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"]));
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy =>
                policy.RequireRole(Roles.BBDAdmin));
        });
        services.AddScoped<UserBLL>();
        services.AddScoped<UploadDocumentBLL>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BursaryManagementAPI", Version = "v1" });
            c.AddSecurityDefinition("token", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Name = HeaderNames.Authorization,
                Scheme = "Bearer"
            });
            // dont add global security requirement
            // c.AddSecurityRequirement(/*...*/);
            c.OperationFilter<SecureEndpointAuthRequirementFilter>();
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BursaryManagementAPI v1");
            });
        }
            
        app.UseAuthentication();
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthorization();
        app.UseCors("AllowAnyOrigin");


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
