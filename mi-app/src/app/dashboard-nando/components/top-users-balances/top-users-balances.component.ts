import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-top-users-balances',
  imports: [CommonModule],
  styleUrl: './top-users-balances.component.css',
  templateUrl: './top-users-balances.component.html',
})
export class TopUsersBalancesComponent {
  private readonly service = inject(DashboardService);
  readonly users = this.service.getTopUsers();
}