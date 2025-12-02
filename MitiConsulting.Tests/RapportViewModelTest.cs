using Xunit;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using MitiConsulting.UI.ViewModels;
using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.ApplicationCore.Services;
using MitiConsulting.Domain.Interfaces;

public class RapportViewModelTest
{
    [Fact]
    public async Task LireRapportAsync_Should_Fill_ListeRapport()
    {
        // Arrange
        var mockService = new Mock<RapportService>(null, null);
        var mockMapper = new Mock<IMapper>();

        // Fake données retournées par le service
        var fakeRapports = new List<ListeDTO>
        {
            new ListeDTO (1,"Rapport A" ,2025),
            new ListeDTO (2,"Rapport B",2004)
        };

        mockService.Setup(s => s.GetRapportsAsync(1))
                   .ReturnsAsync(fakeRapports);

        mockService.Setup(s => s.GetNombreRapportAsync())
                   .ReturnsAsync(2);

        // IMPORTANT : empêcher le constructeur d'appeler ChargerRapportsAsync()
        var vm = new RapportViewModel(mockService.Object, mockMapper.Object);

        // Act
        await vm.LireRapportAsync(1);

        // Assert
        Assert.NotEmpty(vm.ListeRapport);
        Assert.Equal(2, vm.ListeRapport.Count);
        Assert.Equal("Client A", vm.ListeRapport[0].NomRapport);
        Assert.Equal("Client B", vm.ListeRapport[1].NomRapport);
    }
}
