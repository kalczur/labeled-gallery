import React, { createContext, useContext, useState } from "react";
import { UserService } from "../services/UserService";
import { LoginDto, UserInfoDto } from "../models/UserModels";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

interface UserContextDto {
  userInfo: UserInfoDto;
  login: (dto: LoginDto) => Promise<void>;
  logout: () => void;
}

const userService = new UserService();

const authContext = createContext<UserContextDto>(undefined);

export function ProvideAuth({ children }) {
  const auth = useProvideAuth();
  if (auth.userInfo) {
    return <authContext.Provider value={ auth }>{ children }</authContext.Provider>;
  } else {
    return <div>Loading...</div>;
  }
}

export const useAuth = () => {
  return useContext(authContext);
};

function useProvideAuth(): UserContextDto {
  const [userInfo, setUserInfo] = useState<UserInfoDto>();
  const queryClient = useQueryClient();

  useQuery(["getUserInfo"], userService.getUserInfo, {
    onSuccess: (x) => {
      setUserInfo(x);
    },
  });

  const reloadMutation = useMutation(userService.getUserInfo, {
    onSuccess: (x) => {
      setUserInfo(x);
    },
  });

  const logoutMutation = useMutation(userService.logout, {
    onSuccess: async () => {
      await queryClient.invalidateQueries(["logout"]);
      reloadMutation.mutate();
    },
  });

  return {
    userInfo,
    login: (dto: LoginDto) => userService.login(dto),
    logout: logoutMutation.mutate,
  };
}
