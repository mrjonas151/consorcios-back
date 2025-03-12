using ConsorcioAPI.Data;
using ConsorcioAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace ConsorcioAPI.Controllers;

public static class GrupoController
{
    public static RouteGroupBuilder MapGrupo(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (Grupo grupo, AppDbContext db) =>
        {
            var administradora = await db.Administradoras.FindAsync(grupo.AdministradoraId);
            if (administradora is null)
                return Results.NotFound("Administradora não encontrada");

            db.Grupos.Add(grupo);
            await db.SaveChangesAsync();
            return Results.Created($"/grupos/{grupo.Id}", grupo);
        });

        group.MapGet("/", async (AppDbContext db) =>
            await db.Grupos
                .Include(g => g.Administradora)
                .Include(g => g.Cotas)
                .ToListAsync());

        group.MapGet("/{id}", async (int id, AppDbContext db) =>
        {
            var grupo = await db.Grupos
                .Include(g => g.Administradora)
                .Include(g => g.Cotas)
                .FirstOrDefaultAsync(g => g.Id == id);

            return grupo is null ? Results.NotFound("Grupo não encontrado") : Results.Ok(grupo);
        });

        group.MapPut("/{id}", async (int id, Grupo grupoAtualizado, AppDbContext db) =>
        {
            var grupo = await db.Grupos.FindAsync(id);
            if (grupo is null)
                return Results.NotFound("Grupo não encontrado");

            var administradora = await db.Administradoras.FindAsync(grupoAtualizado.AdministradoraId);
            if (administradora is null)
                return Results.NotFound("Administradora não encontrada");

            grupo.Nome = grupoAtualizado.Nome;
            grupo.AdministradoraId = grupoAtualizado.AdministradoraId;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, AppDbContext db) =>
        {
            var grupo = await db.Grupos.FindAsync(id);
            if (grupo is null)
                return Results.NotFound("Grupo não encontrado");

            db.Grupos.Remove(grupo);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return group;
    }
}