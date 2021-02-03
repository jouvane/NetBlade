namespace NetBlade.Core.Events.PublishStrategy
{
    /// <summary>
    /// Run each notification handler on it's own thread using Task.Run(). Returns when all threads (handlers) are finished. In case of any exception(s), they are captured in an AggregateException by Task.WhenAll.
    /// </summary>
    public interface IParallelWhenAll { }
}
