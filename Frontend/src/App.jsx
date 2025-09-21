import React, { useState, useEffect, createContext, useContext } from 'react';
import { Search, ShoppingCart, Package, History, X, Plus, Minus, Filter } from 'lucide-react';

// API Configuration
const API_BASE_URL = 'http://localhost:5191/api';
const USER_ID = '99999999-9999-9999-9999-999999999999';

// API Service
const api = {
  headers: {
    'Content-Type': 'application/json',
    'X-User-Id': USER_ID
  },
  
  async get(endpoint) {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      headers: this.headers
    });
    return response.json();
  },
  
  async post(endpoint, data) {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'POST',
      headers: this.headers,
      body: JSON.stringify(data)
    });
    return response.json();
  },
  
  async put(endpoint, data) {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'PUT',
      headers: this.headers,
      body: JSON.stringify(data)
    });
    return response.json();
  },
  
  async delete(endpoint) {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'DELETE',
      headers: this.headers
    });
    return response.ok ? {} : response.json();
  }
};

// Cart Context
const CartContext = createContext();
const useCart = () => useContext(CartContext);

const CartProvider = ({ children }) => {
  const [cart, setCart] = useState({ items: [], totalAmount: 0 });
  const [loading, setLoading] = useState(false);

  const fetchCart = async () => {
    try {
      const data = await api.get('/cart');
      setCart(data);
    } catch (error) {
      console.error('Error fetching cart:', error);
    }
  };

  const addToCart = async (productId, quantity = 1) => {
    setLoading(true);
    try {
      const data = await api.post('/cart/items', { productId, quantity });
      setCart(data);
      return true;
    } catch (error) {
      console.error('Error adding to cart:', error);
      return false;
    } finally {
      setLoading(false);
    }
  };

  const updateCartItem = async (productId, quantity) => {
    try {
      const data = await api.put(`/cart/items/${productId}`, { quantity });
      setCart(data);
    } catch (error) {
      console.error('Error updating cart:', error);
    }
  };

  const removeFromCart = async (productId) => {
    try {
      const data = await api.delete(`/cart/items/${productId}`);
      setCart(data);
    } catch (error) {
      console.error('Error removing from cart:', error);
    }
  };

  useEffect(() => {
    fetchCart();
  }, []);

  return (
    <CartContext.Provider value={{ cart, loading, addToCart, updateCartItem, removeFromCart, fetchCart }}>
      {children}
    </CartContext.Provider>
  );
};

// Product Card Component
const ProductCard = ({ product, onAddToCart }) => {
  const [adding, setAdding] = useState(false);
  
  const handleAddToCart = async () => {
    setAdding(true);
    const success = await onAddToCart(product.id);
    if (success) {
      setTimeout(() => setAdding(false), 500);
    } else {
      setAdding(false);
    }
  };

  const franchiseColors = {
    'MI': 'bg-blue-600',
    'CSK': 'bg-yellow-500',
    'RCB': 'bg-red-600',
    'KKR': 'bg-purple-700'
  };

  return (
    <div className="bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow">
      <div className="relative">
        <div className="h-48 bg-gray-200 rounded-t-lg flex items-center justify-center">
          <Package className="h-16 w-16 text-gray-400" />
        </div>
        {product.isAutographed && (
          <span className="absolute top-2 right-2 bg-yellow-400 text-xs font-bold px-2 py-1 rounded">
            AUTOGRAPHED
          </span>
        )}
        <span className={`absolute top-2 left-2 text-white text-xs font-bold px-2 py-1 rounded ${franchiseColors[product.franchise?.code] || 'bg-gray-600'}`}>
          {product.franchise?.code}
        </span>
      </div>
      <div className="p-4">
        <h3 className="font-semibold text-lg mb-1 line-clamp-2">{product.name}</h3>
        <p className="text-sm text-gray-600 mb-2">{product.franchise?.name}</p>
        <p className="text-xs text-gray-500 mb-3 line-clamp-2">{product.description}</p>
        <div className="flex items-center justify-between">
          <span className="text-xl font-bold text-green-600">₹{product.price}</span>
          <button
            onClick={handleAddToCart}
            disabled={!product.isAvailable || adding}
            className={`px-4 py-2 rounded-lg text-white font-medium transition-all ${
              !product.isAvailable 
                ? 'bg-gray-400 cursor-not-allowed' 
                : adding 
                  ? 'bg-green-500' 
                  : 'bg-blue-600 hover:bg-blue-700'
            }`}
          >
            {!product.isAvailable ? 'Out of Stock' : adding ? 'Added!' : 'Add to Cart'}
          </button>
        </div>
      </div>
    </div>
  );
};

// Cart Sidebar Component
const CartSidebar = ({ isOpen, onClose }) => {
  const { cart, updateCartItem, removeFromCart, fetchCart } = useCart();
  const [creatingOrder, setCreatingOrder] = useState(false);

  const handleCheckout = async () => {
    setCreatingOrder(true);
    try {
      await api.post('/order', {
        shippingAddress: '123 Main St, Mumbai, India',
        billingAddress: '123 Main St, Mumbai, India'
      });
      await api.delete('/cart');
      await fetchCart();
      onClose();
      alert('Order placed successfully!');
    } catch (error) {
      console.error('Error creating order:', error);
      alert('Failed to create order');
    } finally {
      setCreatingOrder(false);
    }
  };

  return (
    <div className={`fixed inset-0 z-50 ${isOpen ? 'block' : 'hidden'}`}>
      <div className="absolute inset-0 bg-black bg-opacity-50" onClick={onClose}></div>
      <div className="absolute right-0 top-0 h-full w-96 bg-white shadow-xl">
        <div className="flex items-center justify-between p-4 border-b">
          <h2 className="text-xl font-bold">Shopping Cart</h2>
          <button onClick={onClose} className="p-2 hover:bg-gray-100 rounded">
            <X className="h-5 w-5" />
          </button>
        </div>
        
        <div className="flex-1 overflow-y-auto p-4" style={{ maxHeight: 'calc(100vh - 200px)' }}>
          {cart.items?.length === 0 ? (
            <p className="text-gray-500 text-center py-8">Your cart is empty</p>
          ) : (
            <div className="space-y-4">
              {cart.items?.map(item => (
                <div key={item.id} className="border rounded-lg p-3">
                  <div className="flex justify-between mb-2">
                    <h4 className="font-medium text-sm">{item.product?.name}</h4>
                    <button 
                      onClick={() => removeFromCart(item.product?.id)}
                      className="text-red-500 hover:text-red-700"
                    >
                      <X className="h-4 w-4" />
                    </button>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
        
        <div className="border-t p-4">
          <div className="flex justify-between mb-4">
            <span className="text-lg font-semibold">Total:</span>
            <span className="text-xl font-bold text-green-600">₹{cart.totalAmount}</span>
          </div>
          <button
            onClick={handleCheckout}
            disabled={cart.items?.length === 0 || creatingOrder}
            className="w-full bg-green-600 text-white py-3 rounded-lg font-medium hover:bg-green-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
          >
            {creatingOrder ? 'Processing...' : 'Proceed to Checkout'}
          </button>
        </div>
      </div>
    </div>
  );
};

// Product List Page
const ProductListPage = () => {
  const [products, setProducts] = useState([]);
  const [franchises, setFranchises] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedFranchise, setSelectedFranchise] = useState('');
  const [selectedType, setSelectedType] = useState('');
  const { addToCart } = useCart();

  const productTypes = [
    { value: '', label: 'All Types' },
    { value: 'Jersey', label: 'Jersey' },
    { value: 'Cap', label: 'Cap' },
    { value: 'Flag', label: 'Flag' },
    { value: 'AutographedPhoto', label: 'Autographed Photo' },
    { value: 'Keychain', label: 'Keychain' },
    { value: 'Mug', label: 'Mug' },
    { value: 'Poster', label: 'Poster' }
  ];

  useEffect(() => {
    fetchProducts();
    fetchFranchises();
  }, []);

  const fetchProducts = async () => {
    setLoading(true);
    try {
      const data = await api.get('/products');
      setProducts(data);
    } catch (error) {
      console.error('Error fetching products:', error);
    } finally {
      setLoading(false);
    }
  };

  const fetchFranchises = async () => {
    try {
      const data = await api.get('/franchises');
      setFranchises(data);
    } catch (error) {
      console.error('Error fetching franchises:', error);
    }
  };

  const handleSearch = async () => {
    setLoading(true);
    try {
      const searchData = {
        searchTerm,
        franchiseId: selectedFranchise || null,
        productType: selectedType || null
      };
      const data = await api.post('/products/search', searchData);
      setProducts(data);
    } catch (error) {
      console.error('Error searching products:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="p-6">
      <div className="mb-6 space-y-4">
        <div className="flex gap-4">
          <div className="flex-1">
            <div className="relative">
              <input
                type="text"
                placeholder="Search products..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
                className="w-full pl-10 pr-4 py-2 border rounded-lg focus:outline-none focus:border-blue-500"
              />
              <Search className="absolute left-3 top-2.5 h-5 w-5 text-gray-400" />
            </div>
          </div>
          <button
            onClick={handleSearch}
            className="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
          >
            Search
          </button>
        </div>
        
        <div className="flex gap-4">
          <select
            value={selectedFranchise}
            onChange={(e) => setSelectedFranchise(e.target.value)}
            className="px-4 py-2 border rounded-lg focus:outline-none focus:border-blue-500"
          >
            <option value="">All Franchises</option>
            {franchises.map(franchise => (
              <option key={franchise.id} value={franchise.id}>
                {franchise.name}
              </option>
            ))}
          </select>
          
          <select
            value={selectedType}
            onChange={(e) => setSelectedType(e.target.value)}
            className="px-4 py-2 border rounded-lg focus:outline-none focus:border-blue-500"
          >
            {productTypes.map(type => (
              <option key={type.value} value={type.value}>
                {type.label}
              </option>
            ))}
          </select>
          
          <button
            onClick={() => { setSearchTerm(''); setSelectedFranchise(''); setSelectedType(''); fetchProducts(); }}
            className="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-100 flex items-center gap-2"
          >
            <X className="h-4 w-4" />
            Clear Filters
          </button>
        </div>
      </div>

      {loading ? (
        <div className="flex justify-center py-12">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {products.map(product => (
            <ProductCard key={product.id} product={product} onAddToCart={addToCart} />
          ))}
        </div>
      )}
    </div>
  );
};

// Order History Page
const OrderHistoryPage = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchOrders();
  }, []);

  const fetchOrders = async () => {
    setLoading(true);
    try {
      const data = await api.get('/order');
      setOrders(data);
    } catch (error) {
      console.error('Error fetching orders:', error);
    } finally {
      setLoading(false);
    }
  };

  const getStatusColor = (status) => {
    const colors = {
      'Pending': 'bg-yellow-100 text-yellow-800',
      'Processing': 'bg-blue-100 text-blue-800',
      'Shipped': 'bg-purple-100 text-purple-800',
      'Delivered': 'bg-green-100 text-green-800',
      'Cancelled': 'bg-red-100 text-red-800'
    };
    return colors[status] || 'bg-gray-100 text-gray-800';
  };

  return (
    <div className="p-6">
      <h2 className="text-2xl font-bold mb-6">Order History</h2>
      
      {loading ? (
        <div className="flex justify-center py-12">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
        </div>
      ) : orders.length === 0 ? (
        <div className="text-center py-12">
          <Package className="h-16 w-16 text-gray-400 mx-auto mb-4" />
          <p className="text-gray-500">No orders yet</p>
        </div>
      ) : (
        <div className="space-y-4">
          {orders.map(order => (
            <div key={order.id} className="bg-white rounded-lg shadow-md p-6">
              <div className="flex justify-between items-start mb-4">
                <div>
                  <h3 className="text-lg font-semibold">Order #{order.orderNumber}</h3>
                  <p className="text-sm text-gray-500">
                    {new Date(order.createdAt).toLocaleDateString('en-IN', {
                      year: 'numeric',
                      month: 'long',
                      day: 'numeric',
                      hour: '2-digit',
                      minute: '2-digit'
                    })}
                  </p>
                </div>
                <span className={`px-3 py-1 rounded-full text-sm font-medium ${getStatusColor(order.status)}`}>
                  {order.status}
                </span>
              </div>
              
              <div className="border-t pt-4">
                <div className="space-y-2">
                  {order.items?.map(item => (
                    <div key={item.id} className="flex justify-between text-sm">
                      <span className="text-gray-600">
                        {item.product?.name} x {item.quantity}
                      </span>
                      <span className="font-medium">₹{item.totalPrice}</span>
                    </div>
                  ))}
                </div>
                <div className="border-t mt-4 pt-4 flex justify-between">
                  <span className="text-lg font-semibold">Total</span>
                  <span className="text-xl font-bold text-green-600">₹{order.totalAmount}</span>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

// Main App Component
export default function App() {
  const [activeTab, setActiveTab] = useState('products');
  const [cartOpen, setCartOpen] = useState(false);

  return (
    <CartProvider>
      <div className="min-h-screen bg-gray-50">
        {/* Header */}
        <header className="bg-white shadow-sm sticky top-0 z-40">
          <div className="max-w-7xl mx-auto px-4 py-4">
            <div className="flex items-center justify-between">
              <div className="flex items-center space-x-8">
                <h1 className="text-2xl font-bold text-blue-600">IPL Store</h1>
                <nav className="hidden md:flex space-x-6">
                  <button
                    onClick={() => setActiveTab('products')}
                    className={`flex items-center gap-2 px-3 py-2 rounded-lg transition-colors ${
                      activeTab === 'products' ? 'bg-blue-100 text-blue-600' : 'text-gray-600 hover:text-blue-600'
                    }`}
                  >
                    <Package className="h-4 w-4" />
                    Products
                  </button>
                  <button
                    onClick={() => setActiveTab('orders')}
                    className={`flex items-center gap-2 px-3 py-2 rounded-lg transition-colors ${
                      activeTab === 'orders' ? 'bg-blue-100 text-blue-600' : 'text-gray-600 hover:text-blue-600'
                    }`}
                  >
                    <History className="h-4 w-4" />
                    Orders
                  </button>
                </nav>
              </div>
              
              <button
                onClick={() => setCartOpen(true)}
                className="relative p-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
              >
                <ShoppingCart className="h-5 w-5" />
                <CartContext.Consumer>
                  {({ cart }) => cart.items?.length > 0 && (
                    <span className="absolute -top-2 -right-2 bg-red-500 text-white text-xs rounded-full h-6 w-6 flex items-center justify-center">
                      {cart.items.length}
                    </span>
                  )}
                </CartContext.Consumer>
              </button>
            </div>
          </div>
        </header>

        {/* Main Content */}
        <main className="max-w-7xl mx-auto">
          {activeTab === 'products' && <ProductListPage />}
          {activeTab === 'orders' && <OrderHistoryPage />}
        </main>

        {/* Cart Sidebar */}
        <CartSidebar isOpen={cartOpen} onClose={() => setCartOpen(false)} />
      </div>
    </CartProvider>
  );
}
