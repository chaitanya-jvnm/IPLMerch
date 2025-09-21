using IPLMerch.Application.DTOs;
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
    public ICollection<CartItem> Items { get; set; }
}