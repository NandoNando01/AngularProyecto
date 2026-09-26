import { Component, ElementRef, ViewChild, AfterViewInit, inject, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Chart, registerables } from 'chart.js';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';
import { AuthService } from '../../services/auth.service';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatToolbarModule,
    MatListModule,
    MatChipsModule,
    MatIconModule,
    MatRippleModule,
  ],
  styleUrl: './dashboard.component.css',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements AfterViewInit, OnDestroy {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  @ViewChild('salesChart') salesChartCanvas!: ElementRef<HTMLCanvasElement>;

  private chart: Chart | null = null;

  readonly currentUser = this.authService.currentUser;

  readonly stats = [
    { label: 'Usuarios activos', value: '1,284', icon: 'people', color: '#667eea' },
    { label: 'Productos', value: '3,721', icon: 'inventory_2', color: '#764ba2' },
    { label: 'Ventas hoy', value: '$12,430', icon: 'trending_up', color: '#2ecc71' },
    { label: 'Pedidos', value: '247', icon: 'shopping_cart', color: '#f39c12' },
  ];

  readonly actividades = [
    { name: 'Inicio de sesión exitoso', icon: 'login', status: 'Completado' },
    { name: 'Verificación de perfil', icon: 'verified_user', status: 'Completado' },
    { name: 'Actualización de datos', icon: 'update', status: 'Completado' },
  ];

  readonly monthlySales = {
    labels: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun'],
    values: [65, 72, 80, 75, 90, 95],
  };

  ngAfterViewInit(): void {
    this.initChart();
  }

  ngOnDestroy(): void {
    this.chart?.destroy();
  }

  private initChart(): void {
    const ctx = this.salesChartCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    this.chart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: this.monthlySales.labels,
        datasets: [{
          label: 'Ventas mensuales',
          data: this.monthlySales.values,
          borderColor: '#667eea',
          backgroundColor: 'rgba(102, 126, 234, 0.1)',
          fill: true,
          tension: 0.4,
          pointBackgroundColor: '#667eea',
          pointBorderColor: '#fff',
          pointBorderWidth: 2,
          pointRadius: 5,
          borderWidth: 3,
        }],
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
            ticks: { color: '#636e72' },
          },
          y: {
            grid: { color: 'rgba(0,0,0,0.05)' },
            ticks: { color: '#636e72' },
            beginAtZero: true,
          },
        },
      },
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}