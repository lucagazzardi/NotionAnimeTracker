import { IAnimePersonal } from "./IAnimePersonal";

export interface IAnimeBase {
  malId: number;
  nameDefault: string;
  nameEnglish: string;
  nameJapanese: string;
  cover: string;
  score: number | null;
  startedAiring: Date | null;
  format: string | null;
  episodes: number | null;
  planToWatch: boolean;
  favorite: boolean;
  genres: IKeyValue[];
  studios: IKeyValue[];
  info: IAnimePersonal | null;
}

export interface IKeyValue {
  id: number;
  value: string;
}

