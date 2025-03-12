using ConsorcioAPI.Data;
using ConsorcioAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace ConsorcioAPI.Controllers;

public static class CotasController
{
    public static RouteGroupBuilder MapCotas(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (Cota cota, AppDbContext db) =>
        {
            var grupo = await db.Grupos.FindAsync(cota.GrupoId);
            if (grupo == null)
                return Results.NotFound("Grupo não encontrado");

            if (cota.ClienteId.HasValue)
            {
                var cliente = await db.Clientes.FindAsync(cota.ClienteId);
                if (cliente == null)
                    return Results.NotFound("Cliente não encontrado");
            }

            db.Cotas.Add(cota);
            await db.SaveChangesAsync();
            return Results.Created($"/cotas/{cota.Id}", cota);
        });

        group.MapGet("/", async (AppDbContext db) =>
            await db.Cotas
                .Include(c => c.Grupo)
                .Include(c => c.Cliente)
                .ToListAsync());

        group.MapGet("/{id}", async (int id, AppDbContext db) =>
        {
            var cota = await db.Cotas
                .Include(c => c.Grupo)
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.Id == id);

            return cota is null ? Results.NotFound("Cota não encontrada") : Results.Ok(cota);
        });

        group.MapPut("/{id}", async (int id, Cota cotaAtualizada, AppDbContext db) =>
        {
            var cota = await db.Cotas.FindAsync(id);
            if (cota is null)
                return Results.NotFound("Cota não encontrada");

            var grupo = await db.Grupos.FindAsync(cotaAtualizada.GrupoId);
            if (grupo is null)
                return Results.NotFound("Grupo não encontrado");

            if (cotaAtualizada.ClienteId.HasValue)
            {
                var cliente = await db.Clientes.FindAsync(cotaAtualizada.ClienteId);
                if (cliente is null)
                    return Results.NotFound("Cliente não encontrado");
            }

            cota.NumeroCota = cotaAtualizada.NumeroCota;
            cota.Valor = cotaAtualizada.Valor;
            cota.Status = cotaAtualizada.Status;
            cota.GrupoId = cotaAtualizada.GrupoId;
            cota.ClienteId = cotaAtualizada.ClienteId;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, AppDbContext db) =>
        {
            var cota = await db.Cotas.FindAsync(id);
            if (cota is null)
                return Results.NotFound("Cota não encontrada");

            db.Cotas.Remove(cota);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return group;
    }
}