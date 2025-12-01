using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoMapper;
using System.Linq;

using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.ApplicationCore.Services;

namespace MitiConsulting.UI.ViewModels
{
    public partial class RapportViewModel : ObservableObject
    {
        public ObservableCollection<ListeRapportDTO> ListeRapport { get; } = new ObservableCollection<ListeRapportDTO>();
        public ObservableCollection<PageNumberViewModel> PageNumbers { get; } = new ObservableCollection<PageNumberViewModel>();

        public AsyncRelayCommand<int> LireRapport { get; } 
        public AsyncRelayCommand AjouterRapport { get; } 
        public AsyncRelayCommand ModifierRapport { get; } 
        public AsyncRelayCommand ChargerRapport { get; } 
        public AsyncRelayCommand<int> GoToPageCommand { get; }
        public AsyncRelayCommand PreviousPageCommand { get; }
        public AsyncRelayCommand NextPageCommand { get; }
        
        private readonly RapportService? _service;
        private readonly IMapper? _mapper;

        // Rapport affiché dans la View
        [ObservableProperty] private RapportDTO? rapportDto;
        [ObservableProperty] private UpdateRapportDTO? updateRapportDto;

        // Propriétés pour la pagination
        [ObservableProperty] private int currentPage = 1;
        [ObservableProperty] private int totalPages = 1;
        [ObservableProperty] private int totalItems;
        [ObservableProperty] private int pageSize = 10;
        [ObservableProperty] private bool hasPreviousPage;
        [ObservableProperty] private bool hasNextPage;
        [ObservableProperty] private string paginationInfo = string.Empty;

        // Attributs
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

        public RapportViewModel(RapportService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
            
            LireRapport = new AsyncRelayCommand<int>(LireRapportAsync);
            AjouterRapport = new AsyncRelayCommand(AjoutRapportAsync);
            ModifierRapport = new AsyncRelayCommand(UpdateRapportAsync);
            ChargerRapport = new AsyncRelayCommand(ChargerRapportsAsync);
            GoToPageCommand = new AsyncRelayCommand<int>(GoToPageAsync);
            PreviousPageCommand = new AsyncRelayCommand(PreviousPageAsync, () => HasPreviousPage);
            NextPageCommand = new AsyncRelayCommand(NextPageAsync, () => HasNextPage);
            
            // Charger la première page au démarrage
            _ = ChargerRapportsAsync();
        }

        private async Task ChargerRapportsAsync()
        {
            await GoToPageAsync(1);
        }

        private async Task GoToPageAsync(int pageNumber)
        {
            try
            {
                // Pour l'exemple, nous allons simuler la pagination avec votre méthode existante
                // Vous devrez adapter cela selon votre implémentation réelle
                await LireRapportAsync(pageNumber);
                
                // Simuler des données pour la pagination (à remplacer par vos vraies données)
                TotalItems = 25; // Exemple: 25 rapports au total
                TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
                
                UpdatePaginationInfo();
                UpdatePageNumbers();
                UpdateNavigationButtons();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des rapports: {ex.Message}");
            }
        }

        private async Task PreviousPageAsync()
        {
            if (HasPreviousPage)
            {
                await GoToPageAsync(CurrentPage - 1);
            }
        }

        private async Task NextPageAsync()
        {
            if (HasNextPage)
            {
                await GoToPageAsync(CurrentPage + 1);
            }
        }

        private void UpdatePaginationInfo()
        {
            int startIndex = ((CurrentPage - 1) * PageSize) + 1;
            int endIndex = Math.Min(CurrentPage * PageSize, TotalItems);
            
            PaginationInfo = $"Affichage de {startIndex} à {endIndex} sur {TotalItems} rapports";
        }

        private void UpdatePageNumbers()
        {
            PageNumbers.Clear();
            
            // Afficher maximum 5 numéros de page autour de la page courante
            int startPage = Math.Max(1, CurrentPage - 2);
            int endPage = Math.Min(TotalPages, CurrentPage + 2);
            
            // Ajouter la première page si nécessaire
            if (startPage > 1)
            {
                PageNumbers.Add(new PageNumberViewModel(1, CurrentPage == 1));
                if (startPage > 2)
                {
                    PageNumbers.Add(new PageNumberViewModel(-1, false));
                }
            }
            
            // Ajouter les pages autour de la page courante
            for (int i = startPage; i <= endPage; i++)
            {
                PageNumbers.Add(new PageNumberViewModel(i, i == CurrentPage));
            }
            
            // Ajouter la dernière page si nécessaire
            if (endPage < TotalPages)
            {
                if (endPage < TotalPages - 1)
                {
                    PageNumbers.Add(new PageNumberViewModel(-2, false));
                }
                PageNumbers.Add(new PageNumberViewModel(TotalPages, TotalPages == CurrentPage));
            }
        }

        private void UpdateNavigationButtons()
        {
            HasPreviousPage = CurrentPage > 1;
            HasNextPage = CurrentPage < TotalPages;
            
            PreviousPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
        }

        public async Task LireRapportAsync(int pageNum)
        {
            if (_service == null) return;
            
            var liste = await _service.GetRapportsAsync(pageNum);
            ListeRapport.Clear();
            foreach (var l in liste)
                ListeRapport.Add(l);
            
            CurrentPage = pageNum;
        }

        // public async Task ModifierRapportAsync(int id)
        // {
        //     // Implémentez la logique de modification
        //     // Exemple: navigation vers un formulaire d'édition
        // }

        public async Task AjoutRapportAsync()
        {
            if (_service == null || _mapper == null) return;
            
            var createDto = _mapper.Map<CreatRapportDTO>(this);
            RapportDto = await _service.AjouterRapportAsync(createDto);
        }

        public async Task UpdateRapportAsync()
        {
            if (_service == null || _mapper == null) return;
            
            var dto = _mapper.Map<UpdateRapportDTO>(this);
            RapportDto = await _service.ModifierRapportAsync(dto);
        }

        public async Task GetRapportByIdAsync()
        {
            if (_service == null) return;
            
            var id = this.IdRapport;
            RapportDto = await _service.GetRapportByIdAsync(id);
        }
    }

    // Classe pour représenter un numéro de page dans la pagination
    public class PageNumberViewModel : ObservableObject
    {
        public int Number { get; }
        public bool IsCurrentPage { get; }
        
        public string DisplayText => Number switch
        {
            -1 => "...",
            -2 => "...",
            _ => Number.ToString()
        };
        
        public PageNumberViewModel(int number, bool isCurrentPage)
        {
            Number = number;
            IsCurrentPage = isCurrentPage;
        }
    }
}