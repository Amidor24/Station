using NuGet.Packaging.Signing;

namespace Api.Dtos
{
    public record GetClimate
    {
        //public Guid StationId { get; set; }
        public int Periode { get; set; }
        public required DateTime StartedAt { get; set; }
        public required DateTime EndedtAt { get; set; }
    }
}
