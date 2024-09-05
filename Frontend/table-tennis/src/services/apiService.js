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

export const createOrder = async (orderData) => {
  try {
    const response = await api.post('/order', orderData);
    return response.data;
  } catch (error) {
    console.error('Greška pri kreiranju narudžbe:', error.response?.data?.message || error.message);
    throw new Error(error.response?.data?.message || 'Greška pri kreiranju narudžbe. Pokušajte ponovo.');
  }
};

export const getOrders = async () => {
  try {
    const response = await api.get('/order'); // Promijenjeno iz '/orders' u '/order'
    return response.data;
  } catch (error) {
    console.error('Greška pri dohvaćanju narudžbi:', error);
    throw error;
  }
};

export const addProduct = async (productData) => {
  const response = await api.post('/product', productData);
  return response.data;
};

export const getCategories = async () => {
  const response = await api.get('/category'); // Pretpostavlja se da je ruta za dohvaćanje kategorija '/category'
  return response.data;
};

export const deleteProduct = async (productId) => {
  try {
    const response = await api.delete(`/product/${productId}`);
    return response.data;
  } catch (error) {
    console.error('Greška pri brisanju proizvoda:', error.response?.data?.message || error.message);
    throw new Error('Greška pri brisanju proizvoda. Pokušajte ponovo.');
  }
};


export const updateProduct = async (productId, productData) => {
  try {
    const response = await api.put(`/product/${productId}`, productData);
    return response.data;
  } catch (error) {
    console.error('Greška pri ažuriranju proizvoda:', error.response?.data || error.message);
    throw new Error('Greška pri ažuriranju proizvoda. Pokušajte ponovo.');
  }
};
