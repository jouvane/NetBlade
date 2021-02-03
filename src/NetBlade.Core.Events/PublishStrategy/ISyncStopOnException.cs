namespace NetBlade.Core.Events.PublishStrategy
{
    /// <summary>
    /// Run each notification handler after one another. Returns when all handlers are finished or an exception has been thrown. In case of an exception, any handlers after that will not be run.
    /// </summary>
    public interface ISyncStopOnException { }
}
