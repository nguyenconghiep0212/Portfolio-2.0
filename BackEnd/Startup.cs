using BackEnd.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SignalRWebpack.Hubs;
using System.Text;

namespace BackEnd
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string WebUrl = Configuration["WebUrl"];
            string JwtSecretKey = Configuration["JWTKey:JwtSecretKey"];
            string ValidIssuer = Configuration["JWTKey:ValidIssuer"];
            string ValidAudience = Configuration["JWTKey:ValidAudience"];
            string DbConnection = Configuration["DbConnection"];

            services.AddSignalR();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = ValidIssuer,
                        ValidAudience = ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecretKey))
                    };
                });
            services.AddAuthorization();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.WithOrigins(WebUrl)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Your API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIs...\""
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            services.AddDbContext<SampleContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(DbConnection),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()));
            services.AddDbContext<UserContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(DbConnection),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()));
            services.AddDbContext<CityContext>(options =>
              options.UseSqlServer(
                  Configuration.GetConnectionString(DbConnection),
                  sqlOptions => sqlOptions.EnableRetryOnFailure()));
            services.AddDbContext<IncidentContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(DbConnection),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure();
                    }));
            services.AddDbContext<PoiContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(DbConnection),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<IncidentHub>("/IncidentHub");
                endpoints.MapControllers();
            });

        }
    }
}