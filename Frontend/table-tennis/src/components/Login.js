// src/components/Login.js
import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const Login = () => {
  const { loginUserHandler } = useAuth();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(''); // Resetiraj grešku prilikom pokušaja prijave
    try {
      await loginUserHandler(email, password);
      navigate('/'); // Preusmjeri samo ako je prijava uspješna
    } catch (err) {
      setError('Prijava nije uspjela. Provjerite podatke i pokušajte ponovo.');
    }
  };

  const styles = {
    container: {
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      height: '80vh',
    },
    form: {
      display: 'flex',
      flexDirection: 'column',
      gap: '10px',
      width: '300px',
    },
    input: {
      padding: '10px',
      borderRadius: '5px',
      border: '1px solid #ccc',
    },
    button: {
      padding: '10px',
      border: 'none',
      borderRadius: '5px',
      backgroundColor: '#007bff',
      color: '#fff',
      cursor: 'pointer',
    },
    error: {
      color: 'red',
      marginTop: '10px',
    },
    registerLink: {
      marginTop: '10px',
      color: '#007bff',
      cursor: 'pointer',
    },
  };

  return (
    <div style={styles.container}>
      <h2>Prijava</h2>
      <form onSubmit={handleSubmit} style={styles.form}>
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          style={styles.input}
          required
        />
        <input
          type="password"
          placeholder="Lozinka"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          style={styles.input}
          required
        />
        <button type="submit" style={styles.button}>Prijava</button>
        {error && <p style={styles.error}>{error}</p>}
      </form>
      <span
        style={styles.registerLink}
        onClick={() => navigate('/register')} // Dodaj gumb za navigaciju na registraciju
      >
        Nemate račun? Registrirajte se ovdje.
      </span>
    </div>
  );
};

export default Login;
