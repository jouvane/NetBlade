using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetBlade.Scheduling.Hangfire
{
    public class NetBladeHangfireOptions
    {
        public Action<IConfiguration, string> OnConfigure;

        public NetBladeHangfireOptions()
        {
            this.BackgroundJobServerOptions = null;
            this.DashboardOptions = null;
            this.JobOptions = null;
            this.NameOrConnectionString = null;
            this.PathMatch = "/hangfire";
            this.OnConfigure = this.OnConfigureInternal;
        }

        public BackgroundJobServerOptions BackgroundJobServerOptions { get; set; }

        public DashboardOptions DashboardOptions { get; set; }

        public List<JobOptions> JobOptions { get; set; }

        public string NameOrConnectionString { get; set; }

        public string PathMatch { get; set; }

        public bool PrepareSchemaIfNecessary { get; set; }

        internal void OnConfigureInternal(IConfiguration configuration, string sectionNetBladeHangfireOptions)
        {
            this.DashboardOptions ??= new DashboardOptions();
            this.BackgroundJobServerOptions ??= new BackgroundJobServerOptions();

            this.DashboardOptions.Authorization = new[] { new HangFireAuthorizationFilter() };
            this.BackgroundJobServerOptions.Queues = new string[] { Regex.Match(Environment.MachineName.ToLower(), @"[a-z0-9_-]+").Value, "default" };
        }
    }
}
