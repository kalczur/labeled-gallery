export interface GetGalleryRequestDto {
  searchKeyword: string;
}

export interface GalleryResponseDto {
  galleryItems: GalleryItemResponseDto[];
}

export interface GalleryItemResponseDto {
  id: string;
  name: string;
  image: string;
  detectedObjects: DetectedObject[];
  totalAccuracy: number[];
}

export interface DetectedObject {
  label: string;
  accuracy: number;
}

export interface UpdateGalleryItemsRequestDto {
  imagesToAdd: string[];
}
