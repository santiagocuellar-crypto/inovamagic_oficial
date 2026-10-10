export interface VarianteDto {
  talla: string;
  stock: number;
}

export interface ProductoPublicoDto {
  id: string;
  nombre: string;
  descripcion: string;
  precio: number;
  categoriaNombre: string | null;
  imagenesUrls: string[];
  variantes: VarianteDto[];
}
