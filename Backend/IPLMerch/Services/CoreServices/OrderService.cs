using AutoMapper;
using IPLMerch.Application.DTOs;
using IPLMerch.Application.Models;
using IPLMerch.Enums;
using IPLMerch.Infrastructure.Repositories;

namespace IPLMerch.Services.CoreServices;

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

    public async Task<OrderDto> CreateOrderAsync(Guid userId, CreateOrderDto dto)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null || !cart.Items.Any())
        {
            throw new InvalidOperationException("Cart is empty");
        }

        var order = new Order
        {
            OrderNumber = Guid.NewGuid(),
            UserId = userId,
            ShippingAddress = dto.ShippingAddress,
            BillingAddress = dto.BillingAddress ?? dto.ShippingAddress,
            Status = OrderStatus.Pending,
            Items = new List<OrderItem>()
        };

        decimal totalAmount = 0;
        foreach (var cartItem in cart.Items)
        {
            var product = cartItem.Product;
            if (product.StockQuantity < cartItem.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for {product.Name}");
            }

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = cartItem.Quantity,
                UnitPrice = product.Price,
                TotalPrice = product.Price * cartItem.Quantity
            };

            order.Items.Add(orderItem);
            totalAmount += orderItem.TotalPrice;

            // Update stock
            product.StockQuantity -= cartItem.Quantity;
            await _productRepository.UpdateAsync(product);
        }

        order.TotalAmount = totalAmount;
        await _orderRepository.AddAsync(order);

        // Clear cart after order creation
        await _cartRepository.ClearCartAsync(cart.Id);

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(Guid userId)
    {
        var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}