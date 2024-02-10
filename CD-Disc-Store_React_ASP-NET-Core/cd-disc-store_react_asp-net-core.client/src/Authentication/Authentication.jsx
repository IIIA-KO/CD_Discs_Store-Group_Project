import React from 'react'
import './Authentication.css'
import { useState } from 'react';
import Registration from '../pages/auth/Registration/Registration';
import Login from '../pages/auth/Registration/login/Login';
const Authentication = () => {
  const [isRegistered, setIsRegistered] = useState(true);
  const [showRegistration, setShowRegistration] = useState(true);

  const toggleComponents = () => {
    setIsRegistered(!isRegistered);
    setShowRegistration(!showRegistration);
  };

  return (
    <div className='profile-container'>
      {showRegistration && (
        <div className='button-container'>
          <Registration />
          <button onClick={toggleComponents}>
            {isRegistered ? "If already registered, click here" : "No account yet? Click here"}
          </button>
        </div>
      )}

      {!showRegistration && (
        <div className='button-container'>
          <Login />
          <button onClick={toggleComponents}>Back to Registration</button>
        </div>
      )}
    </div>
  );
};

export default Authentication;