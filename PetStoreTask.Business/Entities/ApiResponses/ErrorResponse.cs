namespace PetStoreTask.Business.Entities.ApiResponses
{
    public class ErrorResponse
    {
        public int Code { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }
    }
}
