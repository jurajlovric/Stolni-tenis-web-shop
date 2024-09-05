import React from 'react';
import { useNavigate } from 'react-router-dom';

const AdminDashboard = () => {
  const navigate = useNavigate();

  const handleAddProduct = () => {
    navigate('/add-product');
  };

  const handleEditProduct = () => {
    navigate('/product-list-admin');
  };

  const handleDeleteProduct = () => {
    navigate('/delete-product');
  };

  const handleViewOrders = () => {
    navigate('/adminorders');
  };

  const buttonStyle = {
    padding: '10px 20px',
    margin: '10px',
    border: 'none',
    borderRadius: '5px',
    backgroundColor: '#007bff',
    color: '#fff',
    cursor: 'pointer',
    transition: 'background-color 0.3s',
  };

  const containerStyle = {
    textAlign: 'center',
    padding: '40px',
    backgroundColor: '#f5f5f5',
    borderRadius: '8px',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    maxWidth: '600px',
    margin: '40px auto',
  };

  return (
    <div style={containerStyle}>
      <h1 style={{ marginBottom: '20px', color: '#333' }}>Admin Dashboard</h1>
      <button onClick={handleAddProduct} style={buttonStyle}>
        Dodaj Proizvod
      </button>
      <button onClick={handleEditProduct} style={buttonStyle}>
        Uredi Proizvode
      </button>
      <button onClick={handleDeleteProduct} style={buttonStyle}>
        Obriši Proizvod
      </button>
      <button onClick={handleViewOrders} style={buttonStyle}>
        Pregledaj Narudžbe
      </button>
    </div>
  );
};

export default AdminDashboard;
