# 🏋️ Fitness Management System

> **ASP.NET Core MVC + Entity Framework Code First** ile geliştirilmiş tam kapsamlı spor salonu yönetim uygulaması.

![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=flat-square&logo=dotnet)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET_Core-MVC-blue?style=flat-square&logo=dotnet)
![Entity Framework](https://img.shields.io/badge/Entity_Framework-Code_First-green?style=flat-square)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat-square&logo=bootstrap)

---

## 📋 Proje Hakkında

Fitness Management System, spor salonlarının üye, antrenör, salon ve supplement yönetimini tek bir panel üzerinden yapabilmesine olanak tanıyan bir yönetim sistemidir. Tam karanlık (dark mode) tasarımıyla modern ve kullanıcı dostu bir arayüz sunar.

---

## ✨ Özellikler

### 👤 Üye Yönetimi
- Üye ekleme, güncelleme, silme
- İsim/soyisim bazlı arama
- Salon bazlı üye ataması
- PDF ve Excel dışa aktarma

### 🏋️ Antrenör Yönetimi
- Antrenör ekleme, güncelleme, silme
- Tecrübe yılı ve yaş takibi
- Özel üye (kişisel koç) ataması
- PDF ve Excel dışa aktarma

### 🏢 Salon Yönetimi
- Salon/şube ekleme, güncelleme, silme
- Adres bilgisi yönetimi
- Salon bazlı arama

### 💊 Supplement Yönetimi
- Ürün ekleme, güncelleme, silme
- Fiyat takibi
- Salon bazlı ürün ataması
- PDF ve Excel dışa aktarma

### 📊 Raporlama
- Üye demografisi (yaş ortalaması, en yaşlı/en genç)
- Şubelere göre üye dağılımı
- Antrenör–Üye sorumluluk atamaları
- Üye–Şube eşleşmeleri
- Şube supplement envanter dağılımı
- Mağaza & ürün analizi
- PDF ve Excel rapor indirme

---

## 🛠️ Teknolojiler

| Katman | Teknoloji |
|--------|-----------|
| Backend | ASP.NET Core 10 MVC |
| ORM | Entity Framework Core (Code First) |
| Veritabanı | SQL Server / LocalDB |
| Frontend | Bootstrap 5.3, Font Awesome 6, Inter Font |
| PDF Export | QuestPDF (Community License) |
| Excel Export | EPPlus |

---

## 🚀 Kurulum

### Gereksinimler
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server / SQL Server Express / LocalDB
- Visual Studio 2022 veya VS Code

### Adımlar

1. **Repoyu klonlayın:**
   ```bash
   git clone https://github.com/yusufcengiz00/FitnessManagement-MVC-CodeFirst.git
   cd FitnessManagement-MVC-CodeFirst
   ```

2. **Bağlantı dizesini yapılandırın:**
   
   `appsettings.json` dosyasındaki `ConnectionStrings` bölümünü kendi SQL Server bilgilerinize göre düzenleyin:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FitnessDb;Trusted_Connection=True;"
     }
   }
   ```

3. **Veritabanını oluşturun:**
   ```bash
   dotnet ef database update
   ```

4. **Uygulamayı çalıştırın:**
   ```bash
   dotnet run
   ```

5. Tarayıcınızda `https://localhost:PORT` adresine gidin.

---

## 🗂️ Proje Yapısı

```
FitnessManagement-MVC-CodeFirst/
├── Controllers/
│   ├── AdminController.cs
│   ├── AntrenorController.cs
│   ├── RaporController.cs
│   ├── SalonController.cs
│   ├── SupplementController.cs
│   └── UyeController.cs
├── Models/
│   ├── Antrenor.cs
│   ├── ApplicationDbContext.cs
│   ├── RaporViewModel.cs
│   ├── Salon.cs
│   ├── Supplement.cs
│   └── Uye.cs
├── Views/
│   ├── Antrenor/       # CRUD sayfaları
│   ├── Rapor/          # Analiz raporları
│   ├── Salon/          # CRUD sayfaları
│   ├── Shared/         # Layout dosyaları
│   ├── Supplement/     # CRUD sayfaları
│   └── Uye/            # CRUD sayfaları
├── Migrations/         # EF Code First migrations
├── wwwroot/
│   ├── AdminTemplate/  # Admin panel şablonu
│   └── UsersTemplate/  # Kullanıcı şablonu
└── Program.cs
```

---

## 🎨 Tasarım

- **Tam Karanlık Mod** — `#0d1117` tabanlı GitHub-style koyu tema
- **Glassmorphism Sidebar** — Şeffaf arka plan ve blur efektleri
- **Renk Kodlaması** — Her modül için farklı vurgu rengi (mavi, mor, sarı, yeşil, cyan)
- **Micro Animasyonlar** — Hover ve geçiş efektleri
- **Inter Fontu** — Modern ve okunabilir tipografi
- **Responsive Tasarım** — Tüm ekran boyutlarında uyumlu

---

## Görseller

<img width="1172" height="872" alt="Ekran görüntüsü 2026-07-01 200414" src="https://github.com/user-attachments/assets/d2e9d27d-60da-441b-8d91-b54c5b96228b" />
<img width="1181" height="901" alt="Ekran görüntüsü 2026-07-01 200336" src="https://github.com/user-attachments/assets/4c60a46e-32e9-4010-996e-8fe91edc201c" />
<img width="1187" height="613" alt="Ekran görüntüsü 2026-07-01 200213" src="https://github.com/user-attachments/assets/41abe7d2-eb39-474b-a028-4551cb591ff7" />
<img width="1183" height="610" alt="Ekran görüntüsü 2026-07-01 200247" src="https://github.com/user-attachments/assets/6332e155-b231-4bdf-b99d-9f89f8631083" />
<img width="1180" height="601" alt="Ekran görüntüsü 2026-07-01 200301" src="https://github.com/user-attachments/assets/4ee0403f-4c72-4240-9176-b592b88b30d4" />
<img width="1182" height="551" alt="Ekran görüntüsü 2026-07-01 200316" src="https://github.com/user-attachments/assets/3b75bccb-3a63-4c37-b009-32cc158a623f" />



---

