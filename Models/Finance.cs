using System.ComponentModel.DataAnnotations;

namespace FinanceControlAPI.Models
{
    public class Finance
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
