namespace Disney.Utilities
{
    public interface IFileStorage
    {
        Task<string> Create(byte[] file, string contentType, string extension, string container, string nombre);
        Task Delete(string ruta, string container);
    }
}
