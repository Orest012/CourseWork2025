import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import '../styles/register.css'

export default function Register() {
  const [email, setEmail] = useState('');
  const [name, setName] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post('https://localhost:7178/api/Account/register', {
        email,
        name,
        password,
      });

      const token = response.data.token || response.data.Token;

      localStorage.setItem('token', token);
      document.cookie = `token=${token}; path=/; max-age=${60 * 60 * 24 * 7}; secure; samesite=strict`;

      setError('');
      navigate('/');
    } catch (err) {
      if (err.response && err.response.status === 400) {
        setError('Користувач з таким email вже існує.');
      } else {
        setError('Помилка при реєстрації. Спробуйте пізніше.');
      }
    }
  };

  return (
    <div className="register-page">
      <div className="register-container">
        <h2 className="register-title">Реєстрація</h2>
        <form onSubmit={handleRegister}>
          <div className="mb-4">
            <label className="register-label">Ім’я</label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="register-input"
              required
            />
          </div>
          <div className="mb-4">
            <label className="register-label">Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="register-input"
              required
            />
          </div>
          <div className="mb-6">
            <label className="register-label">Пароль</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="register-input"
              required
            />
          </div>
          {error && <p className="register-error">{error}</p>}
          <button type="submit" className="register-button">
            Зареєструватися
          </button>
        </form>
      </div>
    </div>
  );  
}
