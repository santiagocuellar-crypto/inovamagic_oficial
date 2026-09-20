namespace ProyectoGrado.Application.Common.Interfaces;

public interface IImageStorageService
{
    Task<string> SubirImagenAsync(Stream contenidoArchivo, string nombreArchivo);
}
