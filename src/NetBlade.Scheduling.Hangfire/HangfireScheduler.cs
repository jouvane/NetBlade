using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetBlade.Scheduling.Hangfire
{
    public class HangfireScheduler : IScheduler
    {
        private static readonly MethodInfo[] _recurringJobMethodInfo = HangfireScheduler.GetRecurringJobMethodInfo();
        private readonly IList<JobOptions> _jobsSchedule;

        public HangfireScheduler(IList<JobOptions> jobsSchedule)
        {
            this._jobsSchedule = jobsSchedule;
        }

        public virtual async Task Abort(string key, Type jobType)
        {
            await Task.Run(() => this.Delete(key));
        }

        public virtual bool Delete(string jobId)
        {
            return BackgroundJob.Delete(jobId);
        }

        public virtual bool Delete(string jobId, string fromState)
        {
            return BackgroundJob.Delete(jobId, fromState);
        }

        public virtual string Enqueue(Expression<Action> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }

        public string Enqueue<TJob, TData>(TData data)
            where TJob : IJob<TData>
            where TData : new()
        {
            return BackgroundJob.Enqueue<TJob>((TJob job) => job.Execute(data));
        }

        public virtual bool Requeue(string jobId)
        {
            return BackgroundJob.Requeue(jobId);
        }

        public virtual bool Requeue(string jobId, string fromState)
        {
            return BackgroundJob.Requeue(jobId, fromState);
        }

        public virtual string Schedule(Expression<Action> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule(methodCall, delay);
        }

        public virtual string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule(methodCall, delay);
        }

        public virtual string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt)
        {
            return BackgroundJob.Schedule(methodCall, enqueueAt);
        }

        public virtual string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt)
        {
            return BackgroundJob.Schedule(methodCall, enqueueAt);
        }

        public virtual string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule(methodCall, delay);
        }

        public virtual string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule(methodCall, delay);
        }

        public virtual string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt)
        {
            return BackgroundJob.Schedule(methodCall, enqueueAt);
        }

        public virtual string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt)
        {
            return BackgroundJob.Schedule(methodCall, enqueueAt);
        }

        public string Schedule<TJob, TData>(TData data, TimeSpan delay)
            where TJob : IJob<TData>
            where TData : new()
        {
            return BackgroundJob.Schedule<TJob>((TJob job) => job.Execute(data), delay);
        }

        public string Schedule<TJob, TData>(TData data, DateTimeOffset delay)
            where TJob : IJob<TData>
            where TData : new()
        {
            return BackgroundJob.Schedule<TJob>((TJob job) => job.Execute(data), delay);
        }

        public virtual void ScheduleJobs()
        {
            IList<JobOptions> jobs = this._jobsSchedule;
            if (jobs != null && jobs.Any())
            {
                foreach (JobOptions j in jobs)
                {
                    Type funcType = typeof(Func<,>).MakeGenericType(j.Type, typeof(Task));
                    Type expressionType = typeof(Expression<>).MakeGenericType(funcType);
                    MethodInfo methodExecuteIJob = j.Type.GetMethod(nameof(IJob<Dictionary<string, string>>.Execute), new[] { typeof(Dictionary<string, string>) });

                    ParameterExpression parameterExpression = Expression.Parameter(j.Type, "job");
                    MethodCallExpression body = Expression.Call(parameterExpression, methodExecuteIJob, Expression.Constant(j.Data));

                    MethodInfo expressionLambdaMethodInfo =
                        typeof(Expression)
                        .GetMethod(nameof(Expression.Lambda), 1, new[] { typeof(Expression), typeof(ParameterExpression[]) })
                        .MakeGenericMethod(funcType);

                    object expression = expressionLambdaMethodInfo.Invoke(null, new object[] { body, new[] { parameterExpression } });

                    MethodInfo addOrUpdateMethodInfo =
                        HangfireScheduler._recurringJobMethodInfo
                        .Select(s => s.MakeGenericMethod(j.Type))
                        .First(f => f.GetParameters().Any(a => a.ParameterType.Equals(expressionType)));

                    string queue = (j.Queue ?? "default").ToLower().Trim();
                    if (queue.Equals("localhost"))
                    {
                        queue = Environment.MachineName;
                    }

                    foreach (string item in queue.Split(';'))
                    {
                        string valueQueue = Regex.Match(item.ToLower(), @"[a-z0-9_-]+").Value;
                        _ = addOrUpdateMethodInfo.Invoke(null, new object[] { expression, j.CronnExpression, TimeZoneInfo.Local, valueQueue });
                    }
                }
            }
        }

        protected static MethodInfo[] GetRecurringJobMethodInfo()
        {
            MethodInfo[] recurringJobMethodInfo = typeof(RecurringJob)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(w => w.IsGenericMethod)
                .Where(w => w.GetParameters().Any(a => a.Name.Equals("cronExpression") && a.ParameterType.Equals(typeof(string))))
                .Where(w => w.GetParameters().Length.Equals(4))
                .ToArray();

            return recurringJobMethodInfo;
        }
    }
}
