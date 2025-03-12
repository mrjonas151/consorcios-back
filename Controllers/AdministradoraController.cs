using ConsorcioAPI.Data;
using ConsorcioAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace ConsorcioAPI.Controllers;

public static class AdministradoraController
{
    public static RouteGroupBuilder MapAdministradora(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (Administradora administradora, AppDbContext db) =>
        {
            if (await db.Administradoras.AnyAsync(a => a.CNPJ == administradora.CNPJ))
                return Results.BadRequest("CNPJ já cadastrado");

            db.Administradoras.Add(administradora);
            await db.SaveChangesAsync();
            return Results.Created($"/administradoras/{administradora.Id}", administradora);
        });

        group.MapGet("/", async (AppDbContext db) =>
            await db.Administradoras.Include(a => a.Grupos).ToListAsync());

        group.MapGet("/{id}", async (int id, AppDbContext db) =>
        {
            var administradora = await db.Administradoras
                .Include(a => a.Grupos)
                .FirstOrDefaultAsync(a => a.Id == id);

            return administradora is null ? Results.NotFound("Administradora não encontrada") : Results.Ok(administradora);
        });

        group.MapPut("/{id}", async (int id, Administradora administradoraAtualizada, AppDbContext db) =>
        {
            var administradora = await db.Administradoras.FindAsync(id);
            if (administradora is null)
                return Results.NotFound("Administradora não encontrada");

            if (administradora.CNPJ != administradoraAtualizada.CNPJ &&
                await db.Administradoras.AnyAsync(a => a.CNPJ == administradoraAtualizada.CNPJ))
                return Results.BadRequest("CNPJ já cadastrado");

            administradora.Nome = administradoraAtualizada.Nome;
            administradora.CNPJ = administradoraAtualizada.CNPJ;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, AppDbContext db) =>
        {
            var administradora = await db.Administradoras.FindAsync(id);
            if (administradora is null)
                return Results.NotFound("Administradora não encontrada");

            db.Administradoras.Remove(administradora);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return group;
    }
}