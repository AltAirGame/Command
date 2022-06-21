namespace MHamidi
{
    public class RepositoryResponse<T>
    {
        public T data;
        public Error error;

        public RepositoryResponse()
        {
        }

        public RepositoryResponse(T data, Error error)
        {
            this.data = data;
            this.error = error;
        }
    }
}