// UstaPlatform.Rules.Default/HaftasonuEkUcretiKurali.cs

using UstaPlatform.Domain;
// USING DÜZELTMESİ: ".Entities" kaldırıldı. Artık UstaPlatform.Domain altında.
// using UstaPlatform.Domain.Entities; <-- Bu satır silindi.

namespace UstaPlatform.Rules.Default
{
    /// <summary>
    /// Örnek bir kural: Hafta sonu (Cumartesi veya Pazar) 
    /// yapılan işlere 50 TL ek ücret uygular.
    /// Bu sınıf, IPricingRule arayüzünü uygular.
    /// </summary>
    public class HaftasonuEkUcretiKurali : IPricingRule
    {
        // Arayüzden gelen zorunlu özellik:
        public string RuleName => "Haftasonu Ek Ücreti";

        // Arayüzden gelen zorunlu metod:
        public decimal CalculatePriceAdjustment(decimal currentPrice, WorkOrder workOrder)
        {
            // İş emrinin planlandığı tarihi al
            DayOfWeek gun = workOrder.PlanlananTarih.DayOfWeek;

            // Eğer gün Cumartesi VEYA Pazar ise
            if (gun == DayOfWeek.Saturday || gun == DayOfWeek.Sunday)
            {
                // Fiyata 50 TL ek ücret yansıt
                return 50.00m;
            }

            // Hafta sonu değilse, fiyata hiçbir etkisi yok (0 TL)
            return 0m;
        }
    }
}