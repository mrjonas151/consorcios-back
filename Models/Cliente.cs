namespace ConsorcioAPI.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<Cota> Cotas { get; set; } = new List<Cota>();
}