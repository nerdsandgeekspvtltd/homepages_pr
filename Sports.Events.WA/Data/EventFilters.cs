namespace Sports.Events.WA.Data
{
    public class EventFilters
    {
        public DateTime? fromDate { get; set; }
        public ScopeType? ScopeType { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
        public List<string>? SearchFields { get; set; }
        public string? Keyword { get; set; }
        //public List<string>? DivisionsIds { get; set; } = new List<string>();
        public string? DivisionsIds { get; set; }
        public List<string>? DisciplinesIds { get; set; } = new List<string>();
        public List<string>? TagIds { get; set; } = new List<string>();
        public List<string>? CreatorIds { get; set; } = new List<string>();
        public List<string> EventIds { get; set; } = new List<string>();
    }

    public enum ScopeType
    {
        Intern,
        Public
    }
}
