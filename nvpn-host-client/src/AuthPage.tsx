import { useState } from 'react';
import { useForm } from 'react-hook-form';
import './AuthPage.css';

type FormData = {
    email: string;
    password: string;
    confirmPassword?: string;
};

type ApiResponse = {
    success: boolean;
    data?: string;
    error?: string;
};

export default function AuthPage() {
    const [mode, setMode] = useState<'login' | 'register'>('login');
    const [apiResponse, setApiResponse] = useState<ApiResponse | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    const { register, handleSubmit, watch, formState: { errors } } = useForm<FormData>();

    const testBackendRequest = async () => {
        setIsLoading(true);
        setApiResponse(null);
        
        try {
            // Используем /api префикс для проксирования
            const response = await fetch('/api/Safety/GenerateAdminKey', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            const data = await response.text();
            
            if (response.ok) {
                setApiResponse({
                    success: true,
                    data: data
                });
            } else {
                setApiResponse({
                    success: false,
                    error: data
                });
            }
        } catch (error) {
            setApiResponse({
                success: false,
                error: error instanceof Error ? error.message : 'Неизвестная ошибка'
            });
        } finally {
            setIsLoading(false);
        }
    };

    const onSubmit = async (data: FormData) => {
        console.log('Отправленные данные:', data);
        
        // Пример запроса при авторизации
        try {
            const response = await fetch('/api/Auth/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email: data.email,
                    password: data.password
                })
            });
            
            const result = await response.json();
            console.log('Ответ сервера:', result);
            
            // Обработка ответа...
        } catch (error) {
            console.error('Ошибка авторизации:', error);
        }
    };

    return (
        <div className="auth-container">
            <h2>{mode === 'login' ? 'Вход в аккаунт' : 'Регистрация'}</h2>

            {/* Тестовая кнопка для проверки бэкенда */}
            <div className="test-backend">
                <button 
                    onClick={testBackendRequest}
                    disabled={isLoading}
                >
                    {isLoading ? 'Загрузка...' : 'Тест бэкенда'}
                </button>
                
                {apiResponse && (
                    <div className={`api-response ${apiResponse.success ? 'success' : 'error'}`}>
                        {apiResponse.success ? (
                            <p>Успешно! Код: {apiResponse.data}</p>
                        ) : (
                            <p>Ошибка: {apiResponse.error}</p>
                        )}
                    </div>
                )}
            </div>

            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="form-group">
                    <label>Email</label>
                    <input
                        {...register('email', {
                            required: 'Email обязателен',
                            pattern: {
                                value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                message: 'Некорректный email'
                            }
                        })}
                        type="email"
                    />
                    {errors.email && <span className="error">{errors.email.message}</span>}
                </div>

                <div className="form-group">
                    <label>Пароль</label>
                    <input
                        {...register('password', {
                            required: 'Пароль обязателен',
                            minLength: {
                                value: 6,
                                message: 'Пароль должен быть не менее 6 символов'
                            }
                        })}
                        type="password"
                    />
                    {errors.password && <span className="error">{errors.password.message}</span>}
                </div>

                {mode === 'register' && (
                    <div className="form-group">
                        <label>Подтвердите пароль</label>
                        <input
                            {...register('confirmPassword', {
                                validate: (value) =>
                                    value === watch('password') || 'Пароли не совпадают'
                            })}
                            type="password"
                        />
                        {errors.confirmPassword && (
                            <span className="error">{errors.confirmPassword.message}</span>
                        )}
                    </div>
                )}

                <button type="submit">
                    {mode === 'login' ? 'Войти' : 'Зарегистрироваться'}
                </button>
            </form>

            <div className="toggle-mode">
                {mode === 'login' ? (
                    <span>
                        Нет аккаунта?{' '}
                        <button type="button" onClick={() => setMode('register')}>
                            Зарегистрироваться
                        </button>
                    </span>
                ) : (
                    <span>
                        Уже есть аккаунт?{' '}
                        <button type="button" onClick={() => setMode('login')}>
                            Войти
                        </button>
                    </span>
                )}
            </div>
        </div>
    );
}