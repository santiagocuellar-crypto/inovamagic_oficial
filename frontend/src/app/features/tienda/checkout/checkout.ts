import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { CarritoService } from '../../../core/services/carrito.service';
import { PedidosService } from '../../../core/services/pedidos.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './checkout.html',
  styleUrl: './checkout.scss',
})
export class Checkout implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  private pedidosService = inject(PedidosService);
  carritoService = inject(CarritoService);

  tenantId = '';
  enviando = signal(false);
  error = signal<string | null>(null);
  pedidoConfirmado = signal<string | null>(null);

  form = this.fb.group({
    clienteNombre: ['', [Validators.required]],
    clienteTelefono: ['', [Validators.required]],
    clienteEmail: [''],
    direccionCalle: ['', [Validators.required]],
    direccionCiudad: ['', [Validators.required]],
    direccionNotas: [''],
  });

  ngOnInit(): void {
    this.tenantId = this.route.snapshot.paramMap.get('tenantId') ?? '';
    this.carritoService.cargarParaTenant(this.tenantId);
  }

  enviar(): void {
    if (this.form.invalid || this.carritoService.items().length === 0) {
      this.form.markAllAsTouched();
      return;
    }

    this.enviando.set(true);
    this.error.set(null);

    this.pedidosService
      .crear({
        tenantId: this.tenantId,
        clienteNombre: this.form.value.clienteNombre!,
        clienteTelefono: this.form.value.clienteTelefono!,
        clienteEmail: this.form.value.clienteEmail || undefined,
        direccionCalle: this.form.value.direccionCalle!,
        direccionCiudad: this.form.value.direccionCiudad!,
        direccionNotas: this.form.value.direccionNotas || undefined,
        items: this.carritoService.items().map((i) => ({
          productoId: i.productoId,
          talla: i.talla,
          cantidad: i.cantidad,
        })),
      })
      .subscribe({
        next: (res) => {
          this.enviando.set(false);
          this.pedidoConfirmado.set(res.id);
          this.carritoService.vaciar();
        },
        error: (err) => {
          this.enviando.set(false);
          this.error.set(err.error?.error ?? 'No se pudo completar el pedido');
        },
      });
  }
}
