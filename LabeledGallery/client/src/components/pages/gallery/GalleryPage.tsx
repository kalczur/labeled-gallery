import React, { useState } from "react";
import { useAuth } from "../../../hooks/useAuth";
import { Button, ScrollView, Text, TextInput, View, StatusBar, Dimensions, StyleSheet } from "react-native";
import { Redirect } from "react-router-native";
import * as MediaLibrary from "expo-media-library";
import { GalleryService } from "../../../services/GalleryService";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { GalleryResponseDto, GetGalleryRequestDto } from "../../../models/GalleryModels";
import { Formik } from "formik";
import ImageGallery from "./ImageGallery";

const galleryService = new GalleryService();

const GalleryPage = () => {
  const { userInfo, logout } = useAuth();
  const [_, requestPermission] = MediaLibrary.usePermissions();
  const [galleryData, setGalleryData] = useState<GalleryResponseDto>();

  const {
    isLoading,
    error,
  } = useQuery<GalleryResponseDto>(["getGallery"], () => galleryService.get({ searchKeyword: "" }), {
    onSuccess: async (data: GalleryResponseDto) => {
      setGalleryData(data);
    },
  });

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

  if (isLoading) return <View><Text>Loading gallery...</Text></View>;
  if (error) return <View><Text>Failed to load gallery</Text></View>;

  if (!userInfo.isAuthenticated) {
    return <Redirect to='/' />;
  }

  return (
    <ScrollView style={ styles.container }>
      <View style={ styles.topBar }>
        <View style={ { width: (Dimensions.get("window").width / 5) } }>
          <Button color={ buttonColor } onPress={ logout } title='Logout' />
        </View>
        <View style={ { width: (Dimensions.get("window").width / 2) } }>
          <Button color={ buttonColor } onPress={ sendAlbumPhotos } title='Send photos from album' />
        </View>
      </View>

      <Formik<GetGalleryRequestDto>
        initialValues={ { searchKeyword: "" } }
        onSubmit={ values => getGalleryMutation.mutate(values) }
      >
        { ({ handleChange, handleSubmit, values }) => (
          <View style={ styles.searchForm }>
            <TextInput
              style={ styles.input }
              placeholder='Search...'
              onChangeText={ handleChange("searchKeyword") }
              value={ values.searchKeyword }
            />
            <View style={ { width: "20%" } }>
              <Button color={ buttonColor } title='Search' onPress={ () => handleSubmit() } />
            </View>
          </View>
        ) }
      </Formik>

      { updateGalleryMutation.isLoading && <Text>Sending photos...</Text> }
      { updateGalleryMutation.error && <Text>Fail to send</Text> }

      { getGalleryMutation.isLoading && <Text style={ { color: "#ccc" } }>Loading gallery...</Text> }

      { galleryData && (
        <View style={ { marginTop: 20 } }>
          <ImageGallery galleryItems={ galleryData.galleryItems } />
        </View>
      ) }

    </ScrollView>
  );
};

const buttonColor = "#3789c4";

const styles = StyleSheet.create({
  container: {
    marginTop: StatusBar.currentHeight,
    padding: 4,
    backgroundColor: "#07001f",
    color: "#fff",
  },
  topBar: {
    flexDirection: "row",
    justifyContent: "space-between",
  },
  searchForm: {
    marginTop: 10,
    flexDirection: "row",
    justifyContent: "space-between",
  },
  input: {
    width: "79%",
    backgroundColor: "#c5d2e8",
    color: "#000",
    padding: 4,
  },
});

export default GalleryPage;
