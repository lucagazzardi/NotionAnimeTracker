import { IAnimeFull } from "./IAnimeFull";

export interface IHistory {
  year: number;
  watchedNumber: number;
  favoritesNumber: number;
  data: IAnimeFull[];
}
