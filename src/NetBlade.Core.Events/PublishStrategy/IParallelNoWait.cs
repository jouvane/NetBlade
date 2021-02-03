namespace NetBlade.Core.Events.PublishStrategy
{
    /// <summary>
    /// Run each notification handler on it's own thread using Task.Run(). Returns immediately and does not wait for any handlers to finish. Note that you cannot capture any exceptions, even if you await the call to Publish.
    /// </summary>
    public interface IParallelNoWait { }
}
