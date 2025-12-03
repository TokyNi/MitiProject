// using Xunit;
using Moq;
using AutoMapper;
using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.ApplicationCore.Interfaces;
using MitiConsulting.Domain.Interfaces;
using MitiConsulting.UI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RapportViewModelTests
{
    private RapportViewModel CreateViewModel(
        Mock<IRapportService> mockService,
        bool skipLoad = true)
    {
        var mockMapper = new Mock<IMapper>();
        return new RapportViewModel(mockService.Object, mockMapper.Object, skipLoad);
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
        Assert.False(vm.HasPreviousPage == false); // Page 2 → HasPrevious = TRUE
        Assert.True(vm.HasNextPage);               // Page 2 < 3 → TRUE
    }
}