using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using MitiConsulting.Infrastructure.Repository;
using MitiConsulting.Infrastructure.Data;
using MitiConsulting.Domain.Models;

public class RapportRepositoryTests
{
    private RapportRepository BuildRepository(out AppDbContext context)
    {
        // Création d'un DbContext InMemory isolé pour chaque test
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        context = new AppDbContext(options);

        return new RapportRepository(context);
    }

    [Fact]
    public async Task AjoutRapportAsync_Should_Add_New_Rapport()
    {
        var repo = BuildRepository(out var context);

        var rapport = new Rapport
        {
            NomRapport = "Client A",
            AnneeDebut = 2022
        };

        await repo.AjoutRapportAsync(rapport);

        Assert.Equal(1, await context.Rapports.CountAsync());
        Assert.Equal("Client A", context.Rapports.First().NomRapport);
    }

    [Fact]
    public async Task GetRapportByIdAsync_Should_Return_Rapport()
    {
        var repo = BuildRepository(out var context);

        var rapport = new Rapport { NomRapport = "Client B", AnneeDebut = 2021 };
        context.Rapports.Add(rapport);
        await context.SaveChangesAsync();

        var result = await repo.GetRapportByIdAsync(rapport.IdRapport);

        Assert.NotNull(result);
        Assert.Equal("Client B", result!.NomRapport);
    }

    [Fact]
    public async Task GetRapportAsync_Should_Return_Paginated_And_Sorted_List()
    {
        var repo = BuildRepository(out var context);

        // Ajout de 20 rapports
        for (int i = 1; i <= 20; i++)
        {
            context.Rapports.Add(new Rapport
            {
                NomRapport = "Client " + i,
                AnneeDebut = 2000 + i
            });
        }
        await context.SaveChangesAsync();

        // Page 1
        var result = await repo.GetRapportAsync(1);

        Assert.NotNull(result);
        Assert.Equal(10, result!.Count); // pagination ok
        Assert.True(result[0].AnneeDebut > result[1].AnneeDebut); // tri DESC ok
    }


    [Fact]
    public async Task ModiferRapportAsync_Should_Update_Rapport()
    {
        var repo = BuildRepository(out var context);

        var rapport = new Rapport
        {
            NomRapport = "Ancien",
            AnneeDebut = 2010
        };
        context.Rapports.Add(rapport);
        await context.SaveChangesAsync();

        // Modification
        var updated = new Rapport
        {
            IdRapport = rapport.IdRapport,
            NomRapport = "Nouveau",
            AnneeDebut = 2025
        };

        await repo.ModiferRapportAsync(updated);

        var result = await context.Rapports.FindAsync(rapport.IdRapport);

        Assert.NotNull(result);
        Assert.Equal("Nouveau", result!.NomRapport);
        Assert.Equal(2025, result.AnneeDebut);
    }

    [Fact]
    public async Task GetNombreRapportAsync_Should_Return_Total_Count()
    {
        var repo = BuildRepository(out var context);

        context.Rapports.Add(new Rapport());
        context.Rapports.Add(new Rapport());
        await context.SaveChangesAsync();

        var count = await repo.GetNombreRapportAsync();

        Assert.Equal(2, count);
    }
}
