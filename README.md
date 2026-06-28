# 🏋️ Fitness Management System

> **ASP.NET Core MVC + Entity Framework Code First** ile geliştirilmiş tam kapsamlı spor salonu yönetim uygulaması.

![.NET](https://img.shields.io/badge/.NET-8.0-purple?style=flat-square&logo=dotnet)
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
| Backend | ASP.NET Core 8 MVC |
| ORM | Entity Framework Core (Code First) |
| Veritabanı | SQL Server / LocalDB |
| Frontend | Bootstrap 5.3, Font Awesome 6, Inter Font |
| PDF Export | iTextSharp / DinkToPdf |
| Excel Export | EPPlus / ClosedXML |

---

## 🚀 Kurulum

### Gereksinimler
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
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

## 📸 Ekran Görüntüleri

| Admin Panel | Üye Listesi |
|------------|-------------|
| Karanlık sidebar, aktif menü gösterimi | Tablo bazlı üye yönetimi |

| Rapor Sayfası | Form Sayfaları |
|--------------|----------------|
| Kart bazlı analiz raporları | Glassmorphism form kartları |

---

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

---

<div align="center">
  <strong>Fitness Management System</strong> — Yusuf Cengiz tarafından geliştirildi
</div>
