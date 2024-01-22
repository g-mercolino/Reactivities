import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vitejs.dev/config/
export default defineConfig({
  //avviamo l'app sulla porta 3000
  server: {
    port: 3000
  },
  plugins: [react()],
})
