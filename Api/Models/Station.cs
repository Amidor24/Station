namespace Api.Models
{
    public class Station
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Reference { get; set; }
    }
}
