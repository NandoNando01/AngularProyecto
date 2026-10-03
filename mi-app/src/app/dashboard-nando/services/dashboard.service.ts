import { Injectable, signal } from '@angular/core';
import { KpiData, RevenueDonutData, SalesDataPoint, UserBalance, RevenueHistoryItem, SalesPeriod } from '../interfaces/dashboard.interface';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly _kpiData: KpiData[] = [
    { title: 'Total Revenue', value: '$58,947', gradientStart: '#4a6cf7', gradientEnd: '#7c3aed', icon: 'show_chart', progress: 78 },
    { title: "Today's Sales", value: '127', gradientStart: '#0cd4d4', gradientEnd: '#2563eb', icon: 'shopping_cart', progress: 65 },
    { title: 'Conversion', value: '0.58%', gradientStart: '#8b5cf6', gradientEnd: '#a855f7', icon: 'pie_chart', progress: 42 },
    { title: "Today's Visits", value: '78.41k', gradientStart: '#ec4899', gradientEnd: '#d946ef', icon: 'visibility', progress: 91 },
  ];

  private readonly _revenueDonut: RevenueDonutData = {
    percentage: 75,
    totalSalesToday: 178,
    yesterdaySales: 170,
  };

  private readonly _salesData: Record<SalesPeriod, SalesDataPoint[]> = {
    today: [
      { label: '06:00', sales: 120, revenue: 2400 },
      { label: '08:00', sales: 450, revenue: 8900 },
      { label: '10:00', sales: 890, revenue: 17200 },
      { label: '12:00', sales: 1200, revenue: 23100 },
      { label: '14:00', sales: 1560, revenue: 29800 },
      { label: '16:00', sales: 1890, revenue: 35600 },
      { label: '18:00', sales: 2100, revenue: 41200 },
      { label: '20:00', sales: 1750, revenue: 33800 },
    ],
    weekly: [
      { label: 'Mon', sales: 3200, revenue: 61200 },
      { label: 'Tue', sales: 2800, revenue: 54300 },
      { label: 'Wed', sales: 4100, revenue: 78900 },
      { label: 'Thu', sales: 3600, revenue: 69500 },
      { label: 'Fri', sales: 5200, revenue: 100200 },
      { label: 'Sat', sales: 4800, revenue: 92400 },
      { label: 'Sun', sales: 2100, revenue: 40800 },
    ],
    monthly: [
      { label: 'Week 1', sales: 14200, revenue: 275000 },
      { label: 'Week 2', sales: 16800, revenue: 321500 },
      { label: 'Week 3', sales: 12500, revenue: 241800 },
      { label: 'Week 4', sales: 19100, revenue: 367200 },
    ],
  };

  private readonly _topUsers: UserBalance[] = [
    { name: 'Sarah Johnson', avatarInitial: 'S', memberSince: '2022', currency: 'BTC', amountBtc: 2.45, amountUsd: 142850 },
    { name: 'Michael Chen', avatarInitial: 'M', memberSince: '2021', currency: 'BTC', amountBtc: 1.82, amountUsd: 106090 },
    { name: 'Emily Rodriguez', avatarInitial: 'E', memberSince: '2023', currency: 'BTC', amountBtc: 1.34, amountUsd: 78120 },
    { name: 'James Williams', avatarInitial: 'J', memberSince: '2020', currency: 'BTC', amountBtc: 0.95, amountUsd: 55370 },
    { name: 'Emma Thompson', avatarInitial: 'E', memberSince: '2022', currency: 'BTC', amountBtc: 0.61, amountUsd: 35560 },
  ];

  private readonly _revenueHistory: RevenueHistoryItem[] = [
    { marketplace: 'Amazon', date: '12 Sep 2024', payouts: 12480, status: 'Completed' },
    { marketplace: 'eBay', date: '11 Sep 2024', payouts: 8750, status: 'Pending' },
    { marketplace: 'Shopify', date: '10 Sep 2024', payouts: 15200, status: 'Completed' },
    { marketplace: 'Walmart', date: '09 Sep 2024', payouts: 6200, status: 'Cancelled' },
    { marketplace: 'Etsy', date: '08 Sep 2024', payouts: 4300, status: 'Completed' },
  ];

  readonly currentPeriod = signal<SalesPeriod>('today');

  getKpiData(): KpiData[] {
    return this._kpiData;
  }

  getRevenueDonut(): RevenueDonutData {
    return this._revenueDonut;
  }

  getSalesData(): SalesDataPoint[] {
    return this._salesData[this.currentPeriod()];
  }

  getTopUsers(): UserBalance[] {
    return this._topUsers;
  }

  getRevenueHistory(): RevenueHistoryItem[] {
    return this._revenueHistory;
  }

  setPeriod(period: SalesPeriod): void {
    this.currentPeriod.set(period);
  }
}