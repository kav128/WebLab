#nullable disable
namespace lab2.Entities
{
    public record User(string FirstName, string LastName)
    {
        public string ProfilePic { get; set; }
    }
}
