namespace Common.Parameters
{
    class ParametersFactory : IParametersFactory
    {

        private readonly IFiltering _filtering;
        private readonly IOptions _options;
        private readonly IPaging _paging;
        private readonly ISorting _sorting;

        public ParametersFactory(IFiltering filtering, IOptions options, IPaging paging, ISorting sorting)
        {
            _filtering = filtering;
            _paging = paging;
            _options = options;
            _sorting = sorting;
        }

        public IFiltering FilteringInstance()
        {
            return _filtering;
        }

        public IOptions OptionsInstance()
        {
            return _options;
        }

        public IPaging PagingInstance()
        {
            return _paging;
        }

        public ISorting SortingInstance()
        {
            return _sorting;
        }
    }
}
