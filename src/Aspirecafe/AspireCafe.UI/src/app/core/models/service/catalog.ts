import { CatalogItemServiceModel } from "./catalogitem";

export interface CatalogServiceModel {
  catalog: { [key: string]: CatalogItemServiceModel[]; };
}
