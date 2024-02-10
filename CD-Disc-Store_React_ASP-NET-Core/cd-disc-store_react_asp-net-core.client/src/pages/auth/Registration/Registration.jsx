import React from 'react';
import { useMutation, useQueryClient } from 'react-query';
import axios from 'axios';
import './Registration.css';
const Registration = () => {
  const queryClient = useQueryClient();
  const [registrationError, setRegistrationError] = React.useState(null);

  const registerUser = async (formData) => {
    try {
      const response = await axios.post('https://localhost:7117/Account/Register', formData);
  
      setRegistrationError(null);
      queryClient.invalidateQueries('userData');
    } catch (error) {
      console.error('Error during registration:', error);
      setRegistrationError(
        error.response?.data?.message || 'Ошибка регистрации. Пожалуйста, проверьте введенные данные.'
      );
      console.error(error.stack);
      console.error('Response data:', error.response?.data);
    }
  };
  

  const { mutate, isLoading, isSuccess } = useMutation(registerUser, {
    mutationKey: ['registration'],
    onMutate: async () => {
      setRegistrationError(null);
      await queryClient.cancelMutations('userData');
    },
    onError: (err) => {
      console.error('Error during registration:', err);
      setRegistrationError(
        err.response?.data?.message || 'Ошибка регистрации. Пожалуйста, проверьте введенные данные.'
      );
      console.error(err.stack);
      console.error('Response data:', err.response?.data);
    },
    onSuccess: (data) => {
      console.log('Registration success:', data);
      // Возможно, здесь вы захотите предпринять какие-то дополнительные действия после успешной регистрации
    },
  });

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
    hasChild: true,
  });

  const [errors, setErrors] = React.useState({});

  const validateForm = () => {
    const newErrors = {};

    if (!formData.email.includes('@')) {
      newErrors.email = 'Invalid email address';
    }

    if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match';
    }

    if (!/^\d{10}$/.test(formData.phoneNumber)) {
      newErrors.phoneNumber = 'Invalid phone number';
    }

    if (!/^\d{4}-\d{2}-\d{2}$/.test(formData.birthDay)) {
      newErrors.birthDay = 'Invalid birthday format (YYYY-MM-DD)';
    }

    if (formData.sex === null) {
      newErrors.sex = 'Sex is required';
    }

    if (formData.hasChild === null) {
      newErrors.hasChild = 'Please confirm if you have a child';
    }

    setErrors(newErrors);

    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();

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
            value={formData.userName}
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
        {errors.city && <div style={{ color: 'red' }}>{errors.city}</div>}
        <br />

        <label>
          Birthday:
          <input
            type="text"
            value={formData.birthDay}
            onChange={(e) => setFormData({ ...formData, birthDay: e.target.value })}
            placeholder="YYYY-MM-DD"
          />
        </label>
        {errors.birthDay && <div style={{ color: 'red' }}>{errors.birthDay}</div>}
        <br />

        <label>
          Married Status:
          <input
            type="radio"
            name="marriedStatus"
            value={true}
            checked={formData.marriedStatus === true}
            onChange={(e) => setFormData({ ...formData, marriedStatus: e.target.checked})}
          /> Married
          <input
            type="radio"
            name="marriedStatus"
            value={false}
            checked={formData.marriedStatus === false}
            onChange={(e) => setFormData({ ...formData, marriedStatus: e.target.checked })}
          /> Single
        </label>
        {errors.marriedStatus && <div style={{ color: 'red' }}>{errors.marriedStatus}</div>}
        <br />

        <label>
          Sex:
          <input
            type="radio"
            name="sex"
            value={true}
            checked={formData.sex === true}
            onChange={() => setFormData({ ...formData, sex: true })}
          /> Male
          <input
            type="radio"
            name="sex"
            value={false}
            checked={formData.sex === false}
            onChange={() => setFormData({ ...formData, sex: false })}
          /> Female
        </label>
        {errors.sex && <div style={{ color: 'red' }}>{errors.sex}</div>}
        <br />

        <label>
          Has a Child:
          <input
            type="checkbox"
            name="hasChild"
            checked={formData.hasChild}
            onChange={() => setFormData({ ...formData, hasChild: !formData.hasChild })}
          />
        </label>
        {errors.hasChild && <div style={{ color: 'red' }}>{errors.hasChild}</div>}
        <br />

        <button type="submit" disabled={isLoading || isSuccess}>Register</button>

        {registrationError && <div style={{ color: 'red' }}>{registrationError}</div>}
        {isSuccess && <div style={{ color: 'green' }}>Вы успешно зарегистрировались!</div>}
      </form>
    </div>
  );
};

export default Registration;
