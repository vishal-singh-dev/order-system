using System.ComponentModel.DataAnnotations.Schema;

namespace auth_service.Models
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public virtual User user { get; set; }
    }
}
