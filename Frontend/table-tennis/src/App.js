// src/App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Header from './components/Header';
import Home from './pages/Home';
import ProductList from './pages/ProductList';
import CartPage from './pages/CartPage';
import Login from './components/Login';
import Register from './components/Register'; // Importirajte Register komponentu
import ProductDetails from './pages/ProductDetails';
import { AuthProvider } from './context/AuthContext'; 
import { CartProvider } from './context/CartContext'; 

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
            <Route path="/register" element={<Register />} /> {/* Dodano Register ruta */}
          </Routes>
        </Router>
      </CartProvider>
    </AuthProvider>
  );
};

export default App;
