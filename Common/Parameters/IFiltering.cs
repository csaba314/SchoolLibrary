namespace Common.Parameters
{
    public interface IFiltering
    {
        string CurrentFilter { get; set; }
        string SearchString { get; set; }
    }
}
