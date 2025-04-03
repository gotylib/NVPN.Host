import { useState } from 'react';
import { useForm } from 'react-hook-form';
import './AuthPage.css';
type FormData = {
    email: string;
    password: string;
    confirmPassword?: string;
};

export default function AuthPage() {
    const [mode, setMode] = useState<'login' | 'register'>('login');
    const { register, handleSubmit, watch, formState: { errors } } = useForm<FormData>();

    const onSubmit = (data: FormData) => {
        console.log('Отправленные данные:', data);
        // логика авторизации/регистрации
    };

    return (
        <div className="auth-container">
            <h2>{mode === 'login' ? 'Вход в аккаунт' : 'Регистрация'}</h2>

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