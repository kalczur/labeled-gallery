import { apiClient } from "./ApiClient";
import { GalleryResponseDto, UpdateGalleryItemsRequestDto } from "../models/GalleryModels";
import mime from "mime";

const normalizeUri = (uri: string) => "file:///" + uri.split("file:/").join("");

export class GalleryService {

  async get(): Promise<GalleryResponseDto> {
    const result = await apiClient.get<GalleryResponseDto>("gallery/get");
    return result.data;
  }

  async update(dto: UpdateGalleryItemsRequestDto): Promise<void> {

    const formData = new FormData();

    dto.imagesToAdd.forEach(x => {
      const uri = normalizeUri(x);

      const image = {
        uri: uri,
        type: mime.getType(uri),
        name: uri.split("/").pop(),
      };

      // @ts-ignore
      formData.append("imagesToAdd", image);
    });

    await apiClient.post<UpdateGalleryItemsRequestDto>("gallery/update", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
  }
}
