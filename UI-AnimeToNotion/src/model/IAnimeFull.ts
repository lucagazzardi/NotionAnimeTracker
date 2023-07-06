import { IAnimeBase } from "./IAnimeBase";
import { IAnimeEdit } from "./IAnimeEdit";
import { IAnimeRelation } from "./IAnimeRelation";

export interface IAnimeFull extends IAnimeBase {
  relations: IAnimeRelation[];
  edit: IAnimeEdit | null;
}


