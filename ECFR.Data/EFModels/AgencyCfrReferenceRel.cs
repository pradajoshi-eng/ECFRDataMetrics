using System;
using System.Collections.Generic;

namespace ECFR.Data.EFModels;

public partial class AgencyCfrReferenceRel
{
    public int Id { get; set; }

    public int AgencyId { get; set; }

    public int TitleId { get; set; }

    public virtual Agency Agency { get; set; } = null!;

    public virtual CfrReference Title { get; set; } = null!;
}
