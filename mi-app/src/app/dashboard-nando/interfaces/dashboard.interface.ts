export interface KpiData {
  title: string;
  value: string;
  gradientStart: string;
  gradientEnd: string;
  icon: string;
  progress: number;
}

export interface RevenueDonutData {
  percentage: number;
  totalSalesToday: number;
  yesterdaySales: number;
}

export interface SalesDataPoint {
  label: string;
  sales: number;
  revenue: number;
}

export interface UserBalance {
  name: string;
  avatarInitial: string;
  memberSince: string;
  currency: string;
  amountBtc: number;
  amountUsd: number;
}

export interface RevenueHistoryItem {
  marketplace: string;
  date: string;
  payouts: number;
  status: 'Pending' | 'Completed' | 'Cancelled';
}

export type SalesPeriod = 'today' | 'weekly' | 'monthly';