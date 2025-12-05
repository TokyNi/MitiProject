using Xunit;
using Moq;
using AutoMapper;
using System.Threading.Tasks;
using MitiConsulting.UI.Fiches.ViewModels;
using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.ApplicationCore.Interfaces;

public class RapportFormViewModelTests
{
    private RapportFormViewModel CreateViewModel(
        Mock<IRapportService> mockService,
        Mock<IMapper> mockMapper)
    {
        return new RapportFormViewModel(mockService.Object, mockMapper.Object);
    }

    [Fact]
    public async Task AjoutRapportAsync_Should_Call_Service_And_Set_RapportDto()
    {
        // ARRANGE
        var mockService = new Mock<IRapportService>();
        var mockMapper = new Mock<IMapper>();

        var vm = CreateViewModel(mockService, mockMapper);

        var inputDto = new CreatRapportDTO
        {
            NomRapport = "Test",
            AnneeDebut = 2020,
            AnneeFin = 2024
        };

        var expectedDto = new RapportDTO
        {
            IdRapport = 1,
            NomRapport = "Test",
            AnneeDebut = 2020,
            AnneeFin = 2024
        };

        mockMapper.Setup(m => m.Map<CreatRapportDTO>(vm))
                  .Returns(inputDto);

        mockService.Setup(s => s.AjouterRapportAsync(inputDto))
                   .ReturnsAsync(expectedDto);

        // ACT
        await vm.AjoutRapportAsync();

        // ASSERT                                                                                       
        Assert.NotNull(vm.RapportDto);
        Assert.Equal(expectedDto.IdRapport, vm.RapportDto.IdRapport);
        Assert.Equal(expectedDto.NomRapport, vm.RapportDto.NomRapport);

        mockMapper.Verify(m => m.Map<CreatRapportDTO>(vm), Times.Once);
        mockService.Verify(s => s.AjouterRapportAsync(inputDto), Times.Once);
    }

    [Fact]
    public async Task UpdateRapportAsync_Should_Call_Service_And_Set_RapportDto()
    {
        // ARRANGE
        var mockService = new Mock<IRapportService>();
        var mockMapper = new Mock<IMapper>();

        var vm = CreateViewModel(mockService, mockMapper);

        var updateDto = new UpdateRapportDTO
        {
            IdRapport = 1,
            NomRapport = "Updated",
            AnneeDebut = 2021,
            AnneeFin = 2024
        };

        var expectedDto = new RapportDTO
        {
            IdRapport = 1,
            NomRapport = "Updated",
            AnneeDebut = 2021,
            AnneeFin = 2024
        };

        mockMapper.Setup(m => m.Map<UpdateRapportDTO>(vm))
                  .Returns(updateDto);

        mockService.Setup(s => s.ModifierRapportAsync(updateDto))
                   .ReturnsAsync(expectedDto);

        // ACT
        await vm.UpdateRapportAsync();

        // ASSERT
        Assert.NotNull(vm.RapportDto);
        Assert.Equal("Updated", vm.RapportDto.NomRapport);

        mockMapper.Verify(m => m.Map<UpdateRapportDTO>(vm), Times.Once);
        mockService.Verify(s => s.ModifierRapportAsync(updateDto), Times.Once);
    }

    [Fact]
    public async Task GetRapportByIdAsync_Should_Set_RapportDto()
    {
        // ARRANGE
        var mockService = new Mock<IRapportService>();
        var mockMapper = new Mock<IMapper>();

        var vm = CreateViewModel(mockService, mockMapper);

        vm.IdRapport = 5;

        var expected = new RapportDTO
        {
            IdRapport = 5,
            NomRapport = "TestRapport",
            AnneeDebut = 2022,
            AnneeFin = 2024
        };

        mockService.Setup(s => s.GetRapportByIdAsync(5))
                   .ReturnsAsync(expected);

        // ACT
        await vm.GetRapportByIdAsync();

        // ASSERT
        Assert.NotNull(vm.RapportDto);
        Assert.Equal(5, vm.RapportDto.IdRapport);
        Assert.Equal("TestRapport", vm.RapportDto.NomRapport);

        mockService.Verify(s => s.GetRapportByIdAsync(5), Times.Once);
    }
}
