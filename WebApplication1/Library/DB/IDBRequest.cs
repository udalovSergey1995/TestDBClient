namespace WebApplication1.Library.DB
{
    public interface IDBRequest<T>
    {
        Task<T> Send(string text);
    }
}
