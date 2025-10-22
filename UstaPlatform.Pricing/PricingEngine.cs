// UstaPlatform.Pricing/PricingEngine.cs

using System.Reflection; // DLL'leri taramak (Reflection) için bu gerekli
using UstaPlatform.Domain; // IPricingRule ve WorkOrder için bu gerekli

namespace UstaPlatform.Pricing
{
    /// <summary>
    /// Ödevin gerektirdiği Fiyatlama Motoru.
    /// Uygulama başladığında belirtilen klasördeki DLL'leri tarar,
    /// IPricingRule uygulayan sınıfları bulur ve bunları fiyat
    /// hesaplamasında kullanır.
    /// </summary>
    public class PricingEngine
    {
        // Bulunan ve yüklenen tüm kuralları bu listede tutacağız.
        private readonly List<IPricingRule> _rules = new List<IPricingRule>();

        // Motoru başlattığımızda kuralları yükleyelim. (Ana Uygulama için)
        public PricingEngine()
        {
            Console.WriteLine("[PricingEngine] Başlatılıyor... Kural DLL'leri taranıyor...");
            LoadRulesFromDirectory();
        }

        // ---- YENİ EKLENEN KOD (Adım 26) ----
        // Bu constructor, SADECE BİRİM TESTLERİ (xUnit) için eklendi.
        // Dışarıdan "sahte" kurallar alarak motorun DLL taramasını
        // es geçmesini ve direkt hesaplama mantığını test etmemizi sağlar.
        public PricingEngine(IEnumerable<IPricingRule> injectedRules)
        {
            Console.WriteLine("[PricingEngine] Test modu başlatıldı. Kurallar enjekte edildi.");
            _rules = new List<IPricingRule>(injectedRules);
        }
        // ---- YENİ EKLENEN KOD BİTİŞİ ----

        private void LoadRulesFromDirectory()
        {
            // 1. Kuralların bulunduğu klasörün yolunu belirleyelim.
            // Uygulamanın çalıştığı dizinde (".../bin/Debug/net8.0") 
            // "Rules" adında bir klasör arayacağız.
            string appDirectory = AppContext.BaseDirectory;
            string rulesDirectory = Path.Combine(appDirectory, "Rules");

            // 2. "Rules" klasörü var mı? Yoksa oluşturalım.
            if (!Directory.Exists(rulesDirectory))
            {
                Console.WriteLine($"[PricingEngine] Uyarı: 'Rules' klasörü bulunamadı.");
                Directory.CreateDirectory(rulesDirectory);
                Console.WriteLine($"[PricingEngine] 'Rules' klasörü oluşturuldu: ({rulesDirectory})");
                return; // Klasör yeni oluştuğu için içi boştur, devam etmeye gerek yok.
            }

            // 3. Klasördeki tüm .dll dosyalarını bul.
            var ruleFiles = Directory.GetFiles(rulesDirectory, "*.dll");
            Console.WriteLine($"[PricingEngine] 'Rules' klasöründe {ruleFiles.Length} adet .dll bulundu.");

            foreach (var file in ruleFiles)
            {
                try
                {
                    // 4. DLL'i (Assembly) hafızaya yükle. (Burası "Reflection" kısmıdır)
                    Assembly ruleAssembly = Assembly.LoadFrom(file);

                    // 5. DLL içindeki tüm tipleri (sınıfları) gez.
                    foreach (var type in ruleAssembly.GetTypes())
                    {
                        // 6. Bu tip, 'IPricingRule' arayüzünü uyguluyor mu?
                        //    Ve bir arayüz veya soyut sınıf değil, "somut" bir sınıf mı?
                        if (typeof(IPricingRule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            // 7. Evet, uyguluyor. Bu sınıftan bir nesne yarat (instance).

                            // HATA DÜZELTMESİ (CS8600): Activator null döndürebilir.
                            // Tiplerin sonuna "?" ekleyerek null olabileceğini belirtiyoruz.
                            IPricingRule? ruleInstance = (IPricingRule?)Activator.CreateInstance(type);

                            if (ruleInstance != null)
                            {
                                _rules.Add(ruleInstance);
                                Console.WriteLine($"[PricingEngine] KURAL YÜKLENDİ: {ruleInstance.RuleName} (Kaynak: {Path.GetFileName(file)})");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // DLL yüklenirken bir hata olursa (örn: bozuk dosya)
                    Console.WriteLine($"[PricingEngine] HATA: {Path.GetFileName(file)} yüklenemedi. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// İş emrinin son fiyatını, yüklenen tüm kuralları
        /// sırayla uygulayarak hesaplar.
        /// </summary>
        public decimal CalculatePrice(WorkOrder workOrder)
        {
            decimal finalPrice = workOrder.TemelUcret;

            if (_rules.Count == 0)
            {
                Console.WriteLine("[PricingEngine] Fiyatı etkileyecek aktif kural bulunamadı.");
            }

            Console.WriteLine($"[PricingEngine] Fiyat hesaplanıyor... (Temel Ücret: {finalPrice:C})");

            // 8. Tüm kuralları sırayla uygula (Kompozisyon).
            foreach (var rule in _rules)
            {
                // HATA DÜZELTMESİ (CS8604): ruleInstance null olabilir diye uyarıyordu.
                // Biz zaten yukarıda "if (ruleInstance != null)" ile kontrol edip
                // listeye eklediğimiz için bu listenin null içermediğinden eminiz.

                decimal adjustment = rule.CalculatePriceAdjustment(finalPrice, workOrder);

                // Sadece fiyatı değiştiren kuralları rapora yazdıralım
                if (adjustment != 0)
                {
                    Console.WriteLine($"  -> Kural Uygulandı: {rule.RuleName}, Etki: {adjustment:C}");
                    finalPrice += adjustment;
                }
            }

            workOrder.SonFiyat = finalPrice;
            Console.WriteLine($"[PricingEngine] HESAPLANAN SON FİYAT: {finalPrice:C}");
            return finalPrice;
        }
    }
}