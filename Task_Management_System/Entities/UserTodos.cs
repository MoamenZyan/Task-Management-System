namespace Project.API.Entities
{
    public class UserTodos
    {
        public int User_Id { get; set; }
        public int Todo_Id { get; set; }

        public User User { get; set; } = null!;
        public Todo Todo { get; set; } = null!;
    }
}
