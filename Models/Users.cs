namespace FinanceControlAPI.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string User {  get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        //public ICollection<Finance?> Finances { get; set; }
    }
}
