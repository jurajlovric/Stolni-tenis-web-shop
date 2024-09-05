import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getProductById } from '../services/apiService';
import { useCart } from '../context/CartContext';

const ProductDetails = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [error, setError] = useState('');
  const [quantity, setQuantity] = useState(1);
  const navigate = useNavigate();
  const { addToCart } = useCart();

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

  const handleAddToCart = (e) => {
    e.preventDefault();
    addToCart(product, quantity);
    alert(`Dodali ste ${quantity} komada proizvoda u košaricu.`);
    navigate('/cart');
  };

  if (error) {
    return <p style={{ color: 'red' }}>{error}</p>;
  }

  if (!product) {
    return <p>Učitavanje...</p>;
  }

  return (
    <div style={{ padding: '20px', textAlign: 'center' }}>
      <img src={product.imageUrl} alt={product.name} style={{ width: '300px', height: '300px', objectFit: 'contain' }} />
      <h2>{product.name}</h2>
      <p>{product.description}</p>
      <p>Cijena: {product.price} eur</p>
      <form onSubmit={handleAddToCart}>
        <input
          type="number"
          value={quantity}
          onChange={(e) => setQuantity(Math.max(1, parseInt(e.target.value) || 1))}
          style={{ margin: '10px', padding: '5px', width: '50px', textAlign: 'center' }}
        />
        <button type="submit" style={{ padding: '10px 20px', borderRadius: '5px', backgroundColor: '#007bff', color: '#fff' }}>
          Dodaj u košaricu
        </button>
      </form>
    </div>
  );
};

export default ProductDetails;
