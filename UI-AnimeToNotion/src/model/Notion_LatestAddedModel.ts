export class Notion_LatestAddedModel {
  orderIndex: number;
  title: string;
  cover: string;
  createdTime: string;

  constructor(orderIndex: number, title: string, cover:string, createdTime: string) {
    this.orderIndex = orderIndex;
    this.title = title;
    this.cover = cover;
    this.createdTime = createdTime;
  }
}
