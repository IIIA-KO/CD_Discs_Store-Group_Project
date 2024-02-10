import React from 'react';
import { useMutation, useQueryClient } from 'react-query';
import axios from 'axios';
import './login.css';
const Login = () => {
  const queryClient = useQueryClient();

  // Mutation function to send login data to the server using Axios
  const loginUser = async ({ userName, password }) => {
    const response = await axios.post('https://localhost:7117/Account/Login', {
      userName,
      password,
    });

    if (!response.data.success) {
      throw new Error(response.data.message);
    }

    // Invalidate and refetch user data after successful login
    queryClient.invalidateQueries('userData');
  };

  // React-Query useMutation hook
  const { mutate } = useMutation(loginUser);

  // Form state and validation
  const [loginData, setLoginData] = React.useState({
    userName: '',
    password: '',
  });

  const [errors, setErrors] = React.useState({});

  // Validation function
  const validateForm = () => {
    const newErrors = {};

    // Add your validation logic here
    // Example: Check if userName and password are provided
    if (!loginData.userName.trim()) {
      newErrors.userName = 'Username is required';
    }

    if (!loginData.password.trim()) {
      newErrors.password = 'Password is required';
    }

    setErrors(newErrors);

    return Object.keys(newErrors).length === 0;
  };

  // Handle login submission
  const handleLogin = (e) => {
    e.preventDefault();

    // Подтвердите форму перед отправкой данных для входа.
    if (validateForm()) {
      mutate(loginData);
    }
  };

  return (
    <div className="login-container">
    <form className='login-form' onSubmit={handleLogin}>
      {/* Username field */}
      <label>
        Username:
        <input
          type="text"
          value={loginData.userName}
          onChange={(e) => setLoginData({ ...loginData, userName: e.target.value })}
        />
      </label>
      {errors.userName && <div style={{ color: 'red' }}>{errors.userName}</div>}

      {/* Password field */}
      <label>
        Password:
        <input
          type="password"
          value={loginData.password}
          onChange={(e) => setLoginData({ ...loginData, password: e.target.value })}
        />
      </label>
      {errors.password && <div style={{ color: 'red' }}>{errors.password}</div>}

      {/* Submit button */}
      <button type="submit">Login</button>
    </form>
    </div>
  );
};

export default Login;
