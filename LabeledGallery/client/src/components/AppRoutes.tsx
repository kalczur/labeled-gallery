import React from "react";
import LoginPage from "./pages/user/LoginPage";
import RegisterPage from "./pages/user/RegisterPage";
import { NavigationContainer } from "@react-navigation/native";
import { createNativeStackNavigator } from "@react-navigation/native-stack";
import GalleryPage from "./pages/gallery/GalleryPage";

const Stack = createNativeStackNavigator();

const AppRoutes = () => {
  return (
    <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen name='LoginPage' component={ LoginPage } />
        <Stack.Screen name='RegisterPage' component={ RegisterPage } />
        <Stack.Screen name='GalleryPage' component={ GalleryPage } />
      </Stack.Navigator>
    </NavigationContainer>
  );
};

export default AppRoutes;
