import React, { useState } from "react";
import { Dimensions, Modal, StyleSheet, TouchableWithoutFeedback, View, Text } from "react-native";
import { GalleryItemResponseDto } from "../../../models/GalleryModels";
import ImageElement from "./ImageElement";

interface Props {
  galleryItems: GalleryItemResponseDto[];
}

const ImageGallery = ({ galleryItems }: Props) => {
  if (!galleryItems) return null;

  const [modalImage, setModalImage] = useState(null);

  return (
    <View style={ styles.container }>
      <Modal style={ styles.modal } animationType='fade' transparent={ true } visible={ !!modalImage }
             onRequestClose={ () => {
             } }>
        <View style={ styles.modal }>
          <Text style={ styles.text } onPress={ () => setModalImage(null) }>Close</Text>
          <ImageElement uri={ modalImage } />
        </View>
      </Modal>

      { galleryItems.map(x =>
        <TouchableWithoutFeedback key={ x.id } onPress={ () => setModalImage(x.image) }>
          <View style={ styles.imageWrap }>
            <ImageElement uri={ x.image } />
          </View>
        </TouchableWithoutFeedback>,
      ) }
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    flexDirection: "row",
    flexWrap: "wrap",
  },
  imageWrap: {
    margin: 2,
    height: (Dimensions.get("window").width / 3) - 8,
    width: (Dimensions.get("window").width / 3) - 8,
  },
  modal: {
    flex: 1,
    padding: 40,
    backgroundColor: "rgba(0, 0, 0, .85)",
  },
  text: {
    color: "#fff",
  },
});

export default ImageGallery;
