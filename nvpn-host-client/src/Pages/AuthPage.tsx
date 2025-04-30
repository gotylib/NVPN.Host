import { useState } from 'react';
import { useForm } from 'react-hook-form';
import './AuthPage.css';

type FormData = {
    name?: string;
    email: string;
    password: string;
    confirmPassword?: string;
    useGoogleAuth: boolean;
};

export default function AuthPage() {
    const [mode, setMode] = useState<'login' | 'register'>('login');
    const [isLoading, setIsLoading] = useState(false);
    const {
        register,
        handleSubmit,
        watch,
        formState: { errors }
    } = useForm<FormData>({
        defaultValues: {
            useGoogleAuth: false
        }
    });

    const onSubmit = async (data: FormData) => {
        setIsLoading(true);
        try {
            const endpoint = mode === 'login' ? '/api/Auth/Login' : '/api/Auth/Register';
            const requestData = mode === 'login'
                ? {
                    email: data.email,
                    password: data.password
                }
                : {
                    name: data.name,
                    email: data.email,
                    password: data.password,
                    useGoogleAuth: data.useGoogleAuth
                };
            await new Promise(resolve => setTimeout(resolve, 2000));
            const response = await fetch(endpoint, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(requestData)
            });

            const result = await response.json();

            if (!response.ok) {
                throw new Error(result.message || 'Authentication failed');
            }

            // Handle successful authentication
        } catch (error) {
            console.error('Authentication error:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleGoogleLogin = () => {
        window.location.href = '/api/Auth/Google';
    };

    return (
        <div className="auth-container">
            <div className="auth-header">
                <h2>{mode === 'login' ? 'Welcome Back' : 'Create Account'}</h2>
                <p>{mode === 'login' ? 'Log in to your account' : 'Register to get started'}</p>
            </div>

            <form onSubmit={handleSubmit(onSubmit)} className="auth-form">
                {/* Поле Name - теперь в самом верху и только для регистрации */}
                {mode === 'register' && (
                    <div className="form-group">
                        <label>Full Name</label>
                        <input
                            {...register('name', {
                                required: 'Name is required',
                                minLength: {
                                    value: 5,
                                    message: 'Name must be at least 5 characters'
                                }
                            })}
                            type="text"
                            placeholder="Enter your full name"
                            className={`form-input ${errors.name ? 'error' : ''}`}
                        />
                        {errors.name && <span className="error-message">{errors.name.message}</span>}
                    </div>
                )}

                {/* Остальные поля */}
                <div className="form-group">
                    <label>Email Address</label>
                    <input
                        {...register('email', {
                            required: 'Email is required',
                            pattern: {
                                value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                message: 'Invalid email address'
                            }
                        })}
                        type="email"
                        placeholder="Enter your email"
                        className={`form-input ${errors.email ? 'error' : ''}`}
                    />
                    {errors.email && <span className="error-message">{errors.email.message}</span>}
                </div>

                <div className="form-group">
                    <label>Password</label>
                    <input
                        {...register('password', {
                            required: 'Password is required',
                            minLength: {
                                value: 6,
                                message: 'Password must be at least 6 characters'
                            }
                        })}
                        type="password"
                        placeholder="Enter your password"
                        className={`form-input ${errors.password ? 'error' : ''}`}
                    />
                    {errors.password && <span className="error-message">{errors.password.message}</span>}
                </div>

                {mode === 'register' && (
                    <>
                        <div className="form-group">
                            <label>Confirm Password</label>
                            <input
                                {...register('confirmPassword', {
                                    validate: (value) =>
                                        value === watch('password') || 'Passwords do not match'
                                })}
                                type="password"
                                placeholder="Confirm your password"
                                className={`form-input ${errors.confirmPassword ? 'error' : ''}`}
                            />
                            {errors.confirmPassword && (
                                <span className="error-message">{errors.confirmPassword.message}</span>
                            )}
                        </div>

                        <div className="form-checkbox">
                            <input
                                type="checkbox"
                                id="useGoogleAuth"
                                {...register('useGoogleAuth')}
                            />
                            <label htmlFor="useGoogleAuth">Enable Google Authenticator</label>
                        </div>
                    </>
                )}

                <button
                    type="submit"
                    className="auth-button"
                    disabled={isLoading}
                >
                    {isLoading ? (
                        <span className="button-loader">
              <span className="spinner"></span>
              Processing...
            </span>
                    ) : mode === 'login' ? 'Sign In' : 'Sign Up'}
                </button>

                <div className="auth-divider">
                    <span>or</span>
                </div>

                <button
                    type="button"
                    onClick={handleGoogleLogin}
                    className="google-button"
                    disabled={isLoading}
                >
                    <svg className="google-icon" viewBox="0 0 24 24">
                        <path d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z" fill="#4285F4"/>
                        <path d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z" fill="#34A853"/>
                        <path d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z" fill="#FBBC05"/>
                        <path d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z" fill="#EA4335"/>
                    </svg>
                    Continue with Google
                </button>
            </form>

            <div className="auth-footer">
                {mode === 'login' ? (
                    <p>
                        Don't have an account?{' '}
                        <button
                            type="button"
                            onClick={() => setMode('register')}
                            disabled={isLoading}
                        >
                            Sign Up
                        </button>
                    </p>
                ) : (
                    <p>
                        Already have an account?{' '}
                        <button
                            type="button"
                            onClick={() => setMode('login')}
                            disabled={isLoading}
                        >
                            Sign In
                        </button>
                    </p>
                )}
            </div>
        </div>
    );
}

