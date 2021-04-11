
import { Component, Input, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';
import { ChartData } from 'src/models/ChartData';

@Component({
    selector: 'chart',
    templateUrl: './chart.component.html',
    styleUrls: ['./chart.component.scss'],
})

export class ChartComponent implements OnInit {
    @Input() chartData: ChartData[];

    ngOnInit() {
        Highcharts.chart('container', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Ocorrências de eventos por sensor'
            },
            // subtitle: {
            //     text: 'Source: WorldClimate.com'
            // },
            xAxis: {
                categories: ['Nordeste', 'Norte', 'Sudeste', 'Sul'],
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Ocorrências'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                },
                series: {
                    animation: false
                }
            },
            series: this.chartData
        });
    }

    ngOnChanges() {
        this.ngOnInit();
    }
}