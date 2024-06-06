// Profile.jsx
import React from 'react';
import styles from './Profile.module.css';
import { useNavigate } from 'react-router-dom';

const Profile = () => {
  const navigate = useNavigate();

  const handleAuthButtonClick = () => {
    // Переход на компонент Authentication при нажатии кнопки Auth
    navigate('/authentication');
  };
  return (
    <>
    
    <div className={styles.profile}>
      <h2>Profile</h2>
      <div className={styles.field}>
        <strong>Username:</strong> string
      </div>
      <div className={styles.field}>
        <strong>Email:</strong> user@example.com
      </div>
      <div className={styles.field}>
        <strong>Phone Number:</strong> string
      </div>
      <div className={styles.field}>
        <strong>Address:</strong> string
      </div>
      <div className={styles.field}>
        <strong>City:</strong> string
      </div>
      <div className={styles.field}>
        <strong>Birthday:</strong> {/* Добавьте логику для вывода даты рождения */}
      </div>
      <div className={styles.field}>
        <strong>Marital Status:</strong> Married
      </div>
      <div className={styles.field}>
        <strong>Sex:</strong> Male
      </div>
      <div className={styles.field}>
        <strong>Has Child:</strong> Yes
      </div>
    </div>
    <div>
      <button className="button" onClick={handleAuthButtonClick}>Auth</button>
    </div>
    
    </>
  );
};

export default Profile;
