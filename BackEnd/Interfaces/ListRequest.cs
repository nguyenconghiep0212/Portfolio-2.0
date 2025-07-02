namespace BackEnd.Interfaces
{
    public class ListRequest
    {
        public int offset { get; set; } = 0;
        public int limit { get; set; } = 10;

        public IncidentDateFilterRequest? date { get; set; }
        public string? name { get; set; }
        public List<string>? cityCode { get; set; }
        public List<int>? severity { get; set; } 
    }

    public class IncidentDateFilterRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
