// UstaPlatform.Rules.Loyalty/LoyaltyDiscountRule.cs

using UstaPlatform.Domain;

namespace UstaPlatform.Rules.Loyalty
{
    /// <summary>
    /// Ödevin demo senaryosu için gereken YENİ DLL kuralı.
    /// Bu kural, müşteriye %10 indirim uygular.
    /// </summary>
    public class LoyaltyDiscountRule : IPricingRule
    {
        public string RuleName => "Sadakat İndirimi (%10)";

        public decimal CalculatePriceAdjustment(decimal currentPrice, WorkOrder workOrder)
        {
            // Bu kural, mevcut fiyat üzerinden %10 indirim yapar.
            // İndirim olduğu için negatif (-) bir değer döndürüyoruz.
            // NOT: Fiyat hesaplaması (150 Temel + 50 Haftasonu = 200) üzerinden
            // %10 indirim yapacaktır. (Yani -20 TL)
            decimal discountAmount = currentPrice * 0.10m;
            return -discountAmount;
        }
    }
}