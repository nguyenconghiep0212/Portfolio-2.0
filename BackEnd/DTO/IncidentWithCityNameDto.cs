namespace BackEnd.DTO
{
    public class IncidentWithCityNameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public int Severity { get; set; }
        public string? Description { get; set; }
        public string LatLng { get; set; }
        public DateTime Time { get; set; }
        public bool Solved { get; set; }
        public string? Dispatcher_Team_Id { get; set; }
    }
}