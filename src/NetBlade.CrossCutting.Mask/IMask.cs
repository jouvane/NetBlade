namespace NetBlade.CrossCutting.Mask
{
    public interface IMask
    {
        string CleanValue(string value);

        string Format(string value);
    }
}
