namespace ReservaSalas.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Responsible { get; set; }
        public bool HasCoffee { get; set; }
        public int? NumberOfPeopleForCoffee { get; set; }
        public string Description { get; set; }
    }
}
