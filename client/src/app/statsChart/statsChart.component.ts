//import * as Highcharts from 'highcharts';
import { Component, Input, OnInit } from '@angular/core';
import { ChartData } from 'src/models/ChartData';
import * as Highcharts from "highcharts";
import { Options } from "highcharts";

@Component({
    selector: 'statsChart',
    templateUrl: './statsChart.component.html',
    styleUrls: ['./statsChart.component.scss'],
})

export class ChartComponent {
    @Input() chartData: ChartData[];
    @Input() Highcharts: typeof Highcharts = Highcharts;
    @Input() update;
    @Input() updateHistogram;

    chartOptions: Options = {
        chart: {
            renderTo: 'chart',
            marginLeft: 100,
            //  plotAreaWidth: 50,
            //   plotAreaHeight: 450,
        },

        title: {
            text: 'Evolução dos valores das tags nas últimas 24 horas'
        },

        yAxis: {
            title: {
                text: ''
            }
        },

        xAxis: {
            type: 'category',
            min: 0,
            labels: {
                // animate: false
            }
        },

        legend: {
            enabled: false
        },

        series: [{
            type: 'spline',
            zoneAxis: 'x',
            zones: [{
                value: 2,
                color: 'red'
            }],
            dataLabels: {
                enabled: true,
                format: '{y:,.2f}'
            },
            dataSorting: {
                enabled: true,
                sortKey: 'y'
            },
            data: [["hello", 1], ["hello", 1], ["hello", 1], ["hello", 1],]
        }]

    }

    ngOnChanges() {
        this.handleUpdate();
    }

    handleUpdate() {
        this.chartOptions.series[0] = {
            type: 'spline',
            data: this.chartData
        }

        this.updateHistogram = true;
    }
}