import React, { useState } from 'react';
import { useMutation } from 'react-query';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './Login.css';
const Login = () => {
  const navigate = useNavigate();
  const [loginError, setLoginError] = useState(null);

  const loginMutation = useMutation((formData) =>
    axios.post('https://localhost:7117/Account/Login', formData)
  );

  const handleSubmit = async (e) => {
    e.preventDefault();

    const userName = e.target.elements.userName.value;
    const password = e.target.elements.password.value;

    try {
      await loginMutation.mutateAsync({ userName, password });
      navigate('/profile');
    } catch (error) {
      console.error('Authorization error:', error);
      setLoginError('Login or password is incorrect');
    }
  };

  return (
    <div className='login'>
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="userName">Username:</label>
          <input type="text" id="userName" name="userName" required />
        </div>
        <div>
          <label htmlFor="password">Password:</label>
          <input type="password" id="password" name="password" required />
        </div>
        <div>
          <button type="submit" disabled={loginMutation.isLoading}>
            {loginMutation.isLoading ? 'Logging in...' : 'Log in'}
          </button>
        </div>
      </form>

      {loginError && <div style={{ color: 'red' }}>{loginError}</div>}
      {loginMutation.isSuccess && <div style={{ color: 'green' }}>Login success!</div>}
    </div>
  );
};

export default Login;