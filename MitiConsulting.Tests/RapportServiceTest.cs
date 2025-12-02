using AutoMapper;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

using MitiConsulting.ApplicationCore.Services;
using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.Domain.Interfaces;
using MitiConsulting.Domain.Models;

public class RapportServiceTests
{
    private readonly Mock<IRapportRepository> _repoMock;
    private readonly IMapper _mapper;
    private readonly RapportService _service;

    public RapportServiceTests()
    {
        _repoMock = new Mock<IRapportRepository>();

        // AutoMapper config
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Rapport, ListeDTO>();
            cfg.CreateMap<Rapport, RapportDTO>();
            cfg.CreateMap<CreatRapportDTO, Rapport>();
            cfg.CreateMap<UpdateRapportDTO, Rapport>();
        });

        _mapper = config.CreateMapper();
        _service = new RapportService(_repoMock.Object, _mapper);
    }

    // --------------------------------------------------------
    //  GET LIST Rapports
    // --------------------------------------------------------
    [Fact]
    public async Task GetRapportsAsync_Should_Return_Mapped_DTOs()
    {
        // Arrange
        var rapports = new List<ListeDTO>
        {
            new ListeDTO(1, "Test", 2024)
        };

        _repoMock
            .Setup(r => r.GetRapportAsync(1))
            .ReturnsAsync(rapports);

        // Act
        var result = await _service.GetRapportsAsync(1);

        // Assert
        Assert.Single(result);
        Assert.Equal("Test", result[0].NomRapport);
    }

    // --------------------------------------------------------
    //  AJOUTER RAPPORT
    // --------------------------------------------------------
    [Fact]
    public async Task AjouterRapportAsync_Should_Map_And_Save()
    {
        // Arrange
        var dto = new CreatRapportDTO
        {
            NomRapport = "Nouveau",
            AnneeDebut = 2025
        };

        Rapport savedEntity = null!;

        _repoMock
            .Setup(r => r.AjoutRapportAsync(It.IsAny<Rapport>()))
            .Callback<Rapport>(r => savedEntity = r)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.AjouterRapportAsync(dto);

        // Assert
        Assert.NotNull(savedEntity);
        Assert.Equal("Nouveau", savedEntity.NomRapport);
        Assert.Equal("Nouveau", result.NomRapport);
    }

    // --------------------------------------------------------
    //  MODIFIER RAPPORT
    // --------------------------------------------------------
    [Fact]
    public async Task ModifierRapportAsync_Should_Map_And_Update()
    {
        // Arrange
        var dto = new UpdateRapportDTO
        {
            IdRapport = 1,
            NomRapport = "Modifié",
            AnneeDebut = 2030
        };

        Rapport entityPassedToRepo = null!;

        _repoMock
            .Setup(r => r.ModiferRapportAsync(It.IsAny<Rapport>()))
            .Callback<Rapport>(r => entityPassedToRepo = r)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.ModifierRapportAsync(dto);

        // Assert
        Assert.NotNull(entityPassedToRepo);
        Assert.Equal("Modifié", entityPassedToRepo.NomRapport);

        Assert.Equal("Modifié", result!.NomRapport);
    }

    // --------------------------------------------------------
    //  GET BY ID
    // --------------------------------------------------------
    [Fact]
    public async Task GetRapportByIdAsync_Should_Return_DTO()
    {
        // Arrange
        var entity = new Rapport
        {
            IdRapport = 2,
            NomRapport = "Test",
            AnneeDebut = 2020
        };

        _repoMock
            .Setup(r => r.GetRapportByIdAsync(2))
            .ReturnsAsync(entity);

        // Act
        var result = await _service.GetRapportByIdAsync(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.NomRapport);
    }

    // --------------------------------------------------------
    //  NOMBRE RAPPORTS
    // --------------------------------------------------------
    [Fact]
    public async Task GetNombreRapportAsync_Should_Return_Value()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetNombreRapportAsync())
            .ReturnsAsync(5);

        // Act
        var result = await _service.GetNombreRapportAsync();

        // Assert
        Assert.Equal(5, result);
    }
}
