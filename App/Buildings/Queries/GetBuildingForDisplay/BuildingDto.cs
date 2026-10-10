namespace App.Buildings.Queries.GetBuildingForDisplay;

public class BuildingDto
{
    public required string Id { get; set; }
    public required string ClusterId { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }
    public required string Country { get; set; }
    public string? Description { get; set; }
}
