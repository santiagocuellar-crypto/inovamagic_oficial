import { Component, inject, signal, OnInit } from '@angular/core';
import { AdminService } from '../../../core/services/admin.service';
import { CategoriaDto } from '../../../core/models/admin.models';

@Component({
  selector: 'app-categorias',
  standalone: true,
  styleUrl: '../panel.scss',
  template: `
    <div class="cabecera"><h1>Categorías</h1></div>

    <div class="tarjeta">
      <div class="fila-cat">
        <input type="text" placeholder="Ej: Uniformes, Vestidos..." [value]="nombre()"
               (input)="nombre.set($any($event.target).value)" />
        <button type="button" class="primario" (click)="crear()">Crear</button>
      </div>
    </div>

    @for (c of categorias(); track c.id) {
      <div class="producto-fila"><div class="datos"><strong>{{ c.nombre }}</strong></div></div>
    } @empty {
      <p class="vacio">Todavía no tienes categorías.</p>
    }
  `,
})
export class Categorias implements OnInit {
  private admin = inject(AdminService);
  categorias = signal<CategoriaDto[]>([]);
  nombre = signal('');

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.admin.listarCategorias().subscribe((c) => this.categorias.set(c));
  }

  crear(): void {
    const valor = this.nombre().trim();
    if (!valor) return;
    this.admin.crearCategoria(valor).subscribe(() => {
      this.nombre.set('');
      this.cargar();
    });
  }
}
