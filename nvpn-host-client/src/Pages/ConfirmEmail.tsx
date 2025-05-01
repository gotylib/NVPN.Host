import React, { useState, useRef, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import './ConfirmEmail.css';

export default function ConfirmEmail() {
    const [code, setCode] = useState<string[]>(['', '', '', '', '', '']);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');
    const inputsRef = useRef<(HTMLInputElement | null)[]>(Array(6).fill(null));
    const location = useLocation();
    const email = location.state?.email || 'your.email@example.com';

    const handleChange = (index: number, value: string) => {
        if (!/^\d*$/.test(value)) return;

        const newCode = [...code];
        newCode[index] = value;
        setCode(newCode);

        if (value && index < 5) {
            inputsRef.current[index + 1]?.focus();
        }
    };

    const handlePaste = (e: React.ClipboardEvent<HTMLFormElement>) => {
        e.preventDefault();
        const pastedData = e.clipboardData.getData('text/plain').trim();

        if (/^\d{6}$/.test(pastedData)) {
            const pastedCode = pastedData.split('');
            setCode(pastedCode);
            inputsRef.current[5]?.focus();
        }
    };

    const handleKeyDown = (index: number, e:React.KeyboardEvent<HTMLInputElement>) => {
        if(e.key === 'Backspace' && !code[index] && index>0){
            inputsRef.current[index -1]?.focus();
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsLoading(true);
        setError('');

        try {
            const verificationCode = code.join('');
            if (verificationCode.length !== 6) {
                throw new Error('Please enter a 6-digit code');
            }
            await new Promise(resolve => setTimeout(resolve, 1500));
            console.log('Verification code:', verificationCode);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Verification failed');
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        inputsRef.current[0]?.focus();
    }, []);

    return (
        <div className="mail-verification-container">
            <div className="mail-verification-header">
                <div className="mail-lock-icon">🔒</div>
                <h2>Проверка безопасности</h2>
                <p className="mail-code-label">📌 Код подтверждения Mail</p>
            </div>

            <div className="mail-email-notice">
                <p>Код отправлен на <strong>{email}</strong></p>
            </div>

            <form onSubmit={handleSubmit} className="mail-verification-form" onPaste={handlePaste}>
                <div className="mail-code-container">
                    {code.map((digit, index) => (
                        <input
                            key={index}
                            ref={(el) => {
                                inputsRef.current[index] = el;
                            }}
                            type="text"
                            maxLength={1}
                            value={digit}
                            onChange={(e) => handleChange(index, e.target.value)}
                            onKeyDown={(e) => handleKeyDown(index, e)}
                            className="mail-code-input"
                            inputMode="numeric"
                        />
                    ))}
                </div>

                {error && <div className="mail-error-message">{error}</div>}

                <button
                    type="submit"
                    className="mail-verify-button"
                    disabled={code.some(d => d === '')}
                >
                    {isLoading ? (
                        <span className="button-loader">
                    <span className="spinner"></span>
                        Проверка...
                    </span>
                            ) : 'Подтвердить'}
                </button>

                <div className="mail-help-section">
                    <p>Возникли проблемы с верификацией?</p>
                    <button type="button" className="mail-help-link">
                        Отправить код повторно
                    </button>
                    <button type="button" className="mail-help-link">
                        Использовать другой способ
                    </button>
                </div>
            </form>
        </div>
    );
}