namespace ConsorcioAPI.Models;

public class Cota
{
    public int Id { get; set; }
    public string NumeroCota { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Status { get; set; } = string.Empty;

    public int GrupoId { get; set; }
    public Grupo Grupo { get; set; } = null!;

    public int? ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
}