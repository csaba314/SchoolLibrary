namespace Common.Parameters
{
    public interface IFiltering
    {
        string CurrentFilter { get; set; }
        string SearchString { get; set; }
        string SearchBy { get; set; }
        string RecordsFilter { get; set; }
    }
}
