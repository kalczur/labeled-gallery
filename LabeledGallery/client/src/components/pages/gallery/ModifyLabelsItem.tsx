import React, { useState } from "react";
import { Button, StyleSheet, Text, TextInput, View } from "react-native";
import { buttonColorDanger, buttonColorSecondary } from "../../../styles/colors";
import { DetectedObject } from "../../../models/GalleryModels";

interface Props {
  detectedObject: DetectedObject;
  editLabel: (x: string) => void;
  removeLabel: () => void;
  initialInEditMode: boolean;
}

const ModifyLabelsItem = ({ detectedObject, removeLabel, editLabel, initialInEditMode }: Props) => {
  const [inEditMode, setInEditMode] = useState(initialInEditMode);
  const [label, setLabel] = useState(detectedObject.label);

  const cancel = () => {
    setLabel(detectedObject.label);
    setInEditMode(false);
  };

  return (
    <View style={ [styles.detectedObject, detectedObject.detectionProvider === "User" && styles.userLabel] }>
      { detectedObject.detectionProvider === "User" ?
        (<View>
            <View style={ styles.detectedObjectButtons }>
              { inEditMode ? (
                <>
                  <TextInput
                    style={ styles.text }
                    value={ label }
                    placeholder='Label'
                    onChangeText={ x => setLabel(x) }
                  />
                  <Button color={ buttonColorDanger } title='Cancel' onPress={ () => cancel() } />
                  <Button color={ buttonColorSecondary } title='Save' onPress={ () => editLabel(label) } />
                </>
              ) : (
                <>
                  <Text style={ styles.text }>{ label }</Text>
                  <Button color={ buttonColorDanger } title='Remove' onPress={ () => removeLabel() } />
                  <Button color={ buttonColorSecondary } title='Edit' onPress={ () => setInEditMode(true) } />
                </>
              ) }
            </View>
          </View>
        ) : (
          <Text style={ styles.text }>{ label }</Text>
        ) }
    </View>
  );
};

const styles = StyleSheet.create({
  text: {
    color: "#fff",
  },
  detectedObject: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    height: 50,
    backgroundColor: "#222",
    padding: 6,
    marginTop: 5,
  },
  detectedObjectButtons: {
    flexDirection: "row",
  },
  userLabel: {
    backgroundColor: "#333",
  },
});

export default ModifyLabelsItem;
