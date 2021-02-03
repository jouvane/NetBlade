namespace NetBlade.Core.Events.PublishStrategy
{
    /// <summary>
    /// Run all notification handlers asynchronously. Returns when all handlers are finished. In case of any exception(s), they will be captured in an AggregateException.
    /// </summary>
    public interface IAsync { }
}
