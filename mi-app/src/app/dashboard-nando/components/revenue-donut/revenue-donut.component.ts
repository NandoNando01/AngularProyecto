import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, inject } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { DashboardService } from '../../services/dashboard.service';
import { MatIconModule } from '@angular/material/icon';

Chart.register(...registerables);

@Component({
  selector: 'app-revenue-donut',
  imports: [MatIconModule],
  styleUrl: './revenue-donut.component.css',
  templateUrl: './revenue-donut.component.html',
})
export class RevenueDonutComponent implements AfterViewInit, OnDestroy {
  private readonly service = inject(DashboardService);

  @ViewChild('donutCanvas') donutCanvas!: ElementRef<HTMLCanvasElement>;

  private chart: Chart | null = null;

  readonly data = this.service.getRevenueDonut();

  ngAfterViewInit(): void {
    this.initChart();
  }

  ngOnDestroy(): void {
    this.chart?.destroy();
  }

  private initChart(): void {
    const ctx = this.donutCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    this.chart = new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['Filled', 'Remaining'],
        datasets: [{
          data: [this.data.percentage, 100 - this.data.percentage],
          backgroundColor: ['#2fd8d8', '#2a2477'],
          borderWidth: 0,
        }],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '80%',
        plugins: {
          legend: { display: false },
        },
      },
    });
  }
}