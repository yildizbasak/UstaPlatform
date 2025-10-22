// UstaPlatform.Domain/Schedule.cs

using System;
using System.Collections.Generic;
using UstaPlatform.Domain; // WorkOrder sınıfını kullanabilmek için bu gerekli

namespace UstaPlatform.Domain
{
    /// <summary>
    /// Ustaların iş emri takvimi.
    /// Ödev gereği tarihe göre iş emirlerini listeleyen 
    /// bir Dizinleyici (Indexer) içerir.
    /// </summary>
    public class Schedule
    {
        // Takvimi saklamak için en verimli yapı Dictionary'dir.
        // Her bir tarihi (DateOnly), o tarihteki iş emirleri listesine (List<WorkOrder>) eşler.
        private readonly Dictionary<DateOnly, List<WorkOrder>> _takvim =
            new Dictionary<DateOnly, List<WorkOrder>>();

        /// <summary>
        /// Ödevde istenen DİZİNLEYİCİ (INDEXER).
        /// Schedule[DateOnly gün] yapısını kullanarak o güne ait 
        /// iş emirleri listesine kolay erişim sağlar.
        /// </summary>
        /// <param name="gun">Bilgisi istenen gün (DateOnly formatında)</param>
        /// <returns>O güne ait İş Emirleri listesi. O gün iş yoksa boş liste döner.</returns>
        public List<WorkOrder> this[DateOnly gun]
        {
            get
            {
                // "TryGetValue" metodu, o tarihte bir kayıt olup olmadığını kontrol eder.
                // Varsa "isEmirleri" değişkenine atar ve true döner.
                if (_takvim.TryGetValue(gun, out var isEmirleri))
                {
                    return isEmirleri;
                }

                // O gün için hiç kayıt (iş emri) yoksa, null veya hata dönmek yerine
                // boş bir liste dönmek, kodu kullanan kişi için daha güvenli bir yaklaşımdır.
                return new List<WorkOrder>();
            }
        }

        /// <summary>
        /// Çizelgeye yeni bir iş emri eklemek için yardımcı bir metod.
        /// </summary>
        public void AddWorkOrder(WorkOrder workOrder)
        {
            DateOnly gun = workOrder.PlanlananTarih;

            // O gün için _takvim'de bir liste yoksa, önce listeyi oluşturmalıyız.
            if (!_takvim.ContainsKey(gun))
            {
                _takvim[gun] = new List<WorkOrder>();
            }

            // Artık o güne ait listenin varlığından eminiz, iş emrini ekleyebiliriz.
            _takvim[gun].Add(workOrder);
        }
    }
}