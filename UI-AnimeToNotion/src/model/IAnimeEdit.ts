export interface IAnimeEdit {
  id: string | null;
  status: string | null;
  personalScore: number | null;
  startedOn: Date | null;
  finishedOn: Date | null;
  notes: string | null;
  completedYear: ICompletedYear;
}

export interface ICompletedYear {
  id: number;
  notionPageId: string;
  value: number;
}
