import { useEffect, useState } from 'react';
import axios from 'axios';
import '../styles/myTickets.css';

export default function MyTickets() {
  const [tickets, setTickets] = useState([]);
  const [topEvent, setTopEvent] = useState(null);  // Додано стейт для збереження топової події
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchTicketsAndTopEvent = async () => {
      const token = localStorage.getItem('token');
      try {
        // Отримуємо квитки
        const ticketsResponse = await axios.get('https://localhost:7178/api/Tickets/ExportUserTickets', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setTickets(ticketsResponse.data);

        // Отримуємо топову подію
        const topEventResponse = await axios.get('https://localhost:7178/api/Tickets/top-event', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setTopEvent(topEventResponse.data[0]);  // Оскільки ми отримуємо один об'єкт
      } catch (error) {
        console.error('Помилка при завантаженні квитків або події:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchTicketsAndTopEvent();
  }, []);

  return (
    <div className="my-tickets-page">
      <h1 className="page-title">Мої квитки</h1>

      {loading ? (
        <p>Завантаження...</p>
      ) : tickets.length === 0 ? (
        <p>У вас поки немає куплених квитків.</p>
      ) : (
        <div className="tickets-container">
          {tickets.map(ticket => (
  <div key={ticket.ticket_id} className="ticket-card">
    <h2 className="event-title">{ticket.event.title}</h2>
    <p><strong>Стадіон:</strong> {ticket.event.venue.name}</p>
    <p><strong>Місто:</strong> {ticket.event.venue.city}</p>
    <p><strong>Адреса:</strong> {ticket.event.venue.address}</p>
    <p><strong>Дата події:</strong> {new Date(ticket.event.event_date).toLocaleDateString()}</p>
    <p><strong>Сектор:</strong> {ticket.section}</p>
    <p><strong>Місце:</strong> {ticket.seat_number}</p>
    <p><strong>Ціна:</strong> {ticket.price} грн</p>
    <p><strong>Куплено:</strong> {new Date(ticket.purchased_at).toLocaleString()}</p>
  </div>
))}
        </div>
      )}

      {/* Відображення інформації про топову подію */}
      {topEvent && (
        <div className="top-event-container">
          <h2>Топова подія</h2>
          <div className="top-event-card">
            <h3>{topEvent.eventTitle}</h3>
            <p><strong>Дата:</strong> {new Date(topEvent.eventDate).toLocaleString()}</p>
            <p><strong>Стадіон:</strong> {topEvent.venueName}</p>
            <p><strong>Продано квитків:</strong> {topEvent.ticketsSold}</p>
          </div>
        </div>
      )}
    </div>
  );
}
