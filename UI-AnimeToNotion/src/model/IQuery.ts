export interface IQuery {
  filters: IFilter;
  sort: Sort | null;
  page: IPage;
}

export interface IFilter {
  search: string | null;
  genre: number[] | null;
  status: string | null;
  format: string | null;
  year: string | null;
  malScoreGreater: number | null;
  malScoreLesser: number | null;
  personalScoreGreater: number | null;
  personalScoreLesser: number | null;
  episodesGreater: number | null;
  episodesLesser: number | null;
  favoritesOnly: boolean | null;
  planToWatchOnly: boolean | null;
}

export enum Sort {
  Title = 'Title',
  Status = 'Status',
  MalScore = 'MalScore',
  PersonalScore = 'PersonalScore',
  Upcoming = 'Upcoming',
  StartDate = 'StartDate',
  FinishDate = 'FinishDate',
  AddedDate = 'AddedDate'
}

export interface IPage {
  currentPage: number;
  perPage: number | null;
  totalCount: number | null;
}
