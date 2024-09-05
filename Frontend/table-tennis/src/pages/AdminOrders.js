import React, { useEffect, useState } from 'react';
import { getOrders } from '../services/apiService';

const AdminOrders = () => {
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState('');

  const fetchOrders = async () => {
    try {
      const fetchedOrders = await getOrders();
      setOrders(fetchedOrders);
    } catch (error) {
      console.error('Greška pri dohvaćanju narudžbi:', error);
      setError('Greška pri dohvaćanju narudžbi.');
    }
  };

  useEffect(() => {
    fetchOrders();
  }, []);

  const styles = {
    container: {
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
      flexDirection: 'column',
      minHeight: '80vh',
      padding: '20px',
    },
    table: {
      width: '80%',
      borderCollapse: 'collapse',
      textAlign: 'center',
    },
    th: {
      border: '1px solid #ddd',
      padding: '10px',
      backgroundColor: '#f4f4f4',
    },
    td: {
      border: '1px solid #ddd',
      padding: '10px',
    },
    error: {
      color: 'red',
      marginBottom: '20px',
    },
  };

  return (
    <div style={styles.container}>
      <h2>Pregled Narudžbi</h2>
      {error && <p style={styles.error}>{error}</p>}
      {orders.length === 0 ? (
        <p>Nema narudžbi za prikaz.</p>
      ) : (
        <table style={styles.table}>
          <thead>
            <tr>
              <th style={styles.th}>ID Narudžbe</th>
              <th style={styles.th}>Korisnik</th>
              <th style={styles.th}>Datum</th>
              <th style={styles.th}>Status</th>
              <th style={styles.th}>Ukupno</th>
            </tr>
          </thead>
          <tbody>
            {orders.map((order) => (
              <tr key={order.orderId}>
                <td style={styles.td}>{order.orderId}</td>
                <td style={styles.td}>{order.userId}</td>
                <td style={styles.td}>{new Date(order.orderDate).toLocaleDateString()}</td>
                <td style={styles.td}>{order.status}</td>
                <td style={styles.td}>{order.totalAmount} kn</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default AdminOrders;
