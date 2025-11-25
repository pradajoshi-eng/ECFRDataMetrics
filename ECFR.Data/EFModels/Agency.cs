using System;
using System.Collections.Generic;

namespace ECFR.Data.EFModels;

public partial class Agency
{
    public int Id { get; set; }

    public int? AgencyId { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public string? DisplayName { get; set; }

    public string? SortableName { get; set; }

    public string? Slug { get; set; }

    public virtual ICollection<AgencyCfrReferenceRel> AgencyCfrReferenceRels { get; set; } = new List<AgencyCfrReferenceRel>();

    public virtual Agency? AgencyNavigation { get; set; }

    public virtual ICollection<AgencySnapshot> AgencySnapshots { get; set; } = new List<AgencySnapshot>();

    public virtual ICollection<Agency> InverseAgencyNavigation { get; set; } = new List<Agency>();
}
