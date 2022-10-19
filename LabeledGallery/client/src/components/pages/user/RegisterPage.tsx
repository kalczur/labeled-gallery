import React from "react";
import { Button, Text, TextInput, View } from "react-native";
import { Formik } from "formik";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { UserService } from "../../../services/UserService";
import { useAuth } from "../../../hooks/useAuth";
import { RegisterDto } from "../../../models/UserModels";
import { Picker } from "@react-native-picker/picker";

const userService = new UserService();

interface Props {
  navigation: any;
}

const RegisterPage = ({ navigation }: Props) => {
  const { userInfo } = useAuth();

  if (userInfo.isAuthenticated) {
    navigation.navigate("GalleryPage");
  }

  const queryClient = useQueryClient();

  const registerMutation = useMutation(userService.register, {
    onSuccess: async () => {
      await queryClient.invalidateQueries(["register"]);
    },
  });

  return (
    <View>
      <Formik<RegisterDto>
        initialValues={ { name: "", email: "", password: "", objectsDetectionProvider: "Gcp" } }
        onSubmit={ values => registerMutation.mutate(values) }
      >
        { ({ handleChange, handleSubmit, values }) => (
          <View>
            <TextInput
              placeholder='Name'
              onChangeText={ handleChange("name") }
              value={ values.name }
            />

            <TextInput
              placeholder='Email'
              onChangeText={ handleChange("email") }
              value={ values.email }
            />

            <TextInput
              placeholder='Password'
              secureTextEntry={true}
              onChangeText={ handleChange("password") }
              value={ values.password }
            />

            <Picker
              selectedValue='Gcp'
              onValueChange={ () => handleChange("password") }
              mode='dropdown'
            >
              <Picker.Item label='Gcp' value='Gcp' />
            </Picker>

            <Button onPress={ () => handleSubmit() } title='Register' />
          </View>
        ) }
      </Formik>

      <Button
        title='Login'
        onPress={ () => navigation.navigate("LoginPage") }
      />

      { registerMutation.error && <Text>Error</Text> }
      { registerMutation.isLoading && <Text>Loading...</Text> }
    </View>
  );
};

export default RegisterPage;
