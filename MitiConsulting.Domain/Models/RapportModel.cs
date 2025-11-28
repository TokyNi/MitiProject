using System.ComponentModel.DataAnnotations;
namespace MitiConsulting.Domain.Models
{
    public class Rapport{
        [Key]
        public int IdRapport { get; set;}
        [MaxLength(200)]
        public string NomClient { get ; set;} = string.Empty;
        [MaxLength(200)]
        public string Adresse { get ; set;} = string.Empty;
        public string NomRapport { get ; set;} = string.Empty;
        public int AnneeDebut { get ; set;}
        public int AnneeFin { get ; set;}
        [MaxLength(200)]
        public string Pays { get ; set;} = string.Empty;
        [MaxLength(200)]
        public string LieuxMission { get ; set;} = string.Empty;
        [MaxLength(200)]
        public string Financement { get ; set;} = string.Empty;
        public DateTime DateDebut{ get ; set;}
        public DateTime DateFin { get ; set;}
        public int NombreEmploye { get ; set; }
        public int NombreMoisTravail { get ; set;}
        public int NombreMoisMission { get; set;}
        [MaxLength(200)]
        public string NomAssocie { get ; set; } = string.Empty;
        // [Range(00.,100000)]
        public decimal Montant { get; set; }
        public string DescriptifProjet { get ; set; } = string.Empty;
        public string DescriptionServices { get ; set; } = string.Empty;
    }
}