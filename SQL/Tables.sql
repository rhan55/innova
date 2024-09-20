IF NOT EXISTS(select * from sys.tables Where name = 'Mesajlar')
BEGIN

CREATE TABLE [dbo].[Mesajlar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[KullaniciID] [uniqueidentifier] NOT NULL,
	[KarsiKullaniciID] [uniqueidentifier] NOT NULL,
	[Mesaj] [nvarchar](4000) NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[GorulmeTarihi] [datetime] NULL,
	[Dosya] [nvarchar](max) NULL,
 CONSTRAINT [PK_Mesajlar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Mesajlar] ADD  CONSTRAINT [DF_Mesajlar_ID]  DEFAULT (newid()) FOR [ID]
GO
GO
IF NOT EXISTS(select * from sys.tables Where name = 'AnaSayfaTakvim')
BEGIN

CREATE TABLE [dbo].[AnaSayfaTakvim](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[KullaniciID] [uniqueidentifier] NULL,
	[Tarih] [datetime] NULL,
	[Durumu] [nvarchar](50) NULL,
	[Baslik] [nvarchar](100) NULL,
	[Aciklama] [nvarchar](max) NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [uniqueidentifier] NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AnaSayfaTakvim] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'B2BSepet')
BEGIN
CREATE TABLE [dbo].[B2BSepet](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [nvarchar](100) NULL,
	[Modul] [nvarchar](50) NULL,
	[CariID] [nvarchar](100) NULL,
	[StokID] [nvarchar](100) NULL,
	[Seri] [nvarchar](100) NULL,
	[Birim] [nvarchar](50) NULL,
	[Miktar] [decimal](18, 5) NULL,
	[Fiyat] [decimal](18, 5) NULL,
	[Tutar] [decimal](18, 5) NULL,
	[IslemTipi] [smallint] NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[SilinenTarih] [datetime] NULL,
 CONSTRAINT [PK_Sepet] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

GO

IF NOT EXISTS(select * from sys.tables Where name = 'BankaHesaplari')
BEGIN
CREATE TABLE [dbo].[BankaHesaplari](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[BankaID] [uniqueidentifier] NULL,
	[Kod] [nvarchar](100) NULL,
	[Isim] [nvarchar](100) NULL,
	[HesapNo] [nvarchar](100) NULL,
	[Iban] [nvarchar](100) NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
 CONSTRAINT [PK_Bankalar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

GO
IF NOT EXISTS(select * from sys.tables Where name = 'BelgeKalemler')
BEGIN
CREATE TABLE [dbo].[BelgeKalemler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[BelgeID] [uniqueidentifier] NULL,
	[Durumu] [bit] NULL,
	[StokID] [uniqueidentifier] NULL,
	[Seri] [nvarchar](100) NULL,
	[Miktar] [decimal](18, 8) NULL,
	[Fiyat] [decimal](18, 8) NULL,
	[KdvOrani] [decimal](18, 5) NULL,
	[IskontoOrani1] [decimal](18, 5) NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[SilinenTarih] [datetime] NULL,
 CONSTRAINT [PK_BelgeKalemler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS(select * from sys.tables Where name = 'Belgeler')
BEGIN
CREATE TABLE [dbo].[Belgeler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[Tip] [nvarchar](10) NULL,
	[Durumu] [nvarchar](100) NULL,
	[Tarih] [datetime] NULL,
	[BelgeNo] [nvarchar](50) NULL,
	[ProjeID] [uniqueidentifier] NULL,
	[CariID] [uniqueidentifier] NULL,
	[DepoCikisID] [uniqueidentifier] NULL,
	[DepoGirisID] [uniqueidentifier] NULL,
	[Aciklama1] [nvarchar](500) NULL,
	[SatisPersonelID] [uniqueidentifier] NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[SilinenTarih] [datetime] NULL,
 CONSTRAINT [PK_Belgeler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'CariHareketleri')
BEGIN
CREATE TABLE [dbo].[CariHareketleri](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[CariID] [uniqueidentifier] NOT NULL,
	[Tarih] [datetime] NOT NULL,
	[VadeTarihi] [datetime] NULL,
	[BelgeNo] [nvarchar](50) NULL,
	[HareketTipi] [nvarchar](50) NULL,
	[GC] [char](1) NOT NULL,
	[Tutar] [decimal](18, 5) NOT NULL,
	[DovizTipi] [nvarchar](3) NOT NULL,
	[Kur] [decimal](18, 5) NOT NULL,
	[DovizTutar] [decimal](18, 5) NOT NULL,
	[Aciklama] [nvarchar](max) NULL,
	[PlasiyerID] [uniqueidentifier] NULL,
	[BaglantiID] [uniqueidentifier] NULL,
	[Baglanti] [nvarchar](100) NULL,
	[GrupKodu1ID] [uniqueidentifier] NULL,
	[GrupKodu2ID] [uniqueidentifier] NULL,
	[Silindi] [bit] NOT NULL,
	[SilinenTarih] [datetime] NULL,
	[SilenKullanici] [uniqueidentifier] NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CariHareketleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'CariKisiler')
BEGIN
CREATE TABLE [dbo].[CariKisiler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[CariID] [uniqueidentifier] NOT NULL,
	[Isim] [nvarchar](150) NOT NULL,
	[EMail] [nvarchar](100) NULL,
	[Gorev] [nvarchar](100) NULL,
	[Telefon] [nvarchar](50) NULL,
	[Aktif] [bit] NOT NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CariKisiler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Cariler')
BEGIN
CREATE TABLE [dbo].[Cariler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[Aktif] [bit] NOT NULL,
	[TipID] [uniqueidentifier] NULL,
	[KayitTarihi] [datetime] NULL,
	[Kod] [nvarchar](50) NOT NULL,
	[Isim] [nvarchar](500) NULL,
	[Unvan] [nvarchar](500) NULL,
	[Adres] [nvarchar](1000) NULL,
	[Ilce] [nvarchar](75) NULL,
	[Il] [nvarchar](75) NULL,
	[Ulke] [nvarchar](50) NULL,
	[Bolge] [nvarchar](100) NULL,
	[TCKimlikNo] [nvarchar](50) NULL,
	[VergiDairesi] [nvarchar](50) NULL,
	[VergiNumarasi] [nvarchar](50) NULL,
	[PostaKodu] [nvarchar](50) NULL,
	[Alici] [bit] NULL,
	[Satici] [bit] NULL,
	[Personel] [bit] NULL,
	[Telefon1] [nvarchar](50) NULL,
	[Telefon2] [nvarchar](50) NULL,
	[EMail] [nvarchar](250) NULL,
	[Faks] [nvarchar](50) NULL,
	[CepTelefonu] [nvarchar](50) NULL,
	[WebSite] [nvarchar](250) NULL,
	[GrupKodu1ID] [uniqueidentifier] NULL,
	[GrupKodu2ID] [uniqueidentifier] NULL,
	[GrupKodu3ID] [uniqueidentifier] NULL,
	[GrupKodu4ID] [uniqueidentifier] NULL,
	[GrupKodu5ID] [uniqueidentifier] NULL,
	[GrupKodu6ID] [uniqueidentifier] NULL,
	[MuhasebeKodu] [nvarchar](50) NULL,
	[Kilitli] [bit] NULL,
	[KilitAciklamasi] [nvarchar](300) NULL,
	[DovizID] [uniqueidentifier] NULL,
	[VadeGunu] [int] NULL,
	[Iskonto1] [decimal](18, 8) NULL,
	[ListeFiyat] [decimal](18, 8) NULL,
	[Aciklama1] [nvarchar](1000) NULL,
	[Aciklama2] [nvarchar](1000) NULL,
	[Aciklama3] [nvarchar](1000) NULL,
	[Aciklama4] [nvarchar](1000) NULL,
	[Aciklama5] [nvarchar](1000) NULL,
	[Aciklama6] [nvarchar](1000) NULL,
	[LimitAsimindaUyar] [bit] NULL,
	[LimitAsimindaDurdur] [bit] NULL,
	[CekSenetRiski] [bit] NULL,
	[Limit] [decimal](18, 8) NULL,
	[KayitYapanTarihDB] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[SilinenTarih] [datetime] NULL,
	[ServisPersoneli] [bit] NULL,
	[KullaniciAdi] [nvarchar](100) NULL,
	[Parola] [nvarchar](100) NULL,
	[RiskAciklama] [ntext] NULL,
	[PlasiyerID] [uniqueidentifier] NULL,
	[AnaCariID] [uniqueidentifier] NULL,
	[TeslimCariID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Cariler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'CariNotlar')
BEGIN
CREATE TABLE [dbo].[CariNotlar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[CariID] [uniqueidentifier] NOT NULL,
	[Aciklama] [text] NOT NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CariNotlar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'ChangeLog')
BEGIN
CREATE TABLE [dbo].[ChangeLog](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[DatabaseName] [varchar](256) NOT NULL,
	[EventType] [varchar](50) NOT NULL,
	[ObjectName] [varchar](256) NOT NULL,
	[ObjectType] [varchar](25) NOT NULL,
	[SqlCommand] [varchar](max) NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[LoginName] [varchar](256) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Crm2Kayitlar')
BEGIN
CREATE TABLE [dbo].[Crm2Kayitlar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[AlanKullanici] [uniqueidentifier] NULL,
	[Sozlesme] [bit] NULL,
	[VerenKullanici] [uniqueidentifier] NULL,
	[KabulTarihi] [datetime] NULL,
	[Bayi] [nvarchar](150) NULL,
	[Tarih] [datetime] NULL,
	[ProjeTipi] [uniqueidentifier] NULL,
	[BlokSayisi] [nvarchar](150) NULL,
	[Miktar] [nvarchar](150) NULL,
	[Unvan] [nvarchar](150) NULL,
	[Ad] [nvarchar](150) NULL,
	[Soyad] [nvarchar](150) NULL,
	[Telefon1] [nvarchar](150) NULL,
	[Telefon2] [nvarchar](150) NULL,
	[Gorev] [uniqueidentifier] NULL,
	[UlasimSekli] [uniqueidentifier] NULL,
	[ProjeAdi] [nvarchar](150) NULL,
	[Il] [uniqueidentifier] NULL,
	[Ilce] [nvarchar](150) NULL,
	[Mahalle] [nvarchar](150) NULL,
	[Ada] [nvarchar](150) NULL,
	[Parsel] [nvarchar](150) NULL,
	[PortalNumarasi] [nvarchar](150) NULL,
	[Resim] [nvarchar](max) NULL,
	[SonucTarihi] [datetime] NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [uniqueidentifier] NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
	[Silindi] [bit] NULL,
	[SilinenTarih] [datetime] NULL,
	[SilenKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Crm2Kayitlar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Crm2Notlar')
BEGIN
CREATE TABLE [dbo].[Crm2Notlar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[KayitID] [uniqueidentifier] NULL,
	[KullaniciID] [uniqueidentifier] NULL,
	[Aciklama] [nvarchar](max) NULL,
	[KayitTarihi] [datetime] NULL,
 CONSTRAINT [PK_Crm2Notlar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Depolar')
BEGIN
CREATE TABLE [dbo].[Depolar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[Kod] [nvarchar](50) NULL,
	[Isim] [nvarchar](100) NOT NULL,
	[Adres] [nvarchar](max) NULL,
	[Telefon] [nvarchar](100) NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Depolar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Dosyalar')
BEGIN
CREATE TABLE [dbo].[Dosyalar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Modul] [nvarchar](50) NOT NULL,
	[KayitID] [uniqueidentifier] NOT NULL,
	[Dosya] [nvarchar](max) NOT NULL,
	[Isim] [nvarchar](max) NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Dosyalar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Dovizler')
BEGIN
CREATE TABLE [dbo].[Dovizler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[Kod] [nvarchar](50) NULL,
	[Isim] [nvarchar](50) NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Dovizler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'GorevHareketleri')
BEGIN
CREATE TABLE [dbo].[GorevHareketleri](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[GorevID] [uniqueidentifier] NULL,
	[KullaniciID] [uniqueidentifier] NULL,
	[Tamamlandi] [bit] NULL,
	[TamamlamaTarihi] [datetime] NULL,
	[Aciklama] [nvarchar](max) NULL,
 CONSTRAINT [PK_GorevHareketleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'GorevKullanicilari')
BEGIN
CREATE TABLE [dbo].[GorevKullanicilari](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[GorevID] [uniqueidentifier] NULL,
	[KullaniciID] [uniqueidentifier] NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_GorevKullanicilari] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Gorevler')
BEGIN
CREATE TABLE [dbo].[Gorevler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[GorevTipiID] [uniqueidentifier] NULL,
	[Durumu] [nvarchar](100) NULL,
	[Aciklama] [nvarchar](max) NULL,
	[BaslangicTarihi] [datetime] NULL,
	[Periyot] [nvarchar](10) NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[SilinenTarih] [datetime] NULL,
 CONSTRAINT [PK_Gorevler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'GrupKodlari')
BEGIN
CREATE TABLE [dbo].[GrupKodlari](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[Kod] [nvarchar](100) NULL,
	[Deger] [nvarchar](max) NULL,
	[UstID] [uniqueidentifier] NULL,
	[Aktif] [bit] NULL,
 CONSTRAINT [PK_GrupKodlari] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Hatalar')
BEGIN
CREATE TABLE [dbo].[Hatalar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Tarih] [datetime] NULL,
	[Modul] [nvarchar](100) NULL,
	[Dosya] [nvarchar](100) NULL,
	[Hata] [nvarchar](max) NULL,
	[Hata2] [nvarchar](max) NULL,
	[Surum] [nvarchar](50) NULL,
 CONSTRAINT [PK_Hatalar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Hatirlaticilar')
BEGIN
CREATE TABLE [dbo].[Hatirlaticilar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[Modul] [nvarchar](50) NULL,
	[KayitID] [uniqueidentifier] NULL,
	[HatirlatmaTarihi] [datetime] NULL,
	[HatirlatilacakKullanici] [uniqueidentifier] NULL,
	[Aciklama] [nvarchar](max) NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [uniqueidentifier] NULL,
	[Gosterme] [bit] NULL,
 CONSTRAINT [PK_Hatirlaticilar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Kasalar')
BEGIN
CREATE TABLE [dbo].[Kasalar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[Kod] [nvarchar](100) NULL,
	[Isim] [nvarchar](100) NULL,
	[DovizID] [uniqueidentifier] NULL,
	[PersonelID] [uniqueidentifier] NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Kasalar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Kullanicilar')
BEGIN
CREATE TABLE [dbo].[Kullanicilar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[KullaniciAdi] [nvarchar](100) NOT NULL,
	[Parola] [nvarchar](100) NOT NULL,
	[Ad] [nvarchar](100) NOT NULL,
	[Soyad] [nvarchar](100) NULL,
	[Telefon] [nvarchar](100) NULL,
	[Adres] [nvarchar](500) NULL,
	[Il] [nvarchar](100) NULL,
	[Ilce] [nvarchar](100) NULL,
	[Aktif] [bit] NOT NULL,
	[Aciklama1] [nvarchar](max) NULL,
	[Aciklama2] [nvarchar](max) NULL,
	[Aciklama3] [nvarchar](max) NULL,
	[Tarih] [datetime] NULL,
	[Resim] [nvarchar](500) NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NOT NULL,
	[SilinenTarih] [datetime] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[Onay] [bit] NULL,
	[SicilNo] [nvarchar](50) NULL,
	[PersonelParola] [nvarchar](50) NULL,
 CONSTRAINT [PK_Uyelikler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Loglar')
BEGIN
CREATE TABLE [dbo].[Loglar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[Modul] [nvarchar](100) NULL,
	[Aciklama1] [nvarchar](max) NULL,
	[Aciklama2] [nvarchar](max) NULL,
	[KayitTarihi] [datetime] NULL,
	[Kullanici] [nvarchar](100) NULL,
 CONSTRAINT [PK_Loglar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'LoglarGiris')
BEGIN
CREATE TABLE [dbo].[LoglarGiris](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ProgramAdi] [nvarchar](max) NULL,
	[Surum] [nvarchar](max) NULL,
	[Tarih] [datetime] NULL,
	[Sirket] [nvarchar](250) NULL,
	[ConnectionString] [nvarchar](250) NULL,
	[KullaniciAdi] [nvarchar](250) NULL,
	[Parola] [nvarchar](250) NULL,
	[IP] [nvarchar](100) NULL,
 CONSTRAINT [PK_LoglarGiris] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'MailKaliplari')
BEGIN
CREATE TABLE [dbo].[MailKaliplari](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[Kod] [nvarchar](50) NULL,
	[Isim] [nvarchar](100) NOT NULL,
	[Icerik] [nvarchar](max) NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_MailKaliplari] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Menuler')
BEGIN
CREATE TABLE [dbo].[Menuler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Menu] [nvarchar](100) NULL,
	[UstID] [uniqueidentifier] NULL,
	[icon] [nvarchar](250) NULL,
	[url] [nvarchar](250) NULL,
	[Sira] [int] NULL,
	[Aktif] [bit] NULL,
 CONSTRAINT [PK_Menuler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'MenulerUretim')
BEGIN
CREATE TABLE [dbo].[MenulerUretim](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Menu] [nvarchar](100) NULL,
	[UstID] [uniqueidentifier] NULL,
	[icon] [nvarchar](250) NULL,
	[url] [nvarchar](250) NULL,
	[Sira] [int] NULL,
	[Aktif] [bit] NULL,
	[Resim] [nvarchar](max) NULL,
 CONSTRAINT [PK_MenulerUretim] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Mesajlar')
BEGIN
CREATE TABLE [dbo].[Mesajlar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[KullaniciID] [uniqueidentifier] NOT NULL,
	[KarsiKullaniciID] [uniqueidentifier] NOT NULL,
	[Mesaj] [nvarchar](4000) NOT NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[GorulmeTarihi] [datetime] NULL,
	[Dosya] nvarchar(max) NULL,
 CONSTRAINT [PK_Mesajlar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Parametreler')
BEGIN
CREATE TABLE [dbo].[Parametreler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[Modul] [nvarchar](100) NOT NULL,
	[Isim] [nvarchar](250) NOT NULL,
	[Deger] [nvarchar](max) NULL,
	[Tip] [nvarchar](100) NULL,
 CONSTRAINT [PK_Parametreler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'ParametrelerStandart')
BEGIN
CREATE TABLE [dbo].[ParametrelerStandart](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Modul] [nvarchar](100) NOT NULL,
	[Isim] [nvarchar](250) NOT NULL,
	[Deger] [nvarchar](max) NULL,
	[Tip] [nvarchar](50) NULL,
	[Kategori] [nvarchar](100) NULL,
 CONSTRAINT [PK_ParametrelerStandart] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'PersonelCalisma')
BEGIN
CREATE TABLE [dbo].[PersonelCalisma](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[SicilNo] [nvarchar](50) NULL,
	[KayitTarihi] [datetime] NULL,
	[KapanmaTarihi] [datetime] NULL,
 CONSTRAINT [PK_PersonelCalisma] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Personeller')
BEGIN
CREATE TABLE [dbo].[Personeller](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[Isim] [nvarchar](100) NULL,
	[EMail] [nvarchar](100) NULL,
	[Telefon] [nvarchar](100) NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[SilinenTarih] [datetime] NULL,
 CONSTRAINT [PK_Personeller] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Plasiyerler')
BEGIN
CREATE TABLE [dbo].[Plasiyerler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[Isim] [nvarchar](250) NULL,
	[Aciklama1] [nvarchar](250) NULL,
	[Aciklama2] [nvarchar](250) NULL,
 CONSTRAINT [PK_Plasiyerler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaCariStokSozlesmeleri')
BEGIN
CREATE TABLE [dbo].[SatinalmaCariStokSozlesmeleri](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Baslangic] [datetime] NULL,
	[Bitis] [datetime] NULL,
	[Miktar] [decimal](18, 4) NULL,
	[Aciklama] [nvarchar](max) NULL,
	[CDate] [datetime] NULL,
	[Kullanici] [nvarchar](100) NULL,
	[KayitID] [int] NULL,
	[CariID] [uniqueidentifier] NULL,
	[StokID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SatinalmaCariStokSozlesmeleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaCariTutarSozlesmeleri')
BEGIN
CREATE TABLE [dbo].[SatinalmaCariTutarSozlesmeleri](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Baslangic] [datetime] NULL,
	[Bitis] [datetime] NULL,
	[Tutar] [decimal](18, 4) NULL,
	[Aciklama] [nvarchar](max) NULL,
	[CDate] [datetime] NULL,
	[Kullanici] [nvarchar](100) NULL,
	[KayitID] [int] NULL,
	[CariID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SatinalmaCariTutarSozlesmeleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaDosyalar')
BEGIN
CREATE TABLE [dbo].[SatinalmaDosyalar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Modul] [nvarchar](100) NULL,
	[Dosya] [nvarchar](max) NULL,
	[Isim] [nvarchar](250) NULL,
	[DosyaUzantisi] [nvarchar](50) NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[KayitID] [int] NULL,
 CONSTRAINT [PK_SatinalmaDosyalar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaMenuler')
BEGIN
CREATE TABLE [dbo].[SatinalmaMenuler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Kod] [nvarchar](100) NULL,
	[Isim] [nvarchar](100) NULL,
 CONSTRAINT [PK_SatinalmaMenuler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaSifreler')
BEGIN
CREATE TABLE [dbo].[SatinalmaSifreler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Sifre] [nvarchar](max) NULL,
	[Aciklam] [nvarchar](max) NULL,
	[Aciklama] [nvarchar](max) NULL,
	[Kullanildi] [bit] NULL,
	[KullananPersonel] [nvarchar](max) NULL,
	[KullanilanTarih] [datetime] NULL,
	[KayitTarihi] [datetime] NULL,
 CONSTRAINT [PK_SatinalmaSifreler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaSubeler')
BEGIN
CREATE TABLE [dbo].[SatinalmaSubeler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Kod] [nvarchar](50) NULL,
	[Isim] [nvarchar](max) NULL,
	[KisaKodu] [nvarchar](50) NULL,
	[KisaKodu2] [nvarchar](50) NULL,
	[RecID] [int] NULL,
 CONSTRAINT [PK_SatinalmaSubeler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaTalepHareketleri')
BEGIN
CREATE TABLE [dbo].[SatinalmaTalepHareketleri](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TalepID] [int] NULL,
	[Modul] [nvarchar](50) NULL,
	[Durumu] [nvarchar](250) NULL,
	[CariID] [uniqueidentifier] NULL,
	[Fiyat] [decimal](18, 8) NULL,
	[DovizBirimi] [nvarchar](50) NULL,
	[Aciklama1] [nvarchar](max) NULL,
	[Aciklama2] [nvarchar](max) NULL,
	[Aciklama3] [nvarchar](max) NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
 CONSTRAINT [PK_SatinalmaTalepHareketleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaTalepleri')
BEGIN
CREATE TABLE [dbo].[SatinalmaTalepleri](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SubeKodu] [nvarchar](10) NULL,
	[StokID] [uniqueidentifier] NULL,
	[StokKodu] [nvarchar](100) NULL,
	[StokAdi] [nvarchar](250) NULL,
	[OlcuBirimi] [nvarchar](50) NULL,
	[Bakiye] [decimal](18, 2) NULL,
	[SonAlisFiyati] [decimal](18, 2) NULL,
	[TalepMiktari] [decimal](18, 2) NULL,
	[TalepMiktariYedek] [decimal](18, 2) NULL,
	[Aciklama1] [nvarchar](max) NULL,
	[Aciklama2] [nvarchar](max) NULL,
	[Aciklama3] [nvarchar](max) NULL,
	[Silindi] [bit] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[SilinenTarih] [datetime] NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
	[Durumu] [nvarchar](100) NULL,
	[SecilenTeklifID] [int] NULL,
	[SecilenCariID] [uniqueidentifier] NULL,
	[SecilenFiyat] [decimal](18, 8) NULL,
	[SecilenDovizBirimi] [nvarchar](50) NULL,
	[BitisNoktasi] [nvarchar](100) NULL,
	[Aktarildi] [bit] NULL,
	[AktarimNo] [nvarchar](50) NULL,
	[PdfDosyaAdi] [nvarchar](max) NULL,
	[SifreliGecis] [bit] NULL,
 CONSTRAINT [PK_SatinalmaTalepleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaTarihler')
BEGIN
CREATE TABLE [dbo].[SatinalmaTarihler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Tarih] [datetime] NULL,
	[KilitTarihi] [datetime] NULL,
 CONSTRAINT [PK_SatinalmaTarihler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaTeklifleri')
BEGIN
CREATE TABLE [dbo].[SatinalmaTeklifleri](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CariID] [uniqueidentifier] NULL,
	[StokID] [uniqueidentifier] NULL,
	[Baslangic] [datetime] NULL,
	[Bitis] [datetime] NULL,
	[Miktar] [decimal](18, 4) NULL,
	[Fiyat] [decimal](18, 4) NULL,
	[DovizBirimi] [nvarchar](100) NULL,
	[Aciklama1] [nvarchar](250) NULL,
	[Aciklama2] [nvarchar](250) NULL,
	[Aciklama3] [nvarchar](250) NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NULL,
	[SilinenTarih] [datetime] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[Sozlesmeli] [nvarchar](10) NULL,
 CONSTRAINT [PK_SatinalmaTeklifleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaYetkiler')
BEGIN
CREATE TABLE [dbo].[SatinalmaYetkiler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Kullanici] [nvarchar](50) NULL,
	[MenuID] [int] NULL,
	[Yetki] [bit] NULL,
 CONSTRAINT [PK_SatinalmaYetkiler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaYetkilerKategori')
BEGIN
CREATE TABLE [dbo].[SatinalmaYetkilerKategori](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Kullanici] [nvarchar](50) NULL,
	[MenuID] [nvarchar](max) NULL,
	[Yetki] [bit] NULL,
 CONSTRAINT [PK_SatinalmaYetkilerKategori] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'SatinalmaYetkilerSube')
BEGIN
CREATE TABLE [dbo].[SatinalmaYetkilerSube](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Kullanici] [nvarchar](50) NULL,
	[MenuID] [int] NULL,
	[Yetki] [bit] NULL,
 CONSTRAINT [PK_SatinalmaYetkilerSube] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'StokFiyatlari')
BEGIN
CREATE TABLE [dbo].[StokFiyatlari](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[StokID] [uniqueidentifier] NULL,
	[CariID] [uniqueidentifier] NULL,
	[FiyatGrubu] [nvarchar](50) NULL,
	[Tip] [nvarchar](50) NULL,
	[Fiyat] [decimal](18, 8) NULL,
	[BaslangicTarihi] [datetime] NULL,
	[BitisTarihi] [datetime] NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[SilinenTarih] [datetime] NULL,
 CONSTRAINT [PK_StokFiyatlari] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'StokHareketleri')
BEGIN
CREATE TABLE [dbo].[StokHareketleri](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[StokID] [uniqueidentifier] NOT NULL,
	[Tarih] [datetime] NOT NULL,
	[VadeTarihi] [datetime] NULL,
	[BelgeNo] [nvarchar](50) NULL,
	[HareketTipi] [nvarchar](50) NULL,
	[GC] [char](1) NOT NULL,
	[Miktar] [decimal](18, 5) NOT NULL,
	[KdvOrani] [decimal](18, 5) NULL,
	[IskontoOrani1] [decimal](18, 5) NULL,
	[Tutar] [decimal](18, 5) NULL,
	[DovizTipi] [nvarchar](3) NULL,
	[Kur] [decimal](18, 5) NULL,
	[DovizTutar] [decimal](18, 5) NULL,
	[Aciklama] [nvarchar](max) NULL,
	[PlasiyerID] [uniqueidentifier] NULL,
	[BaglantiID] [uniqueidentifier] NULL,
	[Baglanti] [nvarchar](100) NULL,
	[GrupKodu1ID] [uniqueidentifier] NULL,
	[GrupKodu2ID] [uniqueidentifier] NULL,
	[Silindi] [bit] NOT NULL,
	[SilinenTarih] [datetime] NULL,
	[SilenKullanici] [uniqueidentifier] NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_StokHareketleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Stoklar')
BEGIN
CREATE TABLE [dbo].[Stoklar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[Durumu] [bit] NOT NULL,
	[Kod] [nvarchar](50) NULL,
	[Isim] [nvarchar](100) NOT NULL,
	[Aciklama] [nvarchar](500) NULL,
	[Barkod] [nvarchar](50) NULL,
	[OlcuBirimi] [nvarchar](50) NULL,
	[GrupKodu1ID] [uniqueidentifier] NULL,
	[GrupKodu2ID] [uniqueidentifier] NULL,
	[GrupKodu3ID] [uniqueidentifier] NULL,
	[GrupKodu4ID] [uniqueidentifier] NULL,
	[GrupKodu5ID] [uniqueidentifier] NULL,
	[GrupKodu6ID] [uniqueidentifier] NULL,
	[KdvAlis] [decimal](18, 8) NOT NULL,
	[KdvSatis] [decimal](18, 8) NOT NULL,
	[Oiv] [decimal](18, 8) NOT NULL,
	[Otv] [decimal](18, 8) NOT NULL,
	[OtvFiyat] [decimal](18, 8) NOT NULL,
	[TevkifatPay] [int] NOT NULL,
	[TevkifatPayda] [int] NOT NULL,
	[IskontoSatis1] [decimal](18, 8) NOT NULL,
	[VadeGunu] [int] NOT NULL,
	[MinimumStok] [decimal](18, 8) NOT NULL,
	[MaxsimumStok] [decimal](18, 8) NOT NULL,
	[LimitUyarisi] [bit] NOT NULL,
	[LimitDisindaIslemiDurdur] [bit] NOT NULL,
	[EksiBakiyeUyarisi] [bit] NOT NULL,
	[EksiBakiyedeIslemiDurdur] [bit] NOT NULL,
	[StokKilitle] [bit] NOT NULL,
	[UreticiFirmaID] [uniqueidentifier] NULL,
	[MarkaID] [uniqueidentifier] NULL,
	[ModelID] [uniqueidentifier] NULL,
	[RenkID] [uniqueidentifier] NULL,
	[BedenID] [uniqueidentifier] NULL,
	[KaliteID] [uniqueidentifier] NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[Silindi] [bit] NOT NULL,
	[SilenKullanici] [uniqueidentifier] NULL,
	[SilinenTarih] [datetime] NULL,
	[AnaStokID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Stoklar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'StokNotlar')
BEGIN
CREATE TABLE [dbo].[StokNotlar](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[StokID] [uniqueidentifier] NOT NULL,
	[Aciklama] [text] NOT NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_StokNotlar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'StokSayimlari')
BEGIN
CREATE TABLE [dbo].[StokSayimlari](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[Firma] [nvarchar](100) NULL,
	[Sube] [nvarchar](100) NULL,
	[Depo] [nvarchar](100) NULL,
	[Barkod] [nvarchar](100) NULL,
	[Stok] [nvarchar](100) NULL,
	[Tarih] [datetime] NULL,
	[Miktar] [decimal](18, 8) NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeyapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NULL,
	[SilinenTarih] [datetime] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
 CONSTRAINT [PK_StokSayim] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'StokSerileri')
BEGIN
CREATE TABLE [dbo].[StokSerileri](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[StokID] [uniqueidentifier] NULL,
	[SeriNo] [nvarchar](100) NULL,
 CONSTRAINT [PK_StokSerileri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Subeler')
BEGIN
CREATE TABLE [dbo].[Subeler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[Firma] [nvarchar](100) NULL,
	[Kod] [nvarchar](50) NULL,
	[Isim] [nvarchar](100) NOT NULL,
	[Adres] [nvarchar](max) NULL,
	[Telefon] [nvarchar](100) NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Subeler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Uyelikler')
BEGIN
CREATE TABLE [dbo].[Uyelikler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Isim] [nvarchar](100) NOT NULL,
	[Unvan] [nvarchar](500) NULL,
	[VergiNumarasi] [nvarchar](11) NULL,
	[VergiDairesi] [nvarchar](250) NULL,
	[Adres] [nvarchar](250) NULL,
	[Iletisim] [nvarchar](100) NULL,
	[EMail] [nvarchar](100) NULL,
	[UyelikBaslangicTarihi] [datetime] NULL,
	[UyelikBitisTarihi] [datetime] NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [nvarchar](100) NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenlemeYapanKullanici] [nvarchar](100) NULL,
	[Silindi] [bit] NOT NULL,
	[SilinenTarih] [datetime] NULL,
	[SilenKullanici] [nvarchar](100) NULL,
	[ApiUrl] [nvarchar](max) NULL,
	[AcilisSayfasi] [nvarchar](max) NULL,
	[Resim] [nvarchar](max) NULL,
 CONSTRAINT [PK_Uyeliklerr] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'UyelikOdemeleri')
BEGIN
CREATE TABLE [dbo].[UyelikOdemeleri](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[KullaniciID] [uniqueidentifier] NULL,
	[Uygulama] [nvarchar](50) NULL,
	[Tutar] [decimal](18, 8) NULL,
	[Durum] [nvarchar](max) NULL,
	[OrderID] [nvarchar](100) NULL,
	[UzatilacakAy] [int] NULL,
	[KrediKartIsim] [nvarchar](250) NULL,
	[KrediKartNo] [nvarchar](100) NULL,
	[KrediKartSonKullanim] [nvarchar](50) NULL,
	[KrediKartCVV] [nvarchar](50) NULL,
	[SonucKodu] [nvarchar](max) NULL,
	[SonucAciklama] [nvarchar](max) NULL,
 CONSTRAINT [PK_UyelikOdemeleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'UyelikPaketleri')
BEGIN
CREATE TABLE [dbo].[UyelikPaketleri](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Isim] [nvarchar](100) NULL,
	[Ay] [int] NULL,
	[Tutar] [decimal](18, 2) NULL,
	[ResimUrl] [nvarchar](max) NULL,
	[Aciklama] [nvarchar](max) NULL,
 CONSTRAINT [PK_UyelikPaketleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Yetkiler')
BEGIN
CREATE TABLE [dbo].[Yetkiler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NULL,
	[KullaniciID] [uniqueidentifier] NOT NULL,
	[MenuID] [uniqueidentifier] NOT NULL,
	[Gor] [bit] NOT NULL,
	[Duzenle] [bit] NOT NULL,
	[Sil] [bit] NOT NULL,
	[KayitTarihi] [datetime] NOT NULL,
	[KayitYapanKullanici] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Yetkiler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS(select * from sys.tables Where name = 'Ziyaretler')
BEGIN
CREATE TABLE [dbo].[Ziyaretler](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UyelikID] [uniqueidentifier] NOT NULL,
	[CariID] [uniqueidentifier] NOT NULL,
	[Tarih] [datetime] NULL,
	[KullaniciID] [uniqueidentifier] NULL,
	[ZiyaretTipi] [uniqueidentifier] NULL,
	[Tamamlandi] [bit] NULL,
	[TeklifVerildi] [bit] NULL,
	[Aciklama] [nvarchar](500) NULL,
	[TamamlamaAciklamasi] [nvarchar](500) NULL,
	[TamamlamaTarihi] [datetime] NULL,
	[TamamlayanKullaniciID] [uniqueidentifier] NULL,
	[KayitTarihi] [datetime] NULL,
	[KayitYapanKullanici] [uniqueidentifier] NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[DuzenmaYapanKullanici] [uniqueidentifier] NULL,
	[Silindi] [bit] NULL,
	[SilinenTarih] [datetime] NULL,
	[SilenKullanici] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Ziyaretler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER TABLE [dbo].[AnaSayfaTakvim] ADD  CONSTRAINT [DF_AnaSayfaTakvim_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[B2BSepet] ADD  CONSTRAINT [DF_B2BSepet_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[B2BSepet] ADD  CONSTRAINT [DF_Sepet_IslemTipi]  DEFAULT ((0)) FOR [IslemTipi]
GO
ALTER TABLE [dbo].[B2BSepet] ADD  CONSTRAINT [DF_Sepet_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[B2BSepet] ADD  CONSTRAINT [DF_Sepet_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[BankaHesaplari] ADD  CONSTRAINT [DF_Bankalar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[BankaHesaplari] ADD  CONSTRAINT [DF_Bankalar_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[BelgeKalemler] ADD  CONSTRAINT [DF_BelgeKalemler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[BelgeKalemler] ADD  CONSTRAINT [DF_BelgeKalemler_Durumu]  DEFAULT ((1)) FOR [Durumu]
GO
ALTER TABLE [dbo].[BelgeKalemler] ADD  CONSTRAINT [DF_BelgeKalemler_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[BelgeKalemler] ADD  CONSTRAINT [DF_BelgeKalemler_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[Belgeler] ADD  CONSTRAINT [DF_Belgeler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Belgeler] ADD  CONSTRAINT [DF_Table_1_KayitYapanTarihDB]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[Belgeler] ADD  CONSTRAINT [DF_Belgeler_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[CariHareketleri] ADD  CONSTRAINT [DF_CariHareketleri_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[CariHareketleri] ADD  CONSTRAINT [DF_CariHareketleri_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[CariKisiler] ADD  CONSTRAINT [DF_CariKisiler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_Aktif]  DEFAULT ((1)) FOR [Aktif]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_Bolge]  DEFAULT (NULL) FOR [Bolge]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_Alici]  DEFAULT ((0)) FOR [Alici]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_Satici]  DEFAULT ((0)) FOR [Satici]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_Personel]  DEFAULT ((0)) FOR [Personel]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_Kilitli]  DEFAULT ((0)) FOR [Kilitli]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Table_1_KrediAsimindaDurdur]  DEFAULT ((0)) FOR [LimitAsimindaDurdur]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_KayitYapanTarihDB]  DEFAULT (getdate()) FOR [KayitYapanTarihDB]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_ServisPersoneli]  DEFAULT ((0)) FOR [ServisPersoneli]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_KullaniciAdi]  DEFAULT (NULL) FOR [KullaniciAdi]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_Parola]  DEFAULT (NULL) FOR [Parola]
GO
ALTER TABLE [dbo].[Cariler] ADD  CONSTRAINT [DF_Cariler_RiskAciklama]  DEFAULT (NULL) FOR [RiskAciklama]
GO
ALTER TABLE [dbo].[CariNotlar] ADD  CONSTRAINT [DF_CariNotlar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[CariNotlar] ADD  CONSTRAINT [DF_CariNotlar_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[ChangeLog] ADD  CONSTRAINT [DF_EventsLog_EventDate]  DEFAULT (getdate()) FOR [EventDate]
GO
ALTER TABLE [dbo].[Crm2Kayitlar] ADD  CONSTRAINT [DF_Crm2Kayitlar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Crm2Kayitlar] ADD  CONSTRAINT [DF_Crm2Kayitlar_Sozlesme]  DEFAULT ((0)) FOR [Sozlesme]
GO
ALTER TABLE [dbo].[Crm2Kayitlar] ADD  CONSTRAINT [DF_Crm2Kayitlar_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[Crm2Kayitlar] ADD  CONSTRAINT [DF_Crm2Kayitlar_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[Crm2Notlar] ADD  CONSTRAINT [DF_Crm2Notlar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Depolar] ADD  CONSTRAINT [DF_Depolar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Dosyalar] ADD  CONSTRAINT [DF_Dosyalar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Dovizler] ADD  CONSTRAINT [DF_Dovizler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[GorevHareketleri] ADD  CONSTRAINT [DF_GorevHareketleri_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[GorevKullanicilari] ADD  CONSTRAINT [DF_GorevKullanicilari_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Gorevler] ADD  CONSTRAINT [DF_Gorevler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Gorevler] ADD  CONSTRAINT [DF_Gorevler_Durumu]  DEFAULT (N'Beklemede') FOR [Durumu]
GO
ALTER TABLE [dbo].[Gorevler] ADD  CONSTRAINT [DF_Table_1_KayitYapanTarihDB_1]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[Gorevler] ADD  CONSTRAINT [DF_Gorevler_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[GrupKodlari] ADD  CONSTRAINT [DF_GrupKodlari_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[GrupKodlari] ADD  CONSTRAINT [DF_GrupKodlari_Aktif]  DEFAULT ((1)) FOR [Aktif]
GO
ALTER TABLE [dbo].[Hatirlaticilar] ADD  CONSTRAINT [DF_Hatirlaticilar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Hatirlaticilar] ADD  CONSTRAINT [DF_Hatirlaticilar_Gosterme]  DEFAULT ((0)) FOR [Gosterme]
GO
ALTER TABLE [dbo].[Kasalar] ADD  CONSTRAINT [DF_Kasalar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Kullanicilar] ADD  CONSTRAINT [DF_Kullanicilar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Kullanicilar] ADD  CONSTRAINT [DF_Uyelikler_Aktif]  DEFAULT ((1)) FOR [Aktif]
GO
ALTER TABLE [dbo].[Kullanicilar] ADD  CONSTRAINT [DF_Uyelikler_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[Kullanicilar] ADD  CONSTRAINT [DF_Uyelikler_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[Kullanicilar] ADD  CONSTRAINT [DF_Kullanicilar_Onaylandi]  DEFAULT ((0)) FOR [Onay]
GO
ALTER TABLE [dbo].[Loglar] ADD  CONSTRAINT [DF_Loglar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[LoglarGiris] ADD  CONSTRAINT [DF_LoglarGiris_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[LoglarGiris] ADD  CONSTRAINT [DF_LoglarGiris_Tarih]  DEFAULT (getdate()) FOR [Tarih]
GO
ALTER TABLE [dbo].[MailKaliplari] ADD  CONSTRAINT [DF_MailKaliplari_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Menuler] ADD  CONSTRAINT [DF_Menuler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Menuler] ADD  CONSTRAINT [DF_Menuler_Sira]  DEFAULT ((999)) FOR [Sira]
GO
ALTER TABLE [dbo].[Menuler] ADD  CONSTRAINT [DF_Menuler_Akif]  DEFAULT ((1)) FOR [Aktif]
GO
ALTER TABLE [dbo].[MenulerUretim] ADD  CONSTRAINT [DF_MenulerUretim_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[MenulerUretim] ADD  CONSTRAINT [DF_MenulerUretim_Sira]  DEFAULT ((999)) FOR [Sira]
GO
ALTER TABLE [dbo].[MenulerUretim] ADD  CONSTRAINT [DF_MenulerUretim_Akif]  DEFAULT ((1)) FOR [Aktif]
GO
ALTER TABLE [dbo].[Mesajlar] ADD  CONSTRAINT [DF_Mesajlar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Parametreler] ADD  CONSTRAINT [DF_Parametreler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[ParametrelerStandart] ADD  CONSTRAINT [DF_ParametrelerStandart_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[PersonelCalisma] ADD  CONSTRAINT [DF_PersonelCalisma_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Personeller] ADD  CONSTRAINT [DF_Personeller_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Personeller] ADD  CONSTRAINT [DF_Personeller_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[Personeller] ADD  CONSTRAINT [DF_Personeller_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[Plasiyerler] ADD  CONSTRAINT [DF_Plasiyerler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[SatinalmaCariStokSozlesmeleri] ADD  CONSTRAINT [DF_SatinalmaCariStokSozlesmeleri_CDate]  DEFAULT (getdate()) FOR [CDate]
GO
ALTER TABLE [dbo].[SatinalmaCariTutarSozlesmeleri] ADD  CONSTRAINT [DF_SatinalmaCariTutarSozlesmeleri_CDate]  DEFAULT (getdate()) FOR [CDate]
GO
ALTER TABLE [dbo].[SatinalmaDosyalar] ADD  CONSTRAINT [DF_SatinalmaDosyalar_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[SatinalmaSifreler] ADD  CONSTRAINT [DF_SatinalmaSifreler_Kullanildi]  DEFAULT ((0)) FOR [Kullanildi]
GO
ALTER TABLE [dbo].[SatinalmaSifreler] ADD  CONSTRAINT [DF_SatinalmaSifreler_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[SatinalmaTalepHareketleri] ADD  CONSTRAINT [DF_SatinalmaTalepHareketleri_Fiyat]  DEFAULT ((0)) FOR [Fiyat]
GO
ALTER TABLE [dbo].[SatinalmaTalepleri] ADD  CONSTRAINT [DF_SatinalmaTalepleri_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[SatinalmaTalepleri] ADD  CONSTRAINT [DF_SatinalmaTalepleri_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[SatinalmaTalepleri] ADD  CONSTRAINT [DF_SatinalmaTalepleri_Aktarildi]  DEFAULT ((0)) FOR [Aktarildi]
GO
ALTER TABLE [dbo].[SatinalmaTalepleri] ADD  CONSTRAINT [DF_SatinalmaTalepleri_SifreliGecis]  DEFAULT ((0)) FOR [SifreliGecis]
GO
ALTER TABLE [dbo].[SatinalmaTeklifleri] ADD  CONSTRAINT [DF_SatinalmaTeklifleri_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[SatinalmaYetkiler] ADD  CONSTRAINT [DF_SatinalmaYetkiler_Yetki]  DEFAULT ((0)) FOR [Yetki]
GO
ALTER TABLE [dbo].[SatinalmaYetkilerKategori] ADD  CONSTRAINT [DF_SatinalmaYetkilerKategori_Yetki]  DEFAULT ((0)) FOR [Yetki]
GO
ALTER TABLE [dbo].[SatinalmaYetkilerSube] ADD  CONSTRAINT [DF_SatinalmaYetkilerSube_Yetki]  DEFAULT ((0)) FOR [Yetki]
GO
ALTER TABLE [dbo].[StokFiyatlari] ADD  CONSTRAINT [DF_StokFiyatlari_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[StokFiyatlari] ADD  CONSTRAINT [DF_StokFiyatlari_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[StokFiyatlari] ADD  CONSTRAINT [DF_StokFiyatlari_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[StokHareketleri] ADD  CONSTRAINT [DF_StokHareketleri_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[StokHareketleri] ADD  CONSTRAINT [DF_StokHareketleri_KdvOrani]  DEFAULT ((0)) FOR [KdvOrani]
GO
ALTER TABLE [dbo].[StokHareketleri] ADD  CONSTRAINT [DF_StokHareketleri_IndirimOrani]  DEFAULT ((0)) FOR [IskontoOrani1]
GO
ALTER TABLE [dbo].[StokHareketleri] ADD  CONSTRAINT [DF_StokHareketleri_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[Stoklar] ADD  CONSTRAINT [DF_Stoklar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Stoklar] ADD  CONSTRAINT [DF_Stoklar_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[StokNotlar] ADD  CONSTRAINT [DF_StokNotlar_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[StokNotlar] ADD  CONSTRAINT [DF_StokNotlar_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[StokSayimlari] ADD  CONSTRAINT [DF_StokSayim_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[StokSayimlari] ADD  CONSTRAINT [DF_StokSayim_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[StokSayimlari] ADD  CONSTRAINT [DF_StokSayim_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[StokSerileri] ADD  CONSTRAINT [DF_StokSerileri_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Subeler] ADD  CONSTRAINT [DF_Subeler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Uyelikler] ADD  CONSTRAINT [DF_Uyelikler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Uyelikler] ADD  CONSTRAINT [DF_Uyeliklerr_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[Uyelikler] ADD  CONSTRAINT [DF_Uyeliklerr_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[UyelikOdemeleri] ADD  CONSTRAINT [DF_UyelikOdemeleri_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[UyelikPaketleri] ADD  CONSTRAINT [DF_UyelikPaketleri_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Yetkiler] ADD  CONSTRAINT [DF_Yetkiler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Ziyaretler] ADD  CONSTRAINT [DF_Ziyaretler_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Ziyaretler] ADD  CONSTRAINT [DF_Ziyaretler_Tamamlandi]  DEFAULT ((0)) FOR [Tamamlandi]
GO
ALTER TABLE [dbo].[Ziyaretler] ADD  CONSTRAINT [DF_Ziyaretler_TeklifVerildi]  DEFAULT ((0)) FOR [TeklifVerildi]
GO
ALTER TABLE [dbo].[Ziyaretler] ADD  CONSTRAINT [DF_Ziyaretler_Silindi]  DEFAULT ((0)) FOR [Silindi]
GO
ALTER TABLE [dbo].[Cariler]  WITH CHECK ADD  CONSTRAINT [FK_Cariler_Plasiyerler] FOREIGN KEY([PlasiyerID])
REFERENCES [dbo].[Plasiyerler] ([ID])
GO
ALTER TABLE [dbo].[Cariler] CHECK CONSTRAINT [FK_Cariler_Plasiyerler]
GO
ALTER TABLE [dbo].[Cariler]  WITH CHECK ADD  CONSTRAINT [FK_Cariler_Uyelikler] FOREIGN KEY([UyelikID])
REFERENCES [dbo].[Uyelikler] ([ID])
GO
ALTER TABLE [dbo].[Cariler] CHECK CONSTRAINT [FK_Cariler_Uyelikler]
GO
ALTER TABLE [dbo].[Kullanicilar]  WITH CHECK ADD  CONSTRAINT [FK_Kullanicilar_Kullanicilar] FOREIGN KEY([UyelikID])
REFERENCES [dbo].[Uyelikler] ([ID])
GO
ALTER TABLE [dbo].[Kullanicilar] CHECK CONSTRAINT [FK_Kullanicilar_Kullanicilar]
GO
ALTER TABLE [dbo].[Parametreler]  WITH CHECK ADD  CONSTRAINT [FK_Parametreler_Uyelikler] FOREIGN KEY([UyelikID])
REFERENCES [dbo].[Uyelikler] ([ID])
GO
ALTER TABLE [dbo].[Parametreler] CHECK CONSTRAINT [FK_Parametreler_Uyelikler]
GO
ALTER TABLE [dbo].[Plasiyerler]  WITH CHECK ADD  CONSTRAINT [FK_Plasiyerler_Uyelikler] FOREIGN KEY([UyelikID])
REFERENCES [dbo].[Uyelikler] ([ID])
GO
ALTER TABLE [dbo].[Plasiyerler] CHECK CONSTRAINT [FK_Plasiyerler_Uyelikler]
GO
ALTER TABLE [dbo].[SatinalmaTalepleri]  WITH CHECK ADD  CONSTRAINT [FK_SatinalmaTalepleri_SatinalmaTeklifleri] FOREIGN KEY([SecilenTeklifID])
REFERENCES [dbo].[SatinalmaTeklifleri] ([ID])
GO
ALTER TABLE [dbo].[SatinalmaTalepleri] CHECK CONSTRAINT [FK_SatinalmaTalepleri_SatinalmaTeklifleri]
GO
ALTER TABLE [dbo].[SatinalmaYetkiler]  WITH CHECK ADD  CONSTRAINT [FK_SatinalmaYetkiler_Yetkiler] FOREIGN KEY([MenuID])
REFERENCES [dbo].[SatinalmaMenuler] ([ID])
GO
ALTER TABLE [dbo].[SatinalmaYetkiler] CHECK CONSTRAINT [FK_SatinalmaYetkiler_Yetkiler]
GO
ALTER TABLE [dbo].[SatinalmaYetkilerSube]  WITH CHECK ADD  CONSTRAINT [FK_SatinalmaYetkilerSube_YetkilerSube] FOREIGN KEY([MenuID])
REFERENCES [dbo].[SatinalmaSubeler] ([ID])
GO
ALTER TABLE [dbo].[SatinalmaYetkilerSube] CHECK CONSTRAINT [FK_SatinalmaYetkilerSube_YetkilerSube]
GO
