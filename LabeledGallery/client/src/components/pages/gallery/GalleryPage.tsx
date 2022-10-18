import React, { useState } from "react";
import { useAuth } from "../../../hooks/useAuth";
import { Button, Image, ScrollView, Text, TextInput, View } from "react-native";
import * as MediaLibrary from "expo-media-library";
import { GalleryService } from "../../../services/GalleryService";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { GalleryResponseDto, GetGalleryRequestDto } from "../../../models/GalleryModels";
import { Formik } from "formik";

const galleryService = new GalleryService();

interface Props {
  navigation: any;
}

const GalleryPage = ({ navigation }: Props) => {
  const { userInfo, logout } = useAuth();
  const [_, requestPermission] = MediaLibrary.usePermissions();
  const [galleryData, setGalleryData] = useState<GalleryResponseDto>();

  if (!userInfo.isAuthenticated) {
    navigation.navigate("LoginPage");
  }

  const {
    isLoading,
    error,
  } = useQuery<GalleryResponseDto>(["getGallery"], () => galleryService.get({ searchKeyword: "" }), {
    onSuccess: async (data: GalleryResponseDto) => {
      setGalleryData(data);
    },
  });

  if (isLoading) return <View><Text>Loading gallery...</Text></View>;
  if (error) return <View><Text>Failed to load gallery</Text></View>;

  const queryClient = useQueryClient();

  const getGalleryMutation = useMutation(galleryService.get, {
    onSuccess: async (data: GalleryResponseDto) => {
      setGalleryData(data);
      await queryClient.invalidateQueries(["get"]);
    },
  });

  const updateGalleryMutation = useMutation(galleryService.update, {
    onSuccess: async () => {
      await queryClient.invalidateQueries(["updateGallery"]);
    },
  });

  const sendAlbumPhotos = async () => {
    await requestPermission();

    const album = await MediaLibrary.getAlbumAsync("Camera");
    const albumAsset = await MediaLibrary.getAssetsAsync({ album: album });

    await updateGalleryMutation.mutate({ imagesToAdd: albumAsset.assets.map(x => x.uri) });
  };

  return (
    <ScrollView>
      <Button onPress={ logout } title='Logout' />
      <Button onPress={ sendAlbumPhotos } title='Send photos from album' />

      <Formik<GetGalleryRequestDto>
        initialValues={ { searchKeyword: "" } }
        onSubmit={ values => getGalleryMutation.mutate(values) }
      >
        { ({ handleChange, handleSubmit, values }) => (
          <View style={ { display: "flex" } }>
            <TextInput
              placeholder=''
              onChangeText={ handleChange("searchKeyword") }
              value={ values.searchKeyword }
            />
            <Button title='Search' onPress={ () => handleSubmit() } />
          </View>
        ) }
      </Formik>

      { updateGalleryMutation.isLoading && <Text>Sending photos...</Text> }
      { updateGalleryMutation.error && <Text>Fail to send</Text> }

      { updateGalleryMutation.isLoading && <Text>Loading gallery...</Text> }

      <Text>Name: { userInfo.account.name }</Text>
      <Text>Email: { userInfo.account.email }</Text>

      <View>
        { galleryData && galleryData.galleryItems && galleryData.galleryItems.map(x =>
          <View key={ x.name } style={ { marginTop: 20 } }>
            <Text>{ x.name }</Text>
            <Image
              source={ { uri: x.image } }
              style={ { width: 100, height: 100 } } />

            <Text>{ x.detectedObjects.map((objects) => <Text>{ objects.label }, </Text>) }</Text>
          </View>,
        ) }
      </View>

    </ScrollView>
  );
};

export default GalleryPage;
