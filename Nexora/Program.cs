
using System.Configuration;
using System.Data;
using System.Text;
using Application.Interface.Repository;
using Application.Interface.Service;
using CloudinaryDotNet;
using Common.Helpers;
using Common.MapperProfile;
using Infrastructure.Data;
using Infrastructure.Repository.AuthRepository;
using Infrastructure.Repository.CouncelorRepository;
using Infrastructure.Services.AuthService;
using Infrastructure.Services.CloudinaryService;
using Infrastructure.Services.CouncelorService;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Repository.BookinRepository;
using Infrastructure.Services.BookinService;
using Infrastructure.Services.ReviewService;
using Infrastructure.Repository.ReviewRepository;
using Infrastructure.Repository.AdminRepository;
using Infrastructure.Services.AdminService;

using Common.NHub;
using Infrastructure.Repository;
using Infrastructure.Repository.NotificationRepository;

namespace Nexora
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

           
           
            var builder = WebApplication.CreateBuilder(args);


            // ✅ Load .env file (optional)
            DotNetEnv.Env.Load();

            // ✅ Allow frontend requests (CORS)
            var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL")
                ?? builder.Configuration["AppSettings:FrontendUrl"];

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(frontendUrl) // http://localhost:5173
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // For sending cookies
                });
            });


            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

           

            builder.Services.AddSingleton<DapperContext>();

            builder.Host.UseSerilog();
            builder.Services.AddScoped<IJwtTokenGenerator , JwtTokenGenerator>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
            builder.Services.AddScoped<ICouncelorRepo, Councelor>();
            builder.Services.AddScoped<ICouncelorService, CouncelorLogin>();
            builder.Services.AddScoped<IBookingRepository, BookinRepository>();
            builder.Services.AddScoped<IBookingService,BookingService>();
            builder.Services.AddScoped<IReviewService,ReviewService>();
            builder.Services.AddScoped<IReviewRepository,ReviewRepository>();
            builder.Services.AddScoped<IBookinRepositiryByCouncelor,BookingByCouncelorRepo>();
            builder.Services.AddScoped<IBookingServiceByCouncelor,BookingServiceByCouncelor>();
            builder.Services.AddScoped<IPaymentRepository,PaymentRepository>();
            builder.Services.AddScoped<IPaymentService,PaymentService>();
            builder.Services.AddScoped<IAdminRepository,AdminDashBoardRepository>();
            builder.Services.AddScoped<IAdminService, AdminDashBoardService>();
            builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
            builder.Services.AddScoped<IAdminUserService,AdminUserService >();
            builder.Services.AddScoped<IAdminPaymentRepository, AdminPaymentRepository>();
           builder.Services.AddScoped<IAdminPaymentService,AdminPaymentService>();
            builder.Services.AddScoped<INotificationRepository,NotificationRepository>();
            builder.Services.AddSignalR();


            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserServices>();
            // Add services to the container.
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nexora Api", Version = "v1" });

               
                c.AddSecurityDefinition("cookieAuth", new OpenApiSecurityScheme
                {
                    Name = "accessToken",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Cookie,
                    Description = "JWT token stored in the 'accessToken' cookie"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "cookieAuth",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
            });




            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
       


           builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.ContainsKey("accessToken"))
                            {
                                context.Token = context.Request.Cookies["accessToken"];
                            }
                            return Task.CompletedTask;
                        }
                    };

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });



            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });





            


            var app = builder.Build();
            app.UseCors("AllowFrontend");
            app.MapHub<NotificationHub>("/notificationHub");

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
        }
    }
}
