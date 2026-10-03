import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, inject, signal } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { DashboardService } from '../../services/dashboard.service';
import { SalesPeriod } from '../../interfaces/dashboard.interface';

Chart.register(...registerables);

@Component({
  selector: 'app-sales-analytics',
  styleUrl: './sales-analytics.component.css',
  templateUrl: './sales-analytics.component.html',
})
export class SalesAnalyticsComponent implements AfterViewInit, OnDestroy {
  private readonly service = inject(DashboardService);

  @ViewChild('salesChart') salesChart!: ElementRef<HTMLCanvasElement>;

  private chart: Chart | null = null;

  readonly periods: { key: SalesPeriod; label: string }[] = [
    { key: 'today', label: 'Today' },
    { key: 'weekly', label: 'Weekly' },
    { key: 'monthly', label: 'Monthly' },
  ];

  readonly activePeriod = signal<SalesPeriod>('today');

  ngAfterViewInit(): void {
    this.initChart();
  }

  ngOnDestroy(): void {
    this.chart?.destroy();
  }

  setPeriod(period: SalesPeriod): void {
    this.activePeriod.set(period);
    this.service.setPeriod(period);
    this.updateChart();
  }

  private initChart(): void {
    const ctx = this.salesChart.nativeElement.getContext('2d');
    if (!ctx) return;

    const data = this.service.getSalesData();

    this.chart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: data.map(d => d.label),
        datasets: [
          {
            label: 'Sales',
            data: data.map(d => d.sales),
            backgroundColor: '#38bdf8',
            borderRadius: 4,
            yAxisID: 'y',
            order: 2,
          },
          {
            label: 'Revenue',
            data: data.map(d => d.revenue),
            borderColor: '#e84393',
            backgroundColor: 'rgba(232, 67, 147, 0.1)',
            pointBackgroundColor: '#e84393',
            pointBorderColor: '#ffffff',
            pointBorderWidth: 2,
            pointRadius: 4,
            borderWidth: 2,
            tension: 0.3,
            fill: true,
            type: 'line' as const,
            yAxisID: 'y1',
            order: 1,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
        },
        scales: {
          x: {
            grid: { display: false },
            ticks: { color: '#a9a7d1' },
          },
          y: {
            position: 'left',
            grid: { color: 'rgba(255,255,255,0.06)' },
            ticks: { color: '#a9a7d1' },
            beginAtZero: true,
          },
          y1: {
            position: 'right',
            grid: { display: false },
            ticks: { color: '#a9a7d1' },
            beginAtZero: true,
          },
        },
      },
    });
  }

  private updateChart(): void {
    if (!this.chart) return;
    const data = this.service.getSalesData();
    this.chart.data.labels = data.map(d => d.label);
    this.chart.data.datasets[0].data = data.map(d => d.sales);
    this.chart.data.datasets[1].data = data.map(d => d.revenue);
    this.chart.update();
  }
}