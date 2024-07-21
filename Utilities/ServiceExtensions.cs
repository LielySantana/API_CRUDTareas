using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using API_CRUDTareas.Models.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using static API_CRUDTareas.Application.AppSettings;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using AutoMapper;
using System.Collections.Generic;
using API_CRUDTareas.Interfaces;
using API_CRUDTareas.Services;

namespace API_CRUDTareas.Application
{
    /// <summary>
    /// Clase estática que proporciona métodos de extensión para la configuración de servicios en la aplicación.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configura los servicios de identidad
        /// </summary>
        /// <param name="services">Colección de servicios en la que se agregan los servicios de Identity.</param>
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            // Configura la identidad central de la app con la regla de que requiere correos electrónicos únicos
            services.AddIdentityCore<IdentityUser>(field => field.User.RequireUniqueEmail = true)
            .AddSignInManager<SignInManager<IdentityUser>>()
            .AddEntityFrameworkStores<TasksContext>()
            .AddDefaultTokenProviders();
        }

        /// <summary>
        /// Configura Swagger para la generación de documentación de la API.
        /// </summary>
        /// <param name="services">Colección de servicios en la que se agregan los servicios de Swagger.</param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {

                // Configura la autenticación en Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Encabezado de autorización JWT usando el esquema Bearer",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                // Agrega un requisito de seguridad a Swagger para usar JWT
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        
                        new string[] { }
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        /// <summary>
        /// Configura la sección 'AppSettings' del archivo de configuración para la aplicación.
        /// Registra la configuración como una instancia única y la hace disponible a través de la inyección de dependencias.
        /// </summary>
        /// <param name="services">Colección de servicios de la aplicación.</param>
        /// <param name="builder">Configuración de la aplicación obtenida desde el archivo appsettings.json.</param>
        public static void ConfigureAppSettings(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var appSettings = new AppSettings();
            builder.Configuration.Bind(appSettings);
            services.AddSingleton(appSettings);
        }

        /// <summary>
        /// Configura la autenticación JWT para la aplicación.
        /// Registra la configuración de JWT desde la sección 'Jwt' del archivo de configuración.
        /// Configura el esquema de autenticación por defecto como JWT Bearer y añade el esquema de cookies para sesiones de aplicación.
        /// </summary>
        /// <param name="services">Colección de servicios de la aplicación.</param>
        /// <param name="configuration">Configuración de la aplicación obtenida desde el archivo appsettings.json.</param>
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

            services.AddSingleton(jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            }).AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.Cookie.Name = "TASKSESSION";
            });
        }

        /// <summary>
        /// Configura el DbContext de la aplicación utilizando una cadena de conexión obtenida desde la configuración.
        /// </summary>
        /// <param name="services">Colección de servicios de la aplicación donde se registrarán los servicios.</param>
        /// <param name="configuration">Configuración de la aplicación obtenida desde el archivo appsettings.json.</param>
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContextPool<TasksContext>(options =>
            //    options.UseOracle(configuration.GetConnectionString("Oracle")));
            services.AddDbContextPool<TasksContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SQLServer")));
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
