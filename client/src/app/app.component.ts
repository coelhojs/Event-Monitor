import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { ChartData } from 'src/models/ChartData';
import { EventStats } from 'src/models/EventStats';
import { HistogramData } from 'src/models/HistogramData';

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
  histogramData: HistogramData[];
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
      .withUrl('http://0477f5f4a724e6.localhost.run/hub/events')
      .build();

    this.connection.on('updateEvents', (events: EventStats[]) => {
      this.eventsStats = events;
      this.dataSource = new MatTableDataSource(this.eventsStats);
    });

    this.connection.on('updateChart', (chartData: ChartData[]) => {
      this.chartData = chartData;
    });

    this.connection.on('updateHistogram', (histogramData: HistogramData[]) => {
      this.histogramData = histogramData;
    });
  }

  getStats(): any {
    this.http.get('http://0477f5f4a724e6.localhost.run/Event/GetStats')
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
    this.http.get('http://0477f5f4a724e6.localhost.run/Event/StartAggregator')
      .subscribe(res => { }, err => {
        if (err.status != 409) {
          console.error(err);
        }
      });
  }
}
