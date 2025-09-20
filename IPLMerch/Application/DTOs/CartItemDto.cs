namespace IPLMerch.Application.DTOs;

public class CartItemDto
{
    public Guid Id { get; set; }
    public ProductDto Product { get; set; }
    public int Quantity { get; set; }
    public decimal SubTotal { get; set; }
}