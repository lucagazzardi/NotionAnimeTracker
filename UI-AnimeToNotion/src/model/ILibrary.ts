import { IAnimeFull } from "./IAnimeFull";

export interface ILibrary{
  data: IAnimeFull[];
  pageInfo: IPageInfo;
}

export interface IPageInfo {
  currentPage: number;
  perPage: number;
  lastPage: number;
  totalCount: number;
  hasNextPage: boolean;
}
