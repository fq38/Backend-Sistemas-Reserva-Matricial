namespace ReservaSalas.Dto
{
    public class CreateReservationDto
    {
        // Propriedades que o front-end realmente envia
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Responsible { get; set; }
        public bool HasCoffee { get; set; }
        public int? NumberOfPeopleForCoffee { get; set; }
        public string? Description { get; set; }
    }
}