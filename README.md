Proje: UstaPlatform - Þehrin Uzmanlýk Platformu
Bu proje, Nesne Yönelimli Programlama (NYP) ve Ýleri C# dersi kapsamýnda geliþtirilmiþtir. Amacý, Arcadia þehrindeki uzmanlarla vatandaþ taleplerini eþleþtiren, dinamik ve geniþletilebilir bir platform oluþturmaktýr. 


Projenin temel odak noktasý, katý kurallar yerine, Açýk/Kapalý Prensibine (OCP) uygun, "Plug-in" (Eklenti) tabanlý bir fiyatlandýrma mimarisi kurmaktýr. 


Uygulanan Teknik Gereksinimler
Proje, ödev rehberinde belirtilen aþaðýdaki Ýleri C# özelliklerini ve SOLID prensiplerini uygulamaktadýr:


Açýk/Kapalý Prensibi (OCP): Sistemin kalbi olan PricingEngine, ana kodu deðiþtirmeden yeni fiyat kurallarýnýn (.dll dosyalarý olarak) sisteme eklenmesine olanak tanýr. 



Baðýmlýlýklarýn Tersine Çevrilmesi (DIP): Fiyatlama motoru, somut kural sýnýflarýna deðil, IPricingRule arayüzüne baðýmlýdýr. 


init-only Özelliði: WorkOrder gibi varlýk sýnýflarýnda, ID ve KayitZamani gibi alanlar nesne oluþturulduktan sonra deðiþtirilemez. 


Dizinleyici (Indexer): Schedule sýnýfý, Schedule[DateOnly gün] formatýnda eriþime izin vererek o güne ait iþ emirlerini listeler. 


Özel IEnumerable<T> Koleksiyonu: Route sýnýfý, IEnumerable arayüzünü uygular ve new Route { {10, 20} } gibi koleksiyon baþlatýcýlarý (collection initializers) destekler. 

Kurulum ve Çalýþtýrma Bilgisi 

Projeyi Visual Studio 2022 ile açýn.

Solution Explorer'da Solution 'UstaPlatform' üzerine sað týklayýp "Restore NuGet Packages" (NuGet Paketlerini Geri Yükle) deyin.

Solution Explorer'da Solution 'UstaPlatform' üzerine sað týklayýp "Rebuild Solution" (Çözümü Yeniden Derle) deyin.

(Bu adým, UstaPlatform.Rules.Default.dll ve UstaPlatform.Rules.Loyalty.dll dosyalarýný UstaPlatform.App projesinin /bin/Debug/.../Rules klasörüne otomatik olarak kopyalayacaktýr.)

Solution Explorer'da UstaPlatform.App projesini "Set as Startup Project" (Baþlangýç Projesi Olarak Ayarla) olarak seçin.

Yeþil "Play" (Oynat) tuþuna (veya F5'e) basarak projeyi çalýþtýrýn.

Tasarým Kararlarý: Plug-in (Eklenti) Mimarisi Nasýl Çalýþýyor? 

Projenin en kritik gereksinimi, ana uygulama deðiþmeden sisteme yeni fiyat kurallarý ekleyebilmektir.  Bu mimari, C#'ýn "Reflection" (Yansýma) özelliði kullanýlarak saðlanmýþtýr.



IPricingRule Arayüzü (Sözleþme): UstaPlatform.Domain projesinde, tüm kurallarýn uymasý gereken bir sözleþme (IPricingRule) tanýmlanmýþtýr. Bu arayüz, Baðýmlýlýklarýn Tersine Çevrilmesi (DIP) prensibini uygular. 


PricingEngine (Fiyatlama Motoru): UstaPlatform.Pricing projesindeki bu sýnýf, uygulama baþladýðýnda (new PricingEngine() çaðrýldýðýnda) çalýþýr.


DLL Taramasý (Reflection): Motor, uygulamanýn çalýþtýðý dizindeki /Rules klasörünü tarar.  System.Reflection kütüphanesini kullanarak bu klasördeki tüm .dll dosyalarýný bulur ve bu dosyalarýn "içine bakar".


Kural Yükleme: Motor, DLL'ler içinde IPricingRule arayüzünü uygulayan tüm somut sýnýflarý bulur,  bu sýnýflardan nesneler (instance) yaratýr ve özel bir List<IPricingRule> listesine ekler.


Kompozisyon ile Fiyatlama: Fiyat hesaplamasý istendiðinde (CalculatePrice), motor bu listedeki tüm kurallarý temel ücret üzerine sýrayla uygular (Temel Ücret + Kural1 + Kural2...). 

Bu tasarým sayesinde, /Rules klasörüne (örneðin BayramIndirimi.dll adýnda) yeni bir dosya býrakmak, sistemin yeniden derlenmeye gerek kalmadan bu kuralý otomatik olarak tanýmasýný ve çalýþtýrmasýný saðlar.

Zorunlu Demo Akýþý ve Çýktýsý 

Ödevin zorunlu senaryosu, sisteme yeni bir kural (LoyaltyDiscountRule.dll) eklendiðinde fiyatýn otomatik olarak deðiþtiðini göstermektir. 

Proje ilk çalýþtýrýldýðýnda (Rebuild Solution ile) /Rules klasöründe 2 adet DLL (UstaPlatform.Rules.Default.dll ve UstaPlatform.Rules.Loyalty.dll) bulunur.

Aþaðýdaki konsol çýktýsý, motorun bu iki kuralý da bulduðunu ve hesaplamaya dahil ettiðini kanýtlar:

UstaPlatform Simülasyonu Baþlatýlýyor...
Ýþ Emri ... oluþturuldu.
Ýþ Emri Planlanan Tarih: 26.10.2025 (Pazar)
-----------------------------------------

ÖDEVÝN ANA SENARYOSU: PLUG-IN FÝYATLAMA MOTORU

Fiyatlama Motoru (PricingEngine) baþlatýlýyor...
[PricingEngine] Baþlatýlýyor... Kural DLL'leri taranýyor...
[PricingEngine] 'Rules' klasöründe 2 adet .dll bulundu.
[PricingEngine] KURAL YÜKLENDÝ: Haftasonu Ek Ücreti (Kaynak: UstaPlatform.Rules.Default.dll)
[PricingEngine] KURAL YÜKLENDÝ: Sadakat Ýndirimi (%10) (Kaynak: UstaPlatform.Rules.Loyalty.dll)

...Motor taramayý bitirdi.
Mevcut kurallar kullanýlarak fiyat hesaplanýyor:
[PricingEngine] Fiyat hesaplanýyor... (Temel Ücret: ?150,00)
  -> Kural Uygulandý: Haftasonu Ek Ücreti, Etki: ?50,00
  -> Kural Uygulandý: Sadakat Ýndirimi (%10), Etki: -?20,00
[PricingEngine] HESAPLANAN SON FÝYAT: ?180,00

-----------------------------------------
ÝÞ EMRÝ ÖZETÝ (Senaryo 1):
  -> Temel Ücret: ?150,00
  -> SON FÝYAT  : ?180,00
-----------------------------------------
Bu çýktý, motorun yeni .dll dosyasýný (LoyaltyDiscountRule) yeniden derlemeye gerek kalmadan otomatik olarak bulduðunu ve 150 TL'lik temel ücrete önce 50 TL eklediðini (200 TL), ardýndan bu 200 TL üzerinden %10 indirim (-20 TL) uygulayarak son fiyatý 180 TL olarak hesapladýðýný kanýtlar. 

Opsiyonel Testler (Ek Puan) 

Ek puan gereksinimi için UstaPlatform.Tests adýnda bir xUnit projesi oluþturulmuþtur.

Bu proje, Visual Studio Test Explorer üzerinden çalýþtýrýldýðýnda, hem Schedule sýnýfýnýn Dizinleyici (Indexer) özelliði hem de PricingEngine sýnýfýnýn hesaplama mantýðý için yazýlan testlerin tamamý baþarýyla geçmektedir (2 Passed).