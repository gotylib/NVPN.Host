import { Routes, Route } from 'react-router-dom';
import AuthPage from './Pages/AuthPage.tsx';


export default function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<AuthPage />} />
            <Route path="/auth" element={<AuthPage />} />
        </Routes>
    );
}