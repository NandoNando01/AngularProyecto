import { HttpErrorResponse } from '@angular/common/http';

export function getApiErrorMessage(error: unknown): string {
  if (error instanceof HttpErrorResponse && error.error?.message) {
    return error.error.message;
  }
  return 'Ha ocurrido un error inesperado. Inténtelo nuevamente.';
}
