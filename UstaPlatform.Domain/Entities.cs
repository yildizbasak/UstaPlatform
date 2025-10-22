// UstaPlatform.Domain/Entities.cs

namespace UstaPlatform.Domain
{
    /// <summary>
    /// Onaylanmış, ustaya atanmış ve planlanmış iş.
    /// </summary>
    public class WorkOrder
    {
        public Guid Id { get; init; }
        public DateTime KayitZamani { get; init; }
        public DateOnly PlanlananTarih { get; set; }

        public Usta AtanmisUsta { get; set; }
        public Vatandas TalepEdenVatandas { get; set; }

        public string IsTanimi { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;

        public decimal TemelUcret { get; set; }
        public decimal SonFiyat { get; set; }

        public WorkOrder(Guid id)
        {
            Id = id;
            KayitZamani = DateTime.UtcNow;

            // Constructor içinde null olabilecek referansları başlatalım
            AtanmisUsta = new Usta();
            TalepEdenVatandas = new Vatandas();
        }
    }

    /// <summary>
    /// Hizmet veren uzman.
    /// </summary>
    public class Usta
    {
        public int Id { get; init; }
        public string AdSoyad { get; set; } = string.Empty;
        public string UzmanlikAlani { get; set; } = string.Empty;
        public double Puan { get; set; }
        public int Yogunluk { get; set; }
    }

    /// <summary>
    /// Hizmet talep eden kişi.
    /// </summary>
    public class Vatandas
    {
        public int Id { get; init; }
        public string AdSoyad { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
    }

    /// <summary>
    /// Vatandaşın açtığı iş talebi.
    /// </summary>
    public class Talep
    {
        public int Id { get; init; }
        public Vatandas TalepEden { get; set; }
        public string IsTanimi { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;
        public DateTime IstenenTarih { get; set; }

        public Talep()
        {
            TalepEden = new Vatandas();
        }
    }
}