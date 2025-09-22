using IPLMerch.Enums;

namespace IPLMerch.Application.Models;

public class Order : BaseEntity
{
    /// <summary>
    /// Tracking Information for the Order
    /// </summary>
    public Guid OrderNumber { get; set; }

    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public string ShippingAddress { get; set; }
    public string BillingAddress { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    
    public User User { get; set; }
    public ICollection<OrderItem> Items { get; set; }
}

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
        
    public Order Order { get; set; }
    public Product Product { get; set; }
}