namespace MHamidi
{
    public class Error
    {
        public string message;
        public int code;

        public Error()
        {
        }

        public Error(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }
}