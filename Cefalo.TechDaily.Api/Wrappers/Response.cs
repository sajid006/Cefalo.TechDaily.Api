namespace Cefalo.TechDaily.Api.Wrappers
{
    public class Response<T>
    {
        public Response() { }
        public Response(T response) {
            status = "success";
            this.response = response;
            message = "";
        }
        public string status { get; set; }
        public string message { get; set; }
        public T response { get; set; }
    }
}
