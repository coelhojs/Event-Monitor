import { Component, Input } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { EventStats } from 'src/models/EventStats';

@Component({
  selector: 'statsTable',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class DashboardComponent {
  @Input() eventsStats: EventStats[];
  @Input() dataSource: MatTableDataSource<EventStats>;

  columns = ['region', 'sensor', 'counter', 'status'];
  labels = ['Região', 'Sensor', 'Quantidade', "Status"];

  constructor() {
    this.dataSource = new MatTableDataSource(this.eventsStats);
  }
}