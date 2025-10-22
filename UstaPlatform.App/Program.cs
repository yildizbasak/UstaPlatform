// UstaPlatform.App/Program.cs

using UstaPlatform.Domain; // USING DÜZELTMESİ: ".Entities" kaldırıldı.
using UstaPlatform.Pricing; // Fiyatlama Motorumuzu kullanabilmek için bu satır gerekli!
using System.Globalization;

// ---- UstaPlatform Simülasyonu ----
Console.WriteLine("UstaPlatform Simülasyonu Başlatılıyor...");
Console.WriteLine("-----------------------------------------");

// Ödev Gerekliliği: Nesne Başlatıcılar (Object Initializers)
var vatandas = new Vatandas
{
    Id = 1,
    AdSoyad = "Ayşe Yılmaz",
    Telefon = "555-111-2233"
};

var usta = new Usta
{
    Id = 101,
    AdSoyad = "Mehmet Demir",
    UzmanlikAlani = "Tesisat",
    Puan = 4.8,
    Yogunluk = 2
};

// Ödev Gerekliliği: init-only özellikleri test ediliyor
var isEmri = new WorkOrder(Guid.NewGuid()) // ID constructor'da veriliyor
{
    TalepEdenVatandas = vatandas,
    AtanmisUsta = usta,
    IsTanimi = "Mutfak musluğu acil sızıntı tamiri",
    Adres = "Arcadia, Menekşe Mah. Lale Sok. No: 12",
    PlanlananTarih = new DateOnly(2025, 10, 26), // Pazar gününe (Hafta sonu) denk geliyor
    TemelUcret = 150.00m // Kurallar hariç temel fiyat
};
Console.WriteLine($"İş Emri {isEmri.Id} oluşturuldu.");
Console.WriteLine($"İş Emri Planlanan Tarih: {isEmri.PlanlananTarih} (Pazar)");
Console.WriteLine("-----------------------------------------");
Console.WriteLine("\nÖDEVİN ANA SENARYOSU: PLUG-IN FİYATLAMA MOTORU\n");


// 1. FİYATLAMA MOTORUNU BAŞLAT
// Bu 'new' komutu, motorun kurucu metodunu (constructor) tetikler.
// Motor, constructor içinde "Rules" klasörünü taramaya başlar.
Console.WriteLine("Fiyatlama Motoru (PricingEngine) başlatılıyor...");
PricingEngine motor = new PricingEngine();

Console.WriteLine("\n...Motor taramayı bitirdi.");
Console.WriteLine("Mevcut kurallar kullanılarak fiyat hesaplanıyor:");

// 2. FİYATI HESAPLA
// Motor, hafızaya yüklediği kuralları (şu anda sadece 'HaftasonuEkUcretiKurali')
// iş emrine uygular.
motor.CalculatePrice(isEmri);

Console.WriteLine("\n-----------------------------------------");
Console.WriteLine($"İŞ EMRİ ÖZETİ (Senaryo 1):");
Console.WriteLine($"  -> Temel Ücret: {isEmri.TemelUcret:C}");
Console.WriteLine($"  -> SON FİYAT  : {isEmri.SonFiyat:C}");
Console.WriteLine("-----------------------------------------");

Console.WriteLine("\nSimülasyonun ilk aşaması tamamlandı.");
Console.WriteLine("Devam etmek için bir tuşa basın (Bu, demo senaryosundaki 'uygulamayı kapatma' anıdır)...");
Console.ReadLine(); // Burada durup kullanıcıdan tuşa basmasını bekleriz

// ---- BURASI ÖDEVİN ZORUNLU DEMO SENARYOSUNU GÖSTERİR ----
//
// Normalde burada uygulamayı kapatıp,
// "LoyaltyDiscountRule.dll" dosyasını elle "Rules" klasörüne kopyalayıp
// uygulamayı yeniden çalıştırmamız gerekir.
//
// Biz şimdi, o "elle kopyalanmış" DLL'i simüle etmek için
// yeni bir motor oluşturacağız. Gerçek hayatta bu, uygulamanın
// yeniden başlatılmasıyla aynı şeydir.
//
// Adım 18'de "LoyaltyDiscountRule.dll" dosyasını oluşturduktan sonra,
// bu kod o DLL'i de otomatik olarak bulacaktır.
//
// -------------------------------------------------------------

Console.WriteLine("\n\n----- SENARYO 2 BAŞLIYOR -----");
Console.WriteLine("Uygulama 'kapatıldı' ve 'Rules' klasörüne YENİ BİR İNDİRİM DLL'i bırakıldı.");
Console.WriteLine("Uygulama yeniden başlatılıyor...\n");

Console.WriteLine("Fiyatlama Motoru (PricingEngine) 2. kez başlatılıyor...");
// 3. MOTORU YENİDEN BAŞLAT (Uygulamanın yeniden başladığını simüle eder)
// Bu yeni motor, "Rules" klasörünü TEKRAR tarayacak.
// Eğer Adım 18'i yapıp "LoyaltyDiscountRule.dll" dosyasını eklersek,
// bu taramada o kural da bulunacaktır.
PricingEngine motor2 = new PricingEngine();

Console.WriteLine("\n...Motor taramayı bitirdi.");
Console.WriteLine("Yeni kural setiyle fiyat (aynı iş emri için) tekrar hesaplanıyor:");

// 4. FİYATI YENİDEN HESAPLA
motor2.CalculatePrice(isEmri);

Console.WriteLine("\n-----------------------------------------");
Console.WriteLine($"İŞ EMRİ ÖZETİ (Senaryo 2 - Yeni Kural ile):");
Console.WriteLine($"  -> Temel Ücret: {isEmri.TemelUcret:C}");
Console.WriteLine($"  -> SON FİYAT  : {isEmri.SonFiyat:C}");
Console.WriteLine("-----------------------------------------");
Console.WriteLine("Plug-in mimarisi testi başarıyla tamamlandı.");

// Kültür ayarını en üste taşıdık, bu satırların burada olmasına gerek yok.
// CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("tr-TR");
// CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("tr-TR");