namespace WebApplication1.Library.DB
{
    public interface IDBRequest<T>
    {
        Task<T> Get(string text);
    }
}
