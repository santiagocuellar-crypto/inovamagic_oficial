using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using ProyectoGrado.Application.Common.Interfaces;

namespace ProyectoGrado.Infrastructure.Services;

public class CloudinaryImageStorageService : IImageStorageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryImageStorageService(IConfiguration configuration)
    {
        var cloudName = configuration["Cloudinary:CloudName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> SubirImagenAsync(Stream contenidoArchivo, string nombreArchivo)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(nombreArchivo, contenidoArchivo),
            Folder = "proyecto-grado/productos"
        };

        var resultado = await _cloudinary.UploadAsync(uploadParams);

        if (resultado.Error is not null)
            throw new InvalidOperationException($"Error subiendo imagen: {resultado.Error.Message}");

        return resultado.SecureUrl.ToString();
    }
}
