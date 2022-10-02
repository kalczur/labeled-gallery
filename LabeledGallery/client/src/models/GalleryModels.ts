export interface GalleryResponseDto {
  galleryItems: GalleryItemResponseDto[];
}

export interface GalleryItemResponseDto {
  id: string;
  name: string;
  url: string;
  totalAccuracy: string;
  detectedObjects: DetectedObject[];
}

export interface DetectedObject {
  label: string;
  accuracy: number;
}

export interface UpdateGalleryItemsRequestDto {
  imagesToAdd: string[];
}
