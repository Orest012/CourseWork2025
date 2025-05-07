import { useEffect, useState } from 'react';
import axios from 'axios';
import { Link, useNavigate } from 'react-router-dom';
import '../styles/userInfo.css'; // створити за бажанням для стилізації

export default function UserInfo() {
  const [userInfo, setUserInfo] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUserInfo = async () => {
      const token = localStorage.getItem('token');
      try {
        const response = await axios.get('https://localhost:7178/api/Account/GetUserInfo', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setUserInfo(response.data);
      } catch (error) {
        console.error('Помилка при завантаженні інформації про користувача:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserInfo();
  }, []);

  const handleLoginRedirect = () => {
    navigate('/login'); // перехід на сторінку логіну
  };

  return (
    <div className="user-info-page">
      <h1 className="page-title">Інформація про користувача</h1>

      {loading ? (
        <p>Завантаження...</p>
      ) : userInfo ? (
        <div className="user-info-card">
          <p><strong>ID:</strong> {userInfo.id}</p>
          <p><strong>Ім'я:</strong> {userInfo.name}</p>
          <p><strong>Email:</strong> {userInfo.email}</p>
          <p><strong>Роль:</strong> {userInfo.role}</p>
          <p><strong>Зареєстрований:</strong> {userInfo.createdAt}</p>
        </div>
      ) : (
        <p>Не вдалося отримати дані користувача.</p>
      )}

      {/* Кнопка для переходу на сторінку логіну */}
      <button onClick={handleLoginRedirect} className="login-button">
        Log out
      </button>
    </div>
  );
}
