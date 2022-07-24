import React from "react";
import { useAuth } from "../../../hooks/useAuth";

interface Props {
  navigation: any;
}

const GalleryPage = ({ navigation }: Props) => {
  const { userInfo } = useAuth();

  if (!userInfo.isAuthenticated) {
    navigation.navigate("LoginPage");
  }
  
  return (
    <div>
      <div>Name: { userInfo.account.name }</div>
      <div>Email: { userInfo.account.email }</div>
    </div>
  );
};

export default GalleryPage;
