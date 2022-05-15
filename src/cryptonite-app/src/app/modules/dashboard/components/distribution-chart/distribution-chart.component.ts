import { Component, ElementRef, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Label, SingleDataSet } from 'ng2-charts';
import { ChartOptions, ChartType } from 'chart.js';
import { Distribution } from '../../../../models/portofolio/distribution';
import * as pluginLabels from 'chartjs-plugin-labels';
import { generateColor } from '../../../../common/utils/colors';

@Component({
  selector: 'app-distribution-chart',
  templateUrl: './distribution-chart.component.html',
  styleUrls: ['./distribution-chart.component.scss']
})
export class DistributionChartComponent implements OnInit, OnChanges {
  @Input() distribution: Distribution;
  @ViewChild('chartCanvas') chartCanvas: ElementRef;

  plugins = [pluginLabels];
  symbols: Label[] = [];
  data: SingleDataSet = [];
  doughnutChartType: ChartType = 'doughnut';
  options: ChartOptions = {
    maintainAspectRatio: false,
    tooltips: {
      displayColors: false,
      mode: 'label'
    },
    cutoutPercentage: 50,
    elements: {
      arc: {
        borderWidth: 15
      }
    },
    plugins: {
      labels: {
        render: 'label',
        fontColor: 'white',
        position: 'default',
        arc: true
      }
    }
  };

  pieChartColors = [
    {
      backgroundColor: ['#fc5858', '#19d863', '#fdf57d', '#fdf57d', '#fdf57d'],
      borderColor: '#151A21'
    }
  ];

  ngOnInit(): void {
    // eslint-disable-next-line @typescript-eslint/no-this-alias
    this.options.tooltips.callbacks = {
      label: (tooltipItem: Chart.ChartTooltipItem, data: Chart.ChartData) => '',
      afterLabel: (tooltipItem: Chart.ChartTooltipItem, data: Chart.ChartData) =>
        `${Math.round(parseFloat(this.data[tooltipItem.index].toString()) * 100) / 100} ${this.distribution.currency} (${(parseFloat(this.data[tooltipItem.index].toString()) / this.distribution.totalValue * 100).toFixed(2)}%)`
    };
    this.symbols = this.distribution.items.map(x => x.symbol);
    this.data = this.distribution.items.map(x => x.value);

    this.pieChartColors = [
      {
        backgroundColor: generateColor('#6e2d16', '#fd7943', this.symbols.length),
        borderColor: '#151A21'
      }
    ];
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes && changes.distribution.currentValue.items) {
      for (let i = 0; i < this.distribution.items.length; i++) {
        const item = this.distribution.items[i];
        this.data[i] = item.value;
        this.symbols[i] = item.symbol;
      }
    }
  }
}
