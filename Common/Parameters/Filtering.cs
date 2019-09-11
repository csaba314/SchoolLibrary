namespace Common.Parameters
{
    public class Filtering : IFiltering
    {
        public string SearchString { get; set; } = string.Empty;
        public string CurrentFilter { get; set; } = string.Empty;
    }
}
