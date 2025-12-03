using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MitiConsulting.UI.Fiches.ViewModels;
using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.ApplicationCore.Interfaces;
using MitiConsulting.Domain.Interfaces;

public class RapportsListViewModelTests
{
    private RapportsListViewModel CreateViewModel(Mock<IRapportService> mockService)
    {
        return new RapportsListViewModel(mockService.Object);
    }

    [Fact]
    public async Task GoToPageAsync_Should_LoadData_And_UpdatePagination()
    {
        // ARRANGE
        var mockService = new Mock<IRapportService>();

        mockService.Setup(s => s.GetRapportsAsync(2))
            .ReturnsAsync(new List<ListeDTO>
            {
                new ListeDTO(1,"R1",2020),
                new ListeDTO(2,"R2",2021)
            });

        mockService.Setup(s => s.GetNombreRapportAsync())
            .ReturnsAsync(25); // totalItems

        var vm = CreateViewModel(mockService);

        // ACT
        await vm.GoToPageCommand.ExecuteAsync(2);

        // ASSERT
        Assert.Equal(2, vm.CurrentPage);
        Assert.Equal(25, vm.TotalItems);
        Assert.Equal(3, vm.TotalPages);   // 25 / 10 = 3 pages
        Assert.Equal(2, vm.ListeRapport.Count);
        Assert.Equal("R1", vm.ListeRapport[0].NomRapport);
        Assert.True(vm.HasPreviousPage); // Page 2 → HasPrevious = TRUE
        Assert.True(vm.HasNextPage);     // Page 2 < 3 → TRUE
        Assert.Equal("Affichage de 11 à 20 sur 25 rapports", vm.PaginationInfo);
    }

    [Fact]
    public async Task PreviousPageAsync_Should_Navigate_To_Previous_Page()
    {
        // ARRANGE
        var mockService = new Mock<IRapportService>();

        mockService.Setup(s => s.GetRapportsAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<ListeDTO> { new ListeDTO(1, "Test", 2020) });

        mockService.Setup(s => s.GetNombreRapportAsync())
            .ReturnsAsync(30);

        var vm = CreateViewModel(mockService);

        await vm.GoToPageCommand.ExecuteAsync(3); // currentPage = 3

        // ACT
        await vm.PreviousPageCommand.ExecuteAsync(null);

        // ASSERT
        Assert.Equal(2, vm.CurrentPage);
    }

    [Fact]
    public async Task NextPageAsync_Should_Navigate_To_Next_Page()
    {
        // ARRANGE
        var mockService = new Mock<IRapportService>();

        mockService.Setup(s => s.GetRapportsAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<ListeDTO> { new ListeDTO(1, "Test", 2020) });

        mockService.Setup(s => s.GetNombreRapportAsync())
            .ReturnsAsync(30); // total pages = 3

        var vm = CreateViewModel(mockService);

        await vm.GoToPageCommand.ExecuteAsync(1);

        // ACT
        await vm.NextPageCommand.ExecuteAsync(null);

        // ASSERT
        Assert.Equal(2, vm.CurrentPage);
    }

    [Fact]
    public async Task LireRapportAsync_Should_Load_Page_Data()
    {
        // ARRANGE
        var mockService = new Mock<IRapportService>();

        mockService.Setup(s => s.GetRapportsAsync(1))
            .ReturnsAsync(new List<ListeDTO>
            {
                new ListeDTO(10,"TestA",2019),
                new ListeDTO(20,"TestB",2020)
            });

        var vm = CreateViewModel(mockService);

        // ACT
        await vm.LireRapportAsync(1);

        // ASSERT
        Assert.Equal(2, vm.ListeRapport.Count);
        Assert.Equal("TestA", vm.ListeRapport[0].NomRapport);
        Assert.Equal(1, vm.CurrentPage);
    }
}
