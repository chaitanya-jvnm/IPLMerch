namespace IPLMerch.Application.Models;

public class Cart : BaseEntity
{
    public Guid UserId { get; set; }
        
    public User User { get; set; }
    public ICollection<CartItem> Items { get; set; }
        
    public decimal TotalAmount => Items?.Sum(i => i.Quantity * i.Product.Price) ?? 0;
}