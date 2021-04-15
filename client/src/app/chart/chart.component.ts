
import { Component, Input, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';
import { HistogramData } from 'src/models/HistogramData';

@Component({
    selector: 'chart',
    templateUrl: './chart.component.html',
    styleUrls: ['./chart.component.scss'],
})

export class ChartComponent implements OnInit {
    @Input() chartData: HistogramData[];

    ngOnInit() {
        Highcharts.chart('chart', {
            chart: {
                type: 'spline'
            },
            title: {
                text: 'Evolução dos valores das tags nas últimas 24 horas'
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                type: 'datetime',
                dateTimeLabelFormats: {
                    minute: '%H:%M'
                },
                title: {
                    text: 'Hora'
                }
            },
            yAxis: {
                title: {
                    text: 'Valor'
                },
                min: 0
            },
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: 'Valor: {point.y:.5f}'
            },
        
            plotOptions: {
                series: {
                    marker: {
                        enabled: true
                    }
                }
            },
        
            colors: ['#7cb5ec', '#cccccc', '#90ed7d', '#f7a35c'],        
        
            series: this.chartData
        });
    }

    ngOnChanges() {
        this.ngOnInit();
    }
}