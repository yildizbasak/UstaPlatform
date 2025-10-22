// UstaPlatform.Domain/Route.cs

using System.Collections;
using System.Collections.Generic;

namespace UstaPlatform.Domain
{
    /// <summary>
    /// Bir uzmanın günlük ziyaret edeceği adreslerin (X, Y) sırası.
    /// Ödev gereği IEnumerable arayüzünü uygular
    /// ve Koleksiyon Başlatıcıları destekler.
    /// </summary>
    public class Route : IEnumerable<(int X, int Y)>
    {
        // Rota duraklarını (koordinatları) saklamak için dahili bir liste kullanıyoruz.
        private readonly List<(int X, int Y)> _duraklar = new List<(int X, int Y)>();

        /// <summary>
        /// Ödevde istenen "Koleksiyon Başlatıcı" (Collection Initializer) desteği.
        /// Bu metod sayesinde "new Route { {10, 20}, {30, 40} }" 
        /// şeklinde kod yazabiliyoruz.
        /// </summary>
        public void Add(int X, int Y)
        {
            _duraklar.Add((X, Y));
        }

        // IEnumerable<(int X, int Y)> arayüzünün gerektirdiği metod.
        // Bu sayede Route nesnesi üzerinde "foreach" döngüsü kullanabiliriz.
        public IEnumerator<(int X, int Y)> GetEnumerator()
        {
            return _duraklar.GetEnumerator();
        }

        // IEnumerable arayüzünün eski (non-generic) versiyonunun gerektirdiği metod.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}