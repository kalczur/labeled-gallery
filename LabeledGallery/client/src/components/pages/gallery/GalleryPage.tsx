import React from "react";
import { useAuth } from "../../../hooks/useAuth";
import { Button, Text, View } from "react-native";
import * as MediaLibrary from "expo-media-library";
import { GalleryService } from "../../../services/GalleryService";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { GalleryResponseDto } from "../../../models/GalleryModels";

const galleryService = new GalleryService();

interface Props {
  navigation: any;
}

const GalleryPage = ({ navigation }: Props) => {
  const { userInfo, logout } = useAuth();
  const [_, requestPermission] = MediaLibrary.usePermissions();

  if (!userInfo.isAuthenticated) {
    navigation.navigate("LoginPage");
  }

  const { data: galleryData, isLoading, error } = useQuery<GalleryResponseDto>(["getGallery"], galleryService.get);

  if (isLoading) return <View><Text>Loading...</Text></View>;
  if (error) return <View><Text>Error</Text></View>;

  const queryClient = useQueryClient();
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
    <View>
      <Button onPress={ logout } title='Logout' />
      <Button onPress={ sendAlbumPhotos } title='Send photos from album' />

      { updateGalleryMutation.isLoading && <Text>Sending photos...</Text> }
      { updateGalleryMutation.error && <Text>Fail to send</Text> }

      <Text>Name: { userInfo.account.name }</Text>
      <Text>Email: { userInfo.account.email }</Text>

      <View>
        { galleryData.galleryItems.map(x =>
          <View key={ x.id }>
            <Text>{ x.name }: { x.totalAccuracy }</Text>
          </View>,
        ) }
      </View>

    </View>
  );
};

const getFile = async (path: string) => {
  const result = await fetch(path);
  return result.blob();
};

export default GalleryPage;
