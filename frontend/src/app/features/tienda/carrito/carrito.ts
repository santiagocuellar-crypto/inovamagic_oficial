import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CarritoService } from '../../../core/services/carrito.service';

@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './carrito.html',
  styleUrl: './carrito.scss',
})
export class Carrito implements OnInit {
  private route = inject(ActivatedRoute);
  carritoService = inject(CarritoService);

  tenantId = '';

  ngOnInit(): void {
    this.tenantId = this.route.snapshot.paramMap.get('tenantId') ?? '';
    this.carritoService.cargarParaTenant(this.tenantId);
  }

  cambiarCantidad(productoId: string, talla: string, cantidad: number): void {
    this.carritoService.actualizarCantidad(productoId, talla, cantidad);
  }

  quitar(productoId: string, talla: string): void {
    this.carritoService.quitar(productoId, talla);
  }
}
