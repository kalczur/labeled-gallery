﻿import React from "react";
import { Button, Dimensions, StatusBar, StyleSheet, Text, TextInput, View } from "react-native";
import { Redirect, useHistory } from "react-router-native";
import { Formik } from "formik";
import { useAuth } from "../../../hooks/useAuth";
import { LoginDto } from "../../../models/UserModels";

const LoginPage = () => {
  const { userInfo, login } = useAuth();
  const history = useHistory();

  if (userInfo.isAuthenticated) {
    return <Redirect to='/galleryPage' />;
  }

  return (
    <View style={ styles.container }>
      <Formik<LoginDto>
        initialValues={ { email: "", password: "" } }
        onSubmit={ values => login(values) }
      >
        { ({ handleChange, handleSubmit, values, isSubmitting, errors }) => (
          <View>
            <TextInput
              style={ styles.input }
              placeholder='Email'
              onChangeText={ handleChange("email") }
              value={ values.email }
            />

            <TextInput
              style={ styles.input }
              placeholder='Password'
              secureTextEntry={ true }
              onChangeText={ handleChange("password") }
              value={ values.password }
            />

            <View style={ styles.button }>
              <Button color={ buttonPrimaryColor } onPress={ () => handleSubmit() } title='Login' />
            </View>

            { isSubmitting && <Text>Loading...</Text> }
          </View>
        ) }
      </Formik>

      <View style={ styles.button }>
        <Button
          color={ buttonSecondaryColor }
          title='Register'
          onPress={ () => history.push("/registerPage") }
        />
      </View>

    </View>
  );
};

const buttonPrimaryColor = "#36b54e";
const buttonSecondaryColor = "#3789c4";

const styles = StyleSheet.create({
  container: {
    marginTop: StatusBar.currentHeight,
    padding: 4,
    backgroundColor: "#090326",
    color: "#fff",
    height: Dimensions.get("window").height - StatusBar.currentHeight,
  },
  input: {
    backgroundColor: "#ccc",
    marginTop: 5,
    padding: 4,
  },
  button: {
    marginTop: 10,
  },
});

export default LoginPage;
