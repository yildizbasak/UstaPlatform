# Proje: UstaPlatform - Şehrin Uzmanlık Platformu

Bu proje, Nesne Yönelimli Programlama (NYP) ve İleri C\# dersi kapsamında geliştirilmiştir. Amacı, Arcadia şehrindeki uzmanlarla vatandaş taleplerini eşleştiren, dinamik ve genişletilebilir bir platform oluşturmaktır.

Projenin temel odak noktası, katı kurallar yerine, Açık/Kapalı Prensibine (OCP) uygun, "Plug-in" (Eklenti) tabanlı bir fiyatlandırma mimarisi kurmaktır.

## Uygulanan Teknik Gereksinimler

Proje, ödev rehberinde belirtilen aşağıdaki İleri C\# özelliklerini ve SOLID prensiplerini uygulamaktadır:

  * **Açık/Kapalı Prensibi (OCP):** Sistemin kalbi olan `PricingEngine`, ana kodu değiştirmeden yeni fiyat kurallarının (`.dll` dosyaları olarak) sisteme eklenmesine olanak tanır.
  * **Bağımlılıkların Tersine Çevrilmesi (DIP):** Fiyatlama motoru, somut kural sınıflarına değil, `IPricingRule` arayüzüne bağımlıdır.
  * **init-only Özelliği:** `WorkOrder` gibi varlık sınıflarında, `ID` ve `KayitZamani` gibi alanlar nesne oluşturulduktan sonra değiştirilemez.
  * **Dizinleyici (Indexer):** `Schedule` sınıfı, `Schedule[DateOnly gün]` formatında erişime izin vererek o güne ait iş emirlerini listeler.
  * **Özel `IEnumerable<T>` Koleksiyonu:** `Route` sınıfı, `IEnumerable` arayüzünü uygular ve `new Route { {10, 20} }` gibi koleksiyon başlatıcıları (collection initializers) destekler.

## Kurulum ve Çalıştırma Bilgisi

1.  Projeyi Visual Studio 2022 ile açın.
2.  Solution Explorer'da `Solution 'UstaPlatform'` üzerine sağ tıklayıp "Restore NuGet Packages" (NuGet Paketlerini Geri Yükle) deyin.
3.  Solution Explorer'da `Solution 'UstaPlatform'` üzerine sağ tıklayıp "Rebuild Solution" (Çözümü Yeniden Derle) deyin.
      * *(Bu adım, `UstaPlatform.Rules.Default.dll` ve `UstaPlatform.Rules.Loyalty.dll` dosyalarını `UstaPlatform.App` projesinin `/bin/Debug/.../Rules` klasörüne otomatik olarak kopyalayacaktır.)*
4.  Solution Explorer'da `UstaPlatform.App` projesini "Set as Startup Project" (Başlangıç Projesi Olarak Ayarla) olarak seçin.
5.  Yeşil "Play" (Oynat) tuşuna (veya F5'e) basarak projeyi çalıştırın.

## Tasarım Kararları: Plug-in (Eklenti) Mimarisi Nasıl Çalışıyor?

Projenin en kritik gereksinimi, ana uygulama değişmeden sisteme yeni fiyat kuralları ekleyebilmektir. Bu mimari, C\#'ın "Reflection" (Yansıma) özelliği kullanılarak sağlanmıştır.

  * **`IPricingRule` Arayüzü (Sözleşme):** `UstaPlatform.Domain` projesinde, tüm kuralların uyması gereken bir sözleşme (`IPricingRule`) tanımlanmıştır. Bu arayüz, Bağımlılıkların Tersine Çevrilmesi (DIP) prensibini uygular.
  * **`PricingEngine` (Fiyatlama Motoru):** `UstaPlatform.Pricing` projesindeki bu sınıf, uygulama başladığında (`new PricingEngine()` çağrıldığında) çalışır.
  * **DLL Taraması (Reflection):** Motor, uygulamanın çalıştığı dizindeki `/Rules` klasörünü tarar. `System.Reflection` kütüphanesini kullanarak bu klasördeki tüm `.dll` dosyalarını bulur ve bu dosyaların "içine bakar".
  * **Kural Yükleme:** Motor, DLL'ler içinde `IPricingRule` arayüzünü uygulayan tüm somut sınıfları bulur, bu sınıflardan nesneler (instance) yaratır ve özel bir `List<IPricingRule>` listesine ekler.
  * **Kompozisyon ile Fiyatlama:** Fiyat hesaplaması istendiğinde (`CalculatePrice`), motor bu listedeki tüm kuralları temel ücret üzerine sırayla uygular (Temel Ücret + Kural1 + Kural2...).

Bu tasarım sayesinde, `/Rules` klasörüne (örneğin `BayramIndirimi.dll` adında) yeni bir dosya bırakmak, sistemin yeniden derlenmeye gerek kalmadan bu kuralı otomatik olarak tanımasını ve çalıştırmasını sağlar.

## Zorunlu Demo Akışı ve Çıktısı

Ödevin zorunlu senaryosu, sisteme yeni bir kural (`LoyaltyDiscountRule.dll`) eklendiğinde fiyatın otomatik olarak değiştiğini göstermektir.

Proje ilk çalıştırıldığında (Rebuild Solution ile) `/Rules` klasöründe 2 adet DLL (`UstaPlatform.Rules.Default.dll` ve `UstaPlatform.Rules.Loyalty.dll`) bulunur.

Aşağıdaki konsol çıktısı, motorun bu iki kuralı da bulduğunu ve hesaplamaya dahil ettiğini kanıtlar:

```
UstaPlatform Simülasyonu Başlatılıyor...
İş Emri ... oluşturuldu.
İş Emri Planlanan Tarih: 26.10.2025 (Pazar)
-----------------------------------------

ÖDEVİN ANA SENARYOSU: PLUG-IN FİYATLAMA MOTORU

Fiyatlama Motoru (PricingEngine) başlatılıyor...
[PricingEngine] Başlatılıyor... Kural DLL'leri taranıyor...
[PricingEngine] 'Rules' klasöründe 2 adet .dll bulundu.
[PricingEngine] KURAL YÜKLENDİ: Haftasonu Ek Ücreti (Kaynak: UstaPlatform.Rules.Default.dll)
[PricingEngine] KURAL YÜKLENDİ: Sadakat İndirimi (%10) (Kaynak: UstaPlatform.Rules.Loyalty.dll)

...Motor taramayı bitirdi.
Mevcut kurallar kullanılarak fiyat hesaplanıyor:
[PricingEngine] Fiyat hesaplanıyor... (Temel Ücret: ?150,00)
  -> Kural Uygulandı: Haftasonu Ek Ücreti, Etki: ?50,00
  -> Kural Uygulandı: Sadakat İndirimi (%10), Etki: -?20,00
[PricingEngine] HESAPLANAN SON FİYAT: ?180,00
```

```
-----------------------------------------
İŞ EMRİ ÖZETİ (Senaryo 1):
  -> Temel Ücret: ?150,00
  -> SON FİYAT  : ?180,00
-----------------------------------------
```

Bu çıktı, motorun yeni `.dll` dosyasını (`LoyaltyDiscountRule`) yeniden derlemeye gerek kalmadan otomatik olarak bulduğunu ve 150 TL'lik temel ücrete önce 50 TL eklediğini (200 TL), ardından bu 200 TL üzerinden %10 indirim (-20 TL) uygulayarak son fiyatı 180 TL olarak hesapladığını kanıtlar.

## Opsiyonel Testler (Ek Puan)

Ek puan gereksinimi için `UstaPlatform.Tests` adında bir xUnit projesi oluşturulmuştur.

Bu proje, Visual Studio Test Explorer üzerinden çalıştırıldığında, hem `Schedule` sınıfının Dizinleyici (Indexer) özelliği hem de `PricingEngine` sınıfının hesaplama mantığı için yazılan testlerin tamamı başarıyla geçmektedir (2 Passed).
