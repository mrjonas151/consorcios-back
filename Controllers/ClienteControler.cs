using ConsorcioAPI.Data;
using ConsorcioAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace ConsorcioAPI.Controllers;

public static class ClienteController
{
    public static RouteGroupBuilder MapCliente(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (Cliente cliente, AppDbContext db) =>
        {
            if (await db.Clientes.AnyAsync(c => c.CPF == cliente.CPF))
                return Results.BadRequest("CPF já cadastrado");

            if (await db.Clientes.AnyAsync(c => c.Email == cliente.Email))
                return Results.BadRequest("Email já cadastrado");

            db.Clientes.Add(cliente);
            await db.SaveChangesAsync();
            return Results.Created($"/clientes/{cliente.Id}", cliente);
        });

        group.MapGet("/", async (AppDbContext db) =>
            await db.Clientes
                .Include(c => c.Cotas)
                .ThenInclude(cota => cota.Grupo)
                .ToListAsync());

        group.MapGet("/{id}", async (int id, AppDbContext db) =>
        {
            var cliente = await db.Clientes
                .Include(c => c.Cotas)
                .ThenInclude(cota => cota.Grupo)
                .FirstOrDefaultAsync(c => c.Id == id);

            return cliente is null ? Results.NotFound("Cliente não encontrado") : Results.Ok(cliente);
        });

        group.MapPut("/{id}", async (int id, Cliente clienteAtualizado, AppDbContext db) =>
        {
            var cliente = await db.Clientes.FindAsync(id);
            if (cliente is null)
                return Results.NotFound("Cliente não encontrado");

            if (cliente.CPF != clienteAtualizado.CPF &&
                await db.Clientes.AnyAsync(c => c.CPF == clienteAtualizado.CPF))
                return Results.BadRequest("CPF já cadastrado");

            if (cliente.Email != clienteAtualizado.Email &&
                await db.Clientes.AnyAsync(c => c.Email == clienteAtualizado.Email))
                return Results.BadRequest("Email já cadastrado");

            cliente.Nome = clienteAtualizado.Nome;
            cliente.CPF = clienteAtualizado.CPF;
            cliente.Email = clienteAtualizado.Email;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, AppDbContext db) =>
        {
            var cliente = await db.Clientes.FindAsync(id);
            if (cliente is null)
                return Results.NotFound("Cliente não encontrado");

            db.Clientes.Remove(cliente);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return group;
    }
}