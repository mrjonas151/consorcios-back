namespace ConsorcioAPI.Models;

public class Administradora
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}