namespace ReservaSalas.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
