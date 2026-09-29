import { Component, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  cargando = signal(false);
  errorMensaje = signal<string | null>(null);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  enviar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.cargando.set(true);
    this.errorMensaje.set(null);

    this.authService
      .login({
        email: this.form.value.email!,
        password: this.form.value.password!,
      })
      .subscribe({
        next: () => {
          this.cargando.set(false);
          this.router.navigateByUrl('/');
        },
        error: (err) => {
          this.cargando.set(false);
          this.errorMensaje.set(
            err.error?.error ?? 'Correo o contraseña incorrectos',
          );
        },
      });
  }
}
