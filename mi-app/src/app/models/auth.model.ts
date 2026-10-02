import type { components } from './generated/api-types';

type GeneratedLoginDto = components['schemas']['LoginDto'];
type GeneratedRegisterDto = components['schemas']['RegisterDto'];
type GeneratedAuthResponseDto = components['schemas']['AuthResponseDto'];

export interface LoginRequest extends GeneratedLoginDto {}
export interface RegisterRequest extends GeneratedRegisterDto {}
export interface AuthUserData extends GeneratedAuthResponseDto {}

export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data?: T;
}

export type AuthResponse = ApiResponse<AuthUserData>;
