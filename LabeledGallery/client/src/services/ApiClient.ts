import axios from "axios";

export const apiClient = axios.create({
  baseURL: "http://localhost:5001/api/v1/",
  headers: {
    "Accept": "application/json, text/plain",
    "Content-Type": "application/json;charset=utf-8",
  },
  withCredentials: true,
});
