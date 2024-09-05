import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { addProduct, getCategories } from '../services/apiService';

const AddProduct = () => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [price, setPrice] = useState('');
  const [imageUrl, setImageUrl] = useState('');
  const [categoryId, setCategoryId] = useState('');
  const [categories, setCategories] = useState([]);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  // Dohvaćanje svih kategorija pri učitavanju komponente
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await getCategories();
        setCategories(response);
      } catch (error) {
        console.error('Greška pri dohvaćanju kategorija:', error);
        setError('Greška pri dohvaćanju kategorija.');
      }
    };

    fetchCategories();
  }, []);

  const handleAddProduct = async (e) => {
    e.preventDefault();

    // Provjera je li cijena broj i je li ispravno postavljen `categoryId`
    if (!categoryId || categoryId === '') {
      setError('Odaberite kategoriju za proizvod.');
      return;
    }

    // Pretpostavljamo da je categoryName već dohvaćen ili odabran s popisa
    const categoryName = categories.find(cat => cat.categoryId === categoryId)?.name || '';

    const productData = {
      name,
      description,
      price: parseFloat(price),
      categoryId, // Prosljeđivanje točno odabranog `categoryId`
      categoryName, // Dodajemo i CategoryName kako bismo ispunili zahtjeve backend-a
      imageUrl,
    };

    console.log('Podaci koji se šalju:', productData); // Ispis podataka za provjeru

    try {
      await addProduct(productData);
      alert('Proizvod uspješno dodan!');
      navigate('/admin-dashboard'); // Vraćanje na admin dashboard nakon dodavanja proizvoda
    } catch (err) {
      console.error('Greška pri dodavanju proizvoda:', err.response?.data || err.message);
      setError('Greška pri dodavanju proizvoda. Provjerite podatke i pokušajte ponovo.');
    }
  };

  // Stilovi za stranicu
  const styles = {
    container: {
      maxWidth: '600px',
      margin: '0 auto',
      padding: '20px',
      border: '1px solid #ccc',
      borderRadius: '8px',
      boxShadow: '0 2px 10px rgba(0, 0, 0, 0.1)',
      backgroundColor: '#f9f9f9',
    },
    header: {
      textAlign: 'center',
      marginBottom: '20px',
    },
    input: {
      width: '100%',
      padding: '10px',
      margin: '10px 0',
      borderRadius: '4px',
      border: '1px solid #ddd',
      fontSize: '16px',
    },
    button: {
      padding: '10px 20px',
      backgroundColor: '#007bff',
      color: '#fff',
      border: 'none',
      borderRadius: '5px',
      cursor: 'pointer',
      fontSize: '16px',
      width: '100%',
      transition: 'background-color 0.3s',
    },
    buttonHover: {
      backgroundColor: '#0056b3',
    },
    error: {
      color: 'red',
      textAlign: 'center',
      marginBottom: '10px',
    },
    select: {
      width: '100%',
      padding: '10px',
      borderRadius: '4px',
      border: '1px solid #ddd',
      fontSize: '16px',
      marginBottom: '10px',
    },
  };

  return (
    <div style={styles.container}>
      <h2 style={styles.header}>Dodaj Proizvod</h2>
      {error && <p style={styles.error}>{error}</p>}
      <form onSubmit={handleAddProduct}>
        <input
          type="text"
          placeholder="Ime proizvoda"
          value={name}
          onChange={(e) => setName(e.target.value)}
          style={styles.input}
          required
        />
        <textarea
          placeholder="Opis proizvoda"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          style={styles.input}
          rows="4"
          required
        />
        <input
          type="number"
          placeholder="Cijena"
          value={price}
          onChange={(e) => setPrice(e.target.value)}
          style={styles.input}
          required
        />
        <input
          type="text"
          placeholder="URL slike"
          value={imageUrl}
          onChange={(e) => setImageUrl(e.target.value)}
          style={styles.input}
        />
        <select
          value={categoryId}
          onChange={(e) => setCategoryId(e.target.value)}
          style={styles.select}
          required
        >
          <option value="">Odaberi kategoriju</option>
          {categories.map((category) => (
            <option key={category.categoryId} value={category.categoryId}>
              {category.name}
            </option>
          ))}
        </select>
        <button
          type="submit"
          style={styles.button}
          onMouseOver={(e) => (e.target.style.backgroundColor = '#0056b3')}
          onMouseOut={(e) => (e.target.style.backgroundColor = '#007bff')}
        >
          Dodaj proizvod
        </button>
      </form>
    </div>
  );
};

export default AddProduct;
