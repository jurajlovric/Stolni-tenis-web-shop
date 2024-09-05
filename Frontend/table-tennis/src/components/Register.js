import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const Register = () => {
  const { registerUserHandler } = useAuth();
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const defaultRoleId = 'b356d1ff-80fc-4713-a27f-b11f3e5a9230';

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      await registerUserHandler(username, email, password, defaultRoleId);
      navigate('/');
    } catch (err) {
      setError('Greška pri registraciji: ' + err.message);
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
  };

  return (
    <div style={styles.container}>
      <h2>Registracija</h2>
      <form onSubmit={handleSubmit} style={styles.form}>
        <input
          type="text"
          placeholder="Korisničko ime"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          style={styles.input}
          required
        />
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
        <button type="submit" style={styles.button}>Registracija</button>
        {error && <p style={styles.error}>{error}</p>}
      </form>
    </div>
  );
};

export default Register;
