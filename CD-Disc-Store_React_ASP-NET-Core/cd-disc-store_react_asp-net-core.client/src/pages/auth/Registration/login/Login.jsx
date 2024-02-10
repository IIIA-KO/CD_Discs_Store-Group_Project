

import React, { useState } from 'react';
import { useMutation } from 'react-query';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const Login = () => {
  const navigate = useNavigate();
  const [loginError, setLoginError] = useState(null); // Состояние для отслеживания ошибки входа

  const loginMutation = useMutation((formData) =>
    axios.post('https://localhost:7117/Account/Login', formData)
  );

  const handleSubmit = async (e) => {
    e.preventDefault();

    const userName = e.target.elements.userName.value;
    const password = e.target.elements.password.value;

    try {
      await loginMutation.mutateAsync({ userName, password });
      // Переход на компонент Profile после успешного входа
      navigate('/profile');
    } catch (error) {
      console.error('Ошибка авторизации:', error);
      // Установка ошибки в состояние, чтобы отобразить сообщение об ошибке
      setLoginError('Логин или пароль неверны');
    }
  };

  return (
    <div>
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

      {/* Отображение блока с сообщением об успешном или неудачном входе */}
      {loginError && <div style={{ color: 'red' }}>{loginError}</div>}
      {loginMutation.isSuccess && <div style={{ color: 'green' }}>Успешный вход!</div>}
    </div>
  );
};

export default Login;






// import React from 'react';
// import { useMutation } from 'react-query';
// import axios from 'axios';
// import { useNavigate } from 'react-router-dom'; // Используйте useNavigate

// const Login = () => {
//   const navigate = useNavigate(); // Замените useHistory на useNavigate
//   const loginMutation = useMutation((formData) =>
//     axios.post('https://localhost:7117/Account/Login', formData)
//   );

//   const handleSubmit = async (e) => {
//     e.preventDefault();

//     const userName = e.target.elements.userName.value;
//     const password = e.target.elements.password.value;

//     try {
//       await loginMutation.mutateAsync({ userName, password });
//       // Переход на компонент Profile после успешного входа
//       navigate('/films');
//     } catch (error) {
//       console.error('Ошибка авторизации:', error);
//     }
//   };

//   return (
//     <div>
//       <h2>Login</h2>
//       <form onSubmit={handleSubmit}>
//         <div>
//           <label htmlFor="userName">Username:</label>
//           <input type="text" id="userName" name="userName" required />
//         </div>
//         <div>
//           <label htmlFor="password">Password:</label>
//           <input type="password" id="password" name="password" required />
//         </div>
//         <div>
//           <button type="submit" disabled={loginMutation.isLoading}>
//             {loginMutation.isLoading ? 'Logging in...' : 'Log in'}
//           </button>
//         </div>
//       </form>
//     </div>
//   );
// };

// export default Login;
