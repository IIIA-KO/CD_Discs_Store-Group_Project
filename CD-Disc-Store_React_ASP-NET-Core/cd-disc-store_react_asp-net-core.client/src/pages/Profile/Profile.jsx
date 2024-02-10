import React from 'react'
import Registration from '../auth/Registration/Registration';
import Login from '../auth/Registration/login/Login';
import './Profile.css'
import { useState } from 'react';
const Profile = () => {
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

export default Profile;