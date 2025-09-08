import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UsuariosService } from '../../Services/usuarios.service';
import { FormsModule } from '@angular/forms'; 
import { CommonModule } from '@angular/common'; 

@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [FormsModule, CommonModule],
  template: `
    <div class="login-container">
      <h2 class="login-title">Login</h2>
      <form (ngSubmit)="onSubmitLogin()" class="login-form">
        <input 
          [(ngModel)]="email" 
          name="email" 
          type="email" 
          placeholder="Correo electrónico" 
          required 
          class="form-input" 
        />
        <input 
          [(ngModel)]="password" 
          name="password" 
          type="password" 
          placeholder="Contraseña" 
          required 
          class="form-input" 
        />
        <button type="submit" class="btn btn-primary">Ingresar</button>
      </form>
      <div class="session-actions">
        <button class="btn btn-secondary" (click)="onLogout()">Cerrar sesión</button>
        <button class="btn btn-info" (click)="checkSession()">Verificar sesión</button>
      </div>
    </div>
  `,
  styles: [`
    .login-container {
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      height: 100vh;
      background-color: #f7f9fc;
      font-family: Arial, sans-serif;
    }

    .login-form {
      padding: 40px;
      background-color: #ffffff;
      border-radius: 10px;
      box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
      width: 100%;
      max-width: 380px;
      text-align: center;
    }
    
    .login-title {
      text-align: center;
      color: #333;
      margin-bottom: 30px;
    }
    
    .form-input {
      width: 100%;
      padding: 12px;
      margin-bottom: 20px;
      border: 1px solid #e0e0e0;
      border-radius: 5px;
      font-size: 16px;
    }
    
    .btn-primary {
      width: 100%;
      padding: 12px;
      background-color: #007bff;
      color: white;
      border: none;
      border-radius: 5px;
      font-size: 16px;
      cursor: pointer;
    }

    .btn-primary:hover {
      background-color: #0056b3;
    }
    
    .session-actions {
      display: flex;
      justify-content: space-around;
      margin-top: 20px;
      width: 100%;
      max-width: 380px;
    }
    
    .btn-secondary, .btn-info {
      flex: 1;
      padding: 10px;
      border-radius: 5px;
      border: 1px solid #ccc;
      background-color: #f1f1f1;
      color: #555;
      cursor: pointer;
      margin: 0 5px;
    }
  `]
})
export class UsuariosComponent {
  email = '';
  password = '';

  constructor(private usuariosService: UsuariosService, private router: Router) {}

  onSubmitLogin() {
    const body = { correo: this.email.trim(), pwd: this.password.trim() }; 
    this.usuariosService.login(body.correo, body.pwd).subscribe({
      next: res => {
        console.log('Login correcto:', res);
        this.router.navigate(['/dashboard']);
      },
      error: () => alert('Correo o contraseña incorrectos')
    });
  }

  onLogout() {
    this.usuariosService.logout().subscribe(() => console.log('Sesión cerrada'));
  }

  checkSession() {
    this.usuariosService.me().subscribe({
      next: res => console.log('Usuario logueado:', res),
      error: () => console.log('No hay sesión activa')
    });
  }
}