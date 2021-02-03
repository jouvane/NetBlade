namespace NetBlade.Core.Commands
{
    public class Int32CommandResponse : CommandResponse<int>
    {
        protected Int32CommandResponse(bool success)
            : base(success)
        {
        }
    }
}
