import { environment } from './../environments/environment';
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
  // chartData: ChartData[];
  histogramErrorData: HistogramData[];
  histogramProcessedData: HistogramData[];
  title = 'event-monitor';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.initWebSocket();
    this.connection.start()
      .then(() => {
        this.connection.invoke('Update');
      })
    //this.startAggregator();
  }

  initWebSocket() {
    this.connection = new HubConnectionBuilder()
      .withUrl(environment.apiUrl + "/hub/events")
      .build();

    this.connection.on('updateEvents', (events: EventStats[]) => {
      this.eventsStats = events;
      this.dataSource = new MatTableDataSource(this.eventsStats);
    });

    this.connection.on('updateProcessedHistogram', (histogramData: HistogramData[]) => {
      this.histogramProcessedData = histogramData;
    });

    this.connection.on('updateErrorHistogram', (histogramData: HistogramData[]) => {
      this.histogramErrorData = histogramData;
    });
  }

  getStats(): any {
    this.http.get(environment.apiUrl + "/Event/GetStats")
      .subscribe(res => {
      }, err => {
        if (err.status != 409) {
          console.error(err);
          return null;
        }
      });
  }

  processChartData(chartData): ChartData[] {
    let data: ChartData[] = []
    let item: ChartData = null;

    chartData.map(list => {

      item.name = list.name;

      list.map(dataItem => {
        let rawData = dataItem.data.Split(';');

        item.data = [rawData[0], rawData[1]];

        data.push(item);
      });
    });

    return data;
  }

  startAggregator(): any {
    this.http.get(environment.apiUrl + "/Event/StartAggregator")
      .subscribe(res => { }, err => {
        if (err.status != 409) {
          console.error(err);
        }
      });
  }
}

