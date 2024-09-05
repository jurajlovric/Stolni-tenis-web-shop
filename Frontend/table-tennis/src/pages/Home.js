// src/pages/Home.js
import React from 'react';

const Home = () => {
  // Stilizacija početne stranice
  const styles = {
    container: {
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      height: '80vh',
      textAlign: 'center',
    },
    heading: {
      fontSize: '2.5rem',
      marginBottom: '20px',
      color: '#333',
    },
    button: {
      padding: '10px 20px',
      fontSize: '1rem',
      borderRadius: '5px',
      border: 'none',
      backgroundColor: '#007bff',
      color: '#fff',
      cursor: 'pointer',
      transition: 'background-color 0.3s',
    },
    buttonHover: {
      backgroundColor: '#0056b3',
    },
  };

  return (
    <div style={styles.container}>
      <h1 style={styles.heading}>Dobro došli na stolno tenisku stranicu!</h1>
      <button
        style={styles.button}
        onMouseOver={(e) => (e.target.style.backgroundColor = styles.buttonHover.backgroundColor)}
        onMouseOut={(e) => (e.target.style.backgroundColor = '#007bff')}
        onClick={() => window.location.href = '/products'} // Navigacija na stranicu proizvoda
      >
        Pogledajte proizvode
      </button>
    </div>
  );
};

export default Home;
