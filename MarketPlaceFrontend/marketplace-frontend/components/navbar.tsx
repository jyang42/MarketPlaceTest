'use client';
import React from 'react';
import { useAuth } from '../app/context/AuthContext';

const Navbar: React.FC = () => {
  const { isLoggedIn, logOut } = useAuth();

  if (!isLoggedIn) {
    return null; // Hide the navbar if not logged in
  }

  return (
    <nav className="bg-gray-800 text-white p-4">
      <ul className="flex space-x-4">
        <li><a href="/">Home</a></li>
        <li><a href="/jobs/create">Post A Job</a></li>
        <li><a href="/login" onClick={logOut}>Logout</a></li>
      </ul>
    </nav>
  );
};

export default Navbar;