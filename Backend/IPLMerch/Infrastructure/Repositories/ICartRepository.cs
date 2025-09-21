using IPLMerch.Application.Models;

namespace IPLMerch.Infrastructure.Repositories;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<Cart?> GetCartByUserIdAsync(Guid userId);

    Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId);

    Task AddItemToCartAsync(CartItem cartItem);

    Task UpdateCartItemAsync(CartItem cartItem);

    Task RemoveItemFromCartAsync(CartItem cartItem);

    Task ClearCartAsync(Guid cartId);
}