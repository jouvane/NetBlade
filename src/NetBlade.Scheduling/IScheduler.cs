using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetBlade.Scheduling
{
    public interface IScheduler
    {
        Task Abort(string key, Type jobType);

        bool Delete(string jobId);

        bool Delete(string jobId, string fromState);

        string Enqueue(Expression<Action> methodCall);

        string Enqueue<TJob, TData>(TData data)
             where TJob : IJob<TData>
            where TData : new();

        bool Requeue(string jobId);

        bool Requeue(string jobId, string fromState);

        string Schedule(Expression<Action> methodCall, TimeSpan delay);

        string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay);

        string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt);

        string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt);

        string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);

        string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);

        string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt);

        string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt);

        string Schedule<TJob, TData>(TData data, TimeSpan delay)
            where TJob : IJob<TData>
            where TData : new();

        string Schedule<TJob, TData>(TData data, DateTimeOffset delay)
            where TJob : IJob<TData>
           where TData : new();
    }
}
