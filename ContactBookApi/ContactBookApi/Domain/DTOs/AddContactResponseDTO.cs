namespace ContactBookApi.Domain.DTOs
{
    public class AddContactResponseDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? FaceBookHandle { get; set; }

        public string? AddedBy { get; set; }
    }
}
