namespace IPLMerch.Application.Models;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
        
    public Cart Cart { get; set; }
    public ICollection<Order> Orders { get; set; }
}