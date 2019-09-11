using Common.Parameters;

namespace MVC.ViewModels
{
    public interface IParameterizedViewModel
    {
        IFiltering Filtering { get; set; }
        ISorting Sorting { get; set; }
        IPaging Paging { get; set; }
        IOptions Options { get; set; }
    }
}