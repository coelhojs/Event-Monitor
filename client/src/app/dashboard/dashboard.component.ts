import { animate, state, style, transition, trigger } from '@angular/animations';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { map } from 'rxjs/operators';
import { EventStats } from 'src/models/EventStats';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class DashboardComponent {
  @Input() eventsStats: EventStats[];
  @Input() dataSource: MatTableDataSource<EventStats>;

  columns = ['region', 'sensor', 'counter'];
  labels = ['Região', 'Sensor', 'Quantidade'];

  constructor() { 
  this.dataSource = new MatTableDataSource(this.eventsStats);
  }

  // /** Based on the screen size, switch from standard to one column per row */
  // cards = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
  //   map(({ matches }) => {
  //     if (matches) {
  //       return [
  //         { title: 'Card 1', cols: 1, rows: 1 },
  //         { title: 'Card 2', cols: 1, rows: 1 }
  //         // { title: 'Card 3', cols: 1, rows: 1 },
  //         // { title: 'Card 4', cols: 1, rows: 1 }
  //       ];
  //     }

  //     return [
  //       { title: 'Card 1', cols: 1, rows: 1 },
  //       { title: 'Card 2', cols: 1, rows: 1 }
  //       // { title: 'Card 3', cols: 1, rows: 2 },
  //       // { title: 'Card 4', cols: 1, rows: 1 }
  //     ];
  //   })
  // );
}

// function parseEventsStats(events: EventStats[]): EventStats[] {
//   let summarizedStats = events.filter(item => {
//     return item.sensor == null;
//   })

//   summarizedStats.forEach(item => {
//     item.details = events.filter(event => {
//       return event.region == item.region && event.sensor != null;
//     });
//   });

//   return summarizedStats;
// }
