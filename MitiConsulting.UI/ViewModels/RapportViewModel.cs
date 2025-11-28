using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoMapper;

using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.ApplicationCore.Services;

namespace MitiConsulting.UI.ViewModels
{
    public partial class RapportViewModel : ObservableObject
    {
        public ObservableCollection<ListeRapportDTO> ListeRapport { get; } = new ObservableCollection<ListeRapportDTO>();

        public AsyncRelayCommand<int> LireRapport { get; } = default;
        public AsyncRelayCommand AjouterRapport { get; } = default;
        public AsyncRelayCommand ModifierRapport { get; } = default;
        public AsyncRelayCommand ChargerRapport { get; } = default;
        private readonly RapportService? _service;
        private readonly IMapper? _mapper;

        //Rapport affiché dans la View
        [ObservableProperty] private RapportDTO? rapportDto;
        [ObservableProperty] private UpdateRapportDTO? updateRapportDto;

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

        public async Task LireRapportAsync(int pageNum)
        {
            var liste = await _service.GetRapportsAsync(pageNum);
            ListeRapport.Clear();
            foreach (var l in liste)
                ListeRapport.Add(l);
        }
        public async Task AjoutRapportAsync()
        {
            var createDto = _mapper.Map<CreatRapportDTO>(this);
            RapportDto = await _service.AjouterRapportAsync(createDto);
        }
        public async Task UpdateRapportAsync()
        {
            var dto = _mapper.Map<UpdateRapportDTO>(this);
            RapportDto = await _service.ModifierRapportAsync(dto);
        }
        public async Task GetRapportByIdAsync()
        {
            var id = this.IdRapport;
            RapportDto = await _service.GetRapportByIdAsync(id);
        }

    }
}
