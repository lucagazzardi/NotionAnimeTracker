export interface IAnimeEdit {
  id: string | null;
  planToWatch: boolean;
  status: string | null;
  personalScore: number | null;
  startedOn: Date | null;
  finishedOn: Date | null;
  favorite: boolean;
  notes: string | null;
  completedYear: ICompletedYear;
}

export interface ICompletedYear {
  id: number;
  notionPageId: string;
  value: number;
}
