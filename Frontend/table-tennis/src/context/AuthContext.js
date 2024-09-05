// src/context/AuthContext.js
import React, { createContext, useContext, useState, useEffect } from 'react';
import { loginUser, registerUser } from '../services/apiService';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      try {
        setUser(JSON.parse(storedUser));
      } catch (error) {
        console.error('Error parsing user data from localStorage:', error);
        localStorage.removeItem('user');
      }
    }
  }, []);

  const loginUserHandler = async (email, password) => {
    try {
      const response = await loginUser(email, password);
      if (response) {
        // Dodajemo naziv uloge dohvaćen s backend-a
        const roleName = await getRoleNameById(response.roleId); // Pretpostavimo da postoji funkcija koja dohvaća naziv uloge
        const userWithRole = { ...response, roleName }; // Dodajemo naziv uloge korisničkom objektu
        setUser(userWithRole);
        localStorage.setItem('user', JSON.stringify(userWithRole));
      } else {
        throw new Error('Prijava nije uspjela');
      }
    } catch (error) {
      console.error('Error during login:', error.message);
      throw error;
    }
  };

  const registerUserHandler = async (username, email, password, roleId) => {
    try {
      const response = await registerUser(username, email, password, roleId);
      const roleName = await getRoleNameById(response.roleId); // Pretpostavimo da postoji funkcija koja dohvaća naziv uloge
      const userWithRole = { ...response, roleName };
      setUser(userWithRole);
      localStorage.setItem('user', JSON.stringify(userWithRole));
    } catch (error) {
      console.error('Greška pri registraciji:', error.message);
      throw error;
    }
  };

  const logoutUserHandler = () => {
    setUser(null);
    localStorage.removeItem('user');
  };

  // Dodajte funkciju koja dohvaća naziv uloge na temelju roleId
const getRoleNameById = async (roleId) => {
  try {
    const response = await fetch(`/api/roles/${roleId}`); // Prilagodite URL prema vašoj API strukturi
    if (response.ok) {
      const data = await response.json();
      return data.roleName; // Pretpostavka da API vraća objekt s roleName
    } else {
      console.error('Failed to fetch role name:', response.statusText);
      return null;
    }
  } catch (error) {
    console.error('Error fetching role name:', error);
    return null;
  }
};


  return (
    <AuthContext.Provider
      value={{ user, loginUserHandler, registerUserHandler, logoutUserHandler }}
    >
      {children}
    </AuthContext.Provider>
  );
};
