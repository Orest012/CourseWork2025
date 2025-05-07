import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import '../styles/eventDatail.css';

export default function EventDetails() {
  const { id } = useParams();
  const [event, setEvent] = useState(null);
  const [tickets, setTickets] = useState([]);
  const [error, setError] = useState('');
  const [ticketMessage, setTicketMessage] = useState('');
  const [selectedSeat, setSelectedSeat] = useState(null);
  const userId = 100;

  useEffect(() => {
    const fetchEventData = async () => {
      const token = localStorage.getItem('token');
      try {
        const [eventRes, ticketsRes] = await Promise.all([
          axios.get(`https://localhost:7178/api/Event/GetEvent/${id}`, {
            headers: { Authorization: `Bearer ${token}` },
          }),
          axios.get(`https://localhost:7178/api/Tickets/event/${id}`, {
            headers: { Authorization: `Bearer ${token}` },
          }),
        ]);

        setEvent(eventRes.data);
        setTickets(ticketsRes.data);
      } catch (err) {
        setError('Не вдалося завантажити деталі події');
        console.error(err);
      }
    };

    fetchEventData();
  }, [id]);

  const availableTickets = tickets.filter(ticket => !ticket.isSold);
  const hasAvailableTickets = availableTickets.length > 0;

  const handleBuyTicket = async () => {
    if (!selectedSeat) {
      setTicketMessage('Будь ласка, виберіть місце.');
      return;
    }

    const token = localStorage.getItem('token');
    setTicketMessage('');

    try {
      await axios.post(
        `https://localhost:7178/api/Tickets/buy/${selectedSeat.ticketId}/user/${userId}`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      );

      setTicketMessage(`Квиток на місце №${selectedSeat.seatNumber} успішно придбано 🎉`);
    } catch (err) {
      setTicketMessage('Помилка під час купівлі квитка');
      console.error(err);
    }
  };

  if (error) return <div className="event-error">{error}</div>;
  if (!event) return <div className="event-loading">Завантаження...</div>;

  return (
    <div className="event-container">
      <div className="event-header">
        <h1>{event.title}</h1>
        <p className="event-description">{event.description}</p>

        {event.imageUrl && (
          <div className="event-image">
            <img src={event.imageUrl} alt="Місце події" className="venue-image" />
          </div>
        )}
      </div>

      <div className="event-details">
        <div className="event-info">
          <p><strong>Дата:</strong> {new Date(event.eventDate).toLocaleDateString()}</p>
          <p><strong>Вид спорту:</strong> {event.sportType || 'Не вказано'}</p>
        </div>

        <div className="event-venue">
          <h3>Місце проведення</h3>
          <p><strong>{event.venueName}</strong></p>
          <p>{event.venueAddress}, {event.venueCity}</p>
          <p><strong>Вмістимість:</strong> {event.venueCapacity}</p>
        </div>

        <div className="event-organizer">
          <h3>Організатор</h3>
          <p><strong>{event.organizerName}</strong></p>
          <p>{event.organizerContact}</p>
        </div>

        <div className="ticket-info">
          <h3>Квитки</h3>
          <p><strong>Усього квитків:</strong> {event.totalTickets}</p>
          <p><strong>Продано квитків:</strong> {event.ticketsSold}</p>
          <p><strong>Середня ціна:</strong> {event.averagePrice ? `${event.averagePrice} грн` : 'Н/Д'}</p>
        </div>

        <div className="event-ticket">
          {hasAvailableTickets ? (
            <>
              <button onClick={handleBuyTicket} className="ticket-button">
                Придбати квиток
              </button>
              {ticketMessage && <p className="ticket-message">{ticketMessage}</p>}
            </>
          ) : (
            <p className="ticket-message">На жаль, усі квитки розпродано 😞</p>
          )}
        </div>
      </div>

      {/* Вибір місця лише якщо квитки є */}
      {hasAvailableTickets && (
        <div className="seat-selection">
          <h3>Вибір місць</h3>
          <p>Оберіть доступне місце</p>
          <div className="seat-map">
            {availableTickets.map((ticket) => (
              <button
                key={ticket.ticketId}
                className={`seat-button ${selectedSeat?.ticketId === ticket.ticketId ? 'selected' : ''}`}
                onClick={() => setSelectedSeat(ticket)}
              >
                Місце №{ticket.seatNumber}
              </button>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
