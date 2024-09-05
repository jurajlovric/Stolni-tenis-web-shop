import React, { useState, useEffect } from 'react';
import { getProducts, deleteProduct } from '../services/apiService';

const DeleteProduct = () => {
  const [products, setProducts] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await getProducts();
        setProducts(response);
      } catch (error) {
        console.error('Greška pri dohvaćanju proizvoda:', error);
        setError('Greška pri dohvaćanju proizvoda.');
      }
    };

    fetchProducts();
  }, []);

  const handleDelete = async (productId) => {
    try {
      if (window.confirm('Jeste li sigurni da želite obrisati ovaj proizvod?')) {
        await deleteProduct(productId);
        setProducts(products.filter((product) => product.productId !== productId));
        alert('Proizvod je uspješno obrisan.');
      }
    } catch (error) {
      console.error('Greška pri brisanju proizvoda:', error);
      setError('Greška pri brisanju proizvoda. Pokušajte ponovo.');
    }
  };

  const styles = {
    container: {
      maxWidth: '800px',
      margin: '0 auto',
      padding: '20px',
      border: '1px solid #ccc',
      borderRadius: '8px',
      boxShadow: '0 2px 10px rgba(0, 0, 0, 0.1)',
      backgroundColor: '#f9f9f9',
    },
    header: {
      textAlign: 'center',
      marginBottom: '20px',
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
      backgroundColor: '#ff4d4f',
      color: '#fff',
      border: 'none',
      borderRadius: '5px',
      cursor: 'pointer',
    },
    error: {
      color: 'red',
      textAlign: 'center',
      marginBottom: '10px',
    },
  };

  return (
    <div style={styles.container}>
      <h2 style={styles.header}>Lista proizvoda</h2>
      {error && <p style={styles.error}>{error}</p>}
      {products.length === 0 ? (
        <p>Nema dostupnih proizvoda.</p>
      ) : (
        products.map((product) => (
          <div key={product.productId} style={styles.productRow}>
            <span>{product.name}</span>
            <button style={styles.button} onClick={() => handleDelete(product.productId)}>
              Obriši
            </button>
          </div>
        ))
      )}
    </div>
  );
};

export default DeleteProduct;
