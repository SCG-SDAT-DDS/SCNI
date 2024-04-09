
namespace Negocio.Servicios
{
    public interface IServicio<T>
    {
        T BuscarPorId(int id);
        bool Guardar(T obj);
        bool Modificar(T obj);
        bool CambiarHabilitado(int id, bool habilitado);
    }
}
