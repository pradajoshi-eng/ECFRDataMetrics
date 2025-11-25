using System;
using System.Collections.Generic;

namespace ECFR.Data.EFModels;

public partial class CfrReference
{
    public int Id { get; set; }

    public int Title { get; set; }

    public string? SubTitle { get; set; }
    public string? Chapter { get; set; }
    public string? SubChapter { get; set; }

    public virtual ICollection<AgencyCfrReferenceRel> AgencyCfrReferenceRels { get; set; } = new List<AgencyCfrReferenceRel>();
}
