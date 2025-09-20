namespace IPLMerch.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public string ShippingAddress { get; set; }
    public string BillingAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public List<ProductDto> Items { get; set; }
}