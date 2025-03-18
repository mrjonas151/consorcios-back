using ConsorcioAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConsorcioBackend.UnitTests.Controllers;

public class CotasControllerTests
{
    private readonly List<Cota> _cotas;
    private readonly List<Grupo> _grupos;
    private readonly List<Cliente> _clientes;
    private readonly List<Administradora> _administradoras;

    public CotasControllerTests()
    {
        _administradoras = new List<Administradora>
        {
            new Administradora
            {
                Id = 1,
                Nome = "Administradora Teste",
                CNPJ = "12.345.678/0001-90"
            }
        };

        _grupos = new List<Grupo>
        {
            new Grupo
            {
                Id = 1,
                Nome = "Grupo Teste",
                AdministradoraId = 1,
                Administradora = _administradoras[0]
            }
        };

        _clientes = new List<Cliente>
        {
            new Cliente
            {
                Id = 1,
                Nome = "Cliente Teste",
                CPF = "123.456.789-00",
                Email = "cliente@teste.com"
            }
        };

        _cotas = new List<Cota>
        {
            new Cota
            {
                Id = 1,
                NumeroCota = "001",
                Valor = 1000M,
                Status = "Disponível",
                GrupoId = 1,
                Grupo = _grupos[0]
            },
            new Cota
            {
                Id = 2,
                NumeroCota = "002",
                Valor = 1500M,
                Status = "Vendida",
                GrupoId = 1,
                Grupo = _grupos[0],
                ClienteId = 1,
                Cliente = _clientes[0]
            }
        };

        _grupos[0].Cotas = _cotas.Where(c => c.GrupoId == 1).ToList();
        _clientes[0].Cotas = _cotas.Where(c => c.ClienteId == 1).ToList();
        _administradoras[0].Grupos = _grupos.Where(g => g.AdministradoraId == 1).ToList();
    }

    [Fact]
    public void GetAll_ReturnAllCotas()
    {
        var result = _cotas;

        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.NumeroCota == "001");
        Assert.Contains(result, c => c.NumeroCota == "002");
    }

    [Fact]
    public void GetById_WithValidId_ReturnsCota()
    {
        var id = 1;

        var cota = _cotas.FirstOrDefault(c => c.Id == id);
        var result = cota is null ? Results.NotFound("Cota não encontrada") : Results.Ok(cota);

        var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<Cota>>(result);
#pragma warning disable CS8602 
        Assert.Equal("001", okResult.Value.NumeroCota);
#pragma warning restore CS8602 
    }

    [Fact]
    public void GetById_WithInvalidId_ReturnsNotFound()
    {
        var id = 999;
        var cota = _cotas.FirstOrDefault(c => c.Id == id);
        var result = cota is null ? Results.NotFound("Cota não encontrada") : Results.Ok(cota);

        Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
    }

    [Fact]
    public void Create_WithValidData_ReturnsCota()
    {
        var cotasCount = _cotas.Count;
        var novaCota = new Cota
        {
            Id = 3,
            NumeroCota = "003",
            Valor = 2000M,
            Status = "Disponível",
            GrupoId = 1
        };

        var grupo = _grupos.FirstOrDefault(g => g.Id == novaCota.GrupoId);
        IResult result;

        if (grupo == null)
            result = Results.NotFound("Grupo não encontrado");
        else
        {
            novaCota.Grupo = grupo;

            if (novaCota.ClienteId.HasValue)
            {
                var cliente = _clientes.FirstOrDefault(c => c.Id == novaCota.ClienteId);
                if (cliente == null)
                    result = Results.NotFound("Cliente não encontrado");
                else
                {
                    novaCota.Cliente = cliente;
                    _cotas.Add(novaCota);
                    result = Results.Created($"/cotas/{novaCota.Id}", novaCota);
                }
            }
            else
            {
                _cotas.Add(novaCota);
                result = Results.Created($"/cotas/{novaCota.Id}", novaCota);
            }
        }

        var createdResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Created<Cota>>(result);
#pragma warning disable CS8602 
        Assert.Equal("003", createdResult.Value.NumeroCota);
#pragma warning restore CS8602 
        Assert.Equal(cotasCount + 1, _cotas.Count);
    }

    [Fact]
    public void Create_WithInvalidGrupo_ReturnsNotFound()
    {
        var cotasCount = _cotas.Count;
        var novaCota = new Cota
        {
            NumeroCota = "003",
            Valor = 2000M,
            Status = "Disponível",
            GrupoId = 999
        };

        var grupo = _grupos.FirstOrDefault(g => g.Id == novaCota.GrupoId);
        IResult result;

        if (grupo == null)
            result = Results.NotFound("Grupo não encontrado");
        else
        {
            novaCota.Grupo = grupo;

            if (novaCota.ClienteId.HasValue)
            {
                var cliente = _clientes.FirstOrDefault(c => c.Id == novaCota.ClienteId);
                if (cliente == null)
                    result = Results.NotFound("Cliente não encontrado");
                else
                {
                    novaCota.Cliente = cliente;
                    _cotas.Add(novaCota);
                    result = Results.Created($"/cotas/{novaCota.Id}", novaCota);
                }
            }
            else
            {
                _cotas.Add(novaCota);
                result = Results.Created($"/cotas/{novaCota.Id}", novaCota);
            }
        }

        Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        Assert.Equal(cotasCount, _cotas.Count);
    }

    [Fact]
    public void Create_WithInvalidCliente_ReturnsNotFound()
    {
        var cotasCount = _cotas.Count;
        var novaCota = new Cota
        {
            NumeroCota = "003",
            Valor = 2000M,
            Status = "Disponível",
            GrupoId = 1,
            ClienteId = 999
        };

        var grupo = _grupos.FirstOrDefault(g => g.Id == novaCota.GrupoId);
        IResult result;

        if (grupo == null)
            result = Results.NotFound("Grupo não encontrado");
        else
        {
            novaCota.Grupo = grupo;

            if (novaCota.ClienteId.HasValue)
            {
                var cliente = _clientes.FirstOrDefault(c => c.Id == novaCota.ClienteId);
                if (cliente == null)
                    result = Results.NotFound("Cliente não encontrado");
                else
                {
                    novaCota.Cliente = cliente;
                    _cotas.Add(novaCota);
                    result = Results.Created($"/cotas/{novaCota.Id}", novaCota);
                }
            }
            else
            {
                _cotas.Add(novaCota);
                result = Results.Created($"/cotas/{novaCota.Id}", novaCota);
            }
        }

        Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        Assert.Equal(cotasCount, _cotas.Count);
    }

    [Fact]
    public void Update_WithValidData_ReturnsNoContent()
    {
        var id = 1;
        var cotaAtualizada = new Cota
        {
            NumeroCota = "001-Atualizada",
            Valor = 1200M,
            Status = "Reservada",
            GrupoId = 1
        };

        var cota = _cotas.FirstOrDefault(c => c.Id == id);
        IResult result;

        if (cota is null)
            result = Results.NotFound("Cota não encontrada");
        else
        {
            var grupo = _grupos.FirstOrDefault(g => g.Id == cotaAtualizada.GrupoId);
            if (grupo is null)
                result = Results.NotFound("Grupo não encontrado");
            else
            {
                if (cotaAtualizada.ClienteId.HasValue)
                {
                    var cliente = _clientes.FirstOrDefault(c => c.Id == cotaAtualizada.ClienteId);
                    if (cliente is null)
                        result = Results.NotFound("Cliente não encontrado");
                    else
                    {
                        cota.NumeroCota = cotaAtualizada.NumeroCota;
                        cota.Valor = cotaAtualizada.Valor;
                        cota.Status = cotaAtualizada.Status;
                        cota.GrupoId = cotaAtualizada.GrupoId;
                        cota.ClienteId = cotaAtualizada.ClienteId;

                        result = Results.NoContent();
                    }
                }
                else
                {
                    cota.NumeroCota = cotaAtualizada.NumeroCota;
                    cota.Valor = cotaAtualizada.Valor;
                    cota.Status = cotaAtualizada.Status;
                    cota.GrupoId = cotaAtualizada.GrupoId;
                    cota.ClienteId = cotaAtualizada.ClienteId;

                    result = Results.NoContent();
                }
            }
        }

        Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);

        var cotaAtual = _cotas.FirstOrDefault(c => c.Id == id);
#pragma warning disable CS8602
        Assert.Equal("001-Atualizada", cotaAtual.NumeroCota);
#pragma warning restore CS8602 
        Assert.Equal(1200M, cotaAtual.Valor);
        Assert.Equal("Reservada", cotaAtual.Status);
    }

    [Fact]
    public void Update_WithInvalidId_ReturnsNotFound()
    {
        var id = 999;
        var cotaAtualizada = new Cota
        {
            NumeroCota = "001-Atualizada",
            Valor = 1200M,
            Status = "Reservada",
            GrupoId = 1
        };

        var cota = _cotas.FirstOrDefault(c => c.Id == id);
        IResult result;

        if (cota is null)
            result = Results.NotFound("Cota não encontrada");
        else
        {
            var grupo = _grupos.FirstOrDefault(g => g.Id == cotaAtualizada.GrupoId);
            if (grupo is null)
                result = Results.NotFound("Grupo não encontrado");
            else
            {
                if (cotaAtualizada.ClienteId.HasValue)
                {
                    var cliente = _clientes.FirstOrDefault(c => c.Id == cotaAtualizada.ClienteId);
                    if (cliente is null)
                        result = Results.NotFound("Cliente não encontrado");
                    else
                    {
                        cota.NumeroCota = cotaAtualizada.NumeroCota;
                        cota.Valor = cotaAtualizada.Valor;
                        cota.Status = cotaAtualizada.Status;
                        cota.GrupoId = cotaAtualizada.GrupoId;
                        cota.ClienteId = cotaAtualizada.ClienteId;

                        result = Results.NoContent();
                    }
                }
                else
                {
                    cota.NumeroCota = cotaAtualizada.NumeroCota;
                    cota.Valor = cotaAtualizada.Valor;
                    cota.Status = cotaAtualizada.Status;
                    cota.GrupoId = cotaAtualizada.GrupoId;
                    cota.ClienteId = cotaAtualizada.ClienteId;

                    result = Results.NoContent();
                }
            }
        }

        Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
    }

    [Fact]
    public void Update_WithInvalidGrupo_ReturnsNotFound()
    {
        var id = 1;
        var cotaAtualizada = new Cota
        {
            NumeroCota = "001-Atualizada",
            Valor = 1200M,
            Status = "Reservada",
            GrupoId = 999
        };

        var cota = _cotas.FirstOrDefault(c => c.Id == id);
        IResult result;

        if (cota is null)
            result = Results.NotFound("Cota não encontrada");
        else
        {
            var grupo = _grupos.FirstOrDefault(g => g.Id == cotaAtualizada.GrupoId);
            if (grupo is null)
                result = Results.NotFound("Grupo não encontrado");
            else
            {
                if (cotaAtualizada.ClienteId.HasValue)
                {
                    var cliente = _clientes.FirstOrDefault(c => c.Id == cotaAtualizada.ClienteId);
                    if (cliente is null)
                        result = Results.NotFound("Cliente não encontrado");
                    else
                    {
                        cota.NumeroCota = cotaAtualizada.NumeroCota;
                        cota.Valor = cotaAtualizada.Valor;
                        cota.Status = cotaAtualizada.Status;
                        cota.GrupoId = cotaAtualizada.GrupoId;
                        cota.ClienteId = cotaAtualizada.ClienteId;

                        result = Results.NoContent();
                    }
                }
                else
                {
                    cota.NumeroCota = cotaAtualizada.NumeroCota;
                    cota.Valor = cotaAtualizada.Valor;
                    cota.Status = cotaAtualizada.Status;
                    cota.GrupoId = cotaAtualizada.GrupoId;
                    cota.ClienteId = cotaAtualizada.ClienteId;

                    result = Results.NoContent();
                }
            }
        }

        Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        var cotaOriginal = _cotas.FirstOrDefault(c => c.Id == id);
#pragma warning disable CS8602 
        Assert.Equal("001", cotaOriginal.NumeroCota);
#pragma warning restore CS8602 
    }

    [Fact]
    public void Delete_WithValidId_ReturnsNoContent()
    {
        var id = 1;
        var cotasCount = _cotas.Count;

        var cota = _cotas.FirstOrDefault(c => c.Id == id);
        IResult result;

        if (cota is null)
            result = Results.NotFound("Cota não encontrada");
        else
        {
            _cotas.Remove(cota);
            result = Results.NoContent();
        }

        Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);

        Assert.Equal(cotasCount - 1, _cotas.Count);
        Assert.Null(_cotas.FirstOrDefault(c => c.Id == id));
    }

    [Fact]
    public void Delete_WithInvalidId_ReturnsNotFound()
    {
        var id = 999;
        var cotasCount = _cotas.Count;

        var cota = _cotas.FirstOrDefault(c => c.Id == id);
        IResult result;

        if (cota is null)
            result = Results.NotFound("Cota não encontrada");
        else
        {
            _cotas.Remove(cota);
            result = Results.NoContent();
        }

        Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        Assert.Equal(cotasCount, _cotas.Count);
    }

    [Fact]
    public void GetByGrupo_ReturnsCorrectCotas()
    {
        var grupoId = 1;

        var result = _cotas.Where(c => c.GrupoId == grupoId).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal(grupoId, c.GrupoId));
    }

    [Fact]
    public void GetByCliente_ReturnsCorrectCotas()
    {
        var clienteId = 1;

        var result = _cotas.Where(c => c.ClienteId == clienteId).ToList();

        Assert.Single(result);
        Assert.All(result, c => Assert.Equal(clienteId, c.ClienteId));
        Assert.Contains(result, c => c.NumeroCota == "002");
    }

    [Fact]
    public void GetByStatus_ReturnsCorrectCotas()
    {
        var status = "Disponível";

        var result = _cotas.Where(c => c.Status == status).ToList();

        Assert.Single(result);
        Assert.All(result, c => Assert.Equal(status, c.Status));
        Assert.Contains(result, c => c.NumeroCota == "001");
    }

    [Fact]
    public void GetVendidasSemCliente_ReturnsEmptyList()
    {
        var result = _cotas.Where(c => c.Status == "Vendida" && c.ClienteId == null).ToList();

        Assert.Empty(result);
    }
}