// UstaPlatform.Tests/ScheduleTests.cs

using UstaPlatform.Domain;
using Xunit; // Hata veren 'Assert' ve 'Fact' için bu satýr gerekli!

namespace UstaPlatform.Tests
{
    public class ScheduleTests
    {
        [Fact] // Bu, xUnit'e bunun bir test metodu olduðunu söyler.
        public void Schedule_Indexer_ShouldReturnCorrectWorkOrdersForDate()
        {
            // ---- 1. ARRANGE (Hazýrlýk) ----
            var schedule = new Schedule();
            var testTarihi = new DateOnly(2025, 11, 1); // 1 Kasým 2025
            var isEmri1 = new WorkOrder(Guid.NewGuid())
            {
                IsTanimi = "Test Ýþi 1",
                PlanlananTarih = testTarihi
            };
            var isEmri2 = new WorkOrder(Guid.NewGuid())
            {
                IsTanimi = "Baþka Günün Ýþi",
                PlanlananTarih = testTarihi.AddDays(1) // 2 Kasým
            };
            schedule.AddWorkOrder(isEmri1);
            schedule.AddWorkOrder(isEmri2);

            // ---- 2. ACT (Eylem) ----
            List<WorkOrder> bulunanIsEmirleri = schedule[testTarihi];
            List<WorkOrder> bosListe = schedule[testTarihi.AddDays(10)];

            // ---- 3. ASSERT (Doðrulama) ----
            Assert.NotNull(bulunanIsEmirleri);
            Assert.Single(bulunanIsEmirleri);
            Assert.Equal("Test Ýþi 1", bulunanIsEmirleri[0].IsTanimi);
            Assert.NotNull(bosListe);
            Assert.Empty(bosListe);
        }
    }
}