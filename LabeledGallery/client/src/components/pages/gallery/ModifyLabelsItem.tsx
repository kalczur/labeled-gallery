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
            <View style={ styles.detectedObjectRow }>
              { inEditMode ? (
                <>
                  <TextInput
                    style={ styles.input }
                    value={ label }
                    placeholder='Label'
                    placeholderTextColor='#bbb'
                    onChangeText={ x => setLabel(x) }
                  />
                  <View style={ styles.detectedObjectButtons }>
                    <View>
                      <Button color={ buttonColorDanger } title='Cancel' onPress={ () => cancel() } />
                    </View>
                    <View style={ { marginLeft: 5 } }>
                      <Button color={ buttonColorSecondary } title='Save' onPress={ () => {
                        editLabel(label);
                        setInEditMode(false);
                      } } />
                    </View>
                  </View>
                </>
              ) : (
                <>
                  <Text style={ styles.textLabel }>{ label }</Text>
                  <View style={ styles.detectedObjectButtons }>
                    <View>
                      <Button color={ buttonColorDanger } title='Remove' onPress={ () => removeLabel() } />
                    </View>
                    <View style={ { marginLeft: 5 } }>
                      <Button color={ buttonColorSecondary } title='Edit' onPress={ () => setInEditMode(true) } />
                    </View>
                  </View>
                </>
              ) }
            </View>
          </View>
        ) : (
          <Text style={ styles.textLabel }>{ label }</Text>
        ) }
    </View>
  );
};

const styles = StyleSheet.create({
  textLabel: {
    color: "#fff",
    width: "60%",
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
  detectedObjectRow: {
    flexDirection: "row",
  },
  detectedObjectButtons: {
    width: "40%",
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "center",
  },
  userLabel: {
    backgroundColor: "#333",
  },
  input: {
    color: "#fff",
    width: "60%",
  },
});

export default ModifyLabelsItem;
