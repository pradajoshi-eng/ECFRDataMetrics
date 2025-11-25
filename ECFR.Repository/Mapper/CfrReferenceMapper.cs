using ECFR.Core.Models;
using ECFR.Data.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECFR.Repositories.Mapper
{
    public class CfrReferenceMapper : IMapper<CfrReferenceModel, CfrReference>
    {
        public CfrReference Map(CfrReferenceModel source)
        {
            return new CfrReference
            {
                Title = source.Title,
                SubTitle = source.SubTitle,
                Chapter = source.Chapter,
                SubChapter = source.SubChapter
            };
        }

        public CfrReferenceModel Map(CfrReference destination)
        {
            return new CfrReferenceModel
            {
                Title = destination.Title,
                SubTitle = destination.SubTitle,
                Chapter = destination.Chapter,
                SubChapter = destination.SubChapter
            };
        }
    }
}
