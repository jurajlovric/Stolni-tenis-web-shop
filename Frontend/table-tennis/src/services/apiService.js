import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api',
});

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


export const getProducts = async () => {
  try {
    const response = await api.get('/product');
    return response.data;
  } catch (error) {
    console.error('Greška pri dohvaćanju proizvoda:', error.response?.data?.message || error.message);
    throw new Error('Greška pri dohvaćanju proizvoda. Pokušajte ponovo.');
  }
};

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
    const response = await api.get('/order');
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
  const response = await api.get('/category');
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
