using System.Text.Json.Serialization;

namespace nidirect_pointer_poc_core.DTOs
{
    public sealed class PointerDto
    {
        public string? OrganisationName { get; set; }
        public string? SubBuildingName { get; set; }
        public string? BuildingName { get; set; }
        public string? BuildingNumber { get; set; }
        public string? PrimaryThorfare { get; set; }
        public string? AltThorfareName1 { get; set; }
        public string? SecondaryThorfare { get; set; }
        public string? Locality { get; set; }
        public string? TownLand { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string? Postcode { get; set; }
        public string? Blpu { get; set; }
        public int? UniqueBuildingId { get; set; }
        public int? Uprn { get; set; }
        public int? Usrn { get; set; }
        public string? LocalCouncil { get; set; }
        public int? XCor { get; set; }
        public int? Ycor { get; set; }
        public string? TempCoords { get; set; }
        public string? BuildingStatus { get; set; }
        public string? AddressStatus { get; set; }
        public string? Classification { get; set; }

        [JsonPropertyName("Creation_Date")]
        public DateTime? CreationDate { get; set; }

        [JsonPropertyName("Commencement_Date")]
        public DateTime? CommencementDate { get; set; }

        [JsonPropertyName("Archived_Date")]
        public DateTime? ArchivedDate { get; set; }

        public string? Action { get; set; }
        public string? Udprn { get; set; }
        public string? PostTown { get; set; }
    }
}