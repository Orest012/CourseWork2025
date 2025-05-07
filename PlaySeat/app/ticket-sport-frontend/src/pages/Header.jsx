import { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { Link } from 'react-router-dom';
import '../styles/header.css';

export default function Header() {
  const [user, setUser] = useState({ id: null, role: null });

  useEffect(() => {
    const fetchUserInfo = async () => {
      const token = localStorage.getItem('token');
      if (!token) return;

      try {
        const response = await axios.get('https://localhost:7178/api/Account/GetInfo', {
          headers: { Authorization: `Bearer ${token}` },
        });

        const { id, role } = response.data;
        setUser({ id, role });

        // Оновлюємо cookie при кожному запуску
        Cookies.set('userId', id, { expires: 1 });
        Cookies.set('userRole', role, { expires: 1 });
      } catch (err) {
        console.error('Не вдалося отримати інформацію про користувача', err);
      }
    };

    fetchUserInfo(); // завжди викликаємо
  }, []);

  return (
    <header className="main-header">
      <div className="header-container">
        <div className="logo">
          <Link to="/">Sport Events</Link>
        </div>
        <nav className="nav-links">
          <Link to="/eventsList">Головна</Link>
          <Link to="/tickets">Білети</Link>

          {user.role === 'Admin' && (
            <Link to="/create-event">Створити подію</Link>
          )}
          {/* <span className="user-info">
            ID: {user.id} | Роль: {user.role}
          </span> */}

          {/* Кнопка для переходу до сторінки інформації про користувача */}
          <Link to={`/user-info/${user.id}`} className="user-info-button">
            Моя інформація
          </Link>
        </nav>
      </div>
    </header>
  );
}
