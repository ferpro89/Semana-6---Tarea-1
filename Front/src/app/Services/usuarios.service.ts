import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UsuariosService {
  private readonly rutaAPI = 'https://localhost:7020/api/Usuarios';

  constructor(private http: HttpClient) {}

  login(correo: string, password: string) {
    const body = { correo, pwd: password }; 
    return this.http.post<any>(`${this.rutaAPI}/login`, body, { withCredentials: true });
  }

  logout() {
    return this.http.post(`${this.rutaAPI}/logout`, {}, { withCredentials: true });
  }

  me() {
    return this.http.get<any>(`${this.rutaAPI}/me`, { withCredentials: true });
  }
}
