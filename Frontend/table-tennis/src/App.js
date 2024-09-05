// src/App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Header from './components/Header';
import Home from './pages/Home';
import ProductList from './pages/ProductList';
import CartPage from './pages/CartPage';
import Login from './components/Login';
import Register from './components/Register';
import ProductDetails from './pages/ProductDetails';
import { AuthProvider } from './context/AuthContext'; 
import { CartProvider } from './context/CartContext'; 
import AdminDashboard from './pages/AdminDashboard';
import AdminOrders from './pages/AdminOrders';
import AddProduct from './pages/AddProduct';
import DeleteProduct from './pages/DeleteProduct';
import EditProduct from './pages/EditProduct';
import ProductListAdmin from './pages/ProductListAdmin';

const App = () => {
  return (
    <AuthProvider>
      <CartProvider>
        <Router>
          <Header />
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/products" element={<ProductList />} />
            <Route path="/product/:id" element={<ProductDetails />} />
            <Route path="/cart" element={<CartPage />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/admindashboard" element={<AdminDashboard />} /> 
            <Route path="/adminorders" element={<AdminOrders />} /> 
            <Route path="/add-product" element={<AddProduct />} />
            <Route path="/delete-product" element={<DeleteProduct />} />
            <Route path="/edit-product/:id" element={<EditProduct />} />
            <Route path="/product-list-admin" element={<ProductListAdmin />} />
          </Routes>
        </Router>
      </CartProvider>
    </AuthProvider>
  );
};

export default App;
