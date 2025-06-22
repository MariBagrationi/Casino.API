
namespace Casino.Domain.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public virtual User User { get; set; } = null!;
    }

}
