
 
namespace BackEnd.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City_Code { get; set; }
        public int Severity { get; set; }
        public string? Description { get; set; }
        public string LatLng { get; set; }
        public DateTime Time { get; set; }
        public bool Solved { get; set; }
        public string? Dispatcher_Team_Id { get; set; }

    }
}
