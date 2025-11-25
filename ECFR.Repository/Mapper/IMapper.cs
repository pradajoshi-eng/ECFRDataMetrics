using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECFR.Repositories.Mapper
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
        TSource Map(TDestination destination); //for two-way mapping
    }
}
