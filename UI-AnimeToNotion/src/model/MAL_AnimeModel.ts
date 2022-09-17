export class MAL_AnimeModel {
  id: string;
  title: string;
  main_picture: MAL_MainPicture;
  alternative_titles: MAL_AlternativeTitle;
  mean: number;
  start_date: string;
  media_type: string;
  genres: MAL_GeneralObject[];
  studios: MAL_GeneralObject[];
  num_episodes: number;
  related_anime: MAL_RelatedShow[];

  genresJoined: string;
  studiosJoined: string;
  showHidden: string | null;
  

  constructor(id: string, title: string, main_picture: MAL_MainPicture, alternative_titles: MAL_AlternativeTitle, mean: number, start_date: string, media_type: string, genres: MAL_GeneralObject[], studios: MAL_GeneralObject[], num_episodes: number, genresJoined: string, studiosJoined: string, showHidden: string | null, related_anime: MAL_RelatedShow[]) {
    this.id = id;
    this.title = title;
    this.main_picture = main_picture;
    this.alternative_titles = alternative_titles;
    this.mean = mean;
    this.start_date = start_date;
    this.media_type = media_type;
    this.genres = genres;
    this.studios = studios;
    this.num_episodes = num_episodes;
    this.genresJoined = genresJoined;
    this.studiosJoined = studiosJoined;
    this.showHidden = showHidden;
    this.related_anime = related_anime;
  }
}

export class MAL_MainPicture {
  medium: string;
  large: string;

  constructor(medium: string, large: string) {
    this.medium = medium;
    this.large = large;
  }
}

export class MAL_AlternativeTitle {
  synonmys: string[];
  en: string;
  ja: string;

  constructor(synonmys: string[], en: string, ja: string) {
    this.synonmys = synonmys;
    this.en = en;
    this.ja = ja;
  }
}

export class MAL_GeneralObject {
  id: string;
  name: string

  constructor(id: string, name: string) {
    this.id = id;
    this.name = name;
  }
}

export class MAL_RelatedShow {
  node: MAL_RelatedShow_Node;
  relation_type: string;
  relation_type_formatter: string;  

  constructor(node: MAL_RelatedShow_Node, relation_type: string, relation_type_formatter: string) {
    this.node = node;
    this.relation_type = relation_type;
    this.relation_type_formatter = relation_type_formatter;
  }
}

export class MAL_RelatedShow_Node {
  id: number;
  title: string;
  main_picture: MAL_MainPicture;

  constructor(id: number, title: string, main_picture: MAL_MainPicture) {
    this.id = id;
    this.title = title;
    this.main_picture = main_picture;
  }
}

//export var animeListMocked: MAL_AnimeModel[] = [
//  {
//    id: "17549",
//    title: "Non Non Biyori",
//    alternative_titles: {
//      ja: "のんのんびより",
//      synonmys: [],
//      en: "Non Non Biyori"
//    },
//    main_picture: {
//      medium: "https://api-cdn.myanimelist.net/images/anime/2/51581.jpg",
//      large: "https://api-cdn.myanimelist.net/images/anime/2/51581.jpg"
//    },
//    mean: 7.94,
//    start_date: "2013-10-08",
//    media_type: "tv",
//    genres: [
//      {
//        id: "52",
//        name: "CGDCT"
//      },
//      {
//        id: "63",
//        name: "Iyashikei"
//      },
//      {
//        id: "23",
//        name: "School"
//      },
//      {
//        id: "42",
//        name: "Seinen"
//      },
//      {
//        id: "36",
//        name: "Slice of Life"
//      }
//    ],
//    studios: [
//      {
//        id: "300",
//        name: "SILVER LINK."
//      }
//    ],
//    num_episodes: 12,
//    genresJoined: "School, Slice of Life, CTDCT",
//    studiosJoined: "SILVER LINK",
//    showHidden: null
//  },
//  {
//    id: "48736",
//    title: "Sono Bisque Doll wa Koi wo Suru",
//    alternative_titles: {
//      ja: "その着せ替え人形は恋をする",
//      synonmys: [
//        "Sono Kisekae Ningyou wa Koi wo Suru",
//        "KiseKoi"
//      ],
//      en: "My Dress-Up Darling"
//    },
//    main_picture: {
//      medium: "https://cdn.myanimelist.net/images/anime/1179/119897.jpg",
//      large: "https://cdn.myanimelist.net/images/anime/1179/119897.jpg"
//    },
//    mean: 8.32,
//    start_date: "2022-01-09",
//    media_type: "special",
//    genres: [
//      {
//        id: "69",
//        name: "Otaku Culture"
//      },
//      {
//        id: "22",
//        name: "Romance"
//      },
//      {
//        id: "23",
//        name: "School"
//      },
//      {
//        id: "42",
//        name: "Seinen"
//      }
//    ],
//    studios: [
//      {
//        id: "1835",
//        name: "CloverWorks"
//      }
//    ],
//    num_episodes: 12,
//    genresJoined: "School, Seinen, Romance",
//    studiosJoined: "CloverWorks",
//    showHidden: null
//  },
//  {
//    id: "37450",
//    title: "Seishun Buta Yarou wa Bunny Girl Senpai no Yume wo Minai",
//    alternative_titles: {
//      ja: "青春ブタ野郎はバニーガール先輩の夢を見ない",
//      synonmys: [
//        "AoButa"
//      ],
//      en: "Rascal Does Not Dream of Bunny Girl Senpai"
//    },
//    main_picture: {
//      medium: "https://api-cdn.myanimelist.net/images/anime/1301/93586.jpg",
//      large: "https://api-cdn.myanimelist.net/images/anime/1301/93586.jpg"
//    },
//    mean: 8.26,
//    start_date: "2018-10-04",
//    media_type: "movie",
//    genres: [
//      {
//        id: "4",
//        name: "Comedy"
//      },
//      {
//        id: "8",
//        name: "Drama"
//      },
//      {
//        id: "22",
//        name: "Romance"
//      },
//      {
//        id: "23",
//        name: "School"
//      },
//      {
//        id: "37",
//        name: "Supernatural"
//      }
//    ],
//    studios: [
//      {
//        id: "1835",
//        name: "CloverWorks"
//      }
//    ],
//    num_episodes: 12,
//    genresJoined: "School, Comedy, Romance",
//    studiosJoined: "CloverWorks",
//    showHidden: null
//  }
//]
