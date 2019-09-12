namespace Common.Parameters
{
    public interface IOptions
    {
        int Id { get; set; }
        bool IncludeAuthors { get; set; }
        bool IncludeGenres { get; set; }
        bool IncludeRentalHistory { get; set; }
    }
}
