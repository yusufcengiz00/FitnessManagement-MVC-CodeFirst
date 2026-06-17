using System.Collections.Generic;

namespace Project1.Models
{
    public class RaporViewModel
    {
        // --- 3 ADET JOIN SORGUSU ---
        public List<UyeAntrenorSalonDto> UyeDetayListesi { get; set; } // 1. Join (Üye + Antrenör + Salon)
        public List<UyeAktifPlanDto> AktifUyePlanlari { get; set; }    // 2. Join (Üye + Üyelik Tipi/Planı vb.)
        public List<AntrenorSalonDto> AntrenorSalonEslenmesi { get; set; } // 3. Join (Antrenör + Salon)

        // --- 1 ADET JOIN + GROUP BY SORGUSU ---
        public List<AntrenorUyeSayisiDto> AntrenorUyeIstatistikleri { get; set; }

        // --- 2 ADET NORMAL (SERBEST) METRİK ---
        public double YasOrtalamasi { get; set; }
        public int ToplamUrunCesidi { get; set; }
    }

    // DTO'lar (Veri Taşıma Nesneleri)
    public class UyeAntrenorSalonDto
    {
        public string UyeAdi { get; set; }
        public int UyeYas { get; set; }
        public string AntrenorAdi { get; set; }
        public string SalonAdi { get; set; }
    }

    public class UyeAktifPlanDto
    {
        public string UyeAdi { get; set; }
        public string KayitTipi { get; set; } // Örn: Gold Üyelik, Aylık Paket vs.
        public decimal Ucret { get; set; }
    }

    public class AntrenorSalonDto
    {
        public string AntrenorAdi { get; set; }
        public string UzmanlikAlani { get; set; }
        public string SalonAdi { get; set; }
    }

    public class AntrenorUyeSayisiDto
    {
        public string AntrenorAdi { get; set; }
        public int ToplamUyeSayisi { get; set; }
        public double OrtalamaUyeYasi { get; set; }
    }
}