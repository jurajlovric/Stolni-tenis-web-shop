import React, { useState } from 'react';
import { useCart } from '../context/CartContext';
import { useNavigate } from 'react-router-dom';
import { createOrder } from '../services/apiService';
import { useAuth } from '../context/AuthContext';

const CartPage = () => {
  const { cartItems, updateQuantity, removeFromCart, clearCart } = useCart();
  const { user } = useAuth();
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleOrder = async () => {
    try {
      if (!user || !user.userId) {
        throw new Error('Korisnik nije prijavljen ili nema valjan ID.');
      }

      const orderData = {
        userId: user.userId,
        orderItems: cartItems.map((item) => ({
          productId: item.productId,
          quantity: item.quantity,
          price: item.price,
        })),
      };

      await createOrder(orderData);
      alert('Narudžba je uspješno kreirana!');
      clearCart();
      navigate('/');
    } catch (error) {
      console.error('Greška pri kreiranju narudžbe:', error.message);
      setError('Greška pri kreiranju narudžbe: Provjerite podatke i pokušajte ponovo.');
    }
  };

  const styles = {
    container: {
      padding: '20px',
      maxWidth: '800px',
      margin: '0 auto',
    },
    itemRow: {
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
      padding: '10px 0',
      borderBottom: '1px solid #ccc',
    },
    itemDetails: {
      display: 'flex',
      flexDirection: 'column',
      textAlign: 'left',
      flex: 1,
    },
    input: {
      width: '40px',
      padding: '5px',
      marginRight: '10px',
      textAlign: 'center',
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
    removeButton: {
      backgroundColor: '#ff4d4f',
      border: 'none',
      padding: '5px 10px',
      color: '#fff',
      borderRadius: '4px',
      cursor: 'pointer',
      transition: 'background-color 0.3s',
    },
    total: {
      marginTop: '20px',
      textAlign: 'right',
      fontWeight: 'bold',
    },
  };

  const totalAmount = cartItems.reduce((total, item) => total + item.price * item.quantity, 0);

  return (
    <div style={styles.container}>
      <h2>Košarica</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      {cartItems.length === 0 ? (
        <p>Vaša košarica je prazna.</p>
      ) : (
        <>
          {cartItems.map((item) => (
            <div key={item.productId} style={styles.itemRow}>
              <div style={styles.itemDetails}>
                <span>{item.productName || item.name}</span>
                <span>{item.price} eur</span>
              </div>
              <input
                type="number"
                value={item.quantity}
                min="1"
                onChange={(e) => updateQuantity(item.productId, parseInt(e.target.value, 10))}
                style={styles.input}
              />
              <button style={styles.removeButton} onClick={() => removeFromCart(item.productId)}>
                Ukloni
              </button>
            </div>
          ))}
          <div style={styles.total}>Ukupno: {totalAmount.toFixed(2)} eur</div>
          <button style={styles.button} onClick={handleOrder}>
            Naruči
          </button>
        </>
      )}
    </div>
  );
};

export default CartPage;
