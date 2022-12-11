import React from "react";
import LoginPage from "./pages/user/LoginPage";
import RegisterPage from "./pages/user/RegisterPage";
import GalleryPage from "./pages/gallery/GalleryPage";
import { NativeRouter, Route } from "react-router-native";
import { View } from "react-native";

const AppRoutes = () => {
  return (
    <View>
      <NativeRouter>
        <Route exact path='/' component={ LoginPage } />
        <Route exact path='/registerPage' component={ RegisterPage } />
        <Route exact path='/galleryPage' component={ GalleryPage } />
      </NativeRouter>
    </View>
  );
};

export default AppRoutes;
