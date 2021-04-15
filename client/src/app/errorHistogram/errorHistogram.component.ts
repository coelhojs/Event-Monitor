
import { Component, Input, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';
import { HistogramData } from 'src/models/HistogramData';

@Component({
    selector: 'errorHistogram',
    templateUrl: './errorHistogram.component.html',
    styleUrls: ['./errorHistogram.component.scss'],
})

export class ErrorHistogramComponent implements OnInit {
    @Input() histogramData: HistogramData[];
    @Input() status: string;

    ngOnInit() {
        Highcharts.chart('errorHistogram', {
            chart: {
                type: 'column'
            },
            title: {
                text: `Ocorrências de erro por sensor`
            },
            xAxis: {
                categories: ['Nordeste', 'Norte', 'Sudeste', 'Sul'],
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Erros'
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