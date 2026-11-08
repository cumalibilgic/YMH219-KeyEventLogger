# YMH219-KeyEventLogger

Windows Forms ile geliştirilmiş **eğitsel** bir klavye olay kaydedicisi. Nesne Tabanlı Programlama (YMH 219) dersi ek uygulama ödevidir.

Uygulama yalnızca **kendi penceresi aktifken** basılan tuşları gösterir. Global hook, gizli çalışma, ağ üzerinden gönderim yoktur; gerçek bir keylogger değildir.

## Kullanılan Teknolojiler
- C# / .NET 10 (Windows Forms)
- Visual Studio 2022
- Harici NuGet paketi kullanılmamıştır

## Kurulum ve Çalıştırma
1. Depoyu klonlayın: `git clone https://github.com/cumalibilgic/YMH219-KeyEventLogger.git`
2. `YMH219-KeyEventLogger.csproj` dosyasını Visual Studio ile açın.
3. `F5` ile çalıştırın.

## Özellikler
- KeyDown / KeyUp olaylarını tuş adı ve tarih-saat ile listeleme
- Başlat / Durdur ile kayıt kontrolü, kayıt durumu göstergesi (kırmızı = kayıt açık)
- Temizle butonu
- Kayıtları `.txt` dosyasına kaydetme (StreamWriter)
- Toplam tuş sayısı ve en sık kullanılan 3 tuş istatistiği

## Proje Yapısı
- `KeyRecord.cs` : tek bir olay kaydı (sınıf)
- `LogFileWriter.cs` : dosyaya yazma işlemi
- `MainForm.cs` : arayüz ve olay yönetimi
- `Program.cs` : giriş noktası

## Ekran Görüntüleri
> Buraya en az 2 ekran görüntüsü ekleyin (kayıt açıkken ve kaydedilmiş dosya).

![Ekran 1](screenshots/ekran1.png)
![Ekran 2](screenshots/ekran2.png)

## Bilinen Sınırlamalar
- Yalnızca uygulama penceresi odaktayken tuşlar yakalanır.
- Türkçe karakterler tuş kodu adıyla gösterilir (ör. `Oem1`).

## Öğrenci
Cumali Bilgiç — Öğrenci No: 250541008
