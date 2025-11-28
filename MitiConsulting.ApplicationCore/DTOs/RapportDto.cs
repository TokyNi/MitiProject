
namespace MitiConsulting.ApplicationCore.DTOs{
    public class RapportDTO{
        public int IdRapport { get; set;}
        public string NomClient { get ; set;} = string.Empty;
        public string Adresse { get ; set;} = string.Empty;
        public string NomRapport { get ; set;} = string.Empty;
        public int AnneeDebut { get ; set;}
        public int AnneeFin { get ; set;}
        public string Pays { get ; set;} = string.Empty;
        public string LieuxMission { get ; set;} = string.Empty;
        public string Financement { get ; set;} = string.Empty;
        public DateTime DateDebut{ get ; set;}
        public DateTime DateFin { get ; set;}
        public int NombreEmploye { get ; set; }
        public int NombreMoisTravail { get ; set;}
        public int NombreMoisMission { get; set;}
        public string NomAssocie { get ; set; } = string.Empty;
        public decimal Montant { get; set; }
        public string DescriptifProjet { get ; set; } = string.Empty;
        public string DescriptionServices { get ; set; } = string.Empty;
    }
}
