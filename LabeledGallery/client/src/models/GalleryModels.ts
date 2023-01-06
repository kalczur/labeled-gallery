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

type DetectionProvider = "User" | "Gcp";

export interface DetectedObject {
  id: string;
  label: string;
  accuracy: number;
  detectionProvider: DetectionProvider;
}

export interface UpdateGalleryItemsRequestDto {
  imagesToAdd: string[];
}

export interface ModifyDetectedObjectsRequestDto {
  galleryItemId: string;
  detectedObjects: DetectedObject[];
}
