using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Movies.API.Filters;
using Movies.CrossCutting.Comum;
using Movies.IoC;
using NetBlade.API.Security;
using NetBlade.API.Security.Options;
using NetBlade.API.Security.SwaggerGens;
using Serilog;
using Serilog.Enrichers.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;

namespace Movies.API
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        internal static readonly FileVersionInfo _fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            app
                .UsePathBase("/Movies-api");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!"none".Equals(this.Configuration.GetValue<string>("ElasticApm:LogLevel")?.ToLower()))
            {
                app.UseAllElasticApm(this.Configuration);
            }

            IHttpContextAccessor httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            app.UseHttpContextMiddleware();

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithDynamicProperty("TransactionID", () => httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString())
                .Enrich.WithDynamicProperty("HeaderAuthorization", () => httpContextAccessor.HttpContext?.Request?.Headers["Authorization"], minimumLevel: Serilog.Events.LogEventLevel.Error)
                .Enrich.WithDynamicProperty("UserId", () => httpContextAccessor.HttpContext?.User?.FindFirstValue("id"))
                .Enrich.WithDynamicProperty("UserName", () => httpContextAccessor.HttpContext?.User?.FindFirstValue("userName"))
                .Enrich.WithDynamicProperty("UserIdentifier", () => httpContextAccessor.HttpContext?.User?.FindFirstValue("identifier"))
                .Enrich.WithDynamicProperty("UserRepresentedCpfCnpj", () => httpContextAccessor.HttpContext?.User?.FindFirstValue("representedCpfCnpj"))
                .Enrich.WithDynamicProperty("UserRepresentedName", () => httpContextAccessor.HttpContext?.User?.FindFirstValue("representedName"))
                .Enrich.WithHttpContext(serviceProvider)
                .ReadFrom.Configuration(this.Configuration)
                .CreateLogger();

            loggerFactory.AddSerilog();
            Movies.Infrastructure.Migrations.Bootstrapper.AddBootstrapperMigrations(this.Configuration, log => log.AddSerilog(Log.Logger, false));

            CultureInfo[] supportedCultures = { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(supportedCultures[0], supportedCultures[0]),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app
               .UseSwagger()
               .UseSwaggerUI();

            app
                .UseCors("AllowAll")
                .UseAuthentication()
                .UseAuthorization()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", async context =>
                    {
                        string stSgcacDataRelease = this.Configuration.GetValue<string>("DATE_RELEASE");
                        DateTime.TryParseExact(stSgcacDataRelease, "yyyy-MM-dd-HH-mm-ss", new CultureInfo("pt-br"), DateTimeStyles.AssumeUniversal, out DateTime dateValue);
                        await context.Response.WriteAsync($"{Startup._fileVersionInfo.FileVersion} - {dateValue.ToString(new CultureInfo("pt-br"))}");
                    });
                })
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHttpContextMiddleware();

            services
                .AddSecurity(this.Configuration, fnClaimsAdditional: (s, c) => Array.Empty<Claim>());

            services
                .AddMemoryCache()
                .AddControllers(options =>
                {
                    options.Filters.Add<ComumResponseActionFilterAttribute>();
                    options.Filters.Add<HttpContextFilter>();
                })
               .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
               .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddBootstrapperIoC(this.Configuration);

            services
                .AddScoped<TransactionID>(s =>
                {
                    IHttpContextAccessor httpContextAccessor = s.GetRequiredService<IHttpContextAccessor>();
                    string id = httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();
                    return new TransactionID(id);
                });

            services
                .AddSwaggerGen(c =>
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { Name = "Authorization", In = ParameterLocation.Header, Type = SecuritySchemeType.ApiKey, Scheme = "bearer" });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new List<string>() } });
                    c.OperationFilter<AddRotesInHeaderParameter>();
                })
                .PostConfigure<ForwardedHeadersOptions>(options => { })
                .Configure<AddRotesInHeaderParameterOption>(this.Configuration.GetSection("Swagger:AddRotesInHeaderParameter"))
                .Configure<SwaggerGenOptions>(this.Configuration.GetSection("Swagger:SwaggerGenOptions"))
                .Configure<SwaggerUIOptions>(c => this.Configuration.Bind("Swagger:SwaggerUIOptions", c));

            services.AddCors(options => options.AddPolicy("AllowAll", builder => builder.SetIsOriginAllowed((string d) => true).AllowCredentials().AllowAnyMethod().AllowAnyHeader()));
        }
    }
}
