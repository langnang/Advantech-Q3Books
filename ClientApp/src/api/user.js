import axios from 'axios';

export const login = job_number => axios.post("/api/user/login", { job_number })

export const all = () => axios.get("/api/user/all");

export const info = () => axios.get("/api/user/info");