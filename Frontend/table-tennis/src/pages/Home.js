import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Home = () => {
  const navigate = useNavigate();
  const { user } = useAuth();

  const isAdmin = user?.roleId === '3af2c54a-6fe7-4b32-be9c-b6991fab5bdb';

  const buttonStyle = {
    padding: '10px 20px',
    margin: '10px',
    border: 'none',
    borderRadius: '5px',
    backgroundColor: '#007bff',
    color: '#fff',
    cursor: 'pointer',
    transition: 'background-color 0.3s',
  };

  const adminButtonStyle = {
    ...buttonStyle,
    backgroundColor: '#28a745',
  };

  return (
    <div style={{ textAlign: 'center', padding: '40px' }}>
      <h1>Dobro došli u trgovinu stolnoteniske opreme</h1>
      <p>
        Ovdje možete pronaći najbolji izbor stolnoteniske opreme za sve razine
        igrača. Od reketa, stolova, loptica do torbi i dodatne opreme. Pronađite
        sve što vam je potrebno za vašu igru!
      </p>
      <div style={{ marginTop: '20px' }}>
        <button style={buttonStyle} onClick={() => navigate('/products')}>
          Pregledajte Proizvode
        </button>
        <button style={buttonStyle} onClick={() => navigate('/cart')}>
          Košarica
        </button>

        {isAdmin && (
          <button
            style={adminButtonStyle}
            onClick={() => navigate('/admindashboard')}
          >
            Admin Dashboard
          </button>
        )}
      </div>
    </div>
  );
};

export default Home;
