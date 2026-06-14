using System.ComponentModel.DataAnnotations;

namespace Project1.Models
{
    public class Salon
    {
        [Key]
        public int SalonID { get; set; }
        public string SalonAdi { get; set; }

        public string SalonAdres { get; set; }

        

    }
}
