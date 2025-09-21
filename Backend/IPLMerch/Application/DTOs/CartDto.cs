namespace IPLMerch.Application.DTOs;

public class CartDto
{
    public Guid Id { get; set; }
    public List<CartItemDto> Items { get; set; }
    public decimal TotalAmount { get; set; }
}

public class CartItemDto
{
    public Guid Id { get; set; }
    public ProductDto Product { get; set; }
    public int Quantity { get; set; }
    public decimal SubTotal { get; set; }
}