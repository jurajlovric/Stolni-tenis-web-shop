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
        setUser(response);
        localStorage.setItem('user', JSON.stringify(response));
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
      setUser(response);
      localStorage.setItem('user', JSON.stringify(response));
    } catch (error) {
      console.error('Greška pri registraciji:', error.message);
      throw error;
    }
  };

  const logoutUserHandler = () => {
    setUser(null);
    localStorage.removeItem('user');
  };

  return (
    <AuthContext.Provider
      value={{ user, loginUserHandler, registerUserHandler, logoutUserHandler }}
    >
      {children}
    </AuthContext.Provider>
  );
};
