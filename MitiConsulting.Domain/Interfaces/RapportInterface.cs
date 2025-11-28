
// using MitiConsulting.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using MitiConsulting.Domain.Models;
// using MitiConsulting.Application.DTOs;

namespace MitiConsulting.Domain.Interfaces{
    public interface IRapportRepository
    {
        Task <List<Rapport>> GetRapportAsync(int pageNum); // pour la pagination 
        Task <Rapport> AjoutRapportAsync(Rapport rapport) ;
        Task <Rapport?> ModiferRapportAsync(Rapport rapport);
        Task <Rapport> GetRapportByIdAsync(int id);
        //Recherche (à faire)
    }
}
