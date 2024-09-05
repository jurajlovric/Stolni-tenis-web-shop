// src/pages/ProductListAdmin.js
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getProducts } from '../services/apiService';

const ProductListAdmin = () => {
  const [products, setProducts] = useState([]);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await getProducts();
        setProducts(response);
      } catch (err) {
        console.error('Greška pri dohvaćanju proizvoda:', err);
        setError('Greška pri dohvaćanju proizvoda.');
      }
    };

    fetchProducts();
  }, []);

  const handleEdit = (productId) => {
    navigate(`/edit-product/${productId}`);
  };

  const styles = {
    container: {
      padding: '20px',
      maxWidth: '800px',
      margin: '0 auto',
      textAlign: 'center',
    },
    productRow: {
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
      padding: '10px 0',
      borderBottom: '1px solid #ccc',
    },
    button: {
      padding: '5px 10px',
      border: 'none',
      borderRadius: '5px',
      backgroundColor: '#007bff',
      color: '#fff',
      cursor: 'pointer',
      transition: 'background-color 0.3s',
    },
  };

  return (
    <div style={styles.container}>
      <h2>Popis Proizvoda</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      {products.length === 0 ? (
        <p>Nema dostupnih proizvoda.</p>
      ) : (
        products.map((product) => (
          <div key={product.productId} style={styles.productRow}>
            <span>{product.name}</span>
            <button style={styles.button} onClick={() => handleEdit(product.productId)}>
              Uredi
            </button>
          </div>
        ))
      )}
    </div>
  );
};

export default ProductListAdmin;
