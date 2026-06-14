using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project1.Models
{
    public class Uye
    {
        [Key]
        public int UyeID { get; set; }
        public string UyeAdi { get; set; }

        public string UyeSoyadi { get; set; }
        public int Yas { get; set; }

        [ForeignKey("Salonlar")]
        public int SalonID { get; set; }
        public virtual Salon Salonlar { get; set; }
    }
}
