import { apiClient } from "./ApiClient";
import { LoginDto, RegisterDto, UserInfoDto } from "../models/UserModels";

export class UserService {

  async login(loginDto: LoginDto): Promise<void> {
    await apiClient.post<LoginDto, void>("user/login", loginDto);
  }

  async register(registerDto: RegisterDto): Promise<void> {
    await apiClient.post<RegisterDto, void>("user/register", registerDto);
  }

  async getUserInfo(): Promise<UserInfoDto> {
    console.log("getUserInfo");
    const response = await apiClient.get<UserInfoDto>("user/info");
    return response.data;
  }

  async logout(): Promise<void> {
    await apiClient.get("user/logout");
  }
}
