
// using MitiConsulting.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using MitiConsulting.Domain.Models;
// using MitiConsulting.Application.DTOs;

namespace MitiConsulting.Domain.Interfaces{
    public record ListeDTO (int IdRapport, string NomRapport,int AnneeDebut);
    public interface IRapportRepository
    {
        Task <IReadOnlyList<ListeDTO>?> GetRapportAsync(int pageNum); // pour la pagination 
        Task AjoutRapportAsync(Rapport rapport) ;
        Task  ModiferRapportAsync(Rapport rapport);
        Task <Rapport?> GetRapportByIdAsync(int id);
        Task <int> GetNombreRapportAsync();
        //Recherche (à faire)
    }
}
