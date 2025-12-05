using CommunityToolkit.Mvvm.ComponentModel;
using MitiConsulting.ApplicationCore.Interfaces;
using CommunityToolkit.Mvvm.Input;
using MitiConsulting.UI.Fiches.ViewModels;
using AutoMapper;
// using MitiConsulting.UI.Dashboard;

namespace MitiConsulting.UI.Main
{
    public partial class MainViewModel : ObservableObject
    {
        public IRapportService RapportService { get; }
        public IMapper Mapper { get; }

        [ObservableProperty]
        private ObservableObject currentPage;
        public IRelayCommand GoToRapportsList { get; }
        public IRelayCommand GoToRapportForm { get; }

        public MainViewModel(IRapportService service, IMapper mapper)
        {
            RapportService = service;
            Mapper = mapper;
             GoToRapportsList = new RelayCommand(() =>
                NavigateTo(new RapportsListViewModel(RapportService)));

            GoToRapportForm = new RelayCommand(() =>
                NavigateTo(new RapportFormViewModel(
                    RapportService,
                    Mapper
                )));

            // Page de démarrage
            CurrentPage = new RapportsListViewModel(RapportService);
        }

        public void NavigateTo(ObservableObject vm)
        {
            CurrentPage = vm;
        }
    }
}