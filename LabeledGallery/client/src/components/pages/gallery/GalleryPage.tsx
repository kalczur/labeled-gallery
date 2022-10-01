import React from "react";
import { useAuth } from "../../../hooks/useAuth";
import { Button, Text, View } from "react-native";

interface Props {
  navigation: any;
}

const GalleryPage = ({ navigation }: Props) => {
  const { userInfo, logout } = useAuth();

  if (!userInfo.isAuthenticated) {
    navigation.navigate("LoginPage");
  }

  return (
    <View>
      <Button onPress={ logout } title='Logout' />
      <Text>Name: { userInfo.account.name }</Text>
      <Text>Email: { userInfo.account.email }</Text>
    </View>
  );
};

export default GalleryPage;
