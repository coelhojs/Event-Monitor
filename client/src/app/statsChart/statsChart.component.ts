//import * as Highcharts from 'highcharts';
import { Component, Input, OnInit } from '@angular/core';
import { ChartData } from 'src/models/ChartData';
import * as Highcharts from "highcharts";

@Component({
    selector: 'statsChart',
    templateUrl: './statsChart.component.html',
    styleUrls: ['./statsChart.component.scss'],
})

export class ChartComponent onInit{
    @Input() chartData: ChartData[];

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