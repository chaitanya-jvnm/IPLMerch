namespace IPLMerch.Application.DTOs;

public class CartDto
{
    public Guid Id { get; set; }
    public List<CartItemDto> Items { get; set; }
    public decimal TotalAmount { get; set; }
}