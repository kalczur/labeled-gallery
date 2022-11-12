import React from "react";
import { Image, StyleSheet } from "react-native";

interface Props {
  uri: string;
}

const ImageElement = ({ uri }: Props) => {
  return (
    <Image source={ { uri } } style={ styles.image } />
  );
};

const styles = StyleSheet.create({
  image: {
    flex: 1,
    width: null,
    alignSelf: "stretch",
  },
});

export default ImageElement;
