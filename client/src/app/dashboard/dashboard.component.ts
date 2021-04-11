import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { map } from 'rxjs/operators';
import { EventStats } from 'src/models/EventStats';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements AfterViewInit {
  displayedColumns: string[] = ['region', 'sensor', 'counter'];
  dataSource: MatTableDataSource<EventStats>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  connection: signalR.HubConnection;
  eventsStats: EventStats[];

  constructor(private breakpointObserver: BreakpointObserver, private http: HttpClient) {
    this.dataSource = new MatTableDataSource(this.eventsStats);
  }

  ngOnInit(): void {
    this.initWebSocket();
    this.connection.start();
    this.startAggregator();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  initWebSocket() {
    this.connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/hub/events')
      .build();

    this.connection.on('updateEvents', (events: EventStats[]) => {
      this.eventsStats = events;
      //console.log(events);
      this.dataSource = new MatTableDataSource(this.eventsStats);
      //console.log(this.dataSource);
    });

    this.connection.on('startMonitor', (events: EventStats[]) => {
      this.eventsStats = events;
    });

    this.connection.on('stopMonitor', () => {
      //Notificar que parou
    });
  }

  startAggregator(): any {
    this.http.get('http://localhost:5000/Event/StartAggregator')
      .subscribe(res => {
        console.log(res)
      }, err => {
        console.error(err);
      });
  }

  /** Based on the screen size, switch from standard to one column per row */
  cards = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(({ matches }) => {
      if (matches) {
        return [
          { title: 'Card 1', cols: 1, rows: 1 },
          { title: 'Card 2', cols: 1, rows: 1 }
          // { title: 'Card 3', cols: 1, rows: 1 },
          // { title: 'Card 4', cols: 1, rows: 1 }
        ];
      }

      return [
        { title: 'Card 1', cols: 1, rows: 1 },
        { title: 'Card 2', cols: 1, rows: 1 }
        // { title: 'Card 3', cols: 1, rows: 2 },
        // { title: 'Card 4', cols: 1, rows: 1 }
      ];
    })
  );
}

function observableThrowError(arg0: any) {
  throw new Error('Function not implemented.');
}
