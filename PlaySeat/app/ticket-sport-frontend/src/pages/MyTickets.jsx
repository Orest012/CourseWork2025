import { useEffect, useState } from 'react';
import axios from 'axios';
import '../styles/myTickets.css';

export default function MyTickets() {
  const [tickets, setTickets] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchTickets = async () => {
      const token = localStorage.getItem('token');
      try {
        const response = await axios.get('https://localhost:7178/api/Tickets/MyTickets', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setTickets(response.data);
      } catch (error) {
        console.error('Помилка при завантаженні квитків:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchTickets();
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
            <div key={ticket.ticketId} className="ticket-card">
              <h2 className="event-title">{ticket.eventTitle}</h2>
              <p><strong>Стадіон:</strong> {ticket.venue}</p>
              <p><strong>Дата події:</strong> {new Date(ticket.eventDate).toLocaleDateString()}</p>
              <p><strong>Сектор:</strong> {ticket.section}</p>
              <p><strong>Місце:</strong> {ticket.seatNumber}</p>
              <p><strong>Ціна:</strong> {ticket.price} грн</p>
              <p><strong>Куплено:</strong> {new Date(ticket.purchasedAt).toLocaleString()}</p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
