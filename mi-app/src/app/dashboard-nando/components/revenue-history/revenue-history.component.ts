import { Component, inject } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-revenue-history',
  imports: [CommonModule],
  styleUrl: './revenue-history.component.css',
  templateUrl: './revenue-history.component.html',
})
export class RevenueHistoryComponent {
  private readonly service = inject(DashboardService);
  readonly items = this.service.getRevenueHistory();
}