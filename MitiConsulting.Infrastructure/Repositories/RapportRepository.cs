using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MitiConsulting.Domain.Interfaces;
using MitiConsulting.Domain.Models;
using MitiConsulting.Infrastructure.Data;

namespace MitiConsulting.Infrastructure.Repository
{
    public class RapportRepository :IRapportRepository
    {
        public readonly AppDbContext _context;

        public RapportRepository(AppDbContext context ){
            _context = context;
        }
        public async Task <IReadOnlyList<ListeDTO>?> GetRapportAsync(int pageNum){
            int pageSize = 10;
            return await _context.Rapports
        
            .OrderByDescending(r => r.AnneeDebut) 
            .Select( r => new ListeDTO (
                r.IdRapport,
                r.NomRapport,
                r.AnneeDebut
            ))// ordre des Année
            .Skip((pageNum - 1) * pageSize) // numero de page de la pagination 
            .Take(pageSize) // nombre page à afficher
            .ToListAsync(); 

        }
        public async Task AjoutRapportAsync(Rapport creatRapportDto){
            //methode add pour ajouter
            _context.Rapports.Add(creatRapportDto);
        //ajout dans la base
            await _context.SaveChangesAsync(); 
        
        }
        public async Task<Rapport?> GetRapportByIdAsync(int id){
            return  await _context.Rapports.FindAsync(id);
        }

        public async Task ModiferRapportAsync(Rapport updated)
        {
            var existing = await _context.Rapports
                .FirstOrDefaultAsync(r => r.IdRapport == updated.IdRapport);

            if (existing == null)
                throw new Exception("Rapport introuvable");

            // Mettre à jour toutes les propriétés automatiquement
            _context.Entry(existing).CurrentValues.SetValues(updated);

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetNombreRapportAsync(){
            return await _context.Rapports.CountAsync();
        }    
    }  
}
