using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
