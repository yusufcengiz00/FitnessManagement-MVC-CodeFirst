using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project1.Models
{
    public class Supplement
    {
        [Key]
        public int SupplementID { get; set; }
        public string SupplementAdi { get; set; }
        public int SupplementFiyati { get; set; }

        [ForeignKey("salonlar")]
        public int SalonID { get; set; }
        public virtual Salon salonlar { get; set; }

    }
}
