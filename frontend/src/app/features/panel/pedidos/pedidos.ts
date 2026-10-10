import { Component, inject, signal, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { AdminService } from '../../../core/services/admin.service';
import { PedidoDto, ESTADOS } from '../../../core/models/admin.models';

@Component({
  selector: 'app-pedidos',
  standalone: true,
  imports: [CurrencyPipe, DatePipe],
  styleUrl: '../panel.scss',
  template: `
    <div class="cabecera"><h1>Pedidos</h1></div>

    @for (p of pedidos(); track p.id) {
      <div class="tarjeta pedido">
        <div class="top">
          <div>
            <strong>{{ p.clienteNombre }}</strong>
            <span class="sub">{{ p.clienteTelefono }} · {{ p.direccionCalle }}, {{ p.direccionCiudad }}</span>
            @if (p.direccionNotas) { <span class="sub">{{ p.direccionNotas }}</span> }
          </div>
          <select (change)="cambiarEstado(p, $any($event.target).value)">
            @for (e of estados; track e) {
              <option [value]="e" [selected]="e === p.estado">{{ e }}</option>
            }
          </select>
        </div>
        <ul>
          @for (i of p.items; track $index) {
            <li>{{ i.cantidad }} × {{ i.productoNombre }} (talla {{ i.talla }})</li>
          }
        </ul>
        <div class="total">
          Total {{ p.total | currency: 'COP' : 'symbol-narrow' : '1.0-0' }} ·
          {{ p.fechaCreacion | date: 'dd/MM/yyyy HH:mm' }}
        </div>
      </div>
    } @empty {
      <p class="vacio">Todavía no hay pedidos.</p>
    }
  `,
})
export class Pedidos implements OnInit {
  private admin = inject(AdminService);
  pedidos = signal<PedidoDto[]>([]);
  estados = ESTADOS;

  ngOnInit(): void {
    this.admin.listarPedidos().subscribe((p) => this.pedidos.set(p));
  }

  cambiarEstado(pedido: PedidoDto, nuevoEstado: string): void {
    this.admin.actualizarEstado(pedido.id, nuevoEstado).subscribe(() => {
      this.pedidos.update((lista) =>
        lista.map((p) => (p.id === pedido.id ? { ...p, estado: nuevoEstado } : p)),
      );
    });
  }
}
