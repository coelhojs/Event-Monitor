import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { ChartData } from 'src/models/ChartData';
import { EventStats } from 'src/models/EventStats';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  connection: signalR.HubConnection;
  dataSource: MatTableDataSource<EventStats>;
  eventsStats: EventStats[];
  chartData: ChartData[];
  title = 'event-monitor';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.initWebSocket();
    this.connection.start()
      .then(() => {
        this.connection.invoke('Update');
      })
    this.startAggregator();
  }

  initWebSocket() {
    this.connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/hub/events')
      .build();

    this.connection.on('updateEvents', (events: EventStats[]) => {
      this.eventsStats = events;
      this.dataSource = new MatTableDataSource(this.eventsStats);
    });

    this.connection.on('updateChart', (chartData: ChartData[]) => {
      this.chartData = chartData;
    });

    this.connection.on('startMonitor', (events: EventStats[]) => {
      //
    });

    this.connection.on('stopMonitor', () => {
      //Notificar que parou
    });
  }

  getStats(): any {
    this.http.get('http://localhost:5000/Event/GetStats')
      .subscribe(res => {
        return res;
      }, err => {
        if (err.status != 409) {
          console.error(err);
          return null;
        }
      });
  }

  startAggregator(): any {
    this.http.get('http://localhost:5000/Event/StartAggregator')
      .subscribe(res => {
        console.log(res)
      }, err => {
        if (err.status != 409) {
          console.error(err);
        }
      });
  }
}
