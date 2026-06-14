using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project1.Models
{
    public class Antrenor
    {
        [Key]
        public int AntrenorID { get; set; }
        public string userName { get; set; }
        public int Yas { get; set; }
        public int TecrübeYili { get; set; }

        [ForeignKey("Uye")]
        public int UyeID { get; set; }
        public virtual Uye uyeler { get; set; }
    }
}
