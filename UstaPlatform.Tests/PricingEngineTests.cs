// UstaPlatform.Tests/PricingEngineTests.cs

using UstaPlatform.Domain;
using UstaPlatform.Pricing;
using Xunit; // Hata veren 'Assert' ve 'Fact' için bu satır gerekli!

namespace UstaPlatform.Tests
{
    // ---- Testler için "Sahte" (Stub) Kural Sınıfları ----
    public class Stub_50TL_EkUcretKurali : IPricingRule
    {
        public string RuleName => "Test Kuralı: +50 TL";
        public decimal CalculatePriceAdjustment(decimal currentPrice, WorkOrder workOrder)
        {
            return 50m; // Her zaman 50 TL ekler
        }
    }

    public class Stub_Yuzde10_IndirimKurali : IPricingRule
    {
        public string RuleName => "Test Kuralı: %10 İndirim";
        public decimal CalculatePriceAdjustment(decimal currentPrice, WorkOrder workOrder)
        {
            return -(currentPrice * 0.10m);
        }
    }

    // ---- Ana Test Sınıfı ----
    public class PricingEngineTests
    {
        [Fact]
        public void CalculatePrice_ShouldApplyMultipleRulesCorrectly()
        {
            // ---- 1. ARRANGE (Hazırlık) ----
            var sahteKurallar = new List<IPricingRule>
            {
                new Stub_50TL_EkUcretKurali(),
                new Stub_Yuzde10_IndirimKurali()
            };
            var motor = new PricingEngine(sahteKurallar);
            var isEmri = new WorkOrder(Guid.NewGuid())
            {
                TemelUcret = 200m
            };

            // ---- 2. ACT (Eylem) ----
            decimal sonFiyat = motor.CalculatePrice(isEmri);

            // ---- 3. ASSERT (Doğrulama) ----
            // Beklenen Hesap: 200 + 50 = 250. 250 - (250 * 0.10) = 225.
            Assert.Equal(225m, sonFiyat);
        }
    }
}