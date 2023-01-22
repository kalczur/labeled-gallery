import React, { useState } from "react";
import {
  Dimensions,
  Modal,
  StyleSheet,
  TouchableWithoutFeedback,
  View,
  Text,
  Button,
} from "react-native";
import { DetectedObject, GalleryItemResponseDto } from "../../../models/GalleryModels";
import ImageElement from "./ImageElement";
import { buttonColorSecondary } from "../../../styles/colors";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { GalleryService } from "../../../services/GalleryService";
import ModifyLabelsModalBody from "./ModifyLabelsModalBody";

const galleryService = new GalleryService();

interface Props {
  galleryItems: GalleryItemResponseDto[];
  optimisticUpdate: (id: string, detectedObjects: DetectedObject[]) => void;
}

const ImageGallery = ({ galleryItems, optimisticUpdate }: Props) => {
  const [selectedImage, setSelectedImage] = useState<GalleryItemResponseDto>(null);
  const [modalEdit, setModalEdit] = useState(false);
  const queryClient = useQueryClient();

  const saveLabelsMutation = useMutation(galleryService.modifyDetectedObjects, {
    onSuccess: async (_, values) => {
      await queryClient.invalidateQueries(["modifyDetectedObjects"]);
      optimisticUpdate(selectedImage.id, values.detectedObjects);
    },
  });

  const saveLabels = async (detectedObjects: DetectedObject[]) => {
    await saveLabelsMutation.mutate({
      galleryItemId: selectedImage.id,
      detectedObjects: detectedObjects,
    });
  };

  if (!galleryItems) return null;

  return (
    <View style={ styles.container }>
      { !!selectedImage &&
        <>
          <Modal style={ styles.modal } animationType='fade' transparent={ true } visible
                 onRequestClose={ () => {
                 } }>
            <View style={ styles.modal }>
              <Text style={ [styles.text, styles.close] } onPress={ () => setSelectedImage(null) }>Close</Text>
              <ImageElement uri={ selectedImage.image } />
              <Button color={ buttonColorSecondary } title='Edit labels' onPress={ () => setModalEdit(true) } />
            </View>
          </Modal>

          <Modal
            style={ styles.modal }
            animationType='fade'
            transparent={ true }
            visible={ modalEdit }
            onRequestClose={ () => {
            } }
          >
            <ModifyLabelsModalBody
              initialDetectedObjects={ selectedImage.detectedObjects }
              closeModal={ () => setModalEdit(false) }
              saveLabels={ (x) => saveLabels(x) }
            />
          </Modal>
        </>
      }

      { galleryItems.map(x =>
        <TouchableWithoutFeedback key={ x.id } onPress={ () => setSelectedImage(x) }>
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
    backgroundColor: "rgba(0, 0, 0, .75)",
  },
  close: {
    textAlign: "right",
  },
  text: {
    color: "#fff",
  },
  input: {
    backgroundColor: "#111",
    color: "#fff",
    padding: 4,
  },
  button: {
    marginTop: 10,
  },
});

export default ImageGallery;
