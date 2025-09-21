using AutoMapper;
using IPLMerch.Application.DTOs;
using IPLMerch.Application.Models;
using IPLMerch.Enums;
using IPLMerch.Infrastructure.Repositories;

namespace IPLMerch.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IMapper mapper, ICartRepository cartRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _cartRepository = cartRepository;
    }

    public async Task<OrderDto> CreateOrderAsync(Guid userId, CreateOrderDto address)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart?.Items == null || cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty");
        }

        foreach (var cartItem in cart.Items)
        {
            var product = cartItem.Product;
            if (product.StockQuantity < cartItem.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for {product.Name}");
            }
            
            // Update stock
            product.StockQuantity -= cartItem.Quantity;
            await _productRepository.UpdateAsync(product);
        }

        var order = new Order
        {
            OrderNumber = Guid.NewGuid(),
            UserId = userId,
            ShippingAddress = address.ShippingAddress,
            BillingAddress = address.BillingAddress,
            Status = OrderStatus.Pending,
            Items = cart.Items,
            TotalAmount = cart.TotalAmount,
        };
        
        await _orderRepository.AddAsync(order);

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(Guid userId)
    {
        var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}