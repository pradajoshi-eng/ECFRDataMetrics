namespace ECFR.Core.Models
{
    public class AgencyModel
    {
        public int Id;
        public int AgencyId;
        public string Name;
        public string? Short_name;
        public string? Display_name;
        public string? Sortable_name;
        public string? Slug;
        public List<AgencyModel> Agencies; // recursive for nested agencies
        public List<CfrReferenceModel> Cfr_references;
        public ICollection<AgencySnapshotModel> Snapshots { get; set; } = new List<AgencySnapshotModel>();

        public List<AgencyModel> Children; // recursive for nested agencies from json data

    }
}
