using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.Domain.Interfaces;

namespace MitiConsulting.ApplicationCore.Interfaces
{
    public interface IRapportService
    {
        Task<IReadOnlyList<ListeDTO>> GetRapportsAsync(int pageNum);
        Task<RapportDTO> AjouterRapportAsync(CreatRapportDTO dto);
        Task<RapportDTO?> ModifierRapportAsync(UpdateRapportDTO dto);
        Task<RapportDTO?> GetRapportByIdAsync(int id);
        Task<int> GetNombreRapportAsync();

    }
}
