import { Component, inject, signal, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { of, switchMap, catchError } from 'rxjs';
import { AdminService } from '../../../core/services/admin.service';
import { AuthService } from '../../../core/services/auth.service';
import { CategoriaDto } from '../../../core/models/admin.models';
import { ProductoPublicoDto } from '../../../core/models/catalogo.models';

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [ReactiveFormsModule, CurrencyPipe],
  styleUrl: '../panel.scss',
  template: `
    <div class="cabecera">
      <h1>Productos</h1>
      <button type="button" class="primario" (click)="mostrarForm.set(!mostrarForm())">
        {{ mostrarForm() ? 'Cerrar' : '+ Nuevo producto' }}
      </button>
    </div>

    @if (mensaje()) { <div class="aviso">{{ mensaje() }}</div> }

    @if (mostrarForm()) {
      <form class="tarjeta" [formGroup]="form" (ngSubmit)="guardar()">
        <label>Nombre <input type="text" formControlName="nombre" /></label>
        <label>Descripción <input type="text" formControlName="descripcion" /></label>
        <div class="fila2">
          <label>Precio (COP) <input type="number" formControlName="precio" /></label>
          <label>Categoría
            <select formControlName="categoriaId">
              <option value="">Sin categoría</option>
              @for (c of categorias(); track c.id) {
                <option [value]="c.id">{{ c.nombre }}</option>
              }
            </select>
          </label>
        </div>

        <div>
          <span class="etiqueta">Tallas y stock</span>
          @for (f of filas(); track $index) {
            <div class="talla-fila">
              <input type="text" placeholder="Talla (S, M, L...)" [value]="f.talla"
                     (input)="cambiarTalla($index, $any($event.target).value)" />
              <input type="number" placeholder="Stock" [value]="f.stock"
                     (input)="cambiarStock($index, $any($event.target).value)" />
              <button type="button" class="texto" (click)="quitarFila($index)">Quitar</button>
            </div>
          }
          <button type="button" class="texto" (click)="agregarFila()">+ Agregar talla</button>
        </div>

        <label>Foto (opcional)
          <input type="file" accept="image/*" (change)="onArchivo($event)" />
        </label>

        <button type="submit" class="primario" [disabled]="guardando()">
          {{ guardando() ? 'Guardando...' : 'Guardar producto' }}
        </button>
      </form>
    }

    @for (p of productos(); track p.id) {
      <div class="producto-fila">
        <div class="mini">
          @if (p.imagenesUrls[0]) { <img [src]="p.imagenesUrls[0]" alt="" /> }
        </div>
        <div class="datos">
          <strong>{{ p.nombre }}</strong>
          <span>{{ p.categoriaNombre ?? 'Sin categoría' }}</span>
        </div>
        <div>
          @for (v of p.variantes; track v.talla) {
            <span class="chip">{{ v.talla }}: {{ v.stock }}</span>
          }
        </div>
        <div class="precio">{{ p.precio | currency: 'COP' : 'symbol-narrow' : '1.0-0' }}</div>
      </div>
    } @empty {
      <p class="vacio">Aún no tienes productos. Crea el primero con el botón de arriba.</p>
    }
  `,
})
export class Productos implements OnInit {
  private admin = inject(AdminService);
  private auth = inject(AuthService);
  private fb = inject(FormBuilder);

  productos = signal<ProductoPublicoDto[]>([]);
  categorias = signal<CategoriaDto[]>([]);
  filas = signal<{ talla: string; stock: number }[]>([{ talla: '', stock: 0 }]);
  mostrarForm = signal(false);
  guardando = signal(false);
  mensaje = signal<string | null>(null);
  private archivo: File | null = null;

  form = this.fb.group({
    nombre: ['', Validators.required],
    descripcion: [''],
    precio: [0, [Validators.required, Validators.min(1)]],
    categoriaId: [''],
  });

  ngOnInit(): void {
    this.cargar();
    this.admin.listarCategorias().subscribe((c) => this.categorias.set(c));
  }

  cargar(): void {
    const tenantId = this.auth.usuarioActual()?.tenantId;
    if (!tenantId) return;
    this.admin.listarProductos(tenantId).subscribe((p) => this.productos.set(p));
  }

  agregarFila(): void {
    this.filas.update((f) => [...f, { talla: '', stock: 0 }]);
  }

  quitarFila(i: number): void {
    this.filas.update((f) => f.filter((_, idx) => idx !== i));
  }

  cambiarTalla(i: number, valor: string): void {
    this.filas.update((f) => f.map((x, idx) => (idx === i ? { ...x, talla: valor } : x)));
  }

  cambiarStock(i: number, valor: string): void {
    this.filas.update((f) => f.map((x, idx) => (idx === i ? { ...x, stock: Number(valor) || 0 } : x)));
  }

  onArchivo(evento: Event): void {
    const input = evento.target as HTMLInputElement;
    this.archivo = input.files?.[0] ?? null;
  }

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.mensaje.set('Revisa el nombre y que el precio sea mayor a cero.');
      return;
    }

    const v = this.form.value;
    const variantes = this.filas()
      .filter((f) => f.talla.trim() !== '')
      .map((f) => ({ talla: f.talla.trim(), stock: f.stock }));

    this.guardando.set(true);
    this.mensaje.set(null);
    let fotoFallo = false;

    this.admin
      .crearProducto({
        nombre: v.nombre!,
        descripcion: v.descripcion ?? '',
        precio: Number(v.precio),
        categoriaId: v.categoriaId ? v.categoriaId : null,
        variantes,
      })
      .pipe(
        switchMap((res) =>
          this.archivo
            ? this.admin.subirImagen(res.id, this.archivo).pipe(
                catchError(() => {
                  fotoFallo = true;
                  return of(null);
                }),
              )
            : of(null),
        ),
      )
      .subscribe({
        next: () => {
          this.guardando.set(false);
          this.mensaje.set(
            fotoFallo
              ? 'Producto creado, pero la foto no se pudo subir.'
              : 'Producto creado correctamente.',
          );
          this.form.reset({ nombre: '', descripcion: '', precio: 0, categoriaId: '' });
          this.filas.set([{ talla: '', stock: 0 }]);
          this.archivo = null;
          this.mostrarForm.set(false);
          this.cargar();
        },
        error: (err) => {
          this.guardando.set(false);
          this.mensaje.set(err.error?.error ?? 'No se pudo guardar el producto.');
        },
      });
  }
}
