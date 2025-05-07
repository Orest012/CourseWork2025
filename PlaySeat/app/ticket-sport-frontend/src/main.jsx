import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import Login from './pages/Login';
import Register from './pages/Register';
import EventsList from './pages/EventsList';
import EventDetails from './pages/EventDetails';
import Layout from '../src/pages/Layout'; // Імпортуємо Layout
import CreateEventPage from './pages/CreateEventPage';
import MyTickets from './pages/MyTickets';
import UserInfo from './pages/UserInfo';
import './index.css'; // Шлях краще писати відносно просто

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        <Route path="/" element={<Layout />}>
          <Route index element={<EventsList />} /> {/* Головна сторінка */}
          <Route path="eventsList" element={<EventsList />} />
          <Route path="events/:id" element={<EventDetails />} />
          <Route path="/create-event" element={<CreateEventPage />} />
          <Route path="/tickets" element={<MyTickets />} />
          <Route path="user-info/:id" element={<UserInfo />} />
        </Route>
      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);
