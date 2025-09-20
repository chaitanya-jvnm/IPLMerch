namespace IPLMerch.Application.DTOs;

public class FranchiseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string City { get; set; }
    public string LogoUrl { get; set; }
    public string PrimaryColor { get; set; }
    public string SecondaryColor { get; set; }
}