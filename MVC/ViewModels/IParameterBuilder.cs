namespace MVC.ViewModels
{
    public interface IParameterBuilder
    {
        void Build(IParameterizedViewModel model,
                   string searchString, 
                   string sortingParam,
                   int pageSize, int pageNumber,
                   int id = 0, bool includeAuthors = false, bool includeGenres = false, bool IncludeRentalHistory = false);
    }
}