// src/pages/EditProduct.js
import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getProductById, updateProduct, getCategories } from '../services/apiService';

const EditProduct = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [categories, setCategories] = useState([]);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const productData = await getProductById(id);
        setProduct(productData);
      } catch (err) {
        console.error('Greška pri dohvaćanju proizvoda:', err);
        setError('Greška pri dohvaćanju proizvoda.');
      }
    };

    const fetchCategories = async () => {
      try {
        const response = await getCategories();
        setCategories(response);
      } catch (error) {
        console.error('Greška pri dohvaćanju kategorija:', error);
        setError('Greška pri dohvaćanju kategorija.');
      }
    };

    fetchProduct();
    fetchCategories();
  }, [id]);

  const handleUpdate = async (e) => {
    e.preventDefault();

    if (!product.categoryId) {
      setError('Odaberite kategoriju za proizvod.');
      return;
    }

    try {
      await updateProduct(id, product);
      alert('Proizvod uspješno ažuriran!');
      navigate('/admindashboard');
    } catch (err) {
      console.error('Greška pri ažuriranju proizvoda:', err.response?.data || err.message);
      setError('Greška pri ažuriranju proizvoda. Provjerite podatke i pokušajte ponovo.');
    }
  };

  const styles = {
    container: {
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      padding: '20px',
      backgroundColor: '#f8f9fa',
      borderRadius: '8px',
      boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
      maxWidth: '500px',
      margin: '0 auto',
    },
    input: {
      marginBottom: '10px',
      padding: '10px',
      width: '100%',
      borderRadius: '4px',
      border: '1px solid #ccc',
      boxSizing: 'border-box',
    },
    button: {
      padding: '10px 20px',
      border: 'none',
      borderRadius: '5px',
      backgroundColor: '#007bff',
      color: '#fff',
      cursor: 'pointer',
      transition: 'background-color 0.3s',
    },
    error: {
      color: 'red',
      marginBottom: '10px',
    },
    heading: {
      marginBottom: '20px',
      color: '#333',
    },
  };

  if (!product) return <p>Učitavanje proizvoda...</p>;

  return (
    <div style={styles.container}>
      <h2 style={styles.heading}>Uredi Proizvod</h2>
      {error && <p style={styles.error}>{error}</p>}
      <form onSubmit={handleUpdate}>
        <input
          type="text"
          placeholder="Ime proizvoda"
          value={product.name}
          onChange={(e) => setProduct({ ...product, name: e.target.value })}
          style={styles.input}
          required
        />
        <textarea
          placeholder="Opis proizvoda"
          value={product.description}
          onChange={(e) => setProduct({ ...product, description: e.target.value })}
          style={styles.input}
          required
        />
        <input
          type="number"
          placeholder="Cijena"
          value={product.price}
          onChange={(e) => setProduct({ ...product, price: parseFloat(e.target.value) })}
          style={styles.input}
          required
        />
        <input
          type="text"
          placeholder="URL slike"
          value={product.imageUrl}
          onChange={(e) => setProduct({ ...product, imageUrl: e.target.value })}
          style={styles.input}
        />
        <select
          value={product.categoryId}
          onChange={(e) => setProduct({ ...product, categoryId: e.target.value })}
          style={styles.input}
          required
        >
          <option value="">Odaberi kategoriju</option>
          {categories.map((category) => (
            <option key={category.categoryId} value={category.categoryId}>
              {category.name}
            </option>
          ))}
        </select>
        <button type="submit" style={styles.button}>
          Ažuriraj proizvod
        </button>
      </form>
    </div>
  );
};

export default EditProduct;
