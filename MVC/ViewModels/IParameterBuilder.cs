namespace MVC.ViewModels
{
    public interface IParameterBuilder
    {
        void Build(IParameterizedViewModel model,
                          string searchString = "", string searchBy = "", string recordsFilter = "",
                          string sortingParam = "",
                          int pageSize = 10, int pageNumber = 1,
                          int id = 0, bool includeAuthors = false, bool includeGenres = false, bool IncludeBooks = false,
                          bool IncludeCustomers = false, bool IncludeRentalHistory = false);
    }
}