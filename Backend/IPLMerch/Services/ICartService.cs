using IPLMerch.Application.DTOs;

namespace IPLMerch.Services;

public interface ICartService
{
    Task<CartDto> GetCartAsync(Guid userId);
    Task<CartDto> AddToCartAsync(Guid userId, AddToCartDto dto);
    Task<CartDto> UpdateCartItemAsync(Guid userId, Guid productId, UpdateCartItemDto dto);
    Task<CartDto> RemoveFromCartAsync(Guid userId, Guid productId);
    Task ClearCartAsync(Guid userId);
}