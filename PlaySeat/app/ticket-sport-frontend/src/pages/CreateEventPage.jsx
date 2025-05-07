import { useState } from 'react';
import axios from 'axios';
import '../styles/createEvent.css';

export default function CreateEvent() {
  const [eventData, setEventData] = useState({
    title: '',
    description: '',
    eventDate: '',
    venueName: '',
    organizerName: '',
    sportType: '',
    imageUrl: ''
  });

  const [message, setMessage] = useState('');

  const handleChange = (e) => {
    setEventData({ ...eventData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem('token');

    try {
      const response = await axios.post(
        'https://localhost:7178/api/Event/Create',
        eventData,
        {
          headers: { Authorization: `Bearer ${token}` }
        }
      );

      setMessage('Подія успішно створена!');
      setEventData({
        title: '',
        description: '',
        eventDate: '',
        venueName: '',
        organizerName: '',
        sportType: '',
        imageUrl: ''
      });
    } catch (error) {
      console.error(error);
      setMessage('Помилка при створенні події');
    }
  };

  return (
    <div className="create-event-container">
      <h2>Створити нову подію</h2>
      <form className="create-event-form" onSubmit={handleSubmit}>
        <input name="title" placeholder="Назва" value={eventData.title} onChange={handleChange} required />
        <textarea name="description" placeholder="Опис" value={eventData.description} onChange={handleChange} required />
        <input type="datetime-local" name="eventDate" value={eventData.eventDate} onChange={handleChange} required />
        <input name="venueName" placeholder="Місце проведення" value={eventData.venueName} onChange={handleChange} required />
        <input name="organizerName" placeholder="Організатор" value={eventData.organizerName} onChange={handleChange} required />
        <input name="sportType" placeholder="Вид спорту" value={eventData.sportType} onChange={handleChange} />
        <input name="imageUrl" placeholder="URL зображення" value={eventData.imageUrl} onChange={handleChange} />
        <button type="submit">Створити подію</button>
      </form>
      {message && <p className="create-message">{message}</p>}
    </div>
  );
}
