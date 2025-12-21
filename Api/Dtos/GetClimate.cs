using NuGet.Packaging.Signing;

namespace Api.Dtos
{
    public record GetClimate
    {
        public Guid StationId { get; set; }
        public int Periode { get; set; }
        public required Timestamp StartedAt { get; set; }
        public required Timestamp EndedtAt { get; set; }
    }
}
