namespace Common.Parameters
{
    public interface IParametersFactory
    {
        IFiltering FilteringInstance();
        IOptions OptionsInstance();
        IPaging PagingInstance();
        ISorting SortingInstance();
    }
}
