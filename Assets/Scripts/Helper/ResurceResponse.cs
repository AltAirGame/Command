namespace Helper
{
    public class ResurceResponse
    {
        public string body;
        public int  code=0;
        public bool isSuccess;
        public string message;

        public ResurceResponse()
        {
            
        }

        public ResurceResponse(string body, int code, bool isSuccess, string message)
        {
            this.body = body;
            this.code = code;
            this.isSuccess = isSuccess;
            this.message = message;
        }
    }
}