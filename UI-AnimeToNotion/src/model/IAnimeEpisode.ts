
export interface IAnimeEpisodesRecord {
  animeShowId: string;
  episodes: IAnimeSingleEpisode[];
}

export interface IAnimeSingleEpisode {
  titleEnglish: string;
  titleJapanese: string;
  episodeNumber: number;
  episodeId: string;
  watchedOn: Date | null;
}

export class AnimeEpisode{
  id: string | null;
  animeShowId: string;
  episodeNumber: number;
  watchedOn: Date;

  constructor(id: string | null, animeShowId: string, episodeNumber: number, watchedOn: Date) {
    this.id = id;
    this.animeShowId = animeShowId;
    this.episodeNumber = episodeNumber;
    this.watchedOn = watchedOn;
  }
}
