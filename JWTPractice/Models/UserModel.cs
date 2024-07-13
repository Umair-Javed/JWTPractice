namespace JWTPractice.Models
{
    
    public class UserModel
    {
        private readonly static List<UserModel> userModels = new List<UserModel> {
            new UserModel { Name = "Umair", Role = "Admin", Username = "Umair", Password = "password"},
            new UserModel { Name = "Javed", Role = "Security", Username = "Javed", Password = "password"},
        };

        public string Name { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public static List<UserModel> GetUserModels()
        {
            return userModels;
        }
    }
}
