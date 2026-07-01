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

## 📸 Uygulama Görselleri

### Arayüz

<p align="center">
 <img width="1090" height="842" alt="image" src="https://github.com/user-attachments/assets/16c68359-adb3-421d-b87e-f57b188bc749" />
</p>

---

### Admin Panel Giriş Ekranı

<p align="center">
<img width="1117" height="767" alt="image" src="https://github.com/user-attachments/assets/e0b1b687-9eca-41dc-9ea1-e8af0fa99617" />
</p>

---

### Üye Yönetim Ekranı

<p align="center">
   <img width="1116" height="581" alt="image" src="https://github.com/user-attachments/assets/0d3b69cb-a723-4501-a86a-4ec423820327" />
</p>

---

### Antrenör Yönetim Ekranı

<p align="center">
   <img width="1118" height="590" alt="image" src="https://github.com/user-attachments/assets/445f5acd-1a4b-4c5a-97e2-6c8d9fb47677" />
</p>

---

### Salon/Şube Yönetim Ekranı

<p align="center">
 <img width="1122" height="557" alt="image" src="https://github.com/user-attachments/assets/79c72730-2c07-4f3e-924c-166ead292cd0" />
</p>

---

### Supplement Yönetim Ekranı

<p align="center">
  <img width="1120" height="497" alt="image" src="https://github.com/user-attachments/assets/cd3e9069-0627-4ea0-a1e2-e5f85291902c" />
</p>

---

### Sistem Raporu Ekranı

<p align="center">
 <img width="1120" height="841" alt="image" src="https://github.com/user-attachments/assets/ad67fbfa-5862-415d-af83-7ee7fb114582" />
</p>

