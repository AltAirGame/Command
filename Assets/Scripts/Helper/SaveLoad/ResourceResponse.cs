namespace GameSystems.Core
{
    public class ResourceResponse
    {
        public string body;
        public int  code=0;
        public bool isSuccess;
        public string message;

        public ResourceResponse()
        {
            
        }

        public ResourceResponse(string body, int code, bool isSuccess, string message)
        {
            this.body = body;
            this.code = code;
            this.isSuccess = isSuccess;
            this.message = message;
        }
    }
}