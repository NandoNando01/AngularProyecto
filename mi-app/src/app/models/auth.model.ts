export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
}

export interface AuthUserData {
  id: number;
  email: string;
  fullName: string;
  token: string;
  expiresAt: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data?: T;
}

export type AuthResponse = ApiResponse<AuthUserData>;
