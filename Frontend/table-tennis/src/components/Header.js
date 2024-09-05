// src/components/Header.js
import React from 'react';
import { useAuth } from '../context/AuthContext';
import { Link, useNavigate } from 'react-router-dom';

const Header = () => {
  const { user, logoutUserHandler } = useAuth(); // Pravilno korištenje logoutUserHandler
  const navigate = useNavigate();

  const handleLogout = () => {
    logoutUserHandler();
    navigate('/login'); // Preusmjeri korisnika na stranicu za prijavu
  };

  return (
    <header style={styles.header}>
      <nav style={styles.nav}>
        <div style={styles.leftSection}>
          <Link to="/" style={styles.link}>Home</Link>
          <Link to="/products" style={styles.link}>Proizvodi</Link>
          <Link to="/cart" style={styles.link}>Košarica</Link>
        </div>
        {user ? (
          <div style={styles.rightSection}>
            <span style={styles.username}>{user.username}</span>
            <button onClick={handleLogout} style={styles.logoutButton}>Odjava</button>
          </div>
        ) : (
          <Link to="/login" style={styles.link}>Prijava</Link>
        )}
      </nav>
    </header>
  );
};

const styles = {
  header: {
    display: 'flex',
    justifyContent: 'space-between',
    padding: '10px 20px',
    backgroundColor: '#007bff',
    color: '#fff',
  },
  nav: {
    display: 'flex',
    justifyContent: 'space-between',
    width: '100%',
  },
  leftSection: {
    display: 'flex',
    gap: '20px',
  },
  rightSection: {
    display: 'flex',
    alignItems: 'center',
    gap: '10px',
    marginLeft: 'auto', // Ovo će gurnuti sekciju desno
  },
  link: {
    color: '#fff',
    textDecoration: 'none',
  },
  username: {
    fontWeight: 'bold',
  },
  logoutButton: {
    padding: '5px 10px',
    border: 'none',
    borderRadius: '5px',
    backgroundColor: '#ff4d4f',
    color: '#fff',
    cursor: 'pointer',
  },
};

export default Header;
