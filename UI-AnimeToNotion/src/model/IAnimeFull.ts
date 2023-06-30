import { IAnimeBase } from "./IAnimeBase";
import { IAnimeRelation } from "./IAnimeRelation";

export interface IAnimeFull extends IAnimeBase {
  relations: IAnimeRelation[];

}


