import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToasterService } from 'gazza-toaster';
import { MAL_AnimeModel } from '../../model/MAL_AnimeModel';
import { MalService } from '../utility-components/mal/mal.service';
import { EditService } from './edit.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit {

  id: string | null = null;
  item: MAL_AnimeModel | null = null;
  malBaseUrl: string = 'https://myanimelist.net/anime/';

  isFavorite: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private editService: EditService,
    private malService: MalService,
    private toasterService: ToasterService
  )
  {

  }

  ngOnInit(): void {
    // Get fields from Route
    this.id = this.route.snapshot.paramMap.get('id');

    // Get current item in the service
    this.item = this.editService.getItem();

    // If the last item in the service is null or not the same (for example if the navigation occurred typing the url manually in the searchbar) take the item from Server
    if ((this.item == null && this.id != null) || (this.item != null && this.id != null && this.id !== this.item.id.toString()))
      this.loadItem(this.id);
  }

  /// Load the item from the server if the item is not present
  loadItem(id: string) {
    this.malService.get(id)
      .subscribe(
        {
          next: (data: MAL_AnimeModel) => { this.item = data; console.log(this.item) },
          error: () => { this.toasterService.notifyError("The item could not be retrieved") }
        });
  }

  /// Set the show as favorite or remove from favorites
  // TODO: Call API to Add as favorite
  setAsFavorite(value: boolean) {
    this.isFavorite = value;
  }

}
