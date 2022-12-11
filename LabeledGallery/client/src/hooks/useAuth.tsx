import React, { createContext, useCallback, useContext, useEffect, useState } from "react";
import { UserService } from "../services/UserService";
import { LoginDto, UserInfoDto } from "../models/UserModels";
import { Text } from "react-native";
import { useHistory } from "react-router-native";

interface UserContextDto {
  userInfo: UserInfoDto;
  login: (dto: LoginDto) => Promise<void>;
  logout: () => void;
}

const userService = new UserService();

const authContext = createContext<UserContextDto>(undefined);

export const ProvideAuth = ({ children }) => {
  const auth = useProvideAuth();

  if (auth.userInfo) {
    return <authContext.Provider value={ auth }>{ children }</authContext.Provider>;
  } else {
    return <Text>The server is not responding. Try again later.</Text>;
  }
};

export const useAuth = () => {
  return useContext(authContext);
};

const useProvideAuth = (): UserContextDto => {
  const [userInfo, setUserInfo] = useState<UserInfoDto>();
  const history = useHistory();

  const reloadUserInfo = useCallback(async () => {
    const adminInfo = await userService.getUserInfo();
    setUserInfo(adminInfo);
  }, [userService, setUserInfo]);

  const logout = async () => {
    await userService.logout();
    await reloadUserInfo();
    history.push("/");
  };

  const login = async (dto: LoginDto) => {
    await userService.login(dto);
    await reloadUserInfo();
  };

  useEffect(() => {
    reloadUserInfo();
  }, [reloadUserInfo]);

  return {
    userInfo,
    login,
    logout,
  };
};
