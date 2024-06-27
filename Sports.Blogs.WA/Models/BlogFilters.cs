namespace Sports.Blogs.WA.Models
{
    public class BlogFilters
    {
        public DateTime? FromDate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? OrderByType { get; set; }
        public string? OrderBy { get; set; }
        public string? Keyword { get; set; }
        public string? SearchFields { get; set; }
        //public List<string>? DivisionIds { get; set; }
        public string? DivisionIds { get; set; }
        public List<string>? DisciplineIds { get; set; }
        public List<string>? TagIds { get; set; }
        public List<string>? CreatorIds { get; set; }
        public string? StartingPointLangitude { get; set; }
        public string? StartingPointLongitude { get; set; }
        public string? CreatorClubPointLatitute { get; set; }
        public string? CreatorClubRadius { get; set; }
    }
}
