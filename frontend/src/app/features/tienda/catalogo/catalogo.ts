import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductosService } from '../../../core/services/productos.service';
import { CarritoService } from '../../../core/services/carrito.service';
import { ProductoPublicoDto } from '../../../core/models/catalogo.models';

@Component({
  selector: 'app-catalogo',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './catalogo.html',
  styleUrl: './catalogo.scss',
})
export class Catalogo implements OnInit {
  private route = inject(ActivatedRoute);
  private productosService = inject(ProductosService);
  carritoService = inject(CarritoService);

  tenantId = '';
  productos = signal<ProductoPublicoDto[]>([]);
  cargando = signal(true);
  error = signal<string | null>(null);

  tallaSeleccionada = new Map<string, string>();

  ngOnInit(): void {
    this.tenantId = this.route.snapshot.paramMap.get('tenantId') ?? '';
    this.carritoService.cargarParaTenant(this.tenantId);

    this.productosService.listarPublico(this.tenantId).subscribe({
      next: (productos) => {
        this.productos.set(productos);
        this.cargando.set(false);
      },
      error: () => {
        this.error.set('No se pudo cargar la tienda. Intenta de nuevo.');
        this.cargando.set(false);
      },
    });
  }

  seleccionarTalla(productoId: string, talla: string): void {
    this.tallaSeleccionada.set(productoId, talla);
  }

  tallaDe(productoId: string): string | undefined {
    return this.tallaSeleccionada.get(productoId);
  }

  agregarAlCarrito(producto: ProductoPublicoDto): void {
    const talla = this.tallaDe(producto.id) ?? producto.variantes[0]?.talla;
    if (!talla) return;

    const variante = producto.variantes.find((v) => v.talla === talla);
    if (!variante || variante.stock === 0) return;

    this.carritoService.agregar({
      productoId: producto.id,
      nombre: producto.nombre,
      talla,
      precio: producto.precio,
      cantidad: 1,
      imagenUrl: producto.imagenesUrls[0] ?? null,
      stockDisponible: variante.stock,
    });
  }
}
