import React from "react";
import { Button, Text, TextInput, View } from "react-native";
import { Formik } from "formik";
import { useAuth } from "../../../hooks/useAuth";
import { LoginDto } from "../../../models/UserModels";

interface Props {
  navigation: any;
}

const LoginPage = ({ navigation }: Props) => {
  const { userInfo, login } = useAuth();

  if (userInfo.isAuthenticated) {
    navigation.navigate("GalleryPage");
  }

  return (
    <View>
      <Formik<LoginDto>
        initialValues={ { email: "", password: "" } }
        onSubmit={ values => login(values) }
      >
        { ({ handleChange, handleSubmit, values, isSubmitting, errors }) => (
          <View>
            <TextInput
              placeholder='Email'
              onChangeText={ handleChange("email") }
              value={ values.email }
            />

            <TextInput
              placeholder='Password'
              onChangeText={ handleChange("password") }
              value={ values.password }
            />

            <Button onPress={ () => handleSubmit() } title='Login' />

            { isSubmitting && <Text>Loading...</Text> }
          </View>
        ) }
      </Formik>

      <Button
        title='Register'
        onPress={ () => navigation.navigate("RegisterPage") }
      />

    </View>
  );
};

export default LoginPage;
