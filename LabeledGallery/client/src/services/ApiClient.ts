import axios from "axios";

export const apiClient = axios.create({
  baseURL: "http://10.0.2.2:5001/api/v1/",
  headers: {
    "Accept": "application/json, text/plain, multipart/form-data",
    "Content-Type": "application/json;charset=utf-8",
  },
  withCredentials: true,
});
