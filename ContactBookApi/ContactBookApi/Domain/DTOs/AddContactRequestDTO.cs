namespace ContactBookApi.Domain.DTOs
{
    public class AddContactRequestDTO
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? FaceBookHandle { get; set; }
    }
}
