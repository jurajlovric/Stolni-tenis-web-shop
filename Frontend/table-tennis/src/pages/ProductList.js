import React, { useEffect, useState } from 'react';
import { getProducts } from '../services/apiService';

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const productList = await getProducts();
        setProducts(productList);
      } catch (error) {
        console.error('Greška pri dohvaćanju proizvoda:', error);
        setError('Greška pri dohvaćanju proizvoda.');
      }
    };

    fetchProducts();
  }, []);

  const styles = {
    container: {
      padding: '20px',
      display: 'grid',
      gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))',
      gap: '20px',
    },
    productCard: {
      padding: '10px',
      border: '1px solid #ccc',
      borderRadius: '8px',
      textAlign: 'center',
      boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
      transition: 'transform 0.2s',
    },
    productImage: {
      width: '100%',
      height: '150px',
      objectFit: 'contain',
      marginBottom: '10px',
    },
    productName: {
      fontSize: '16px',
      fontWeight: 'bold',
      margin: '10px 0',
    },
    productPrice: {
      fontSize: '14px',
      color: '#007bff',
    },
    button: {
      marginTop: '10px',
      padding: '8px 12px',
      border: 'none',
      borderRadius: '4px',
      backgroundColor: '#007bff',
      color: '#fff',
      cursor: 'pointer',
      transition: 'background-color 0.3s',
    },
  };

  return (
    <div>
      <h2 style={{ textAlign: 'center', margin: '20px 0' }}>Proizvodi</h2>
      {error ? (
        <p style={{ color: 'red', textAlign: 'center' }}>{error}</p>
      ) : (
        <div style={styles.container}>
          {products.map((product) => (
            <div
              key={product.productId}
              style={styles.productCard}
              onMouseOver={(e) => (e.currentTarget.style.transform = 'scale(1.05)')}
              onMouseOut={(e) => (e.currentTarget.style.transform = 'scale(1)')}
            >
              <img src={product.imageUrl} alt={product.name} style={styles.productImage} />
              <div style={styles.productName}>{product.name}</div>
              <div style={styles.productPrice}>{product.price} eur</div>
              <button
                style={styles.button}
                onClick={() => window.location.href = `/product/${product.productId}`}
              >
                Detalji
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ProductList;
