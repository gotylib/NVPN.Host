import { Routes, Route } from 'react-router-dom';
import AuthPage from './Pages/AuthPage.tsx';
import ConfirmEmail from './Pages/ConfirmEmail.tsx';

export default function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<AuthPage />} />
            <Route path="/auth" element={<AuthPage />} />
            <Route path="/confirm-email" element={<ConfirmEmail />} />
        </Routes>
    );
}