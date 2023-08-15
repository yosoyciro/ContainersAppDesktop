namespace ContainersDesktop.Contracts.Services;
public interface IRepository<T> where T : class
{
    public interface IClaListServicio
    {
        Task<List<T>> ObtenerTodos();
        Task<T> ObtenerPorId();
        Task<bool> Agregar(T objeto);
        Task<bool> Modificar(T objeto);
        Task<bool> BorrarRecuperar(T objeto);
    }
}
