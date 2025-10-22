Proje: UstaPlatform - �ehrin Uzmanl�k Platformu
Bu proje, Nesne Y�nelimli Programlama (NYP) ve �leri C# dersi kapsam�nda geli�tirilmi�tir. Amac�, Arcadia �ehrindeki uzmanlarla vatanda� taleplerini e�le�tiren, dinamik ve geni�letilebilir bir platform olu�turmakt�r. 


Projenin temel odak noktas�, kat� kurallar yerine, A��k/Kapal� Prensibine (OCP) uygun, "Plug-in" (Eklenti) tabanl� bir fiyatland�rma mimarisi kurmakt�r. 


Uygulanan Teknik Gereksinimler
Proje, �dev rehberinde belirtilen a�a��daki �leri C# �zelliklerini ve SOLID prensiplerini uygulamaktad�r:


A��k/Kapal� Prensibi (OCP): Sistemin kalbi olan PricingEngine, ana kodu de�i�tirmeden yeni fiyat kurallar�n�n (.dll dosyalar� olarak) sisteme eklenmesine olanak tan�r. 



Ba��ml�l�klar�n Tersine �evrilmesi (DIP): Fiyatlama motoru, somut kural s�n�flar�na de�il, IPricingRule aray�z�ne ba��ml�d�r. 


init-only �zelli�i: WorkOrder gibi varl�k s�n�flar�nda, ID ve KayitZamani gibi alanlar nesne olu�turulduktan sonra de�i�tirilemez. 


Dizinleyici (Indexer): Schedule s�n�f�, Schedule[DateOnly g�n] format�nda eri�ime izin vererek o g�ne ait i� emirlerini listeler. 


�zel IEnumerable<T> Koleksiyonu: Route s�n�f�, IEnumerable aray�z�n� uygular ve new Route { {10, 20} } gibi koleksiyon ba�lat�c�lar� (collection initializers) destekler. 

Kurulum ve �al��t�rma Bilgisi 

Projeyi Visual Studio 2022 ile a��n.

Solution Explorer'da Solution 'UstaPlatform' �zerine sa� t�klay�p "Restore NuGet Packages" (NuGet Paketlerini Geri Y�kle) deyin.

Solution Explorer'da Solution 'UstaPlatform' �zerine sa� t�klay�p "Rebuild Solution" (��z�m� Yeniden Derle) deyin.

(Bu ad�m, UstaPlatform.Rules.Default.dll ve UstaPlatform.Rules.Loyalty.dll dosyalar�n� UstaPlatform.App projesinin /bin/Debug/.../Rules klas�r�ne otomatik olarak kopyalayacakt�r.)

Solution Explorer'da UstaPlatform.App projesini "Set as Startup Project" (Ba�lang�� Projesi Olarak Ayarla) olarak se�in.

Ye�il "Play" (Oynat) tu�una (veya F5'e) basarak projeyi �al��t�r�n.

Tasar�m Kararlar�: Plug-in (Eklenti) Mimarisi Nas�l �al���yor? 

Projenin en kritik gereksinimi, ana uygulama de�i�meden sisteme yeni fiyat kurallar� ekleyebilmektir.  Bu mimari, C#'�n "Reflection" (Yans�ma) �zelli�i kullan�larak sa�lanm��t�r.



IPricingRule Aray�z� (S�zle�me): UstaPlatform.Domain projesinde, t�m kurallar�n uymas� gereken bir s�zle�me (IPricingRule) tan�mlanm��t�r. Bu aray�z, Ba��ml�l�klar�n Tersine �evrilmesi (DIP) prensibini uygular. 


PricingEngine (Fiyatlama Motoru): UstaPlatform.Pricing projesindeki bu s�n�f, uygulama ba�lad���nda (new PricingEngine() �a�r�ld���nda) �al���r.


DLL Taramas� (Reflection): Motor, uygulaman�n �al��t��� dizindeki /Rules klas�r�n� tarar.  System.Reflection k�t�phanesini kullanarak bu klas�rdeki t�m .dll dosyalar�n� bulur ve bu dosyalar�n "i�ine bakar".


Kural Y�kleme: Motor, DLL'ler i�inde IPricingRule aray�z�n� uygulayan t�m somut s�n�flar� bulur,  bu s�n�flardan nesneler (instance) yarat�r ve �zel bir List<IPricingRule> listesine ekler.


Kompozisyon ile Fiyatlama: Fiyat hesaplamas� istendi�inde (CalculatePrice), motor bu listedeki t�m kurallar� temel �cret �zerine s�rayla uygular (Temel �cret + Kural1 + Kural2...). 

Bu tasar�m sayesinde, /Rules klas�r�ne (�rne�in BayramIndirimi.dll ad�nda) yeni bir dosya b�rakmak, sistemin yeniden derlenmeye gerek kalmadan bu kural� otomatik olarak tan�mas�n� ve �al��t�rmas�n� sa�lar.

Zorunlu Demo Ak��� ve ��kt�s� 

�devin zorunlu senaryosu, sisteme yeni bir kural (LoyaltyDiscountRule.dll) eklendi�inde fiyat�n otomatik olarak de�i�ti�ini g�stermektir. 

Proje ilk �al��t�r�ld���nda (Rebuild Solution ile) /Rules klas�r�nde 2 adet DLL (UstaPlatform.Rules.Default.dll ve UstaPlatform.Rules.Loyalty.dll) bulunur.

A�a��daki konsol ��kt�s�, motorun bu iki kural� da buldu�unu ve hesaplamaya dahil etti�ini kan�tlar:

UstaPlatform Sim�lasyonu Ba�lat�l�yor...
�� Emri ... olu�turuldu.
�� Emri Planlanan Tarih: 26.10.2025 (Pazar)
-----------------------------------------

�DEV�N ANA SENARYOSU: PLUG-IN F�YATLAMA MOTORU

Fiyatlama Motoru (PricingEngine) ba�lat�l�yor...
[PricingEngine] Ba�lat�l�yor... Kural DLL'leri taran�yor...
[PricingEngine] 'Rules' klas�r�nde 2 adet .dll bulundu.
[PricingEngine] KURAL Y�KLEND�: Haftasonu Ek �creti (Kaynak: UstaPlatform.Rules.Default.dll)
[PricingEngine] KURAL Y�KLEND�: Sadakat �ndirimi (%10) (Kaynak: UstaPlatform.Rules.Loyalty.dll)

...Motor taramay� bitirdi.
Mevcut kurallar kullan�larak fiyat hesaplan�yor:
[PricingEngine] Fiyat hesaplan�yor... (Temel �cret: ?150,00)
  -> Kural Uyguland�: Haftasonu Ek �creti, Etki: ?50,00
  -> Kural Uyguland�: Sadakat �ndirimi (%10), Etki: -?20,00
[PricingEngine] HESAPLANAN SON F�YAT: ?180,00

-----------------------------------------
�� EMR� �ZET� (Senaryo 1):
  -> Temel �cret: ?150,00
  -> SON F�YAT  : ?180,00
-----------------------------------------
Bu ��kt�, motorun yeni .dll dosyas�n� (LoyaltyDiscountRule) yeniden derlemeye gerek kalmadan otomatik olarak buldu�unu ve 150 TL'lik temel �crete �nce 50 TL ekledi�ini (200 TL), ard�ndan bu 200 TL �zerinden %10 indirim (-20 TL) uygulayarak son fiyat� 180 TL olarak hesaplad���n� kan�tlar. 

Opsiyonel Testler (Ek Puan) 

Ek puan gereksinimi i�in UstaPlatform.Tests ad�nda bir xUnit projesi olu�turulmu�tur.

Bu proje, Visual Studio Test Explorer �zerinden �al��t�r�ld���nda, hem Schedule s�n�f�n�n Dizinleyici (Indexer) �zelli�i hem de PricingEngine s�n�f�n�n hesaplama mant��� i�in yaz�lan testlerin tamam� ba�ar�yla ge�mektedir (2 Passed).