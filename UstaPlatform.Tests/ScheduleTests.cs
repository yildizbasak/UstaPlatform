// UstaPlatform.Tests/ScheduleTests.cs

using UstaPlatform.Domain;
using Xunit; // Hata veren 'Assert' ve 'Fact' i�in bu sat�r gerekli!

namespace UstaPlatform.Tests
{
    public class ScheduleTests
    {
        [Fact] // Bu, xUnit'e bunun bir test metodu oldu�unu s�yler.
        public void Schedule_Indexer_ShouldReturnCorrectWorkOrdersForDate()
        {
            // ---- 1. ARRANGE (Haz�rl�k) ----
            var schedule = new Schedule();
            var testTarihi = new DateOnly(2025, 11, 1); // 1 Kas�m 2025
            var isEmri1 = new WorkOrder(Guid.NewGuid())
            {
                IsTanimi = "Test ��i 1",
                PlanlananTarih = testTarihi
            };
            var isEmri2 = new WorkOrder(Guid.NewGuid())
            {
                IsTanimi = "Ba�ka G�n�n ��i",
                PlanlananTarih = testTarihi.AddDays(1) // 2 Kas�m
            };
            schedule.AddWorkOrder(isEmri1);
            schedule.AddWorkOrder(isEmri2);

            // ---- 2. ACT (Eylem) ----
            List<WorkOrder> bulunanIsEmirleri = schedule[testTarihi];
            List<WorkOrder> bosListe = schedule[testTarihi.AddDays(10)];

            // ---- 3. ASSERT (Do�rulama) ----
            Assert.NotNull(bulunanIsEmirleri);
            Assert.Single(bulunanIsEmirleri);
            Assert.Equal("Test ��i 1", bulunanIsEmirleri[0].IsTanimi);
            Assert.NotNull(bosListe);
            Assert.Empty(bosListe);
        }
    }
}