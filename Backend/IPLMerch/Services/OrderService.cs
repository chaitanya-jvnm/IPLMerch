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
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<OrderDto> CreateOrderAsync(Guid userId, IList<CartItemDto> cartItemDtos, string address)
    {
        if (cartItemDtos == null || !cartItemDtos.Any())
        {
            throw new InvalidOperationException("Cart is empty");
        }

        var order = new Order
        {
            OrderNumber = Guid.NewGuid(),
            UserId = userId,
            ShippingAddress = address,
            BillingAddress = address,
            Status = OrderStatus.Pending,
            Items = new List<CartItem>()
        };

        decimal totalAmount = 0;
        foreach (var cartItem in cartItemDtos)
        {
            var product = _mapper.Map<Product>(cartItem.Product);
            if (product.StockQuantity < cartItem.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for {product.Name}");
            }

            var item = new CartItem
            {
                ProductId = product.Id,
                Quantity = cartItem.Quantity,
                Product = product
            };

            order.Items.Add(item);
            totalAmount += (item.Product.Price * item.Quantity);

            // Update stock
            product.StockQuantity -= cartItem.Quantity;
            await _productRepository.UpdateAsync(product);
        }

        order.TotalAmount = totalAmount;
        await _orderRepository.AddAsync(order);

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(Guid userId)
    {
        var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}