import { Component, inject, signal } from '@angular/core';
import { DashboardService } from './services/dashboard.service';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HeaderComponent } from './components/header/header.component';
import { StatCardComponent } from './components/stat-card/stat-card.component';
import { RevenueDonutComponent } from './components/revenue-donut/revenue-donut.component';
import { SalesAnalyticsComponent } from './components/sales-analytics/sales-analytics.component';
import { TopUsersBalancesComponent } from './components/top-users-balances/top-users-balances.component';
import { RevenueHistoryComponent } from './components/revenue-history/revenue-history.component';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-dashboard-nando',
  imports: [
    SidebarComponent,
    HeaderComponent,
    StatCardComponent,
    RevenueDonutComponent,
    SalesAnalyticsComponent,
    TopUsersBalancesComponent,
    RevenueHistoryComponent,
    MatIconModule,
  ],
  styleUrl: './dashboard-nando.component.css',
  templateUrl: './dashboard-nando.component.html',
})
export class DashboardNandoComponent {
  private readonly service = inject(DashboardService);
  readonly kpiData = this.service.getKpiData();
  readonly sidebarOpen = signal(false);

  toggleSidebar(): void {
    this.sidebarOpen.update(v => !v);
  }
}