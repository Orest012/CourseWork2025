import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Cookies from 'js-cookie';
import '../styles/eventsList.css';

export default function EventsList() {
  const [events, setEvents] = useState([]);
  const [filteredEvents, setFilteredEvents] = useState([]);
  const [venueFilter, setVenueFilter] = useState('');
  const [sportTypeFilter, setSportTypeFilter] = useState('');
  const [dateFilter, setDateFilter] = useState('');
  const [userRole, setUserRole] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchEvents = async () => {
      const token = localStorage.getItem('token');
      try {
        const response = await axios.get('https://localhost:7178/api/Event/GetEvents', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setEvents(response.data);
        setFilteredEvents(response.data);
      } catch (error) {
        console.error('Помилка при завантаженні подій:', error);
      }
    };
  
    fetchEvents();
  
    // Зчитування ролі з cookie (з fallback і очисткою лапок)
    const rawRole = Cookies.get('userRole');
    const cleanedRole = rawRole?.replace(/"/g, '') || 'User';
    setUserRole(cleanedRole);
  }, []);
  

  const filterEvents = () => {
    const filtered = events.filter(event =>
      (!venueFilter || event.city?.toLowerCase().includes(venueFilter.toLowerCase())) &&
      (!sportTypeFilter || event.sportType?.toLowerCase().includes(sportTypeFilter.toLowerCase())) &&
      (!dateFilter || new Date(event.eventDate).toISOString().split('T')[0] === dateFilter)
    );
    setFilteredEvents(filtered);
  };

  useEffect(() => {
    filterEvents();
  }, [venueFilter, sportTypeFilter, dateFilter]);

  const resetFilters = () => {
    setVenueFilter('');
    setSportTypeFilter('');
    setDateFilter('');
    setFilteredEvents(events);
  };

  const handleDelete = async (eventId) => {
    const token = localStorage.getItem('token');
    if (!window.confirm('Ви впевнені, що хочете видалити цю подію?')) return;

    try {
      await axios.delete(`https://localhost:7178/api/Event/DeleteEvent/${eventId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      // Оновити список після видалення
      const updated = events.filter(e => e.sportEventsId !== eventId);
      setEvents(updated);
      setFilteredEvents(updated);
    } catch (error) {
      console.error('Помилка при видаленні події:', error);
      alert('Не вдалося видалити подію');
    }
  };

  return (
    <div className="events-list-page">
      <h1 className="events-list-title">Список подій</h1>

      {/* Фільтри */}
      <div className="filters-container">
        <input
          type="text"
          placeholder="Фільтр за містом"
          value={venueFilter}
          onChange={(e) => setVenueFilter(e.target.value)}
          className="filter-input"
        />
        <input
          type="text"
          placeholder="Фільтр за видом спорту"
          value={sportTypeFilter}
          onChange={(e) => setSportTypeFilter(e.target.value)}
          className="filter-input"
        />
        <input
          type="date"
          value={dateFilter}
          onChange={(e) => setDateFilter(e.target.value)}
          className="filter-input"
        />
        <button onClick={resetFilters} className="reset-button">
          Скинути фільтри
        </button>
      </div>

      {/* Події */}
      {filteredEvents.length > 0 ? (
        <div className="space-y-4">
          {filteredEvents.map((event) => (
            <div key={event.sportEventsId} className="event-card">
              <div className="event-info w-2/3">
                <h2 className="event-title">{event.title}</h2>
                <p className="event-description">{event.description}</p>
                <p className="event-date">
                  <strong>Дата:</strong> {new Date(event.eventDate).toLocaleDateString()}
                </p>
                <p className="event-venue">
                  <strong>Місто:</strong> {event.city}
                </p>
                <p className="event-sport">
                  <strong>Спорт:</strong> {event.sportType || 'Не вказано'}
                </p>
                <p className="event-organizer">
                  <strong>Організатор:</strong> {event.organizer}
                </p>

                <button
                  onClick={() => navigate(`/events/${event.sportEventsId}`)}
                  className="event-button mt-2"
                >
                  Детальніше
                </button>

                {userRole === 'Admin' && (
                  <button
                    onClick={() => handleDelete(event.sportEventsId)}
                    className="event-button delete-button"
                  >
                    🗑️ Видалити
                  </button>
                )}
              </div>

              <div className="event-image-wrapper">
                {event.imageUrl ? (
                  <img
                    src={event.imageUrl}
                    alt={`Зображення для події ${event.title}`}
                    className="event-image"
                  />
                ) : (
                  <p>Зображення не доступне</p>
                )}
              </div>
            </div>
          ))}
        </div>
      ) : (
        <p className="no-events-message">Подій не знайдено.</p>
      )}
    </div>
  );
}
