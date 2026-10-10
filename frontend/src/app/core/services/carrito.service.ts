import { Injectable, signal, computed } from '@angular/core';
import { ItemCarrito } from '../models/pedido.models';

@Injectable({ providedIn: 'root' })
export class CarritoService {
  private tenantIdActual: string | null = null;
  items = signal<ItemCarrito[]>([]);

  totalItems = computed(() =>
    this.items().reduce((suma, i) => suma + i.cantidad, 0),
  );

  totalPrecio = computed(() =>
    this.items().reduce((suma, i) => suma + i.precio * i.cantidad, 0),
  );

  cargarParaTenant(tenantId: string): void {
    this.tenantIdActual = tenantId;
    const guardado = localStorage.getItem(this.claveStorage());
    this.items.set(guardado ? JSON.parse(guardado) : []);
  }

  agregar(item: ItemCarrito): void {
    const actuales = this.items();
    const existente = actuales.find(
      (i) => i.productoId === item.productoId && i.talla === item.talla,
    );

    if (existente) {
      const nuevaCantidad = Math.min(
        existente.cantidad + item.cantidad,
        existente.stockDisponible,
      );
      this.items.set(
        actuales.map((i) =>
          i === existente ? { ...i, cantidad: nuevaCantidad } : i,
        ),
      );
    } else {
      this.items.set([...actuales, item]);
    }

    this.guardar();
  }

  actualizarCantidad(productoId: string, talla: string, cantidad: number): void {
    this.items.set(
      this.items().map((i) =>
        i.productoId === productoId && i.talla === talla
          ? { ...i, cantidad: Math.max(1, Math.min(cantidad, i.stockDisponible)) }
          : i,
      ),
    );
    this.guardar();
  }

  quitar(productoId: string, talla: string): void {
    this.items.set(
      this.items().filter(
        (i) => !(i.productoId === productoId && i.talla === talla),
      ),
    );
    this.guardar();
  }

  vaciar(): void {
    this.items.set([]);
    this.guardar();
  }

  private guardar(): void {
    localStorage.setItem(this.claveStorage(), JSON.stringify(this.items()));
  }

  private claveStorage(): string {
    return `carrito_${this.tenantIdActual}`;
  }
}
