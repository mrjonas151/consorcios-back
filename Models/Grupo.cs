namespace ConsorcioAPI.Models;

public class Grupo
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int AdministradoraId { get; set; }
    public Administradora Administradora { get; set; } = null!;
    public ICollection<Cota> Cotas { get; set; } = new List<Cota>();
}