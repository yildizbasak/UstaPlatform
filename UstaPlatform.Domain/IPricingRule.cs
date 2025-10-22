// UstaPlatform.Domain/IPricingRule.cs

// USING DÜZELTMESİ: ".Entities" kaldırıldı, çünkü WorkOrder artık UstaPlatform.Domain altında.
using UstaPlatform.Domain;

namespace UstaPlatform.Domain
{
    /// <summary>
    /// Fiyat hesaplama motoruna (PricingEngine) takılabilen (Plug-in)
    /// dinamik fiyat kurallarını tanımlayan arayüz.
    /// </summary>
    public interface IPricingRule
    {
        /// <summary>
        /// Kuralın Adı (Örn: "Haftasonu Ek Ücreti")
        /// </summary>
        string RuleName { get; }

        /// <summary>
        /// Mevcut fiyata etki eden tutarı hesaplar.
        /// Pozitif değer (ek ücret) veya negatif değer (indirim) olabilir.
        /// </summary>
        /// <param name="currentPrice">Mevcut Fiyat</param>
        /// <param name="workOrder">İlgili İş Emri</param>
        /// <returns>Fiyata eklenecek (veya çıkarılacak) tutar.</returns>
        decimal CalculatePriceAdjustment(decimal currentPrice, WorkOrder workOrder);
    }
}