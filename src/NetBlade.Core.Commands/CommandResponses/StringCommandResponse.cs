namespace NetBlade.Core.Commands
{
    public class StringCommandResponse : CommandResponse<string>
    {
        protected StringCommandResponse(bool success)
            : base(success)
        {
        }
    }
}
