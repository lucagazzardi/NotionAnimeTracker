import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToasterService } from 'gazza-toaster';
import { MAL_AnimeModel } from '../../model/MAL_AnimeModel';
import { opacityOnEnter, scaleUpOnEnter } from '../../assets/animations/animations';
import { MalService } from '../../services/mal/mal.service';
import { EditService } from '../../services/edit/edit.service';
import { SelectShowStatus } from '../../model/SelectShowStatus';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss'],
  animations: [
    trigger('opacityOnEnter', [
      transition(':enter', [
        useAnimation(opacityOnEnter)
      ])
    ]),
    trigger('scaleUpOnEnter', [
      transition(':enter', [
        useAnimation(scaleUpOnEnter)
      ])
    ]),
  ]  
})
export class EditComponent implements OnInit {

  id: string | null = null;
  item: MAL_AnimeModel | null = null;
  malBaseUrl: string = 'https://myanimelist.net/anime/';

  //STATUS
  showStatuses = SelectShowStatus;  

  // FAVORITE
  isFavorite: boolean = false;

  //SKELETON
  testSkeleton: boolean = true;
  infoSkeleton = Array(4).fill(0);


  //SELECTED PROPERTIES
  selectedStatus: { id: string, label: string } | null = null;
  selectedRank: number = 0;
  selectedStartDate: Date = new Date();
  selectedFinishDate: Date = new Date();
  selectedNotes: string = '';
  selectedParentShow: string = '';

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

    // If the last item in the service is null or not the same (for example if the navigation occurred typing the url manually in the searchbar) retrieve the item from Server
    if ((this.item == null && this.id != null) || (this.item != null && this.id != null && this.id !== this.item.id.toString()))
      this.loadItem(this.id);
  }

  /// Load the item from server if the item is not present
  loadItem(id: string) {
    this.malService.getShowById(id)
      .subscribe(
        {
          next: (data: MAL_AnimeModel) => { this.item = data },
          error: () => { this.toasterService.notifyError("The item could not be retrieved") }
        });
  }

  //STATUS
  /// Set the selected status
  setStatus(status: { id: string, label: string } | null) {
    this.selectedStatus = status;
  }

  //RATING
  /// Set rank
  setRank(rank: number) {
    this.selectedRank = rank;
  }

  //STARTED DATE
  /// Set start date
  setStartDate(startDate: Date) {
    this.selectedStartDate = startDate;
  }

  //FINISHED DATE
  /// Set finished date
  setFinishDate(finishDate: Date) {
    this.selectedFinishDate = finishDate;
  }

  //NOTES
  /// Set notes
  setNotes(notes: string) {
    this.selectedNotes = notes;
  }

  //PARENT SHOW
  /// Set parent show
  setParentShow(parentShow: string) {
    this.selectedParentShow = parentShow;
  }
 

  /// Set the show as favorite or remove from favorites
  // TODO: Call API to Add as favorite
  setAsFavorite(value: boolean) {
    this.isFavorite = value;
  }
}
