namespace NetBlade.Core
{
    public interface IResponse<TData> : IResponse
    {
        TData Data { get; set; }
    }
}