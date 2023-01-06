import React, { useState } from "react";
import { DetectedObject } from "../../../models/GalleryModels";
import { Button, ScrollView, StyleSheet, Text, View } from "react-native";
import { buttonColorPrimary, buttonColorSecondary } from "../../../styles/colors";
import ModifyLabelsItem from "./ModifyLabelsItem";

interface Props {
  initialDetectedObjects: DetectedObject[];
  closeModal: () => void;
  saveLabels: (x: DetectedObject[]) => void;
}

const ModifyLabelsModalBody = ({ initialDetectedObjects, closeModal, saveLabels }: Props) => {
  const [detectedObjects, setDetectedObjects] = useState<DetectedObject[]>(initialDetectedObjects);
  const [idsInEditMode, setIdsInEditMode] = useState<string[]>([]);

  const removeLabel = (id: string) => {
    setDetectedObjects(prevState => prevState.filter(x => x.id === id));
  };

  const editLabel = (id: string, editedLabel: string) => {
    setDetectedObjects(prevState => {
      const newState = [...prevState];
      newState[id] = { ...newState.find(x => x.id === id), label: editedLabel };
      return newState;
    });

    setDetectedObjects(prevState => prevState.filter(x => x.id === id));
  };

  const addLabel = () => {
    // @ts-ignore
    const id = crypto.randomUUID();

    setDetectedObjects(prevState => [...prevState, {
      id,
      label: "",
      accuracy: 1,
      detectionProvider: "User",
    }]);

    setIdsInEditMode(prevState => [...prevState, id]);
  };

  return (
    <View style={ styles.modal }>
      <Text style={ [styles.text, styles.close] } onPress={ () => closeModal() }>Close</Text>
      <ScrollView>
        { detectedObjects.map(x => (
          <ModifyLabelsItem
            key={ x.id }
            detectedObject={ x }
            initialInEditMode={ idsInEditMode.includes(x.id) }
            removeLabel={ () => removeLabel(x.id) }
            editLabel={ (editedLabel) => editLabel(x.id, editedLabel) }
          />
        )) }
      </ScrollView>
      <View style={ styles.button }>
        <Button color={ buttonColorSecondary } title='Add' onPress={ () => addLabel() } />
      </View>
      <View style={ styles.button }>
        <Button color={ buttonColorPrimary } title='Save' onPress={ () => saveLabels(detectedObjects) } />
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  modal: {
    flex: 1,
    padding: 40,
    backgroundColor: "rgba(0, 0, 0, .75)",
  },
  text: {
    color: "#fff",
  },
  close: {
    textAlign: "right",
  },
  button: {
    marginTop: 10,
  },
});

export default ModifyLabelsModalBody;
