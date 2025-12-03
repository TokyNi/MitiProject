using AutoMapper;
using MitiConsulting.ApplicationCore.DTOs;
// using MitiConsulting.ApplicationCore.Interfaces;
using MitiConsulting.Domain.Models;
using MitiConsulting.Domain.Interfaces;
using MitiConsulting.ApplicationCore.Interfaces;


namespace MitiConsulting.ApplicationCore.Services
{
  public class RapportService : IRapportService
{
    private readonly IRapportRepository _repo;
    private readonly IMapper _mapper;

    public RapportService(IRapportRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ListeDTO>> GetRapportsAsync(int pageNum)
    {
        var result = await _repo.GetRapportAsync(pageNum);
        return _mapper.Map<IReadOnlyList<ListeDTO>>(result);
    }

    public async Task<RapportDTO> AjouterRapportAsync(CreatRapportDTO dto)
    {
        var entity = _mapper.Map<Rapport>(dto);
        await _repo.AjoutRapportAsync(entity);
        return _mapper.Map<RapportDTO>(entity);
    }

    public async Task<RapportDTO?> ModifierRapportAsync(UpdateRapportDTO dto)
    {
        var entity = _mapper.Map<Rapport>(dto);
        await _repo.ModiferRapportAsync(entity);
        return entity is null ? null : _mapper.Map<RapportDTO>(entity);
    }

    public async Task<RapportDTO?> GetRapportByIdAsync(int id)
    {
        var entity = await _repo.GetRapportByIdAsync(id);
        return entity is null ? null : _mapper.Map<RapportDTO>(entity);
    }
    public async Task<int> GetNombreRapportAsync(){
        int nbr = await _repo.GetNombreRapportAsync();
        return nbr;
    }
}

}
