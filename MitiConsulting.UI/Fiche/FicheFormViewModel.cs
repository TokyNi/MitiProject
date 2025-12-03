using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoMapper;
using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.ApplicationCore.Interfaces;

namespace MitiConsulting.UI.Fiches.ViewModels
{
    public partial class RapportFormViewModel : ObservableObject
    {
        private readonly IRapportService _service;
        private readonly IMapper _mapper;

        [ObservableProperty] private RapportDTO? rapportDto;
        [ObservableProperty] private UpdateRapportDTO? updateRapportDto;

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

        public IAsyncRelayCommand AjouterRapport { get; }
        public IAsyncRelayCommand ModifierRapport { get; }

        public RapportFormViewModel(IRapportService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;

            AjouterRapport = new AsyncRelayCommand(AjoutRapportAsync);
            ModifierRapport = new AsyncRelayCommand(UpdateRapportAsync);
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
}
