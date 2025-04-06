import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()], // ✅ Обязательно должен быть (для обработки React-компонентов)
  base: '/',
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:8080', // Для dev-режима (npm run dev)
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, '/api'),
        secure: false // Если бэкенд без HTTPS
      }
    }
  }});