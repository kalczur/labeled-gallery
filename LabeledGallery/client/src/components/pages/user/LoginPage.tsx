import React from "react";
import { Button, TextInput, View } from "react-native";
import { Formik } from "formik";
import { UserService } from "../../../services/UserService";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useAuth } from "../../../hooks/useAuth";
import { LoginDto } from "../../../models/UserModels";

const userService = new UserService();

interface Props {
  navigation: any;
}

const LoginPage = ({ navigation }: Props) => {
  const { userInfo } = useAuth();

  if (userInfo.isAuthenticated) {
    navigation.navigate("GalleryPage");
  }

  const queryClient = useQueryClient();

  const loginMutation = useMutation(userService.login, {
    onSuccess: async () => {
      await queryClient.invalidateQueries(["login"]);
    },
  });

  return (
    <View>
      <Formik<LoginDto>
        initialValues={ { email: "", password: "" } }
        onSubmit={ values => loginMutation.mutate(values) }
      >
        { ({ handleChange, handleSubmit, values }) => (
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
          </View>
        ) }
      </Formik>

      <Button
        title='Register'
        onPress={ () => navigation.navigate("RegisterPage") }
      />

      { loginMutation.error && <div>Error</div> }
      { loginMutation.isLoading && <div>Loading...</div> }
    </View>
  );
};

export default LoginPage;
