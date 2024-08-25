namespace Fiap.TechChallenge.Domain.Entities;

public class Contact(string name, string email, string phoneNumber, short dddNumber)
{
    public long Id { get; set; }
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string PhoneNumber { get; set; } = phoneNumber;
    public short DddNumber { get; set; } = dddNumber;
}