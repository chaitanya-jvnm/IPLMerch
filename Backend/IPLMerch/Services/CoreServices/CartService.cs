using AutoMapper;
using IPLMerch.Application.DTOs;
using IPLMerch.Application.Models;
using IPLMerch.Infrastructure.Repositories;

namespace IPLMerch.Services.CoreServices;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<CartDto> GetCartAsync(Guid userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            cart = await CreateCartForUserAsync(userId);
        }

        return _mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> AddToCartAsync(Guid userId, AddToCartDto dto)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            cart = await CreateCartForUserAsync(userId);
        }

        var product = await _productRepository.GetByIdAsync(dto.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found");
        }

        if (!product.IsAvailable || product.StockQuantity < dto.Quantity)
        {
            throw new InvalidOperationException("Product is not available or insufficient stock");
        }

        var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, dto.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
            await _cartRepository.UpdateCartItemAsync(existingItem);
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
            await _cartRepository.AddItemToCartAsync(cartItem);
        }

        return await GetCartAsync(userId);
    }

    public async Task<CartDto> UpdateCartItemAsync(Guid userId, Guid productId, UpdateCartItemDto dto)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            throw new InvalidOperationException("Cart not found");
        }

        var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, productId);
        if (cartItem == null)
        {
            throw new InvalidOperationException("Item not in cart");
        }

        if (dto.Quantity <= 0)
        {
            await _cartRepository.RemoveItemFromCartAsync(cartItem);
        }
        else
        {
            cartItem.Quantity = dto.Quantity;
            await _cartRepository.UpdateCartItemAsync(cartItem);
        }

        return await GetCartAsync(userId);
    }

    public async Task<CartDto> RemoveFromCartAsync(Guid userId, Guid productId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            throw new InvalidOperationException("Cart not found");
        }

        var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, productId);
        if (cartItem != null)
        {
            await _cartRepository.RemoveItemFromCartAsync(cartItem);
        }

        return await GetCartAsync(userId);
    }

    public async Task ClearCartAsync(Guid userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart != null)
        {
            await _cartRepository.ClearCartAsync(cart.Id);
        }
    }

    private async Task<Cart> CreateCartForUserAsync(Guid userId)
    {
        var cart = new Cart { UserId = userId };
        return await _cartRepository.AddAsync(cart);
    }
}