
import React from 'react';
import { useMutation, useQueryClient } from 'react-query';
import { useState } from 'react';
import axios from 'axios';
import './Registration.css'


const Registration = () => {
  const queryClient = useQueryClient();
  const [registrationError, setRegistrationError] = useState(null); // Состояние для отслеживания ошибки регистрации

  const registerUser = async (formData) => {
    try {
      const response = await axios.post('https://localhost:7117/Account/Register', formData);

      if (!response.data.success) {
        throw new Error(response.data.message);
      }

      setRegistrationError(null); // Очистить предыдущую ошибку, если была

      // Аннулировать и обновить данные пользователя после успешной регистрации.
      queryClient.invalidateQueries('userData');
    } catch (error) {
      setRegistrationError('Ошибка регистрации. Пожалуйста, проверьте введенные данные.');
    }
  };

  // React-Query useMutation hook
  const { mutate, isLoading, error, isSuccess } = useMutation(registerUser);

  // Стан форми та перевірка
  const [formData, setFormData] = React.useState({
    userName: '',
    email: '',
    password: '',
    confirmPassword: '',
    phoneNumber: '',
    address: '',
    city: '',
    birthDay: '',
    marriedStatus: true,
    sex: true,
    hasChild: false,
  });

  const [errors, setErrors] = React.useState({});

  // Функція перевірки
  const validateForm = () => {
    const newErrors = {};


    //  Перевірте, чи дійсна електронна адреса
    if (!formData.email.includes('@')) {
      newErrors.email = 'Invalid email address';
    }

    // Перевірте, чи збігаються пароль і пароль підтвердження
    if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match';
    }

    // Перевірте, чи дійсний номер телефону 
    if (!/^\d{10}$/.test(formData.phoneNumber)) {
      newErrors.phoneNumber = 'Invalid phone number';
    }

    // Перевірте, чи день народження є дійсною датою 
    if (!/^\d{4}-\d{2}-\d{2}$/.test(formData.birthday)) {
      newErrors.birthday = 'Invalid birthday format (YYYY-MM-DD)';
    }

    // Перевірте, чи вказано стать 
    if (!formData.sex.trim()) {
      newErrors.sex = 'Sex is required';
    }

    // Перевірте, чи надано "Has a Child".
    if (!formData.hasChild) {
      newErrors.hasChild = 'Please confirm if you have a child';
    }

    setErrors(newErrors);

    return Object.keys(newErrors).length === 0;
  };

  // Обробка подання форми
  const handleSubmit = (e) => {
    e.preventDefault();

    // Перевірте форму перед надсиланням даних
    if (validateForm()) {
      mutate(formData);
    }
    console.log(formData);
  };

  return (
    <div className='registration-container'>
      <form className='registration-form' onSubmit={handleSubmit}>

        <label>
          Username:
          <input
            type="text"
            value={formData.username}
            onChange={(e) => setFormData({ ...formData, userName: e.target.value })}
          />
        </label>
        <br />

        <label>
          Email:
          <input
            type="text"
            value={formData.email}
            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
          />
        </label>
        {/* Показати помилки перевірки*/}
        {errors.email && <div style={{ color: 'red' }}>{errors.email}</div>}
        <br />

        <label>
          Password:
          <input
            type="password"
            value={formData.password}
            onChange={(e) => setFormData({ ...formData, password: e.target.value })}
          />
        </label>
        {/* Показати помилки перевірки*/}
        {errors.password && <div style={{ color: 'red' }}>{errors.password}</div>}
        <br />

        <label>
          Confirm Password:
          <input
            type="password"
            value={formData.confirmPassword}
            onChange={(e) => setFormData({ ...formData, confirmPassword: e.target.value })}
          />
        </label>
        <br />

        <label>
          Phone Number:
          <input
            type="text"
            value={formData.phoneNumber}
            onChange={(e) => setFormData({ ...formData, phoneNumber: e.target.value })}
          />
        </label>
        {/* Показати помилки перевірки*/}
        {errors.phoneNumber && <div style={{ color: 'red' }}>{errors.phoneNumber}</div>}
        <br />

        <label>
          Address:
          <input
            type="text"
            value={formData.address}
            onChange={(e) => setFormData({ ...formData, address: e.target.value })}
          />
        </label>
        {/* Показати помилки перевірки*/}
        {errors.address && <div style={{ color: 'red' }}>{errors.address}</div>}
        <br />

        <label>
          City:
          <input
            type="text"
            value={formData.city}
            onChange={(e) => setFormData({ ...formData, city: e.target.value })}
          />
        </label>
        {/* Показати помилки перевірки*/}
        {errors.city && <div style={{ color: 'red' }}>{errors.city}</div>}
        <br />

        <label>
          Birthday:
          <input
            type="date"
            value={formData.birthday}

            onChange={(e) => setFormData({ ...formData, birthDay: e.target.value })}
          />
        </label>
        {/* Показати помилки перевірки*/}
        {errors.birthday && <div style={{ color: 'red' }}>{errors.birthDay}</div>}
        <br />

        <label>
          Married Status:
          <input
            type="radio"
            name="marriedStatus"
            value="true"
            checked={formData.marriedStatus === "true"}
            onChange={(e) => setFormData({ ...formData, marriedStatus: e.target.value })}
          /> Married
          <input
            type="radio"
            name="marriedStatus"
            value="false"
            checked={formData.marriedStatus === "false"}
            onChange={(e) => setFormData({ ...formData, marriedStatus: e.target.value })}
          /> Single
        </label>

        {errors.marriedStatus && <div style={{ color: 'red' }}>{errors.marriedStatus.message}</div>}
        <br />

        <label>
          Sex:
          <input
            type="radio"
            name="sex"
            value="male"
            checked={formData.sex === "male"}
            onChange={(e) => setFormData({ ...formData, sex: e.target.value })}
          /> Male
          <input
            type="radio"
            name="sex"
            value="female"
            checked={formData.sex === "female"}
            onChange={(e) => setFormData({ ...formData, sex: e.target.value })}
          /> Female
        </label>

        {errors.sex && <div style={{ color: 'red' }}>{errors.sex.message}</div>}
        <br />



        <label>
          Has a Child:
          <input
            type="checkbox"
            name="hasChild"
            checked={formData.hasChild}
            onChange={(e) => setFormData({ ...formData, hasChild: e.target.checked })}
          />
        </label>
        {/* Показать ошибки валидации */}
        {errors.hasChild && <div style={{ color: 'red' }}>{errors.hasChild.message}</div>}
        <br />



        {/* Submit button */}
        <button type="submit" disabled={isSuccess} >Register</button>

        {/* Отображение блока с сообщением об успешной или неудачной регистрации */}
        {registrationError && <div style={{ color: 'red' }}>{registrationError}</div>}
        {isSuccess && <div style={{ color: 'green' }}>Вы успешно зарегистрировались!</div>}
        {/* ... (остальной код формы) */}

      </form>
    </div>
  );
};

export default Registration;


