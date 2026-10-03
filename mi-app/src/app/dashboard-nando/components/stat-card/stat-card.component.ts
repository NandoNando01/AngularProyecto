import { Component, input } from '@angular/core';
import { KpiData } from '../../interfaces/dashboard.interface';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-stat-card',
  imports: [MatIconModule],
  styleUrl: './stat-card.component.css',
  templateUrl: './stat-card.component.html',
})
export class StatCardComponent {
  readonly data = input.required<KpiData>();

  getCircumference(r: number): number {
    return 2 * Math.PI * r;
  }

  getOffset(r: number, progress: number): number {
    const circumference = 2 * Math.PI * r;
    return circumference - (circumference * progress / 100);
  }
}