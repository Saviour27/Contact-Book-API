namespace ContactBookApi.Domain.Generics
{
    public class Error
    {
        public int Code { get; set; }
        public string? Type { get; set; }
        public string? Message { get; set; }
    }
}
