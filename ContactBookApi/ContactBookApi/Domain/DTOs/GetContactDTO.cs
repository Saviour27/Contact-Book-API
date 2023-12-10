namespace ContactBookApi.Domain.DTOs
{
    public class GetContactDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? FaceBookHandle { get; set; }
    }
}
