namespace DevBoard.API.DTOs
{
    public class User : BaseDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public List<Role>? Roles { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
