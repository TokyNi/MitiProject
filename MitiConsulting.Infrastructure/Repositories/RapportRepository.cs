
using AutoMapper;
using Microsoft.EntityFrameworkCore;
// using MitiConsulting.Application.DTOs;
using MitiConsulting.Domain.Interfaces;
using MitiConsulting.Domain.Models;
using MitiConsulting.Infrastructure.Data;

namespace MitiConsulting.Services;
public class RapportService :IRapportRepository
{
    public readonly AppDbContext _context;
    public readonly IMapper _mapper;

    public RapportService(AppDbContext context , IMapper mapper){
        _context = context;
        _mapper = mapper;   
    }
    public async Task <List<Rapport>> GetRapportAsync(int pageNum){
        int pageSize = 10;
        var entities = await _context.Rapports
        .Select( r => new {
            IdRapport = r.IdRapport,
            NomClient = r.NomClient,
            AnneeDebut = r.AnneeDebut
        })
        .OrderBy(r => r.AnneeDebut) // ordre des Année
        .Skip((pageNum - 1) * pageSize) // numero de page de la pagination 
        .Take(pageSize) // nombre page à afficher
        .ToListAsync(); 
        
        //Afficher uniquement les attributs à afficher dans la liste
        return _mapper.Map<List<Rapport>>(entities);
    }
    public async Task<Rapport> AjoutRapportAsync(Rapport creatRapportDto){
        //convertion de DTO en EF core avec id = 0 ou null 
        var entity = _mapper.Map<Rapport>(creatRapportDto);
        //methode add pour ajouter
        _context.Rapports.Add(entity);
       //ajout dans la base
        await _context.SaveChangesAsync(); 
       //mapper l'entite et le retounr en dto  avec l'id
        return _mapper.Map<Rapport>(entity);
    }
    public async Task<Rapport> GetRapportByIdAsync(int id){
        var entity =  await _context.Rapports.FindAsync(id);
        // await _context.SaveChangesAsync();
        return _mapper.Map<Rapport>(entity);
    }

    public async Task<Rapport?> ModiferRapportAsync(Rapport updateRapportDTO){
        //EF core
        var entity = _mapper.Map<Rapport>(updateRapportDTO);
        //Recuperation du rapport à modifier
        // var entityUpdate = await GetRapportByIdAsync(entity.IdRapport);
        var entityUpdate = await _context.Rapports.FirstOrDefaultAsync(r => r.IdRapport == updateRapportDTO.IdRapport);

        if(entityUpdate == null)
        {
            return null;
        }
        //Modification  
        // var update = _mapper.Map<Rapport>(entityUpdate);
        _mapper.Map(updateRapportDTO, entityUpdate);
        // _context.Rapports.Update(update);

        await _context.SaveChangesAsync();
        //retourne la mise à jour
        return _mapper.Map<Rapport>(entityUpdate);
    }
    
}
