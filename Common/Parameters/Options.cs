namespace Common.Parameters
{
    public class Options : IOptions
    {
        public int Id { get; set; }

        public bool IncludeAuthors { get; set; } = false;
        public bool IncludeGenres { get; set; } = false;
        public bool IncludeRentalHistory { get; set; } = false;
    }
}
