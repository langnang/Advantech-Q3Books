import axios from 'axios';

export const all = () => axios.get("/api/book/all");

export const list = job_number => axios.post("/api/book/list", { job_number })

export const add = () => axios.post("/api/book/add");

export const del = () => axios.post("/api/book/delete");