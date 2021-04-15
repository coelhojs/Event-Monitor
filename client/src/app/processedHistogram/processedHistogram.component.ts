
import { Component, Input, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';
import { HistogramData } from 'src/models/HistogramData';

@Component({
    selector: 'processedHistogram',
    templateUrl: './processedHistogram.component.html',
    styleUrls: ['./processedHistogram.component.scss'],
})

export class ProcessedHistogramComponent implements OnInit {
    @Input() histogramData: HistogramData[];
    @Input() status: string;

    ngOnInit() {
        Highcharts.chart('processedHistogram', {
            chart: {
                type: 'column'
            },
            title: {
                text: `Ocorrências de eventos processados por sensor`
            },
            xAxis: {
                categories: ['Nordeste', 'Norte', 'Sudeste', 'Sul'],
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Processados'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y}</b></td></tr>',
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
            series: this.histogramData
        });
    }

    ngOnChanges() {
        this.ngOnInit();
    }
}