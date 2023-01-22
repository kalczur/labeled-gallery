import React, { useState } from "react";
import { Button, Dimensions, StatusBar, StyleSheet, Text, TextInput, View } from "react-native";
import { Redirect, useHistory } from "react-router-native";
import { Formik } from "formik";
import { useAuth } from "../../../hooks/useAuth";
import { LoginDto } from "../../../models/UserModels";
import { buttonColorPrimary, buttonColorSecondary } from "../../../styles/colors";

const LoginPage = () => {
  const { userInfo, login } = useAuth();
  const [loginError, setLoginError] = useState(false);
  const history = useHistory();

  if (userInfo.isAuthenticated) {
    return <Redirect to='/galleryPage' />;
  }

  const onLogin = async (values: LoginDto) => {
    setLoginError(false);

    try {
      await login(values);
    } catch (_) {
      setLoginError(true);
    }
  };

  return (
    <View style={ styles.container }>
      <Formik<LoginDto>
        initialValues={ { email: "", password: "" } }
        onSubmit={ onLogin }
      >
        { ({ handleChange, handleSubmit, values, isSubmitting }) => (
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
              <Button color={ buttonColorPrimary } onPress={ () => handleSubmit() } title='Login' />
            </View>

            { isSubmitting && <Text style={ styles.text }>Loading...</Text> }
            { loginError && <Text style={ styles.textError }>Incorrect email or password.</Text> }
          </View>
        ) }
      </Formik>

      <View style={ styles.button }>
        <Button
          color={ buttonColorSecondary }
          title='Register'
          onPress={ () => history.push("/registerPage") }
        />
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    marginTop: StatusBar.currentHeight,
    padding: 4,
    backgroundColor: "#090326",
    color: "#fff",
    height: Dimensions.get("window").height - StatusBar.currentHeight,
  },
  input: {
    backgroundColor: "#eee",
    marginTop: 5,
    padding: 4,
  },
  button: {
    marginTop: 10,
  },
  text: {
    color: "#ccc",
  },
  textError: {
    color: "#c00",
  },
});

export default LoginPage;
