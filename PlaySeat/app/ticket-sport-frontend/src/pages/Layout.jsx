import { Outlet } from 'react-router-dom';
import Header from './Header';

export default function Layout() {
  return (
    <>
      <Header />
      <div style={{ paddingTop: '70px' }}> {/* Відступ щоб не перекривалось хедером */}
        <Outlet />
      </div>
    </>
  );
}
