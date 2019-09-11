namespace Common.Parameters
{
    public class Options : IOptions
    {
        public bool IncludeAuthors { get; set; } = false;

        public bool IncludeGenres { get; set; } = false;
    }
}
