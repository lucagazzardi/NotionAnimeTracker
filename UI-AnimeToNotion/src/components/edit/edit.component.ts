import { transition, trigger, useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToasterService } from 'gazza-toaster';
import { opacityOnEnter, scaleUpOnEnter } from '../../assets/animations/animations';
import { EditService } from '../../services/edit/edit.service';
import { SelectShowStatus } from '../../model/form-model/SelectShowStatus';
import { IAnimeFull } from '../../model/IAnimeFull';
import { IAnimeEdit } from '../../model/IAnimeEdit';
import { InternalService } from '../../services/notion/internal.service';
import { IAnimeRelation } from '../../model/IAnimeRelation';
import { SelectItem } from '../../model/form-model/SelectInterface';
import { IAnimePersonal } from '../../model/IAnimePersonal';

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
  item: IAnimeFull | null = null;
  malBaseUrl: string = 'https://myanimelist.net/anime/';

  //STATUS
  showStatuses = SelectShowStatus;  

  // FAVORITE
  isFavorite: boolean = false;

  //SKELETON
  testSkeleton: boolean = true;
  infoSkeleton = Array(4).fill(0);


  //SELECTED PROPERTIES
  selectedStatus: string | null = null;
  selectedRank: number | null = null;
  selectedStartDate: Date | null = null;
  selectedFinishDate: Date | null = null;
  selectedNotes: string | null = null;

  //INITIAL VALUES
  initialStatus: string | null = null;
  initialRank: number | null = null;
  initialStartDate: Date | null = null;
  initialFinishDate: Date | null = null;
  initialNotes: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private editService: EditService,
    private internalService: InternalService,
    private toasterService: ToasterService
  )
  {

  }

  ngOnInit(): void {

    // Get fields from Route
    this.id = this.route.snapshot.paramMap.get('id');

    // Get current item in the service
    this.item = this.editService.getItem() as IAnimeFull;

    // If the last item in the service is null or not the same (for example if the navigation occurred typing the url manually in the searchbar) retrieves the item from Server
    if ((this.item == null && this.id != null) || (this.item != null && this.id != null && this.id !== this.item.malId.toString()))
      this.loadItem(Number(this.id));

    // If the item is present retrieves the edit properties
    else if (this.item != null && this.item.info != null && this.item.edit == null)
      this.loadItemForEdit(this.item.info.id);

    // If the item is not present retrieves only relations
    else if (this.item != null && this.item.relations == null)
      this.loadItemRelations(Number(this.id))
      
  }

  /// Load the item from server if the item is not present
  loadItem(id: number) {
    this.internalService.getAnimeFull(id)
      .subscribe(
        {
          next: (data: IAnimeFull) => { this.item = data },
          error: () => { this.toasterService.notifyError("The item could not be retrieved") },
          complete: () => { this.setInitialValues(this.item!) }
        });
  }

  /// Load the edit properties if the item is present
  loadItemForEdit(id: string) {
    this.internalService.getAnimeForEdit(id)
      .subscribe(
        {
          next: (data: IAnimeFull) => { this.item!.edit = data.edit },
          error: () => { this.toasterService.notifyError("The item could not be retrieved") },
          complete: () => { this.setInitialValues(this.item!) }
        });
  }

  /// Load the relations for an anime
  loadItemRelations(id: number) {
    this.internalService.getAnimeRelations(id)
      .subscribe(
        {
          next: (data: IAnimeRelation[]) => { this.item!.relations = data;},
          error: () => { this.toasterService.notifyError("The relations could not be retrieved") },
          complete: () => { this.setInitialValues(this.item!) }
        });
  }

  /// Set initial values for edit
  setInitialValues(item: IAnimeFull) {

    if (item!.edit == null)
      item!.edit = {} as IAnimeEdit;

    this.initialStatus = item.edit.status ?? null;
    this.initialRank = item.edit?.personalScore ?? null;
    this.initialStartDate = item.edit?.startedOn ?? null;
    this.initialFinishDate = item.edit?.finishedOn ?? null;
    this.initialNotes = item.edit?.notes ?? null;
  }

  //STATUS
  /// Set the selected status
  setStatus(status: SelectItem | null) {
    this.selectedStatus = status != null ? status.viewValue : null;
    this.item!.edit!.status = status?.viewValue ?? "To Watch";
  }

  //RATING
  /// Set rank
  setRank(rank: number | null) {
    this.selectedRank = rank;
    this.item!.edit!.personalScore = rank ?? null;
  }

  //STARTED DATE
  /// Set start date
  setStartDate(startDate: Date | null) {
    this.selectedStartDate = startDate;
    this.item!.edit!.startedOn = startDate ?? null;
  }

  //FINISHED DATE
  /// Set finished date
  setFinishDate(finishDate: Date | null) {
    this.selectedFinishDate = finishDate;
    this.item!.edit!.finishedOn = finishDate ?? null;
  }

  //NOTES
  /// Set notes
  setNotes(notes: string | null) {
    this.selectedNotes = notes;
    this.item!.edit!.notes = notes ?? null;
  }

  save() {

    // If there are no changes
    if (this.areThereChanges() === false && this.item!.info != null) {
      this.toasterService.notifySuccess(this.item!.nameEnglish + " updated");
      return;
    }

    // If finishedOn is set but no startedOn exists reset the finishedOn
    if (this.item!.edit!.finishedOn != null && this.item!.edit!.startedOn == null)
      this.item!.edit!.startedOn = this.item!.edit!.finishedOn;

    // If the item has the id it means it's already present in the database so upload is needed, else the anime is added
    if (this.item!.info?.id != null)
      this.updateItem();
    else
      this.addFull();
    
    console.log(this.item!.edit);
  }

  /// Set the show as favorite or remove from favorites
  // TODO: Call API to Add as favorite
  setAsFavorite(value: boolean) {
    this.isFavorite = value;
  }

  updateItem() {
    this.internalService.editAnime(this.item!.edit!)
      .subscribe(
        {
          next: () => { this.toasterService.notifySuccess(this.item!.nameEnglish + " updated") },
          error: () => { this.toasterService.notifyError("The entry could not be updated") }
        });
  }

  addFull() {
    this.internalService.addFull(this.item!)
      .subscribe(
        {
          next: (data: IAnimePersonal) => { this.item!.info = data; this.toasterService.notifySuccess(this.item!.nameEnglish + " updated") },
          error: () => { this.toasterService.notifyError("The entry could not be updated") }
        });
  }

  /// Check if edits have been made
  areThereChanges(): boolean {
    return  this.initialStatus != this.selectedStatus ||
      this.initialRank != this.selectedRank ||
      this.initialStartDate != this.selectedStartDate ||
      this.initialFinishDate != this.selectedFinishDate ||
      this.initialNotes != this.selectedNotes
  }
}
