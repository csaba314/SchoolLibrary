namespace Common.Parameters
{
    public class Paging : IPaging
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
