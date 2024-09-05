// src/services/apiService.js
import axios from 'axios';

// Kreiranje API instance
const api = axios.create({
  baseURL: 'http://localhost:5000/api', // Provjerite da URL odgovara vašem backendu
});

// Funkcija za prijavu korisnika
export const loginUser = async (email, password) => {
  try {
    const response = await api.post('/user/login', { email, password });
    return response.data;
  } catch (error) {
    console.error('Greška pri prijavi:', error.response?.data?.message || error.message);
    throw new Error(error.response?.data?.message || 'Prijava nije uspjela. Provjerite podatke i pokušajte ponovo.');
  }
};


export const registerUser = async (username, email, password, roleId) => {
  try {
    const response = await api.post('/user/register', {
      username,
      email,
      password,
      roleId,
    });
    return response.data;
  } catch (error) {
    console.error('Greška pri registraciji:', error.response?.data?.message || error.message);
    throw new Error(error.response?.data?.message || 'Registracija nije uspjela. Provjerite podatke i pokušajte ponovo.');
  }
};


// Funkcija za dohvaćanje svih proizvoda
export const getProducts = async () => {
  try {
    const response = await api.get('/product');
    return response.data;
  } catch (error) {
    console.error('Greška pri dohvaćanju proizvoda:', error.response?.data?.message || error.message);
    throw new Error('Greška pri dohvaćanju proizvoda. Pokušajte ponovo.');
  }
};

// Funkcija za dohvaćanje proizvoda prema ID-u
export const getProductById = async (id) => {
  try {
    const response = await api.get(`/product/${id}`);
    return response.data;
  } catch (error) {
    console.error('Greška pri dohvaćanju proizvoda:', error.response?.data?.message || error.message);
    throw new Error('Greška pri dohvaćanju proizvoda. Pokušajte ponovo.');
  }
};

// Funkcija za kreiranje narudžbe
export const createOrder = async (order) => {
  try {
    const response = await api.post('/order', order);
    return response.data;
  } catch (error) {
    console.error('Greška pri kreiranju narudžbe:', error.response?.data?.message || error.message);
    throw new Error('Greška pri kreiranju narudžbe. Pokušajte ponovo.');
  }
};
