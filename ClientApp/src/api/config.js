import axios from 'axios';

export const all = () => axios.get("/api/config")