using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoMapper;
using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.ApplicationCore.Interfaces;
using MitiConsulting.Domain.Interfaces;


namespace MitiConsulting.UI.Fiches.ViewModels
{
    public partial class RapportsListViewModel : ObservableObject
    {
        public ObservableCollection<ListeDTO> ListeRapport { get; } = new();
        public ObservableCollection<PageNumberViewModel> PageNumbers { get; } = new();

        public IAsyncRelayCommand<int> LireRapport { get; }
        public IAsyncRelayCommand ChargerRapport { get; }
        public IAsyncRelayCommand<int> GoToPageCommand { get; }
        public IAsyncRelayCommand PreviousPageCommand { get; }
        public IAsyncRelayCommand NextPageCommand { get; }

        private readonly IRapportService _service;

        [ObservableProperty] private int currentPage = 1;
        [ObservableProperty] private int totalPages = 1;
        [ObservableProperty] private int totalItems;
        [ObservableProperty] private int pageSize = 10;
        [ObservableProperty] private bool hasPreviousPage;
        [ObservableProperty] private bool hasNextPage;
        [ObservableProperty] private string paginationInfo = string.Empty;

        public RapportsListViewModel(IRapportService service)
        {
            _service = service;

            LireRapport = new AsyncRelayCommand<int>(LireRapportAsync);
            ChargerRapport = new AsyncRelayCommand(ChargerRapportsAsync);
            GoToPageCommand = new AsyncRelayCommand<int>(GoToPageAsync);

            PreviousPageCommand = new AsyncRelayCommand(PreviousPageAsync);
            NextPageCommand = new AsyncRelayCommand(NextPageAsync);

            _ = ChargerRapportsAsync();
        }

        private async Task ChargerRapportsAsync()
        {
            await GoToPageAsync(1);
        }

        private async Task GoToPageAsync(int pageNumber)
        {
            await LireRapportAsync(pageNumber);

            TotalItems = await _service.GetNombreRapportAsync();
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);

            UpdatePaginationInfo();
            UpdatePageNumbers();
            UpdateNavigationButtons();
        }

        private void UpdatePaginationInfo()
        {
            int start = ((CurrentPage - 1) * PageSize) + 1;
            int end = Math.Min(CurrentPage * PageSize, TotalItems);
            PaginationInfo = $"Affichage de {start} à {end} sur {TotalItems} rapports";
        }

        private void UpdatePageNumbers()
        {
            PageNumbers.Clear();

            int start = Math.Max(1, CurrentPage - 2);
            int end = Math.Min(TotalPages, CurrentPage + 2);

            for (int i = start; i <= end; i++)
                PageNumbers.Add(new PageNumberViewModel(i, i == CurrentPage));
        }

        private void UpdateNavigationButtons()
        {
            HasPreviousPage = CurrentPage > 1;
            HasNextPage = CurrentPage < TotalPages;

            PreviousPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
        }

        private async Task PreviousPageAsync()
        {
            if (HasPreviousPage)
                await GoToPageAsync(CurrentPage - 1);
        }

        private async Task NextPageAsync()
        {
            if (HasNextPage)
                await GoToPageAsync(CurrentPage + 1);
        }

        public async Task LireRapportAsync(int pageNum)
        {
            var liste = await _service.GetRapportsAsync(pageNum);

            ListeRapport.Clear();
            foreach (var l in liste)
                ListeRapport.Add(l);

            CurrentPage = pageNum;
        }
    }
}
    public class PageNumberViewModel : ObservableObject
    {
        public int Number { get; }
        public bool IsCurrentPage { get; }

        public string DisplayText => Number <= 0 ? "..." : Number.ToString();

        public PageNumberViewModel(int number, bool isCurrentPage)
        {
            Number = number;
            IsCurrentPage = isCurrentPage;
        }
    }

