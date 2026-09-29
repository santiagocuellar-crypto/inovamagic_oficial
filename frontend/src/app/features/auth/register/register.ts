import { Component, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  cargando = signal(false);
  errorMensaje = signal<string | null>(null);

  form = this.fb.group({
    nombreTenant: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  enviar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.cargando.set(true);
    this.errorMensaje.set(null);

    this.authService
      .registrar({
        nombreTenant: this.form.value.nombreTenant!,
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
            err.error?.error ?? 'No se pudo crear la cuenta',
          );
        },
      });
  }
}
