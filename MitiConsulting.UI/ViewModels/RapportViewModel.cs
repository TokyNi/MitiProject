using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoMapper;
using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.ApplicationCore.Interfaces;
using MitiConsulting.Domain.Interfaces;

namespace MitiConsulting.UI.ViewModels
{
    public partial class RapportViewModel : ObservableObject
    {
        public ObservableCollection<ListeDTO> ListeRapport { get; } = new();
        public ObservableCollection<PageNumberViewModel> PageNumbers { get; } = new();

        public IAsyncRelayCommand<int> LireRapport { get; }
        public IAsyncRelayCommand AjouterRapport { get; }
        public IAsyncRelayCommand ModifierRapport { get; }
        public IAsyncRelayCommand ChargerRapport { get; }
        public IAsyncRelayCommand<int> GoToPageCommand { get; }
        public IAsyncRelayCommand PreviousPageCommand { get; }
        public IAsyncRelayCommand NextPageCommand { get; }

        private readonly IRapportService _service;
        private readonly IMapper _mapper;

        [ObservableProperty] private RapportDTO? rapportDto;
        [ObservableProperty] private UpdateRapportDTO? updateRapportDto;

        [ObservableProperty] private int currentPage = 1;
        [ObservableProperty] private int totalPages = 1;
        [ObservableProperty] private int totalItems;
        [ObservableProperty] private int pageSize = 10;
        [ObservableProperty] private bool hasPreviousPage;
        [ObservableProperty] private bool hasNextPage;
        [ObservableProperty] private string paginationInfo = string.Empty;

        // Champs du rapport
        [ObservableProperty] private int idRapport;
        [ObservableProperty] private string nomClient = string.Empty;
        [ObservableProperty] private string adresse = string.Empty;
        [ObservableProperty] private string nomRapport = string.Empty;
        [ObservableProperty] private int anneeDebut;
        [ObservableProperty] private int anneeFin;
        [ObservableProperty] private string pays = string.Empty;
        [ObservableProperty] private string lieuxMission = string.Empty;
        [ObservableProperty] private string financement = string.Empty;
        [ObservableProperty] private DateTime dateDebut;
        [ObservableProperty] private DateTime dateFin;
        [ObservableProperty] private int nombreEmploye;
        [ObservableProperty] private int nombreMoisTravail;
        [ObservableProperty] private int nombreMoisMission;
        [ObservableProperty] private string nomAssocie = string.Empty;
        [ObservableProperty] private decimal montant;
        [ObservableProperty] private string descriptifProjet = string.Empty;
        [ObservableProperty] private string descriptionServices = string.Empty;

        public RapportViewModel(IRapportService service, IMapper mapper, bool skipLoad = false)
        {
            _service = service;
            _mapper = mapper;

            LireRapport = new AsyncRelayCommand<int>(LireRapportAsync);
            AjouterRapport = new AsyncRelayCommand(AjoutRapportAsync);
            ModifierRapport = new AsyncRelayCommand(UpdateRapportAsync);
            ChargerRapport = new AsyncRelayCommand(ChargerRapportsAsync);
            GoToPageCommand = new AsyncRelayCommand<int>(GoToPageAsync);

            PreviousPageCommand = new AsyncRelayCommand(PreviousPageAsync);
            NextPageCommand = new AsyncRelayCommand(NextPageAsync);

            if (!skipLoad)
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

        // ---- FIX : METHODES MANQUANTES ----

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

        // ---- CRUD ----

        public async Task LireRapportAsync(int pageNum)
        {
            var liste = await _service.GetRapportsAsync(pageNum);

            ListeRapport.Clear();
            foreach (var l in liste)
                ListeRapport.Add(l);

            CurrentPage = pageNum;
        }

        public async Task AjoutRapportAsync()
        {
            var dto = _mapper.Map<CreatRapportDTO>(this);
            RapportDto = await _service.AjouterRapportAsync(dto);
        }

        public async Task UpdateRapportAsync()
        {
            var dto = _mapper.Map<UpdateRapportDTO>(this);
            RapportDto = await _service.ModifierRapportAsync(dto);
        }

        public async Task GetRapportByIdAsync()
        {
            RapportDto = await _service.GetRapportByIdAsync(IdRapport);
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
}
