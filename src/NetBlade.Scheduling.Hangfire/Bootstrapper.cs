using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetBlade.Scheduling.Hangfire
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration, string sectionNetBladeHangfireOptions = "NetBlade:HangfireOptions", bool autoAddJobsScoped = true, Action<NetBladeHangfireOptions> configureOptions = null)
        {
            configureOptions ??= ((NetBladeHangfireOptions c) =>
            {
                configuration.Bind(sectionNetBladeHangfireOptions, c);
                c.OnConfigure?.Invoke(configuration, sectionNetBladeHangfireOptions);
            });

            services
                .Configure<NetBladeHangfireOptions>(configureOptions)
                .AddSingleton<IScheduler, HangfireScheduler>(s =>
                {
                    IOptions<NetBladeHangfireOptions> NetBladeHangfireOptions = s.GetRequiredService<IOptions<NetBladeHangfireOptions>>();
                    return new HangfireScheduler(NetBladeHangfireOptions?.Value?.JobOptions);
                })
                .AddHangfire((IGlobalConfiguration config) =>
                {
                    string nameOrConnectionString = configuration.GetSection($"{sectionNetBladeHangfireOptions}:{nameof(NetBladeHangfireOptions.NameOrConnectionString)}").Value;
                    config
                    .UseSerilogLogProvider()
                    .UseSqlServerStorage(nameOrConnectionString, new global::Hangfire.SqlServer.SqlServerStorageOptions() { PrepareSchemaIfNecessary = configuration.GetValue<bool>($"{sectionNetBladeHangfireOptions}:{nameof(NetBladeHangfireOptions.PrepareSchemaIfNecessary)}") });
                });

            if (autoAddJobsScoped)
            {
                List<JobOptions> jobOptions = configuration.GetSection($"{sectionNetBladeHangfireOptions}:{nameof(NetBladeHangfireOptions.JobOptions)}").Get<List<JobOptions>>();
                if (jobOptions != null && jobOptions.Any())
                {
                    jobOptions.ForEach(job => services.AddScoped(job.Type));
                }
            }

            return services;
        }

        public static IApplicationBuilder AddHangfire(this IApplicationBuilder app)
        {
            IOptions<NetBladeHangfireOptions> config = app.ApplicationServices.GetRequiredService<IOptions<NetBladeHangfireOptions>>();
            HangfireScheduler hangfireScheduler = (HangfireScheduler)app.ApplicationServices.GetRequiredService<IScheduler>();

            app.UseHangfireServer(config.Value.BackgroundJobServerOptions).UseHangfireDashboard(config.Value.PathMatch, config.Value.DashboardOptions);
            hangfireScheduler.ScheduleJobs();

            return app;
        }
    }
}
