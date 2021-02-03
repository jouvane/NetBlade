using Hangfire.Dashboard;

namespace NetBlade.Scheduling.Hangfire
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
