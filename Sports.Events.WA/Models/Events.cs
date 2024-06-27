namespace Sports.Events.WA.Models
{
    public class Event
    {
        public string Id { get; set; }
        public QueryParameters QueryParameters { get; set; }
        public List<Items> Items { get; set; }
        public int Page { get; set; }
        public int Pagesize { get; set; }
        public string Searchfields { get; set; }
        public int Maximumcount { get; set; }
        public string Orderbytype { get; set; }
    }

    public class QueryParameters
    {
        public List<string> TenantIds { get; set; }
    }

    public class Items
    {
        public string Id { get; set; }
        public bool RequestRequired { get; set; }
        public Creator Creator { get; set; }
        public string Pkey { get; set; }
        public bool Closed { get; set; }
        public int FilterRestrictionCount { get; set; }
        public int InvitedCount { get; set; }
        public bool DisableParticipants { get; set; }
        public string DetailPageId { get; set; }
        public int ParticipantCount { get; set; }
        public int WaitingCount { get; set; }
        public string Description { get; set; }
        public string EventType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime WithDrawalDeadlineTime { get; set; }
        public int MaxParticipating { get; set; }
        public int MaxWaiting { get; set; }
        public string ContractId { get; set; }
        public bool PaymentRequired { get; set; }
        public bool AddressAndIBANRequired { get; set; }
        public string AvatarImageUrl { get; set; }
        public string HeaderImageUrl { get; set; }
        public bool AddressRequired { get; set; }
        public bool IbanRequired { get; set; }
        public string ClubRequired { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
        public string SeminarId { get; set; }
        public bool Disabled { get; set; }
        public bool Deleted { get; set; }
        public List<string> Participants { get; set; }
        public List<string> Waiting { get; set; }
        public List<string> ContactPersons { get; set; }
        public string PublicationCriteria { get; set; }
        public bool IsCompetitiveSport { get; set; }
        public bool IsPopularSport { get; set; }
        public bool HasReferee { get; set; }
        public DateTime PublishStartTime { get; set; }
        public DateTime PublishEndTime { get; set; }
        public bool ChatEnabled { get; set; }
        public List<string> Tags = new List<string>();
        public List<string> Disciplines = new List<string>();
        public List<string> Divisions = new List<string>();
        public List<string> Types = new List<string>();
        public EventLocation Location { get; set; } = new EventLocation();
    }

    public class EventLocation
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Creator
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string AvatarImageUrl { get; set; }


    }

}
