export interface GalleryResponseDto {
  galleryItems: GalleryItemResponseDto[];
}

export interface GalleryItemResponseDto {
  name: string;
  image: string;
  detectedObjects: DetectedObject[];
}

export interface DetectedObject {
  label: string;
  accuracy: number;
}

export interface UpdateGalleryItemsRequestDto {
  imagesToAdd: string[];
}
