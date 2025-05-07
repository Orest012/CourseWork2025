import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import '../styles/login.css'

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post('https://localhost:7178/api/Account/login', {
        email,
        password,
      });

      const token = response.data.token || response.data.Token;

      localStorage.setItem('token', token);
      document.cookie = `token=${token}; path=/; max-age=${60 * 60 * 24 * 7}; secure; samesite=strict`;

      setError('');
      navigate('/eventsList');
    } catch (err) {
      setError('Невірний email або пароль');
    }
  };

  return (
    <div className="login-page">
      <div className="login-container">
        <h2 className="login-title">Вхід</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label className="login-label">Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="login-input"
              required
            />
          </div>
          <div className="mb-6">
            <label className="login-label">Пароль</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="login-input"
              required
            />
          </div>
          {error && <p className="login-error">{error}</p>}
          <button type="submit" className="login-button">
            Увійти
          </button>
        </form>
  
        <div className="login-footer">
          <p>Ще не маєте акаунту?</p>
          <button onClick={() => navigate('/register')}>
            Зареєструватися
          </button>
        </div>
      </div>
    </div>
  );  
}
