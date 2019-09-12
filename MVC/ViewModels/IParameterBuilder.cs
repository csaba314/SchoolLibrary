namespace MVC.ViewModels
{
    public interface IParameterBuilder
    {
        void Build(IParameterizedViewModel model,
                   string searchString, 
                   string sortingParam,
                   int pageSize, int pageNumber,
                   bool includeAuthors = false, bool includeGenres = false);
    }
}