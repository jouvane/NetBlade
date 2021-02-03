namespace NetBlade.Core.Events.PublishStrategy
{
    /// <summary>
    /// Run each notification handler on it's own thread using Task.Run(). Returns when any thread (handler) is finished. Note that you cannot capture any exceptions (See msdn documentation of Task.WhenAny)
    /// </summary>
    public interface IParallelWhenAny { }
}
