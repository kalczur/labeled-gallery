import axios from "axios";

export const apiClient = axios.create({
  baseURL: "https://localhost:5000/api/v1/",
  headers: {
    "Accept": "application/json, text/plain",
    "Content-Type": "application/json;charset=utf-8",
  },
});
