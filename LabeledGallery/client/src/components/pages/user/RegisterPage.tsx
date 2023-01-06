import React from "react";
import { Button, Dimensions, StatusBar, StyleSheet, Text, TextInput, View } from "react-native";
import { Redirect, useHistory } from "react-router-native";
import { Formik } from "formik";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { UserService } from "../../../services/UserService";
import { useAuth } from "../../../hooks/useAuth";
import { RegisterDto } from "../../../models/UserModels";
import { Picker } from "@react-native-picker/picker";

const userService = new UserService();

const RegisterPage = () => {
  const { userInfo } = useAuth();
  const history = useHistory();
  const queryClient = useQueryClient();

  const registerMutation = useMutation(userService.register, {
    onSuccess: async () => {
      await queryClient.invalidateQueries(["register"]);
    },
  });

  if (userInfo.isAuthenticated) {
    return <Redirect to='/galleryPage' />;
  }

  return (
    <View style={ styles.container }>
      <Formik<RegisterDto>
        initialValues={ { name: "", email: "", password: "", objectsDetectionProvider: "Gcp" } }
        onSubmit={ values => registerMutation.mutate(values) }
      >
        { ({ handleChange, handleSubmit, values }) => (
          <View>
            <TextInput
              style={ styles.input }
              placeholder='Name'
              onChangeText={ handleChange("name") }
              value={ values.name }
            />

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

            <Picker
              style={ styles.input }
              selectedValue='Gcp'
              onValueChange={ () => handleChange("password") }
              mode='dropdown'
            >
              <Picker.Item label='Gcp' value='Gcp' />
            </Picker>

            <View style={ styles.button }>
              <Button color={ buttonPrimaryColor } onPress={ () => handleSubmit() } title='Register' />
            </View>
          </View>
        ) }
      </Formik>

      <View style={ styles.button }>
        <Button
          color={ buttonSecondaryColor }
          title='Login'
          onPress={ () => history.push("/") }
        />
      </View>

      { registerMutation.error && <Text>Error</Text> }
      { registerMutation.isLoading && <Text>Loading...</Text> }
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
    backgroundColor: "#eee",
    marginTop: 5,
    padding: 4,
  },
  button: {
    marginTop: 10,
  },
});

export default RegisterPage;
