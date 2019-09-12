using Common.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class ParameterBuilder : IParameterBuilder
    {
        private IParametersFactory _parametersFactory;

        public ParameterBuilder(IParametersFactory parametersFactory)
        {
            _parametersFactory = parametersFactory;
        }

        public void Build(IParameterizedViewModel model, 
                          string searchString, 
                          string sortingParam, 
                          int pageSize, int pageNumber, 
                          int id = 0, bool includeAuthors = false, bool includeGenres = false, bool IncludeRentalHistory = false)
        {
            model.Filtering = _parametersFactory.FilteringInstance();
            model.Filtering.SearchString = searchString;
            model.Filtering.CurrentFilter = searchString;

            model.Sorting = _parametersFactory.SortingInstance();
            model.Sorting.SortingParam = sortingParam;

            model.Paging = _parametersFactory.PagingInstance();
            model.Paging.PageNumber = pageNumber;
            model.Paging.PageSize = pageSize;

            model.Options = _parametersFactory.OptionsInstance();
            model.Options.Id = id;
            model.Options.IncludeAuthors = includeAuthors;
            model.Options.IncludeGenres = includeGenres;
            model.Options.IncludeRentalHistory = IncludeRentalHistory;
        }
    }
}