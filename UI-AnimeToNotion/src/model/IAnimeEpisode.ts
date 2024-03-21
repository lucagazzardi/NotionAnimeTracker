
export interface IAnimeEpisodesRecord {
  animeShowId: number;
  episodes: IAnimeSingleEpisode[];
}

export interface IAnimeSingleEpisode {
  titleEnglish: string;
  titleJapanese: string;
  episodeNumber: number;
  episodeId: number;
  watchedOn: Date | null;
}

export class AnimeEpisode{
  id: number | null;
  animeShowId: number;
  episodeNumber: number;
  watchedOn: Date;

  constructor(id: number | null, animeShowId: number, episodeNumber: number, watchedOn: Date) {
    this.id = id;
    this.animeShowId = animeShowId;
    this.episodeNumber = episodeNumber;
    this.watchedOn = watchedOn;
  }
}
