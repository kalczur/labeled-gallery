import React, { useState } from "react";
import { useAuth } from "../../../hooks/useAuth";
import { Button, ScrollView, Text, TextInput, View, StatusBar, Dimensions, StyleSheet } from "react-native";
import { Redirect } from "react-router-native";
import * as MediaLibrary from "expo-media-library";
import { GalleryService } from "../../../services/GalleryService";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { DetectedObject, GalleryResponseDto, GetGalleryRequestDto } from "../../../models/GalleryModels";
import { Formik } from "formik";
import ImageGallery from "./ImageGallery";
import { buttonColorDisabled, buttonColorSecondary } from "../../../styles/colors";

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
      getGalleryMutation.mutate({ searchKeyword: "" });
    },
  });

  const sendAlbumPhotos = async () => {
    await requestPermission();

    const album = await MediaLibrary.getAlbumAsync("Camera");
    const albumAsset = await MediaLibrary.getAssetsAsync({ album: album });

    // TODO store last photo date and get last 20

    await updateGalleryMutation.mutate({ imagesToAdd: albumAsset.assets.map(x => x.uri) });
  };

  const optimisticUpdate = (id: string, detectedObjects: DetectedObject[]) => {
    setGalleryData(prevState => {
      const newState = { ...prevState };
      newState.galleryItems.find(x => x.id).detectedObjects = detectedObjects;
      return newState;
    });
  };

  if (isLoading) return <View style={ styles.container }><Text style={ styles.text }>Loading gallery...</Text></View>;
  if (error) return <View style={ styles.container }><Text style={ styles.textError }>Failed to load
    gallery.</Text></View>;

  if (!userInfo.isAuthenticated) {
    return <Redirect to='/' />;
  }

  return (
    <ScrollView style={ styles.container }>
      <View style={ styles.topBar }>
        <View style={ { width: (Dimensions.get("window").width / 5) } }>
          <Button color={ buttonColorSecondary } onPress={ logout } title='Logout' />
        </View>
        <View style={ { width: (Dimensions.get("window").width / 2) } }>
          <Button
            color={ updateGalleryMutation.isLoading ? buttonColorDisabled : buttonColorSecondary }
            onPress={ sendAlbumPhotos } title='Send photos from album'
            disabled={ updateGalleryMutation.isLoading || getGalleryMutation.isLoading }
          />
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
              <Button
                color={ buttonColorSecondary }
                title='Search'
                onPress={ () => handleSubmit() }
                disabled={ updateGalleryMutation.isLoading || getGalleryMutation.isLoading }
              />
            </View>
          </View>
        ) }
      </Formik>

      { updateGalleryMutation.isLoading && <Text style={ styles.text }>Sending photos...</Text> }
      { updateGalleryMutation.error && <Text style={ styles.textError }>Fail to send</Text> }

      { getGalleryMutation.isLoading && <Text style={ styles.text }>Loading gallery...</Text> }

      { galleryData && (
        <View style={ { marginTop: 20 } }>
          <ImageGallery galleryItems={ galleryData.galleryItems } optimisticUpdate={ optimisticUpdate } />
        </View>
      ) }

    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    marginTop: StatusBar.currentHeight,
    height: Dimensions.get("window").height - StatusBar.currentHeight,
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
  text: {
    color: "#ccc",
  },
  textError: {
    color: "#c00",
  },
});

export default GalleryPage;
