
namespace WezLekApp.Models
{
    public class GetMedicamentDto
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public int NumberOfTablets { get; set; } //ilość tabletek/saszetek w opakowaniu
        public string Dose { get; set; } //dawka substancji
        public int DailyDosage { get; set; } // ile razy dziennie zarzywamy 
    }
}
