// src/pages/ProductDetails.js
import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getProductById } from '../services/apiService';
import { useCart } from '../context/CartContext'; // Uvozimo useCart da možemo koristiti CartContext

const ProductDetails = () => {
  const { id } = useParams(); // Dohvaćamo ID proizvoda iz URL-a
  const [product, setProduct] = useState(null);
  const [error, setError] = useState('');
  const [quantity, setQuantity] = useState(1);
  const navigate = useNavigate();
  const { addToCart } = useCart(); // Dohvaćamo funkciju addToCart iz CartContexta

  // Dohvaćanje podataka o proizvodu po ID-u
  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const fetchedProduct = await getProductById(id);
        setProduct(fetchedProduct);
      } catch (error) {
        console.error('Greška pri dohvaćanju proizvoda:', error);
        setError('Greška pri dohvaćanju proizvoda.');
      }
    };

    fetchProduct();
  }, [id]);

  // Funkcija za dodavanje proizvoda u košaricu
  const handleAddToCart = () => {
    addToCart(product, quantity); // Dodajemo proizvod u košaricu
    alert(`Dodali ste ${quantity} komada proizvoda u košaricu.`);
    navigate('/cart'); // Preusmjeravamo korisnika na stranicu s košaricom
  };

  // Stilovi za stranicu
  const styles = {
    container: {
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      padding: '20px',
    },
    image: {
      width: '300px',
      height: '300px',
      objectFit: 'contain',
      marginBottom: '20px',
    },
    details: {
      textAlign: 'center',
      marginBottom: '20px',
    },
    input: {
      width: '50px',
      padding: '5px',
      marginRight: '10px',
      textAlign: 'center',
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
    },
  };

  if (error) {
    return <p style={styles.error}>{error}</p>;
  }

  if (!product) {
    return <p>Učitavanje...</p>;
  }

  return (
    <div style={styles.container}>
      <img src={product.imageUrl} alt={product.name} style={styles.image} />
      <div style={styles.details}>
        <h2>{product.name}</h2>
        <p>{product.description}</p>
        <p>Cijena: {product.price} eur</p>
      </div>
      <div>
        <input
          type="number"
          value={quantity}
          onChange={(e) => setQuantity(Math.max(1, parseInt(e.target.value) || 1))}
          style={styles.input}
        />
        <button style={styles.button} onClick={handleAddToCart}>
          Dodaj u košaricu
        </button>
      </div>
    </div>
  );
};

export default ProductDetails;
