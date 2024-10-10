

drop proc [p_KullaniciListesiMesaj]
GO
CREATE proc [dbo].[p_KullaniciListesiMesaj](
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100),
@AranacakKelime nvarchar(max)=''
)
as
BEGIN

if(@AranacakKelime = 'DestekKullanicilari')
BEGIN
set @AranacakKelime = '%'+@AranacakKelime+'%'
select 
* 
from Kullanicilar WITH(NOLOCK)
Where (Silindi = 0 and UyelikID = @UyelikID) and
(KullaniciAdi like '%ykyazilim.com.tr')
END
ELSE
BEGIN

set @AranacakKelime = '%'+@AranacakKelime+'%'
select * from (
select 
*,
(Select COUNT(*) from Mesajlar WITH(NOLOCK) Where Mesajlar.KullaniciID = Kullanicilar.ID and Mesajlar.KarsiKullaniciID = @KullaniciID and Mesajlar.GorulmeTarihi IS NULL) as YeniMesaj,
(Select top(1) KayitTarihi from Mesajlar WITH(NOLOCK) Where Mesajlar.KullaniciID = Kullanicilar.ID and Mesajlar.KarsiKullaniciID = @KullaniciID Order by KayitTarihi desc) as SonMesajTarihi
from Kullanicilar WITH(NOLOCK)
Where (Silindi = 0 and UyelikID = @UyelikID) 
and Kullanicilar.ID <> @KullaniciID
and
(KullaniciAdi like @AranacakKelime or Ad like @AranacakKelime or Soyad like @AranacakKelime)
) YK1
Order by YeniMesaj desc,SonMesajTarihi desc
END

END
go
drop proc [p_KullaniciUretimYetkiKaydet]
go
create PROC [dbo].[p_KullaniciUretimYetkiKaydet](
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100),
@MenuID nvarchar(100),
@Gor bit,
@Duzenle bit,
@Sil bit
)
as
BEGIN
	IF EXISTS (Select * from Yetkiler WITH(NOLOCK) Where UyelikID = @UyelikID and KullaniciID = @KullaniciID and MenuID = @MenuID)
	BEGIN
		Update Yetkiler Set 
			Gor = @Gor,
			Duzenle = @Duzenle,
			Sil = @Sil
		Where UyelikID = @UyelikID and KullaniciID = @KullaniciID and MenuID = @MenuID
	END
	ELSE
	BEGIN
		Insert Into Yetkiler 
		(MenuID,KullaniciID,UyelikID,Gor,Duzenle,Sil,KayitTarihi,KayitYapanKullanici)
		values
		(@MenuID,@KullaniciID,@UyelikID,@Gor,@Duzenle,@Sil,GETDATE(),@KullaniciID)
	END
END

go
drop proc [p_KullaniciUretimYetkileri]
GO
create PROC [dbo].[p_KullaniciUretimYetkileri](
@KullaniciID nvarchar(100) = '45B83A26-D48D-4B59-BD12-92CD2788C093',
@UyelikID nvarchar(100)='68854AF6-504F-48C7-B64E-ECCBF881DB80'
)
as
BEGIN

Select 
Menuler.ID as MenuID,
Kullanicilar.ID as KullaniciID,
Kullanicilar.UyelikID,
Menuler.Menu,
Menuler.UstID,
Menuler.icon,
Menuler.url,
ISNULL(Yetkiler.Gor,0) as Gor,
ISNULL(Yetkiler.Duzenle,0) as Duzenle,
ISNULL(Yetkiler.Sil,0) as Sil
From MenulerUretim as Menuler WITH(NOLOCK)
LEFT OUTER JOIN Kullanicilar WITH(NOLOCK) ON Kullanicilar.ID = @KullaniciID --and UyelikID = @UyelikID
LEFT OUTER JOIN Yetkiler WITH(NOLOCK) ON Yetkiler.MenuID = Menuler.ID and Yetkiler.KullaniciID = Kullanicilar.ID
Order by Menuler.Sira
END

GO
drop proc [p_UretimIsEmriListesi]
GO
CREATE proc [dbo].[p_UretimIsEmriListesi](
@UyelikID nvarchar(100)='',
@aranacakKelime nvarchar(100)=''
)
as
BEGIN

declare @Uygulama nvarchar(100) = (select Top(1) Deger from Parametreler WITH(NOLOCK) Where UyelikID = @UyelikID and Modul = 'Uygulama')
IF @Uygulama = 'NETSIS'
BEGIN
	declare @NetsisDatabase nvarchar(100) = (select Top(1) Deger from Parametreler WITH(NOLOCK) Where UyelikID = @UyelikID and Modul = 'NetsisDatabase')
	
	declare @Sorgu nvarchar(max)='
	select 
	 ISEMRINO as IsEmriNo,
	 TARIH as Tarih,
	 TBLISEMRI.STOK_KODU as StokKodu,
	 TBLSTSABIT.STOK_ADI as StokAdi,
	 TBLISEMRI.MIKTAR as Miktar,
	 ACIKLAMA as Aciklama
	From '+@NetsisDatabase+'.dbo.TBLISEMRI WITH(NOLOCK)
	LEFT OUTER JOIN '+@NetsisDatabase+'.dbo.TBLSTSABIT WITH(NOLOCK) ON TBLSTSABIT.STOK_KODU = TBLISEMRI.STOK_KODU
	WHERE TBLISEMRI.KAPALI = ''H'' and ISEMRINO LIKE ''%'+@aranacakKelime+'%''
	Order by Tarih asc
	'
	EXECUTE(@Sorgu)
END


END

GO
drop proc [p_AnaSayfa]
GO
CREATE proc [dbo].[p_AnaSayfa](
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN
	
	Select 
	Cari = (Select COUNT(*) From Cariler WITH(NOLOCK) Where UyelikID = @UyelikID and Silindi = 0 and Aktif = 1 ),
	Stok = (Select COUNT(*) From Stoklar WITH(NOLOCK) Where UyelikID = @UyelikID and Silindi = 0  ),
	Belge = (Select COUNT(*) From Belgeler WITH(NOLOCK) Where UyelikID = @UyelikID and Silindi = 0 and Tip = 'SS'),
	Gorev = (
			 Select 
			 	COUNT(DISTINCT Gorevler.ID) 
			 From GorevKullanicilari WITH(NOLOCK) 
			 LEFT OUTER JOIN Gorevler WITH(NOLOCK) ON Gorevler.ID = GorevKullanicilari.GorevID
			 Where GorevKullanicilari.UyelikID = @UyelikID  and KullaniciID = @KullaniciID and Gorevler.Durumu <> 'Tamamlandi'
			),
	YeniOkunmamisMesaj = ((Select COUNT(*) from Mesajlar WITH(NOLOCK) Where Mesajlar.KarsiKullaniciID = @KullaniciID and Mesajlar.GorulmeTarihi IS NULL))

END
GO
drop proc [p_AnaSayfaTakvim]
GO
create proc [dbo].[p_AnaSayfaTakvim](
@ID nvarchar(100)
)
as
BEGIN


select 
* 
from AnaSayfaTakvim WITH(NOLOCK)
Where ID = @ID


END
GO
drop proc [p_AnaSayfaTakvimKaydet]
GO
create proc [dbo].[p_AnaSayfaTakvimKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@KullaniciID nvarchar(100),
@Tarih datetime,	
@Durumu nvarchar(max)=null,
@Baslik nvarchar(max)=null,
@Aciklama nvarchar(max)=null

)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Tarih,'Kayıt Eklendi',@Baslik,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into AnaSayfaTakvim
(ID,UyelikID,Tarih,Durumu,Baslik,Aciklama,KayitTarihi,KayitYapanKullanici)
Select @ID,@UyelikID,@Tarih,@Durumu,@Baslik,@Aciklama,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Tarih,'Kayıt Güncellendi',@Baslik,GETDATE(),@KullaniciID)

Update AnaSayfaTakvim set 
Tarih=@Tarih,
Durumu = @Durumu,
Baslik = @Baslik,
Aciklama = @Aciklama,
DuzenlemeTarihi=GETDATE(),
DuzenlemeYapanKullanici=@KullaniciID
Where ID = @ID and UyelikID = @UyelikID
END

END

GO
drop proc [p_AnaSayfaTakvimListesi]
GO
CREATE proc [dbo].[p_AnaSayfaTakvimListesi](
@UyelikID nvarchar(100)='GENEL',
@KullaniciID nvarchar(100),
@BaslangicTarihi datetime,
@BitisTarihi datetime
)
as
BEGIN


	select 
	* 
	from AnaSayfaTakvim WITH(NOLOCK)
	Where UyelikID =@UyelikID and KayitYapanKullanici = @KullaniciID
	and Tarih between @BaslangicTarihi and @BitisTarihi
	Order by Tarih,Baslik


END

GO
drop proc [p_AnaSayfaTakvimSil]
GO
create proc [dbo].[p_AnaSayfaTakvimSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,(select top(1) Baslik From AnaSayfaTakvim WITH(NOLOCK) Where ID = @ID),'Kayıt Silindi',(select top(1) Isim From Subeler WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From AnaSayfaTakvim Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_B2B_Cariler]
GO
CREATE proc [dbo].[p_B2B_Cariler](
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100),
@AranacakKelime nvarchar(100)
)
as
BEGIN

	Select top(100)
	Cariler.*,
	Plasiyerler.Isim as Plasiyer	
	From Cariler WITH(NOLOCK)
	LEFT OUTER JOIN Plasiyerler WITH(NOLOCK) ON Plasiyerler.ID = Cariler.PlasiyerID
	Where Cariler.Silindi = 0 
	and Cariler.UyelikID = @UyelikID
	and 
	(
	    Cariler.Kod like '%'+@AranacakKelime+'%'
	or Cariler.Isim like '%'+@AranacakKelime+'%'
	or Cariler.Unvan like '%'+@AranacakKelime+'%'
	)

END
GO
drop proc [p_B2B_Kategoriler]
GO
CREATE proc [dbo].[p_B2B_Kategoriler](
@UyelikID nvarchar(100),
@CariID nvarchar(100)
)
as
BEGIN

	Select 
	G1.Deger as Kategori1
	From Stoklar WITH(NOLOCK)
	LEFT OUTER JOIN GrupKodlari G1 WITH(NOLOCK) ON G1.ID = Stoklar.GrupKodu1ID
	Where Silindi = 0
	and G1.Deger IS NOT NULL
	and G1.Deger <> ''

END


GO
drop proc [p_B2B_Parametreler]
GO

CREATE proc [dbo].[p_B2B_Parametreler](
@UyelikID nvarchar(100)
)
as

select 
Parametreler.ID,
@UyelikID as UyelikID,
ParametrelerStandart.Modul,
ParametrelerStandart.Isim,
ISNULL(Parametreler.Deger,ParametrelerStandart.Deger) as Deger,
ParametrelerStandart.Tip
from ParametrelerStandart WITH(NOLOCK)
LEFT OUTER JOIN Parametreler WITH(NOLOCK) ON Parametreler.UyelikID = @UyelikID and Parametreler.Modul = ParametrelerStandart.Modul
and Parametreler.Isim = ParametrelerStandart.Isim
Where 1=1 
and ParametrelerStandart.Modul like 'B2B%'


GO
drop proc [p_B2B_SepetEkle]
GO
CREATE proc [dbo].[p_B2B_SepetEkle](
@ID nvarchar(100),
@UyelikID nvarchar(100),
@Modul nvarchar(50),
@CariID nvarchar(100),
@StokID nvarchar(100),
@Seri nvarchar(100),
@Birim nvarchar(50),
@Miktar decimal(18,8),
@Fiyat decimal(18,8),
@Tutar decimal(18,8),
@IslemTipi int,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN
	Insert Into B2BSepet
	(UyelikID,Modul,CariID,StokID,Seri,Birim,Miktar,Fiyat,Tutar,IslemTipi,KayitTarihi,KayitYapanKullanici,Silindi)
	Select 
	@UyelikID,@Modul,@CariID,@StokID,@Seri,@Birim,@Miktar,@Fiyat,@Tutar,0,GETDATE(),@KullaniciID,0
END
ELSE
BEGIN
	Update B2BSepet Set 
		Miktar = @Miktar,
		Fiyat = @Fiyat,
		Tutar = @tutar
	Where UyelikID = @UyelikID and ID = @ID
END

END
GO
drop proc [p_B2B_SepetListele]
GO
CREATE proc [dbo].[p_B2B_SepetListele](
@UyelikID nvarchar(100),
@CariID nvarchar(100)
)
as
BEGIN
	Select top(300)
	B2BSepet.*,
	S.Kod as StokKodu,
	S.Isim as StokAdi,
	S.OlcuBirimi
	From B2BSepet WITH(NOLOCK)
	LEFT OUTER JOIN Stoklar S WITH(NOLOCK) ON S.ID = B2BSepet.StokID
	Where B2BSepet.UyelikID = @UyelikID
	and B2BSepet.CariID = @CariID
	and B2BSepet.Silindi = 0
END

GO
drop proc [p_B2B_SepetSil]
GO

CREATE proc [dbo].[p_B2B_SepetSil](
@UyelikID nvarchar(100),
@ID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

	Update B2BSepet Set 
		Silindi = 1,
		SilinenTarih=GETDATE(),
		SilenKullanici = @KullaniciID
	Where Silindi = 0 and UyelikID = @UyelikID and ID = @ID

END

GO
drop proc [p_B2B_SiparisDetay]
GO
CREATE proc [dbo].[p_B2B_SiparisDetay](
@UyelikID nvarchar(100),
@CariID nvarchar(100),
@BelgeNo nvarchar(100)
)
as
BEGIN

	Select top(100)
	Belgeler.Tarih,
	Belgeler.BelgeNo,
	Cariler.Kod as CariKodu,
	Cariler.Isim as CariAdi,
	BelgeKalemler.*,
	BelgeKalemler.Miktar * BelgeKalemler.Fiyat as Tutar,
	Stoklar.Kod as StokKodu,
	Stoklar.Isim as StokAdi,
	Stoklar.OlcuBirimi as OlcuBirimi
	From Belgeler WITH(NOLOCK)
	inner join BelgeKalemler WITH(NOLOCK) ON BelgeKalemler.Silindi = 0 and BelgeKalemler.BelgeID = Belgeler.ID 
	left outer join Cariler WITH(NOLOCK) ON Cariler.Silindi = 0 and Cariler.ID = Belgeler.CariID
	left outer join Stoklar WITH(NOLOCK) ON Stoklar.Silindi = 0 and Stoklar.ID = BelgeKalemler.StokID
	Where Belgeler.Silindi = 0 
	and Belgeler.UyelikID = @UyelikID
	and Belgeler.CariID = @CariID
	and Belgeler.BelgeNo = @BelgeNo
END
GO
drop proc [p_B2B_Siparislerim]
GO
CREATE proc [dbo].[p_B2B_Siparislerim](
@UyelikID nvarchar(100),
@CariID nvarchar(100)
)
as
BEGIN

	Select top(100)
	Belgeler.*,
	(select SUM(Miktar*Fiyat) From BelgeKalemler WITH(NOLOCK) Where BelgeKalemler.Silindi = 0 and BelgeKalemler.BelgeID = Belgeler.ID) as Tutar
	From Belgeler WITH(NOLOCK)
	Where Belgeler.Silindi = 0 
	and Belgeler.UyelikID = @UyelikID
	and Belgeler.CariID = @CariID
	--and Tip = 'SS'
END
GO
drop proc [p_B2B_Stoklar]
GO
CREATE proc [dbo].[p_B2B_Stoklar](
@UyelikID nvarchar(100),
@CariID nvarchar(100),
@AranacakKelime nvarchar(100)
)
as
BEGIN

	Select 
	Stoklar.*,
	100 as Fiyat,
	0 Bakiye
	From Stoklar WITH(NOLOCK)
	Where Silindi = 0 and UyelikID = @UyelikID 
	and 
	(
		Kod like '%'+@AranacakKelime+'%'
	or Isim like '%'+@AranacakKelime+'%'
	or Aciklama like '%'+@AranacakKelime+'%'
	or Barkod like '%'+@AranacakKelime+'%'
	)

END
GO
drop proc [p_BankaHesaplari]
GO

create proc [dbo].[p_BankaHesaplari](
@ID nvarchar(100)
)
as
BEGIN


select 
* 
from BankaHesaplari WITH(NOLOCK)
Where ID = @ID


END
GO
drop proc [p_BankaHesaplariKaydet]
GO
create proc [dbo].[p_BankaHesaplariKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@BankaID nvarchar(100)=null,
@Kod nvarchar(100)=null,
@Isim nvarchar(100)=null,
@HesapNo nvarchar(100)=null,
@Iban nvarchar(100)=null,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Isim,'Kayıt Eklendi',@Isim,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into BankaHesaplari
(ID,UyelikID,BankaID,Kod,Isim,HesapNo,Iban,KayitTarihi,KayitYapanKullanici)
Select @ID,@UyelikID,@BankaID,@Kod,@Isim,@HesapNo,@Iban,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Isim,'Kayıt Güncellendi',@Isim,GETDATE(),@KullaniciID)

Update BankaHesaplari set 
BankaID=@BankaID,
Kod=@Kod,
Isim=@Isim,
HesapNo=@HesapNo,
Iban=@Iban,
DuzenlemeTarihi=GETDATE(),
DuzenlemeYapanKullanici=@KullaniciID
Where ID = @ID and UyelikID = @UyelikID
END

END
GO
drop proc [p_BankaHesaplariListesi]
GO
create proc [dbo].[p_BankaHesaplariListesi](
@UyelikID nvarchar(100)='GENEL',
@AranacakKelime nvarchar(max)=''
)
as
BEGIN


	select 
	* 
	from BankaHesaplari WITH(NOLOCK)
	Where UyelikID =@UyelikID and (Iban like '%'+@AranacakKelime+'%' or HesapNo like '%'+@AranacakKelime+'%' or Kod like '%'+@AranacakKelime+'%' or Isim like '%'+@AranacakKelime+'%')
	Order by Isim


END
GO
drop proc [p_BankaHesaplariSil]
GO
create proc [dbo].[p_BankaHesaplariSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,(select top(1) Isim From BankaHesaplari WITH(NOLOCK) Where ID = @ID),'Kayıt Silindi',(select top(1) Isim From Depolar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From BankaHesaplari Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_Belge]
GO
CREATE proc [dbo].[p_Belge](
@ID nvarchar(100)='',
@UyelikID nvarchar(100)
)
as
BEGIN

--if @ID <> ''
--BEGIN
--	Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Belge','Belge Detaya Girildi',(select top(1) Isim from Cariler WITH(NOLOCK) Where ID = @ID),GETDATE(),NULL)
--END

select 
Belgeler.*,
Cariler.Isim as CariAdi
from Belgeler WITH(NOLOCK)
LEFT OUTER JOIN Cariler WITH(NOLOCK) ON Cariler.ID = Belgeler.CariID
Where ISNULL(Belgeler.Silindi,0) = 0 and  Belgeler.UyelikID = @UyelikID 
and Belgeler.ID = CASE WHEN @ID = '' THEN NULL ELSE @ID END

Select 
BelgeKalemler.*,
BelgeKalemler.Miktar * BelgeKalemler.Fiyat as Tutar,
Stoklar.Kod as StokKodu,
Stoklar.Isim as StokAdi,
Stoklar.OlcuBirimi as OlcuBirimi
From BelgeKalemler WITH(NOLOCK)
LEFT OUTER JOIN Stoklar WITH(NOLOCK) ON Stoklar.ID = BelgeKalemler.StokID
Where BelgeKalemler.BelgeID = CASE WHEN @ID = '' THEN NULL ELSE @ID END
Order by BelgeKalemler.KayitTarihi

END

GO
drop proc [p_BelgeKalemKaydet]
GO
CREATE proc [dbo].[p_BelgeKalemKaydet](
@ID nvarchar(100),
@BelgeID nvarchar(100),
@Durumu bit=1,
@StokID nvarchar(100),
@Seri nvarchar(50),
@Miktar decimal(18,8),
@KdvOrani decimal(18,8),
@Fiyat decimal(18,8),
@IskontoOrani1 decimal(18,8),
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@BelgeID,'Belge','Belgeye Ürün Eklendi',@StokID,GETDATE(),@KullaniciID)

set @ID = NEWID()

INSERT INTO [dbo].BelgeKalemler
           ([ID]
           ,BelgeID
		   ,Durumu
		   ,StokID
		   ,Seri
		   ,Miktar
		   ,Fiyat
		   ,KdvOrani
		   ,IskontoOrani1
		   ,KayitTarihi
		   ,KayitYapanKullanici
		   ,Silindi
		   )
     VALUES
           (
		    @ID
           ,@BelgeID
		   ,@Durumu
           ,@StokID
		   ,@Seri
		   ,@Miktar
		   ,@Fiyat
		   ,@KdvOrani
		   ,@IskontoOrani1
		   ,GETDATE()
		   ,@KullaniciID
		   ,0
		   )
select @ID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@BelgeID,'Belge','Belgede Ürün Güncellendi',@StokID,GETDATE(),@KullaniciID)

UPDATE [dbo].BelgeKalemler
   SET
	  StokID=@StokID
	 ,Durumu=@Durumu
	 ,Seri=@Seri
	 ,Miktar=@Miktar
	 ,Fiyat=@Fiyat
	 ,KdvOrani=@KdvOrani
	 ,IskontoOrani1=@IskontoOrani1
 WHERE 1=1
 and BelgeID = @BelgeID
 and ID = @ID
 select @ID


END

END
GO
drop proc [p_BelgeKalemSilinenKontrol]
GO
CREATE proc [dbo].[p_BelgeKalemSilinenKontrol](
@BelgeID nvarchar(max)='',
@ID nvarchar(max)=''
)
as
BEGIN

	Delete top(1) from BelgeKalemler Where BelgeID = @BelgeID 
	and CAST(ID as nvarchar(100)) NOT IN (
		Select RTRIM(LTRIM(Kelime)) from dbo.KelimeParcala(@ID,',') Where Kelime <> ''
	)

END
GO
drop proc [p_BelgeKaydet]
GO

CREATE proc [dbo].[p_BelgeKaydet](
@ID nvarchar(100),
@UyelikID nvarchar(100),
@Tip nvarchar(50),
@Durumu nvarchar(50)='',
@Tarih datetime,
@BelgeNo nvarchar(100),
@CariID nvarchar(100)='',
@ProjeID nvarchar(100)='',
@DepoCikisID nvarchar(100),
@DepoGirisID nvarchar(100),
@Aciklama1 nvarchar(500),
@SatisPersonelID nvarchar(100)='',
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Belge','Yeni Belge Açıldı',@BelgeNo,GETDATE(),@KullaniciID)

IF @BelgeNo = ''
BEGIN
	IF NOT EXISTS(Select * from Parametreler WITH(NOLOCK) Where Modul = @Tip)
	BEGIN
		Insert Into Parametreler 
		(UyelikID,Modul,Isim,Deger)
		values
		(@UyelikID,@Tip,'Sayac_BelgeNo',0)
	END
	Update Parametreler set Deger = Deger+1 Where UyelikID = @UyelikID and Modul = @Tip and Isim = 'Sayac_BelgeNo'
	set @BelgeNo = @Tip +CAST(YEAR(GETDATE()) AS nvarchar(max))+'-'+(select top(1) RIGHT('000000'+Deger,5) from Parametreler WITH(NOLOCK) Where UyelikID = @UyelikID and Modul = @Tip and Isim = 'Sayac_BelgeNo')
END

set @ID = NEWID()

INSERT INTO [dbo].Belgeler
           ([ID]
           ,[UyelikID]
           ,Tip
		   ,Durumu
		   ,Tarih
		   ,BelgeNo
		   ,ProjeID
		   ,CariID
		   ,DepoCikisID
		   ,DepoGirisID
		   ,SatisPersonelID
		   ,Aciklama1
		   ,KayitTarihi
		   ,KayitYapanKullanici
		   ,Silindi
		   )
     VALUES
           (
		    @ID
           ,@UyelikID
           ,@Tip
		   ,@Durumu
		   ,@Tarih
		   ,@BelgeNo
		   ,CASE WHEN @ProjeID ='' THEN NULL ELSE @ProjeID END
		   ,CASE WHEN @CariID ='' THEN NULL ELSE @CariID END
		   ,CASE WHEN @DepoCikisID ='' THEN NULL ELSE @DepoCikisID END
		   ,CASE WHEN @DepoGirisID ='' THEN NULL ELSE @DepoGirisID END
		   ,CASE WHEN @SatisPersonelID ='' THEN NULL ELSE @SatisPersonelID END
		   ,@Aciklama1
		   ,GETDATE()
		   ,@KullaniciID
		   ,0
		   )
		   Select @ID
END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Belge','Belge Güncellendi',@BelgeNo,GETDATE(),@KullaniciID)

UPDATE [dbo].[Belgeler]
   SET
       Tip=@Tip
	  ,Durumu=@Durumu
	  ,Tarih=@Tarih
	  ,BelgeNo=@BelgeNo
	  ,ProjeID=CASE WHEN @ProjeID ='' THEN NULL ELSE @ProjeID END
	  ,CariID=CASE WHEN @CariID ='' THEN NULL ELSE @CariID END
	  ,DepoCikisID=CASE WHEN @DepoCikisID ='' THEN NULL ELSE @DepoCikisID END
	  ,DepoGirisID=CASE WHEN @DepoGirisID ='' THEN NULL ELSE @DepoGirisID END
	  ,SatisPersonelID = CASE WHEN @SatisPersonelID ='' THEN NULL ELSE @SatisPersonelID END
	  ,Aciklama1=@Aciklama1
 WHERE 1=1
 and UyelikID = @UyelikID 
 and ID = @ID
 select @ID


END

END
GO
drop proc [p_BelgeListesi]
GO
CREATE proc [dbo].[p_BelgeListesi](
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100),
@Tip nvarchar(max)='',
@Durumu nvarchar(max)='',
@AranacakKelime nvarchar(max)='',
@BelgeNo nvarchar(max)='',
@BaslangicTarihi nvarchar(max)=null,
@BitisTarihi nvarchar(max)=null,
@CariAdi nvarchar(max)=''
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Belge','Belge Listesi','',GETDATE(),NULL)


select top(250)
Belgeler.*,
P.Deger as Proje,
Cariler.Kod as CariKod,
Cariler.Isim as CariIsim,
D1.Isim as DepoCikis,
D2.Isim as DepoGiris,
ISNULL((select SUM(BK.Miktar*BK.Fiyat) from BelgeKalemler BK WITH(NOLOCK) Where BK.BelgeID = Belgeler.ID and BK.Silindi = 0),0) as Tutar,
'TL' as DovizBirimi
from Belgeler WITH(NOLOCK)
LEFT OUTER JOIN Cariler WITH(NOLOCK) ON Cariler.ID = Belgeler.CariID
LEFT OUTER JOIN Depolar D1 WITH(NOLOCK) ON D1.ID = Belgeler.DepoCikisID
LEFT OUTER JOIN Depolar D2 WITH(NOLOCK) ON D2.ID = Belgeler.DepoGirisID
LEFT OUTER JOIN GrupKodlari P WITH(NOLOCK) ON P.ID = Belgeler.ProjeID
Where ISNULL(Belgeler.Silindi,0) = 0 and  Belgeler.UyelikID = @UyelikID and Belgeler.Tip = @Tip
and (
   Belgeler.BelgeNo like '%'+@AranacakKelime+'%'
   and ISNULL(Cariler.Isim,'') like '%'+@AranacakKelime+'%'
   and ISNULL(Cariler.Unvan,'') like '%'+@AranacakKelime+'%'
)
and Cariler.Isim like '%'+@CariAdi+'%'
and Belgeler.BelgeNo like '%'+@BelgeNo+'%'
and Belgeler.Tarih between @BaslangicTarihi and @BitisTarihi
and (Belgeler.Durumu = @Durumu or @Durumu = '')

END
GO
drop proc [p_BelgeSil]
GO
CREATE proc [dbo].[p_BelgeSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100),
@Tip nvarchar(50)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Belge','Belge Silindi',(Select top(1) Isim From Cariler WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Update Belgeler set
Silindi = 1,
SilinenTarih=GETDATE(),
SilenKullanici = @KullaniciID
Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_BelgeTamamla]
GO
CREATE proc [dbo].[p_BelgeTamamla](
@ID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

	Select ''

END
GO
drop proc [p_Cari]
GO
CREATE proc [dbo].[p_Cari](
@ID nvarchar(100),
@UyelikID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Detaya Girildi',(select top(1) Isim from Cariler WITH(NOLOCK) Where ID = @ID),GETDATE(),NULL)

select 
Cariler.*,
G1.Deger as GrupKodu1,
G2.Deger as GrupKodu2,
G3.Deger as GrupKodu3,
G4.Deger as GrupKodu4,
G5.Deger as GrupKodu5,
G6.Deger as GrupKodu6,
P.Isim as PlasiyerAdi,
C1.Isim as AnaCari,
C2.Isim as TeslimCari
from Cariler WITH(NOLOCK)
LEFT OUTER JOIN GrupKodlari G1 WITH(NOLOCK) ON G1.ID = Cariler.GrupKodu1ID
LEFT OUTER JOIN GrupKodlari G2 WITH(NOLOCK) ON G2.ID = Cariler.GrupKodu2ID
LEFT OUTER JOIN GrupKodlari G3 WITH(NOLOCK) ON G3.ID = Cariler.GrupKodu3ID
LEFT OUTER JOIN GrupKodlari G4 WITH(NOLOCK) ON G4.ID = Cariler.GrupKodu4ID
LEFT OUTER JOIN GrupKodlari G5 WITH(NOLOCK) ON G5.ID = Cariler.GrupKodu5ID
LEFT OUTER JOIN GrupKodlari G6 WITH(NOLOCK) ON G6.ID = Cariler.GrupKodu6ID
LEFT OUTER JOIN Plasiyerler P WITH(NOLOCK) ON P.ID = Cariler.PlasiyerID
LEFT OUTER JOIN Cariler C1 WITH(NOLOCK) ON C1.ID = Cariler.AnaCariID
LEFT OUTER JOIN Cariler C2 WITH(NOLOCK) ON C2.ID = Cariler.TeslimCariID
Where ISNULL(Cariler.Silindi,0) = 0 and  Cariler.UyelikID = @UyelikID and Cariler.ID = @ID

END
GO
drop proc [p_CariHareketi]
GO
CREATE proc [dbo].[p_CariHareketi](
@UyelikID nvarchar(100),
@CariID nvarchar(max),
@ID nvarchar(max)
)
as
BEGIN

Select
*
From CariHareketleri WITH(NOLOCK)
Where Silindi = 0
and UyelikID = @UyelikID
and CariID = @CariID
and ID = @ID
Order by Tarih asc
END
GO
drop proc [p_CariHareketiKaydet]
GO
CREATE proc [dbo].[p_CariHareketiKaydet](
@UyelikID nvarchar(100),
@ID nvarchar(100),
@CariID nvarchar(100),
@Tarih datetime,
@VadeTarihi datetime,
@BelgeNo nvarchar(max),
@HareketTipi nvarchar(max),
@GC nvarchar(10),
@Tutar decimal(18,8),
@DovizTipi nvarchar(100),
@Kur decimal(18,8),
@DovizTutar decimal(18,8),
@PlasiyerID nvarchar(100),
@BaglantiID nvarchar(100),
@Baglanti nvarchar(100),
@GrupKodu1ID nvarchar(100),
@GrupKodu2ID nvarchar(100),
@Aciklama nvarchar(500),
@Kullanici nvarchar(max)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Hareket Kaydı',(select top(1) Isim from Cariler WITH(NOLOCK) Where ID = @CariID),GETDATE(),NULL)

IF @ID = ''
BEGIN
	declare @YeniID nvarchar(100) = NEWID()
	Insert Into CariHareketleri
	(ID,UyelikID,CariID,Tarih,VadeTarihi,BelgeNo,HareketTipi,
	GC,Tutar,DovizTipi,Kur,DovizTutar,Aciklama,
	PlasiyerID,
	BaglantiID,
	Baglanti,
	GrupKodu1ID,
	GrupKodu2ID,
	Silindi,KayitTarihi,KayitYapanKullanici)
	values
	(@YeniID,@UyelikID,@CariID,@Tarih,@VadeTarihi,@BelgeNo,@HareketTipi,
	@GC,@Tutar,@DovizTipi,@Kur,@DovizTutar,@Aciklama,
	CASE WHEN @PlasiyerID = '' THEN NULL ELSE @PlasiyerID END,
	CASE WHEN @BaglantiID = '' THEN NULL ELSE @BaglantiID END,
	@Baglanti,
	CASE WHEN @GrupKodu1ID = '' THEN NULL ELSE @GrupKodu1ID END,
	CASE WHEN @GrupKodu2ID = '' THEN NULL ELSE @GrupKodu2ID END,
	0,GETDATE(),@Kullanici)
	Select @YeniID as ID,'Kayıt başarılı.' Bilgi
END
ELSE
BEGIN
	Update CariHareketleri set 
		Tarih = @Tarih,
		VadeTarihi = @VadeTarihi,
		BelgeNo = @BelgeNo,
		HareketTipi = @HareketTipi,
		GC = @GC,
		Tutar = @Tutar,
		DovizTipi = @DovizTipi,
		Kur = @Kur,
		DovizTutar = @DovizTutar,
		Aciklama=@Aciklama,
		PlasiyerID = CASE WHEN @PlasiyerID = '' THEN NULL ELSE @PlasiyerID END,
		BaglantiID = CASE WHEN @BaglantiID = '' THEN NULL ELSE @BaglantiID END,
		Baglanti = @Baglanti,
		GrupKodu1ID = CASE WHEN @GrupKodu1ID = '' THEN NULL ELSE @GrupKodu1ID END,
		GrupKodu2ID = CASE WHEN @GrupKodu2ID = '' THEN NULL ELSE @GrupKodu2ID END,
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici
	Where ID = @ID and UyelikID = @UyelikID
	Select @ID as ID,'Kayıt başarılı.' Bilgi

END



ENd
GO
drop proc [p_CariHareketiSil]
GO
CREATE proc [dbo].[p_CariHareketiSil](
@UyelikID nvarchar(100),
@CariID nvarchar(max),
@ID nvarchar(max),
@Kullanici nvarchar(max)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Hareket Silindi.',(select top(1) Isim from Cariler WITH(NOLOCK) Where ID = @CariID),GETDATE(),NULL)

	Update CariHareketleri set 
		Silindi = 1,
		SilinenTarih = GETDATE(),
		SilenKullanici = @Kullanici
	Where UyelikID = @UyelikID and CariID = @CariID and ID = @ID
END
GO
drop proc [p_CariHareketListesi]
GO

CREATE proc [dbo].[p_CariHareketListesi](
@UyelikID nvarchar(100),
@CariID nvarchar(max),
@BaslangicTarihi datetime=null,
@BitisTarihi datetime=null
)
as
BEGIN

	Select
	*,
	CASE WHEN GC = 'G' THEN Tutar ELSE 0 END as Borc,
	CASE WHEN GC = 'C' THEN Tutar ELSE 0 END as Alacak,
	0 Bakiye
	From CariHareketleri WITH(NOLOCK)
	Where Silindi = 0
	and UyelikID = @UyelikID and CariID = @CariID
	--and Tarih between @BaslangicTarihi and @BitisTarihi
	Order by Tarih asc
END
GO
drop proc [p_CariKaydet]
GO
CREATE proc [dbo].[p_CariKaydet](
@ID nvarchar(100),
@UyelikID nvarchar(100),
@Aktif bit,
@KayitTarihi datetime,
@Kod nvarchar(50),
@Isim nvarchar(500),
@Unvan nvarchar(500),
@Adres nvarchar(1000),
@Ilce nvarchar(75),
@Il nvarchar(75),
@Ulke nvarchar(50),
@Bolge nvarchar(100),
@TCKimlikNo nvarchar(50),
@VergiDairesi nvarchar(50),
@VergiNumarasi nvarchar(50),
@PostaKodu nvarchar(50),
@Alici bit=0,
@Satici bit=0,
@Personel bit=0,
@Telefon1 nvarchar(50),
@Telefon2 nvarchar(50),
@EMail nvarchar(250),
@Faks nvarchar(50),
@CepTelefonu nvarchar(50),
@WebSite nvarchar(250),
@GrupKodu1ID nvarchar(100),
@GrupKodu2ID nvarchar(100),
@GrupKodu3ID nvarchar(100),
@GrupKodu4ID nvarchar(100),
@GrupKodu5ID nvarchar(100),
@GrupKodu6ID nvarchar(100),
@MuhasebeKodu nvarchar(50),
@Kilitli bit=0,
@KilitAciklamasi nvarchar(300),
@DovizID nvarchar(100),
@VadeGunu int=0,
@Iskonto1 decimal(18,8)=0,
@ListeFiyat decimal(18,8)=1,
@Aciklama1 nvarchar(1000),
@Aciklama2 nvarchar(1000),
@Aciklama3 nvarchar(1000),
@Aciklama4 nvarchar(1000),
@Aciklama5 nvarchar(1000),
@Aciklama6 nvarchar(1000),
@LimitAsimindaUyar bit=0,
@LimitAsimindaDurdur bit=0,
@CekSenetRiski bit=0,
@Limit decimal(18,8)=0,
@ServisPersoneli bit=0,
@KullaniciAdi nvarchar(100),
@Parola nvarchar(100),
@RiskAciklama ntext,
@PlasiyerID nvarchar(100),
@AnaCariID nvarchar(100),
@TeslimCariID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Yeni Cari Açıldı',@Isim,GETDATE(),@KullaniciID)

set @ID = NEWID()

INSERT INTO [dbo].[Cariler]
           ([ID]
           ,[UyelikID]
           ,[Aktif]
           ,[KayitTarihi]
           ,[Kod]
           ,[Isim]
           ,[Unvan]
           ,[Adres]
           ,[Ilce]
           ,[Il]
           ,[Ulke]
           ,[Bolge]
           ,[TCKimlikNo]
           ,[VergiDairesi]
           ,[VergiNumarasi]
           ,[PostaKodu]
           ,[Alici]
           ,[Satici]
           ,[Personel]
           ,[Telefon1]
           ,[Telefon2]
           ,[EMail]
           ,[Faks]
           ,[CepTelefonu]
           ,[WebSite]
           ,[GrupKodu1ID]
           ,[GrupKodu2ID]
           ,[GrupKodu3ID]
           ,[GrupKodu4ID]
           ,[GrupKodu5ID]
           ,[GrupKodu6ID]
           ,[MuhasebeKodu]
           ,[Kilitli]
           ,[KilitAciklamasi]
           ,[DovizID]
           ,[VadeGunu]
           ,[Iskonto1]
           ,[ListeFiyat]
           ,[Aciklama1]
           ,[Aciklama2]
           ,[Aciklama3]
           ,[Aciklama4]
           ,[Aciklama5]
           ,[Aciklama6]
           ,[LimitAsimindaUyar]
           ,[LimitAsimindaDurdur]
           ,[CekSenetRiski]
           ,[Limit]
           ,[KayitYapanTarihDB]
           ,[KayitYapanKullanici]
           ,[Silindi]
           ,[ServisPersoneli]
           ,[KullaniciAdi]
           ,[Parola]
           ,[RiskAciklama]
           ,[PlasiyerID]
           ,[AnaCariID]
           ,[TeslimCariID]
		   )
     VALUES
           (
		    @ID
           ,@UyelikID
           ,@Aktif
           ,@KayitTarihi
           ,@Kod
           ,@Isim
           ,@Unvan
           ,@Adres
           ,@Ilce
           ,@Il
           ,@Ulke
           ,@Bolge
           ,@TCKimlikNo
           ,@VergiDairesi
           ,@VergiNumarasi
           ,@PostaKodu
           ,@Alici
           ,@Satici
           ,@Personel
           ,@Telefon1
           ,@Telefon2
           ,@EMail
           ,@Faks
           ,@CepTelefonu
           ,@WebSite
           ,CASE WHEN @GrupKodu1ID = '' THEN NULL ELSE @GrupKodu1ID	 END
           ,CASE WHEN @GrupKodu2ID = '' THEN NULL ELSE @GrupKodu2ID	 END
           ,CASE WHEN @GrupKodu3ID = '' THEN NULL ELSE @GrupKodu3ID	 END
           ,CASE WHEN @GrupKodu4ID = '' THEN NULL ELSE @GrupKodu4ID	 END
           ,CASE WHEN @GrupKodu5ID = '' THEN NULL ELSE @GrupKodu5ID	 END
           ,CASE WHEN @GrupKodu6ID = '' THEN NULL ELSE @GrupKodu6ID	 END
           ,@MuhasebeKodu
           ,@Kilitli
           ,@KilitAciklamasi
           ,CASE WHEN @DovizID = '' THEN NULL ELSE @DovizID END
           ,@VadeGunu
           ,@Iskonto1
           ,@ListeFiyat
           ,@Aciklama1
           ,@Aciklama2
           ,@Aciklama3
           ,@Aciklama4
           ,@Aciklama5
           ,@Aciklama6
           ,@LimitAsimindaUyar
           ,@LimitAsimindaDurdur
           ,@CekSenetRiski
           ,@Limit
           ,GETDATE()
           ,CASE WHEN @KullaniciID = '' THEN NULL ELSE @KullaniciID END
           ,0
           ,@ServisPersoneli
           ,@KullaniciAdi
           ,@Parola
           ,@RiskAciklama
           ,CASE WHEN @PlasiyerID = '' THEN NULL ELSE @PlasiyerID END
           ,CASE WHEN @AnaCariID = '' THEN NULL ELSE @AnaCariID END
           ,CASE WHEN @TeslimCariID = '' THEN NULL ELSE @TeslimCariID END
		   )

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Güncellendi',@Isim,GETDATE(),@KullaniciID)

UPDATE [dbo].[Cariler]
   SET 
       
       [Aktif] = @Aktif
      ,[KayitTarihi] = @KayitTarihi
      ,[Kod] = @Kod
      ,[Isim] = @Isim
      ,[Unvan] = @Unvan
      ,[Adres] = @Adres
      ,[Ilce] = @Ilce
      ,[Il] = @Il
      ,[Ulke] = @Ulke
      ,[Bolge] = @Bolge
      ,[TCKimlikNo] = @TCKimlikNo
      ,[VergiDairesi] = @VergiDairesi
      ,[VergiNumarasi] = @VergiNumarasi
      ,[PostaKodu] = @PostaKodu
      ,[Alici] = @Alici
      ,[Satici] = @Satici
      ,[Personel] = @Personel
      ,[Telefon1] = @Telefon1
      ,[Telefon2] = @Telefon2
      ,[EMail] = @EMail
      ,[Faks] = @Faks
      ,[CepTelefonu] = @CepTelefonu
      ,[WebSite] = @WebSite
      ,[GrupKodu1ID] = CASE WHEN @GrupKodu1ID = '' THEN NULL ELSE @GrupKodu1ID	 END
      ,[GrupKodu2ID] = CASE WHEN @GrupKodu2ID = '' THEN NULL ELSE @GrupKodu2ID	 END
      ,[GrupKodu3ID] = CASE WHEN @GrupKodu3ID = '' THEN NULL ELSE @GrupKodu3ID	 END
      ,[GrupKodu4ID] = CASE WHEN @GrupKodu4ID = '' THEN NULL ELSE @GrupKodu4ID	 END
      ,[GrupKodu5ID] = CASE WHEN @GrupKodu5ID = '' THEN NULL ELSE @GrupKodu5ID	 END
      ,[GrupKodu6ID] = CASE WHEN @GrupKodu6ID = '' THEN NULL ELSE @GrupKodu6ID	 END
      ,[MuhasebeKodu] = @MuhasebeKodu
      ,[Kilitli] = @Kilitli
      ,[KilitAciklamasi] = @KilitAciklamasi
      ,[DovizID] = CASE WHEN @DovizID = '' THEN NULL ELSE @DovizID END
      ,[VadeGunu] = @VadeGunu
      ,[Iskonto1] = @Iskonto1
      ,[ListeFiyat] = @ListeFiyat
      ,[Aciklama1] = @Aciklama1
      ,[Aciklama2] = @Aciklama2
      ,[Aciklama3] = @Aciklama3
      ,[Aciklama4] = @Aciklama4
      ,[Aciklama5] = @Aciklama5
      ,[Aciklama6] = @Aciklama6
      ,[LimitAsimindaUyar] = @LimitAsimindaUyar
      ,[LimitAsimindaDurdur] = @LimitAsimindaDurdur
      ,[CekSenetRiski] = @CekSenetRiski
      ,[Limit] = @Limit
      ,[DuzenlemeTarihi] = GETDATE()
      ,[DuzenlemeYapanKullanici] = CASE WHEN @KullaniciID = '' THEN NULL ELSE @KullaniciID END
      ,[ServisPersoneli] = @ServisPersoneli
      ,[KullaniciAdi] = @KullaniciAdi
      ,[Parola] = @Parola
      ,[RiskAciklama] = @RiskAciklama
      ,[PlasiyerID] = CASE WHEN @PlasiyerID = '' THEN NULL ELSE @PlasiyerID END
      ,[AnaCariID] = CASE WHEN @AnaCariID = '' THEN NULL ELSE @AnaCariID END
      ,[TeslimCariID] = CASE WHEN @TeslimCariID = '' THEN NULL ELSE @TeslimCariID END
 WHERE 1=1
 and UyelikID = @UyelikID 
 and ID = @ID



END

END

GO
drop proc [p_CariKisi]
GO

CREATE proc [dbo].[p_CariKisi](
@ID nvarchar(100),
@UyelikID nvarchar(100)
)
as
BEGIN
Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Kişi Getir',(select top(1) Isim from Cariler WITH(NOLOCK) Where ID = @ID),GETDATE(),NULL)

select 
* 
from CariKisiler WITH(NOLOCK)
Where ID = @ID

END

GO
drop proc [p_CariKisiKaydet]
GO

CREATE proc [dbo].[p_CariKisiKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@CariID nvarchar(100)=null,
@Isim nvarchar(250)=null,	
@Email nvarchar(250)=null,
@Gorev nvarchar(250)=null,
@Telefon nvarchar(100),
@Aktif bit=1,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Yeni Cari Kişi Eklendi',@Isim,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into CariKisiler
(ID,CariID,Isim,EMail,Gorev,Telefon,Aktif,KayitTarihi,KayitYapanKullanici)
Select @ID,@CariID,@Isim,@Email,@Gorev,@Telefon,@Aktif,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Kişi Güncellendi',@Isim,GETDATE(),@KullaniciID)

Update CariKisiler set 
Isim = @Isim,
EMail=@Email,
Gorev = @Gorev,
Telefon = @Telefon,
Aktif = @Aktif,
DuzenlemeTarihi = GETDATE(),
DuzenlemeYapanKullanici = @KullaniciID
Where ID = @ID and CariID = @CariID
END

END
GO
drop proc [p_CariKisiListesi]
GO

CREATE proc [dbo].[p_CariKisiListesi](
@CariID nvarchar(100),
@UyelikID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Kişi Listesi','',GETDATE(),NULL)

select 
* 
from CariKisiler WITH(NOLOCK)
Where CariID = @CariID
Order by Isim

END
GO
drop proc [p_CariKisiSil]
GO

CREATE proc [dbo].[p_CariKisiSil](
@ID nvarchar(100)='',
@CariID nvarchar(100),
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Kişi Silindi',(select top(1) Isim from CariKisiler WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From CariKisiler Where ID = @ID and CariID = @CariID

END
GO
drop proc [p_CariListesi]
GO

CREATE proc [dbo].[p_CariListesi](
@UyelikID nvarchar(100),
@Kod nvarchar(max)='',
@Isim nvarchar(max)='',
@Unvan nvarchar(max)='',
@TCKimlikNo nvarchar(max)='',
@VergiNumarasi nvarchar(max)='',
@CepTelefonu nvarchar(max)=''
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Listesi','',GETDATE(),NULL)


select top(250)
Cariler.*,
G1.Deger as GrupKoduAdi1,
G2.Deger as GrupKoduAdi2,
G3.Deger as GrupKoduAdi3,
G4.Deger as GrupKoduAdi4,
G5.Deger as GrupKoduAdi5,
G6.Deger as GrupKoduAdi6,
P.Isim as PlasiyerAdi,
C1.Isim as AnaCari,
C2.Isim as TeslimCari
from Cariler WITH(NOLOCK)
LEFT OUTER JOIN GrupKodlari G1 WITH(NOLOCK) ON G1.ID = Cariler.GrupKodu1ID
LEFT OUTER JOIN GrupKodlari G2 WITH(NOLOCK) ON G2.ID = Cariler.GrupKodu2ID
LEFT OUTER JOIN GrupKodlari G3 WITH(NOLOCK) ON G3.ID = Cariler.GrupKodu3ID
LEFT OUTER JOIN GrupKodlari G4 WITH(NOLOCK) ON G4.ID = Cariler.GrupKodu4ID
LEFT OUTER JOIN GrupKodlari G5 WITH(NOLOCK) ON G5.ID = Cariler.GrupKodu5ID
LEFT OUTER JOIN GrupKodlari G6 WITH(NOLOCK) ON G6.ID = Cariler.GrupKodu6ID
LEFT OUTER JOIN Plasiyerler P WITH(NOLOCK) ON P.ID = Cariler.PlasiyerID
LEFT OUTER JOIN Cariler C1 WITH(NOLOCK) ON C1.ID = Cariler.AnaCariID
LEFT OUTER JOIN Cariler C2 WITH(NOLOCK) ON C2.ID = Cariler.TeslimCariID
Where ISNULL(Cariler.Silindi,0) = 0 and  Cariler.UyelikID = @UyelikID 
and (
   Cariler.Kod like '%'+@Kod+'%'
and Cariler.Isim like '%'+@Isim+'%'
and Cariler.Unvan like '%'+@Unvan+'%'
and Cariler.TCKimlikNo like '%'+@TCKimlikNo+'%'
and Cariler.VergiNumarasi like '%'+@VergiNumarasi+'%'
and Cariler.CepTelefonu like '%'+@CepTelefonu+'%'
)

END
GO
drop proc [p_CariNot]
GO

CREATE proc [dbo].[p_CariNot](
@ID nvarchar(100),
@UyelikID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Not Detayı','',GETDATE(),NULL)

select 
* 
from CariNotlar WITH(NOLOCK)
Where ID = @ID

END

GO
drop proc [p_CariNotKaydet]
GO

CREATE proc [dbo].[p_CariNotKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@CariID nvarchar(100)=null,
@Aciklama nvarchar(max)=null,	
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Not Eklendi',@Aciklama,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into CariNotlar
(ID,CariID,Aciklama,KayitTarihi,KayitYapanKullanici)
Select @ID,@CariID,@Aciklama,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Not Güncellendi',@Aciklama,GETDATE(),@KullaniciID)

Update CariNotlar set 
Aciklama = @Aciklama,
KayitTarihi = GETDATE(),
KayitYapanKullanici = @KullaniciID
Where ID = @ID and CariID = @CariID
END

END
GO
drop proc [p_CariNotListesi]
GO

CREATE proc [dbo].[p_CariNotListesi](
@UyelikID nvarchar(100),
@CariID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Not Listesi','',GETDATE(),NULL)

select 
* 
from CariNotlar WITH(NOLOCK)
Where CariID = @CariID
Order by KayitTarihi desc

END
GO
drop proc [p_CariNotSil]
GO

CREATE proc [dbo].[p_CariNotSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@CariID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Not Silindi',(select top(1) Aciklama from CariNotlar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)


Delete From CariNotlar Where ID = @ID and CariID = @CariID

END
GO
drop proc [p_CariSil]
GO
CREATE proc [dbo].[p_CariSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Cari','Cari Silindi',(Select top(1) Isim From Cariler WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Update Cariler set
Silindi = 1,
SilinenTarih=GETDATE(),
SilenKullanici = @KullaniciID
Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_Crm2_Cariler]
GO
CREATE proc [dbo].[p_Crm2_Cariler](
@UyelikID nvarchar(100),
@Tur nvarchar(100), --Havuzum, BosHavuz, SozlesmeHavuz, SozlesmeYukle
@AranacakKelime nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @Tur = 'Gun90'
BEGIN

select 
* 
from Crm2Kayitlar WITH(NOLOCK)
Where Silindi = 0 
and UyelikID = @UyelikID
and DATEDIFF(DAY,KabulTarihi,GETDATE()) > 90
and (
Bayi like '%'+@AranacakKelime+'%' or
BlokSayisi like '%'+@AranacakKelime+'%' or
Miktar like '%'+@AranacakKelime+'%' or
Unvan like '%'+@AranacakKelime+'%' or
Ad like '%'+@AranacakKelime+'%' or
Soyad like '%'+@AranacakKelime+'%' or
Telefon1 like '%'+@AranacakKelime+'%' or
Telefon2 like '%'+@AranacakKelime+'%' or
ProjeAdi like '%'+@AranacakKelime+'%' or
Ilce like '%'+@AranacakKelime+'%' or
Mahalle like '%'+@AranacakKelime+'%' or
Ada like '%'+@AranacakKelime+'%' or
Parsel like '%'+@AranacakKelime+'%' or
PortalNumarasi like '%'+@AranacakKelime+'%' 
)
Order by KayitTarihi desc
END 
ELSE IF @Tur = 'Tumu'
BEGIN

select 
* 
from Crm2Kayitlar WITH(NOLOCK)
Where Silindi = 0 
and UyelikID = @UyelikID
and (
Bayi like '%'+@AranacakKelime+'%' or
BlokSayisi like '%'+@AranacakKelime+'%' or
Miktar like '%'+@AranacakKelime+'%' or
Unvan like '%'+@AranacakKelime+'%' or
Ad like '%'+@AranacakKelime+'%' or
Soyad like '%'+@AranacakKelime+'%' or
Telefon1 like '%'+@AranacakKelime+'%' or
Telefon2 like '%'+@AranacakKelime+'%' or
ProjeAdi like '%'+@AranacakKelime+'%' or
Ilce like '%'+@AranacakKelime+'%' or
Mahalle like '%'+@AranacakKelime+'%' or
Ada like '%'+@AranacakKelime+'%' or
Parsel like '%'+@AranacakKelime+'%' or
PortalNumarasi like '%'+@AranacakKelime+'%' 
)
Order by KayitTarihi desc
END 
ELSE IF @Tur = 'Havuzum'
BEGIN

select 
* 
from Crm2Kayitlar WITH(NOLOCK)
Where Silindi = 0 
and UyelikID = @UyelikID
and AlanKullanici = @KullaniciID
and Sozlesme = 0
and (
Bayi like '%'+@AranacakKelime+'%' or
BlokSayisi like '%'+@AranacakKelime+'%' or
Miktar like '%'+@AranacakKelime+'%' or
Unvan like '%'+@AranacakKelime+'%' or
Ad like '%'+@AranacakKelime+'%' or
Soyad like '%'+@AranacakKelime+'%' or
Telefon1 like '%'+@AranacakKelime+'%' or
Telefon2 like '%'+@AranacakKelime+'%' or
ProjeAdi like '%'+@AranacakKelime+'%' or
Ilce like '%'+@AranacakKelime+'%' or
Mahalle like '%'+@AranacakKelime+'%' or
Ada like '%'+@AranacakKelime+'%' or
Parsel like '%'+@AranacakKelime+'%' or
PortalNumarasi like '%'+@AranacakKelime+'%' 
)
END 
ELSE IF @Tur = 'BosHavuz'
BEGIN


select 
* 
from Crm2Kayitlar WITH(NOLOCK)
Where Silindi = 0 
and UyelikID = @UyelikID
and AlanKullanici IS NULL
and Sozlesme = 0
and (
Bayi like '%'+@AranacakKelime+'%' or
BlokSayisi like '%'+@AranacakKelime+'%' or
Miktar like '%'+@AranacakKelime+'%' or
Unvan like '%'+@AranacakKelime+'%' or
Ad like '%'+@AranacakKelime+'%' or
Soyad like '%'+@AranacakKelime+'%' or
Telefon1 like '%'+@AranacakKelime+'%' or
Telefon2 like '%'+@AranacakKelime+'%' or
ProjeAdi like '%'+@AranacakKelime+'%' or
Ilce like '%'+@AranacakKelime+'%' or
Mahalle like '%'+@AranacakKelime+'%' or
Ada like '%'+@AranacakKelime+'%' or
Parsel like '%'+@AranacakKelime+'%' or
PortalNumarasi like '%'+@AranacakKelime+'%' 
)
END
ELSE IF @Tur = 'SozlesmeHavuz'
BEGIN


select 
* 
from Crm2Kayitlar WITH(NOLOCK)
Where Silindi = 0 
and UyelikID = @UyelikID
and AlanKullanici = @KullaniciID
and Sozlesme = 1
and (
Bayi like '%'+@AranacakKelime+'%' or
BlokSayisi like '%'+@AranacakKelime+'%' or
Miktar like '%'+@AranacakKelime+'%' or
Unvan like '%'+@AranacakKelime+'%' or
Ad like '%'+@AranacakKelime+'%' or
Soyad like '%'+@AranacakKelime+'%' or
Telefon1 like '%'+@AranacakKelime+'%' or
Telefon2 like '%'+@AranacakKelime+'%' or
ProjeAdi like '%'+@AranacakKelime+'%' or
Ilce like '%'+@AranacakKelime+'%' or
Mahalle like '%'+@AranacakKelime+'%' or
Ada like '%'+@AranacakKelime+'%' or
Parsel like '%'+@AranacakKelime+'%' or
PortalNumarasi like '%'+@AranacakKelime+'%' 
)
END








eND

GO
drop proc [p_Depo]
GO

CREATE proc [dbo].[p_Depo](
@ID nvarchar(100)
)
as
BEGIN


select 
* 
from Depolar WITH(NOLOCK)
Where ID = @ID


END
GO
drop proc [p_DepoKaydet]
GO

CREATE proc [dbo].[p_DepoKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@Kod nvarchar(200)=null,	
@Isim nvarchar(max)=null,
@Adres nvarchar(max)=null,
@Telefon nvarchar(max)=null,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Eklendi',@Isim,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into Depolar
(ID,UyelikID,Kod,Isim,Adres,Telefon,KayitTarihi,KayitYapanKullanici)
Select @ID,@UyelikID,@Kod,@Isim,@Adres,@Telefon,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Güncellendi',@Isim,GETDATE(),@KullaniciID)

Update Depolar set 
Kod=@Kod,
Isim=@Isim,
Adres=@Adres,
Telefon=@Telefon,
DuzenlemeTarihi=GETDATE(),
DuzenlemeYapanKullanici=@KullaniciID
Where ID = @ID and UyelikID = @UyelikID
END

END

GO
drop proc [p_DepoListesi]
GO
CREATE proc [dbo].[p_DepoListesi](
@UyelikID nvarchar(100)='GENEL',
@AranacakKelime nvarchar(max)=''
)
as
BEGIN


	select 
	* 
	from Depolar WITH(NOLOCK)
	Where UyelikID =@UyelikID and (Kod like '%'+@AranacakKelime+'%' or Isim like '%'+@AranacakKelime+'%')
	Order by Isim


END

GO
drop proc [p_DepoSil]
GO
CREATE proc [dbo].[p_DepoSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,(select top(1) Kod From Depolar WITH(NOLOCK) Where ID = @ID),'Kayıt Silindi',(select top(1) Isim From Depolar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From Depolar Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_Dosya]
GO

CREATE proc [dbo].[p_Dosya](
@ID nvarchar(100),
@UyelikID nvarchar(100),
@Modul nvarchar(100),
@KayitID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Modul,'Cari Dosya Detayı','',GETDATE(),NULL)

select 
* 
from Dosyalar WITH(NOLOCK)
Where ID = @ID and Modul = @Modul and KayitID = @KayitID

END

GO
drop proc [p_DosyaKaydet]
GO

CREATE proc [dbo].[p_DosyaKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@Modul nvarchar(100)=null,
@KayitID nvarchar(100)=null,
@Dosya nvarchar(max)=null,	
@Isim nvarchar(max)=null,	
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Modul,'Dosya Yüklendi',@Dosya,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into Dosyalar
(ID,Modul,KayitID,Dosya,Isim,KayitTarihi,KayitYapanKullanici)
Select @ID,@Modul,@KayitID,@Dosya,@Isim,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Select 0 

END

END
GO
drop proc [p_DosyaListesi]
GO

CREATE proc [dbo].[p_DosyaListesi](
@UyelikID nvarchar(100),
@Modul nvarchar(100),
@KayitID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Modul,'Dosya Listesi','',GETDATE(),null)

select 
* 
from Dosyalar WITH(NOLOCK)
Where Modul = @Modul and KayitID = @KayitID
Order by KayitTarihi desc

END
GO
drop proc [p_DosyaSil]
GO

CREATE proc [dbo].[p_DosyaSil](
@UyelikID nvarchar(100),
@ID nvarchar(100)='',
@Modul nvarchar(100),
@KayitID nvarchar(100),	
@KullaniciID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Modul,'Dosya Yüklendi',(select top(1) Dosya from Dosyalar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From Dosyalar Where ID = @ID and Modul = @Modul and KayitID = @KayitID

END
GO
drop proc [p_Doviz]
GO
CREATE proc [dbo].[p_Doviz](
@ID nvarchar(100)
)
as
BEGIN


select 
* 
from Dovizler WITH(NOLOCK)
Where ID = @ID


END

GO
drop proc [p_DovizKaydet]
GO
	


CREATE proc [dbo].[p_DovizKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@Kod nvarchar(100)=null,	
@Isim nvarchar(100)=null,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Eklendi',@Isim,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into Dovizler
(ID,UyelikID,Kod,Isim,KayitTarihi,KayitYapanKullanici)
Select @ID,@UyelikID,@Kod,@Isim,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Güncellendi',@Isim,GETDATE(),@KullaniciID)

Update Dovizler set 
Kod=@Kod,
Isim=@Isim,
DuzenlemeTarihi=GETDATE(),
DuzenlemeYapanKullanici=@KullaniciID
Where ID = @ID and UyelikID = @UyelikID
END

END

GO
drop proc [p_DovizListesi]
GO


CREATE proc [dbo].[p_DovizListesi](
@UyelikID nvarchar(100)='GENEL',
@AranacakKelime nvarchar(max)=''
)
as
BEGIN


	select 
	* 
	from Dovizler WITH(NOLOCK)
	Where UyelikID =@UyelikID and (Kod like '%'+@AranacakKelime+'%' or Isim like '%'+@AranacakKelime+'%')
	Order by Isim


END
GO
drop proc [p_DovizSil]
GO
CREATE proc [dbo].[p_DovizSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,(select top(1) Kod From Dovizler WITH(NOLOCK) Where ID = @ID),'Kayıt Silindi',(select top(1) Isim From Depolar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From Dovizler Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_FirmaListesi]
GO
CREATE proc [dbo].[p_FirmaListesi](
@UyelikID nvarchar(100)='GENEL',
@AranacakKelime nvarchar(max)=''
)
as
BEGIN


	select
	Firma 
	from Subeler WITH(NOLOCK)
	Where UyelikID =@UyelikID and (Kod like '%'+@AranacakKelime+'%' or Isim like '%'+@AranacakKelime+'%')
	Group by Firma
	Order by Firma


END

GO
drop proc [p_Gorev]
GO
CREATE PROC [dbo].[p_Gorev](
@ID nvarchar(100),
@UyelikID nvarchar(100)
)
as
BEGIN
	-- Kayıt Detayı
	Select 
	Gorevler.* ,
	Kullanicilar.Ad+' '+Kullanicilar.Soyad as KaydiAcan,
	GrupKodlari.Deger as GorevtipiAdi
	from Gorevler WITH(NOLOCK)	
	LEFT OUTER JOIN GrupKodlari WITH(NOLOCK) ON GrupKodlari.ID = Gorevler.GorevTipiID
	LEFT OUTER JOIN Kullanicilar WITH(NOLOCK) ON Kullanicilar.ID = Gorevler.KayitYapanKullanici
	Where Gorevler.Silindi = 0 and Gorevler.UyelikID = @UyelikID and Gorevler.ID = @ID

	-- Atanan Görev Kullanıcıları
	select
	GorevKullanicilari.*,
	Kullanicilar.Ad+' '+Kullanicilar.Soyad+' - '+Kullanicilar.Aciklama1 as Isim
	from GorevKullanicilari WITH(NOLOCK)
	LEFT OUTER JOIN Kullanicilar WITH(NOLOCK) ON Kullanicilar.ID = GorevKullanicilari.KullaniciID
	Where GorevKullanicilari.UyelikID = @UyelikID and GorevKullanicilari.GorevID = @ID

	-- Görev Hareketleri
	select
	GorevHareketleri.*,
	G.Durumu,
	Dosyalar.Dosya,
	Dosyalar.Isim,
	Kullanicilar.Ad+' '+Kullanicilar.Soyad as KaydiKapatan
	from GorevHareketleri WITH(NOLOCK)
	LEFT OUTER JOIN Kullanicilar WITH(NOLOCK) ON Kullanicilar.ID = GorevHareketleri.KullaniciID
	LEFT OUTER JOIN Gorevler G WITH(NOLOCK) ON G.ID = GorevHareketleri.GorevID
	left outer join Dosyalar WITH(NOLOCK) ON Dosyalar.Modul = 'Gorev' and KayitID = GorevHareketleri.ID
	Where GorevHareketleri.UyelikID = @UyelikID and GorevHareketleri.GorevID = @ID

	-- Görev Dosyaları
	select * from Dosyalar WITH(NOLOCK) where Modul = 'Gorev' and KayitID = @ID 

END
GO
drop proc [p_GorevKaydet]
GO
CREATE proc [dbo].[p_GorevKaydet](
@ID nvarchar(100),
@UyelikID nvarchar(100),
@GorevTipiID nvarchar(100),
@Aciklama nvarchar(max),
@BaslangicTarihi datetime,
@Periyot nvarchar(10),
@Durumu nvarchar(100)='',
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN
	set @ID = (select NEWID())
	Insert Into Gorevler 
	(
		ID,
		UyelikID,
		GorevTipiID,
		Aciklama,
		BaslangicTarihi,
		Periyot,
		KayitTarihi,
		KayitYapanKullanici,
		Silindi
)
	values
	(
		@ID,
		@UyelikID,
		@GorevTipiID,
		@Aciklama,
		@BaslangicTarihi,
		@Periyot,
		GETDATE(),
		@KullaniciID,
		0
	)
	Select @ID
END
ELSE
BEGIN
	Update Gorevler Set 
		GorevTipiID=@GorevTipiID,
		Aciklama = @Aciklama,
		BaslangicTarihi = @BaslangicTarihi,
		Periyot = @Periyot,
		DuzenlemeTarihi=GETDATE(),
		DuzenlemeYapanKullanici = @KullaniciID,
		Durumu = @Durumu
	Where ID = @ID
	Select @ID
END

END
GO
drop proc [p_GorevKullaniciKaydet]
GO
CREATE proc [dbo].[p_GorevKullaniciKaydet](
@UyelikID nvarchar(100),
@GorevID nvarchar(100),
@SecilenKullaniciID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN
	declare @ID nvarchar(100) = (Select NEWID())
	Insert Into GorevKullanicilari
	(ID,UyelikID,GorevID,KullaniciID,KayitTarihi,KayitYapanKullanici)
	values 
	(@ID,@UyelikID,@GorevID,@SecilenKullaniciID,GETDATE(),@KullaniciID)
END
GO
drop proc [p_GorevKullanicilari]
GO
CREATE proc [dbo].[p_GorevKullanicilari](
@ID nvarchar(100),
@UyelikID nvarchar(100)
)
as
BEGIN

	
	Select
	*
	From GorevKullanicilari WITH(NOLOCK)
	Where UyelikID = @UyelikID and GorevID = @ID

END
GO
drop proc [p_GorevKullanicilariniSil]
GO

create proc [dbo].[p_GorevKullanicilariniSil](
@UyelikID nvarchar(100),
@GorevID nvarchar(100)
)
as
BEGIN
	
	Delete From GorevKullanicilari Where UyelikID = @UyelikID and GorevID = @GorevID

END
GO
drop proc [p_GorevKullaniciSil]
GO

CREATE proc [dbo].[p_GorevKullaniciSil](
@UyelikID nvarchar(100),
@GorevID nvarchar(100),
@SecilenKullaniciID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN
	Delete from GorevKullanicilari Where UyelikID = @UyelikID and KullaniciID = @SecilenKullaniciID
END
GO
drop proc [p_GorevListesi]
GO

CREATE PROC [dbo].[p_GorevListesi](
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100),
@Baslangic nvarchar(100),
@Bitis nvarchar(100),
@Baslangic2 nvarchar(100)=null,
@Bitis2 nvarchar(100)=null,
@GorevTipiID nvarchar(100)='',
@KayitYapanKullanici nvarchar(100)='',
@AtananKullanici nvarchar(100)='',
@Durum nvarchar(100)='',
@Periyot nvarchar(100)=''
)
as
BEGIN

	Select * from (
	Select 
	Gorevler.*,
	GrupKodlari.Deger as Gorev,
	Kullanicilar.Ad+' '+Kullanicilar.Soyad as KaydiAcan,
	Kullanicilar.Resim,
	ISNULL(Durumu,'Beklemede') as Durum,
	(select COUNT(A1.ID) From GorevKullanicilari as A1 WITH(NOLOCK) Where A1.UyelikID = Gorevler.UyelikID and A1.GorevID = Gorevler.ID) as GorevAtamaSayisi,
	(select COUNT(A1.ID) From GorevHareketleri as A1 WITH(NOLOCK) Where A1.UyelikID = Gorevler.UyelikID and A1.GorevID = Gorevler.ID ) as GorevHareketSayisi,
	(
		select 
			K1.Ad+' '+LEFT(K1.Soyad,1)+'.~'+K1.Resim+'|' 
		From GorevKullanicilari as A1 WITH(NOLOCK) 
		INNER JOIN Kullanicilar K1 ON K1.Silindi = 0 and K1.UyelikID = A1.UyelikID and K1.ID = A1.KullaniciID
		Where A1.UyelikID = Gorevler.UyelikID and A1.GorevID = Gorevler.ID 
		for xml path('')
	) as AtananKullanicilar
	from Gorevler WITH(NOLOCK)
	LEFT OUTER JOIN Kullanicilar WITH(NOLOCK) ON Kullanicilar.ID = Gorevler.KayitYapanKullanici
	LEFT OUTER JOIN GrupKodlari WITH(NOLOCK) ON GrupKodlari.ID = Gorevler.GorevTipiID
	Where Gorevler.Silindi = 0 
	and Gorevler.UyelikID = @UyelikID 
	and (Gorevler.Periyot = @Periyot or @Periyot = '')
	and Gorevler.KayitTarihi between @Baslangic and @Bitis
	and (CAST(Gorevler.GorevTipiID as nvarchar(100)) = @GorevTipiID or @GorevTipiID = '')
	and (CAST(Kullanicilar.ID as nvarchar(100)) = @KayitYapanKullanici or @KayitYapanKullanici = '')
	and (
		Gorevler.KayitYapanKullanici = @KullaniciID
		or
		Gorevler.ID IN (
			select GorevKullanicilari.GorevID from GorevKullanicilari WITH(NOLOCK) Where GorevKullanicilari.KullaniciID = @KullaniciID
		)
	)
	and (
		@AtananKullanici = ''
		or
		Gorevler.ID IN (
			select 
				GorevKullanicilari.GorevID 
			from GorevKullanicilari WITH(NOLOCK) 
			Where CAST(GorevKullanicilari.KullaniciID as nvarchar(100)) = @AtananKullanici
		)
	)
	--and Gorevler.ID IN (
	--		select 
	--			GorevHareketleri.GorevID 
	--		from GorevHareketleri WITH(NOLOCK) 
	--		Where UyelikID = @UyelikID and GorevHareketleri.TamamlamaTarihi between @Baslangic2 and @Bitis2
	--	)
	) YK1
	Where (ISNULL(Durum,'Beklemede') = @Durum or ISNULL(Durum,'Beklemede') = CASE WHEN @Durum = 'Beklemede' Then 'Cevaplandı' ELSE 'XXXXX' END or @Durum = '')

	Order by YK1.BaslangicTarihi asc

	Select 
	Kullanicilar.Resim,
	Kullanicilar.Ad+' '+LEFT(Kullanicilar.Soyad,1)+'.' as Isim,
	SUM(1)
	---
	--SUM(CASE WHEN GorevHareketleri.ID IS NULL THEN 0 ELSE 1 END) 
	as Miktar
	from Gorevler WITH(NOLOCK)	
	INNER JOIN GorevKullanicilari WITH(NOLOCK) ON 		GorevKullanicilari.UyelikID = Gorevler.UyelikID 		and GorevKullanicilari.GorevID = Gorevler.ID
	LEFT OUTER JOIN GorevHareketleri WITH(NOLOCK) ON 		GorevHareketleri.UyelikID = Gorevler.UyelikID 		and GorevHareketleri.GorevID = Gorevler.ID and GorevHareketleri.KullaniciID = GorevKullanicilari.KullaniciID
	LEFT OUTER JOIN Kullanicilar WITH(NOLOCK) ON 		Kullanicilar.ID = GorevKullanicilari.KullaniciID
	Where Gorevler.Silindi = 0 
	and Gorevler.UyelikID = @UyelikID 
	and ISNULL(Gorevler.Durumu,'Beklemede') IN ('Beklemede','Cevaplandı')
	and (
		Gorevler.KayitYapanKullanici = @KullaniciID
		or
		Gorevler.ID IN (
			select GorevKullanicilari.GorevID from GorevKullanicilari WITH(NOLOCK) Where GorevKullanicilari.KullaniciID = @KullaniciID
		)
	)
	Group by Kullanicilar.Resim,Kullanicilar.Ad,LEFT(Kullanicilar.Soyad,1)
	having SUM(1) > 0
END

GO
drop proc [p_GorevSil]
GO


CREATE proc [dbo].[p_GorevSil](
@ID nvarchar(100),
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN
	Update Gorevler Set 
		Silindi = 1,
		SilinenTarih=GETDATE(),
		SilenKullanici = @KullaniciID
	Where UyelikID = @UyelikID and ID = @ID
END
GO
drop proc [p_GorevTamamla]
GO
CREATE PROC [dbo].[p_GorevTamamla](
@UyelikID nvarchar(100),
@GorevID nvarchar(100),
@KullaniciID nvarchar(100),
@Aciklama nvarchar(max),
@Durumu nvarchar(max),
@TamamlamaTarihi datetime
)
as
BEGIN
	declare @ID nvarchar(100)=(Select newid())

	Update Gorevler set Durumu = @Durumu Where UyelikID = @UyelikID and ID = @GorevID

	Insert Into GorevHareketleri
	(ID,UyelikID,GorevID,KullaniciID,Tamamlandi,TamamlamaTarihi,Aciklama)
	values 
	(@ID,@UyelikID,@GorevID,@KullaniciID,1,@TamamlamaTarihi,@Aciklama)

	Select @ID as ID
END
GO
drop proc [p_GrupKodu]
GO
CREATE proc [dbo].[p_GrupKodu](
@ID nvarchar(100)
)
as
BEGIN


select 
* 
from GrupKodlari WITH(NOLOCK)
Where ID = @ID
Order by Deger

END
GO
drop proc [p_GrupKoduKaydet]
GO
CREATE proc [dbo].[p_GrupKoduKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@UstID nvarchar(100)=null,
@Kod nvarchar(200)=null,	
@Deger nvarchar(max)=null,
@Aktif bit=1,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Eklendi',@Deger,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into GrupKodlari
(ID,UyelikID,Kod,Deger,Aktif,UstID)
Select @ID,@UyelikID,@Kod,@Deger,@Aktif,@UstID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Güncellendi',@Deger,GETDATE(),@KullaniciID)

Update GrupKodlari set 
Deger = @Deger,Aktif=@Aktif
Where ID = @ID and UyelikID = @UyelikID
END

END
GO
drop proc [p_GrupKoduListesi]
GO

CREATE proc [dbo].[p_GrupKoduListesi](
@UyelikID nvarchar(100)='GENEL',
@UstID nvarchar(100)=null,
@Kod nvarchar(250),
@AranacakKelime nvarchar(max)=''
)
as
BEGIN

IF @UyelikID = 'GENEL'
BEGIN

	select 
	* 
	from GrupKodlari WITH(NOLOCK)
	Where UyelikID IS NULL and Kod = @Kod and (@UstId IS NULL or UstID = @UstID)
	Order by Deger

END
ELSE
BEGIN

	select 
	* 
	from GrupKodlari WITH(NOLOCK)
	Where UyelikID = @UyelikID and Kod = @Kod and (@UstId IS NULL or UstID = @UstID)
	and Deger like '%'+@AranacakKelime+'%'
	Order by Deger

END

END
GO
drop proc [p_GrupKoduSil]
GO
CREATE proc [dbo].[p_GrupKoduSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,(select top(1) Kod From GrupKodlari WITH(NOLOCK) Where ID = @ID),'Kayıt Silindi',(select top(1) Deger From GrupKodlari WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From GrupKodlari Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_HataKaydet]
GO
CREATE proc [dbo].[p_HataKaydet](
@UyelikID nvarchar(max),
@Kullanici nvarchar(max),
@Modul nvarchar(max),
@Aciklama1 nvarchar(max),
@Aciklama2 nvarchar(max)
)
--WITH ENCRYPTION
as
BEGIN
	SET NOCOUNT ON;
		Insert Into Hatalar 
			(Dosya,Hata,Hata2,Modul,Surum,Tarih) 
		values 
			(null,@Aciklama1,@Aciklama2,@Modul,'',GETDATE())
END





GO
drop proc [p_Hatirlatici]
GO

CREATE proc [dbo].[p_Hatirlatici](
@ID nvarchar(100),
@UyelikID nvarchar(100),
@Modul nvarchar(100)
)
as
BEGIN

select 
* 
from Hatirlaticilar WITH(NOLOCK)
Where ID = @ID and Modul = @Modul and UyelikID = @UyelikID

END

GO
drop proc [p_HatirlaticiKaydet]
GO

CREATE proc [dbo].[p_HatirlaticiKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@Modul nvarchar(100)=null,
@KayitID nvarchar(100)=null,
@HatirlatmaTarihi datetime,
@HatirlatilacakKullanici nvarchar(100)=null,
@Aciklama nvarchar(max),
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Modul,'Yeni Hatırlatıcı Eklendi',@Aciklama,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into Hatirlaticilar
(ID,UyelikID,Modul,KayitID,HatirlatmaTarihi,HatirlatilacakKullanici,Aciklama,KayitTarihi,KayitYapanKullanici)
Select @ID,@UyelikID,@Modul,@KayitID,@HatirlatmaTarihi,@HatirlatilacakKullanici,@Aciklama,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Modul,'Hatırlatıcı Güncellendi',@Aciklama,GETDATE(),@KullaniciID)

Update Hatirlaticilar set
HatirlatmaTarihi=@HatirlatmaTarihi,
HatirlatilacakKullanici=@HatirlatilacakKullanici,
Aciklama=@Aciklama,
KayitTarihi = GETDATE(),
KayitYapanKullanici = @KullaniciID
Where ID = @ID

END

END
GO
drop proc [p_HatirlaticiListesi]
GO

CREATE proc [dbo].[p_HatirlaticiListesi](
@UyelikID nvarchar(100),
@Modul nvarchar(100),
@KayitID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

select 
* 
from Hatirlaticilar WITH(NOLOCK)
Where UyelikID = @UyelikID
and Modul = @Modul 
--and KayitID = @KayitID
and HatirlatilacakKullanici = @KullaniciID
Order by KayitTarihi desc

END
GO
drop proc [p_HatirlaticiSil]
GO

CREATE proc [dbo].[p_HatirlaticiSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100),
@Modul nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Modul,'Hatırlatıcı Silindi',(select top(1) Aciklama from Hatirlaticilar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From Hatirlaticilar Where ID = @ID and UyelikID = @UyelikID 

END
GO
drop proc [p_Kasa]
GO
CREATE proc [dbo].[p_Kasa](
@ID nvarchar(100)
)
as
BEGIN


select 
* 
from Kasalar WITH(NOLOCK)
Where ID = @ID


END

GO
drop proc [p_KasaKaydet]
GO

CREATE proc [dbo].[p_KasaKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@Kod nvarchar(100)=null,	
@Isim nvarchar(100)=null,
@DovizID nvarchar(100)=null,
@PersonelID nvarchar(100)=null,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Eklendi',@Isim,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into Kasalar
(ID,UyelikID,Kod,Isim,DovizID,PersonelID,KayitTarihi,KayitYapanKullanici)
Select @ID,@UyelikID,@Kod,@Isim,@DovizID,@PersonelID,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Güncellendi',@Isim,GETDATE(),@KullaniciID)

Update Kasalar set 
Kod=@Kod,
Isim=@Isim,
DovizID=@DovizID,
PersonelID=@PersonelID,
DuzenlemeTarihi=GETDATE(),
DuzenlemeYapanKullanici=@KullaniciID
Where ID = @ID and UyelikID = @UyelikID
END

END

GO
drop proc [p_KasaListesi]
GO
CREATE proc [dbo].[p_KasaListesi](
@UyelikID nvarchar(100)='GENEL',
@AranacakKelime nvarchar(max)=''
)
as
BEGIN


	select 
	* 
	from Kasalar WITH(NOLOCK)
	Where UyelikID =@UyelikID and (Kod like '%'+@AranacakKelime+'%' or Isim like '%'+@AranacakKelime+'%')
	Order by Isim


END
GO
drop proc [p_KasaSil]
GO
CREATE proc [dbo].[p_KasaSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,(select top(1) Kod From Kasalar WITH(NOLOCK) Where ID = @ID),'Kayıt Silindi',(select top(1) Isim From Depolar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From Kasalar Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_Kullanici]
GO
CREATE proc [dbo].[p_Kullanici](
@ID nvarchar(100),
@UyelikID nvarchar(100)
)
as
BEGIN

select 
* 
from Kullanicilar WITH(NOLOCK)
Where Silindi = 0 and UyelikID = @UyelikID and ID = @ID

END
GO
drop proc [p_KullaniciAdiBul]
GO
CREATE proc [dbo].[p_KullaniciAdiBul](
@KullaniciAdi nvarchar(100)
)
as
BEGIN

Insert Into Loglar (Modul,Aciklama1,Aciklama2,KayitTarihi) values ('Parolamı Unuttum','Parolamı Unuttum',@KullaniciAdi,GETDATE())


select 
* 
from Kullanicilar WITH(NOLOCK)
Where Silindi = 0 and KullaniciAdi = @KullaniciAdi

END
GO
drop proc [p_KullaniciAktivasyonuYap]
GO
CREATE proc [dbo].[p_KullaniciAktivasyonuYap](
@ID nvarchar(max)
)
as
BEGIN

IF EXISTS(select * from Kullanicilar WITH(NOLOCK) Where ID = @ID and ISNULL(Onay,0) = 1)
BEGIN
	Select @ID as ID,'UYARI! Link kullanılamaz, daha önce onaylama yapılmış. ' as Bilgi
	return;
END

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values ((select top(1) UyelikID from Kullanicilar WITH(NOLOCK) Where ID = @ID),'Kullanıcı','Hesap Aktivasyonu Yapıldı',(select top(1) Ad+' '+Soyad from Kullanicilar WITH(NOLOCK) Where ID = @ID),GETDATE(),@ID)

update Kullanicilar set Onay = 1 Where ID = @ID
Select @ID as ID,'Onaylama işlemi başarıyla tamamlandı, sisteme giriş  yapabilirsiniz. <a href="https://app.ykyazilim.com.tr">Giriş Yap</a>' Bilgi

END

GO
drop proc [p_KullaniciGirisi]
GO
CREATE proc [dbo].[p_KullaniciGirisi](
@KullaniciAdi nvarchar(100),
@Parola nvarchar(100)
)
as
BEGIN


IF @KullaniciAdi not like '%@%.%'
BEGIN
	Select 0 as ID, 'UYARI! Kullanıcı adı e-mail formatında değil!' as Bilgi
	return;
END

IF NOT EXISTS(select ID from Kullanicilar WITH(NOLOCK) Where Silindi = 0 and KullaniciAdi = @KullaniciAdi and Parola = @Parola and Aktif = 1) 
BEGIN
	Select 0 as ID, 'UYARI! Kullanıcı adı veya parola yanlış!' as Bilgi
	return;
END

IF EXISTS(select ID from Kullanicilar WITH(NOLOCK) Where Silindi = 0 and KullaniciAdi = @KullaniciAdi and Parola = @Parola and Aktif = 1 and Onay = 0 )
BEGIN
	Select 0 as ID, 'UYARI! Onaylanmamış kullanıcı, lütfen e-mail ile gönderilen linke tıklayarak onaylayınız!' as Bilgi
	return;
END

declare @UyelikID nvarchar(100) = (select top(1) UyelikID from Kullanicilar WITH(NOLOCK) Where Silindi = 0 and KullaniciAdi = @KullaniciAdi)
IF EXISTS(select ID from Uyelikler WITH(NOLOCK) Where Silindi = 0 and ID = @UyelikID and UyelikBitisTarihi <= CAST(GETDATE() as date) )
BEGIN
	Select 0 as ID, 'UYARI! Üyeliğinizin süresi bitmiştir!' as Bilgi
	return;
END

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values ((select top(1) UyelikID from Kullanicilar WITH(NOLOCK) Where Silindi = 0 and KullaniciAdi = @KullaniciAdi),'Kullanıcı','Kullanıcı Girişi',(select top(1) Ad+' '+Soyad from Kullanicilar WITH(NOLOCK) Where Silindi = 0 and KullaniciAdi = @KullaniciAdi),GETDATE(),null)

select 
Kullanicilar.*,
Uyelikler.Isim as UyelikIsim,
Uyelikler.AcilisSayfasi,
'Giriş başarılı.' as Bilgi,
Uyelikler.UyelikBitisTarihi,
DATEDIFF(DAY,GETDATE(),Uyelikler.UyelikBitisTarihi) BitisGunu,
Uyelikler.Resim as Logo
from Kullanicilar WITH(NOLOCK)
LEFT OUTER JOIN Uyelikler WITH(NOLOCK) ON Uyelikler.ID = Kullanicilar.UyelikID
Where Kullanicilar.Silindi = 0 and Aktif = 1 and LEN(@KullaniciAdi) > 0 and LEN(@Parola) > 0
and KullaniciAdi = @KullaniciAdi
and Parola = @Parola

END
GO
drop proc [p_KullaniciKaydet]
GO

CREATE proc [dbo].[p_KullaniciKaydet](
@ID nvarchar(100)='',
@UyelikID nvarchar(100)= null,
@KullaniciAdi nvarchar(100),
@Parola nvarchar(100),
@Ad nvarchar(100),
@Soyad nvarchar(100),
@Aktif bit,
@Telefon nvarchar(100),
@Adres nvarchar(max),
@Il nvarchar(100),
@Ilce nvarchar(100),
@Aciklama1 nvarchar(max),
@Aciklama2 nvarchar(max),
@Aciklama3 nvarchar(max),
@Onay bit = 0,
@Kullanici nvarchar(100),
@Resim nvarchar(max)='',
@Ilk bit=0
)
as
BEGIN

IF @KullaniciAdi not like '%@%.%'
BEGIN
	Select 0 as ID, 'Kullanıcı adı e-mail formatında değil!' as Bilgi
	return;
END

IF NOT EXISTS(select ID from Uyelikler WITH(NOLOCK) Where Silindi = 0 and ID = @UyelikID )
BEGIN
	Select 0 as ID, 'Sistemde üyelik bulunamadı!' as Bilgi
	return;
END

IF EXISTS(select ID from Kullanicilar WITH(NOLOCK) Where Silindi = 0 and UyelikID = @UyelikID and KullaniciAdi = @KullaniciAdi and CAST(ID as nvarchar(100)) <> @ID)
BEGIN
	Select 0 as ID, 'Kullanıcı adı daha önce kullanışmış!' as Bilgi
	return;
END

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) 
values (@UyelikID,'Kullanıcı','Yeni Kullanıcı Eklendi',@Ad+' '+@Soyad,GETDATE(),@ID)

set @ID = NEWID()
Insert Into Kullanicilar
(ID,UyelikID,KullaniciAdi,Parola,Ad,Soyad,Telefon,Adres,Il,Ilce,Aktif,Aciklama1,Aciklama2,Aciklama3,Onay,KayitTarihi,KayitYapanKullanici,Silindi,Resim)
Select @ID,@UyelikID,@KullaniciAdi,@Parola,@Ad,@Soyad,@Telefon,@Adres,@Il,@Ilce,@Aktif,@Aciklama1,@Aciklama2,@Aciklama3,@Onay,GETDATE(),@Kullanici,0,@Resim

Insert Into Yetkiler 
(UyelikID,KullaniciID,MenuID,Gor,Duzenle,Sil,KayitTarihi,KayitYapanKullanici)
select @UyelikID,@ID,ID,1,1,1,GETDATE(),@ID from Menuler WITH(NOLOCK) Where @Ilk = 1


declare @Baslik nvarchar(max) = 'YK YAZILIM - Aktivasyon'
declare @Icerik nvarchar(max) = ''
set @Icerik += '<h2>Merhaba '+@Ad+' '+@Soyad+',</h2><br/>'
set @Icerik += '<strong>Hesabınızı doğrulayarak YK ERP’yi kullanmaya başlayın.</strong><br/><br/>'
set @Icerik += ''
set @Icerik += 'Hesabınızı doğrulamak için <a href="http://app.ykyazilim.com.tr/Aktivasyon/KullaniciOnayla/?kullaniciid='+@ID+'">tıklayın</a>'
set @Icerik += ''
set @Icerik += ''
set @Icerik += ''
/*
EXEC msdb.dbo.sp_send_dbmail @profile_name = 'YKMail',
                    @recipients = @KullaniciAdi,
                    @subject = @Baslik, 
					@body_format = 'HTML',
                    @body = @Icerik;
*/
END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Kullanıcı','Kullanıcı Güncellendi',@Ad+' '+@Soyad,GETDATE(),@ID)

Update Kullanicilar set 
UyelikID = @UyelikID,
KullaniciAdi = @KullaniciAdi,
Parola = @Parola,
Ad = @Ad,
Soyad = @Soyad,
Telefon = @Telefon,
Adres = @Adres,
Il = @Il,
Ilce = @Ilce,
Aktif = @Aktif,
Aciklama1 = @Aciklama1,
Aciklama2 = @Aciklama2,
Aciklama3 = @Aciklama3,
Onay=@Onay,
DuzenlemeTarihi = GETDATE(),
DuzenlemeYapanKullanici = @Kullanici,
Resim= CASE WHEN @Resim = '' THEN Resim ELSE @Resim END
Where ID = @ID

END

Select @ID as ID, 'Kullanıcı başarıyla oluşturuldu.' as Bilgi

END
GO
drop proc [p_KullaniciListesi]
GO
CREATE proc [dbo].[p_KullaniciListesi](
@UyelikID nvarchar(100),
@AranacakKelime nvarchar(max)=''
)
as
BEGIN

if(@AranacakKelime = 'DestekKullanicilari')
BEGIN
set @AranacakKelime = '%'+@AranacakKelime+'%'
select 
* 
from Kullanicilar WITH(NOLOCK)
Where (Silindi = 0 and UyelikID = @UyelikID) and
(KullaniciAdi like '%ykyazilim.com.tr')
END
ELSE
BEGIN
set @AranacakKelime = '%'+@AranacakKelime+'%'
select 
* 
from Kullanicilar WITH(NOLOCK)
Where (Silindi = 0 and UyelikID = @UyelikID) and
(KullaniciAdi like @AranacakKelime or Ad like @AranacakKelime or Soyad like @AranacakKelime)

END

END
GO
drop proc [p_KullaniciSil]
GO
CREATE proc [dbo].[p_KullaniciSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@Kullanici nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Kullanıcı','Kullanıcı Silindi',(Select top(1) Ad+' '+Soyad From Kullanicilar WITH(NOLOCK) Where ID = @ID),GETDATE(),@Kullanici)

Update Kullanicilar set 
Silindi = 1,
SilinenTarih = GETDATE(),
SilenKullanici = @Kullanici
Where ID = @ID



END
GO
drop proc [p_KullaniciSubeYetkileri]
GO
CREATE proc [dbo].[p_KullaniciSubeYetkileri](
@ID nvarchar(100)=''
)
as
BEGIN

	select 
	ISNULL(Yetki,1) as Yetki,
	*
	from SatinalmaSubeler WITH(NOLOCK)
	LEFT OUTER JOIN SatinalmaYetkilerSube WITH(NOLOCK) ON SatinalmaYetkilerSube.MenuID = SatinalmaSubeler.ID
	--Where SatinalmaYetkilerSube.Kullanici = @Kullanici

END

GO
drop proc [p_KullaniciYetkiKaydet]
GO
CREATE proc [dbo].[p_KullaniciYetkiKaydet](
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100),
@MenuID nvarchar(100),
@Gor bit,
@Duzenle bit,
@Sil bit
)
as
BEGIN
	IF EXISTS (Select * from Yetkiler WITH(NOLOCK) Where UyelikID = @UyelikID and KullaniciID = @KullaniciID and MenuID = @MenuID)
	BEGIN
		Update Yetkiler Set 
			Gor = @Gor,
			Duzenle = @Duzenle,
			Sil = @Sil
		Where UyelikID = @UyelikID and KullaniciID = @KullaniciID and MenuID = @MenuID
	END
	ELSE
	BEGIN
		Insert Into Yetkiler 
		(MenuID,KullaniciID,UyelikID,Gor,Duzenle,Sil,KayitTarihi,KayitYapanKullanici)
		values
		(@MenuID,@KullaniciID,@UyelikID,@Gor,@Duzenle,@Sil,GETDATE(),@KullaniciID)
	END
END
GO
drop proc [p_KullaniciYetkileri]
GO
CREATE proc [dbo].[p_KullaniciYetkileri](
@KullaniciID nvarchar(100) = '45B83A26-D48D-4B59-BD12-92CD2788C093',
@UyelikID nvarchar(100)='68854AF6-504F-48C7-B64E-ECCBF881DB80'
)
as
BEGIN

Select 
Menuler.ID as MenuID,
Kullanicilar.ID as KullaniciID,
Kullanicilar.UyelikID,
Menuler.Menu,
Menuler.UstID,
Menuler.icon,
Menuler.url,
ISNULL(Yetkiler.Gor,0) as Gor,
ISNULL(Yetkiler.Duzenle,0) as Duzenle,
ISNULL(Yetkiler.Sil,0) as Sil
From Menuler WITH(NOLOCK)
LEFT OUTER JOIN Kullanicilar WITH(NOLOCK) ON Kullanicilar.ID = @KullaniciID --and UyelikID = @UyelikID
LEFT OUTER JOIN Yetkiler WITH(NOLOCK) ON Yetkiler.MenuID = Menuler.ID and Yetkiler.KullaniciID = Kullanicilar.ID
Order by Menuler.Sira
END
GO
drop proc [p_MailKalibi]
GO

CREATE proc [dbo].[p_MailKalibi](
@ID nvarchar(100)
)
as
BEGIN


select 
* 
from MailKaliplari WITH(NOLOCK)
Where ID = @ID


END

GO
drop proc [p_MailKalibiKaydet]
GO

CREATE proc [dbo].[p_MailKalibiKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@Kod nvarchar(200)=null,	
@Isim nvarchar(max)=null,
@Icerik nvarchar(max)=null,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Eklendi',@Isim,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into MailKaliplari
(ID,UyelikID,Kod,Isim,Icerik,KayitTarihi,KayitYapanKullanici)
Select @ID,@UyelikID,@Kod,@Isim,@Icerik,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Güncellendi',@Isim,GETDATE(),@KullaniciID)

Update MailKaliplari set 
Kod=@Kod,
Isim=@Isim,
Icerik=@Icerik,
DuzenlemeTarihi=GETDATE(),
DuzenlemeYapanKullanici=@KullaniciID
Where ID = @ID and UyelikID = @UyelikID
END

END


GO
drop proc [p_MailKalibiListesi]
GO
CREATE proc [dbo].[p_MailKalibiListesi](
@UyelikID nvarchar(100)='GENEL',
@AranacakKelime nvarchar(max)=''
)
as
BEGIN


	select 
	* 
	from MailKaliplari WITH(NOLOCK)
	Where UyelikID =@UyelikID and (Kod like '%'+@AranacakKelime+'%' or Isim like '%'+@AranacakKelime+'%')
	Order by Isim


END
GO
drop proc [p_MailKalibiSil]
GO
CREATE proc [dbo].[p_MailKalibiSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,(select top(1) Kod From Depolar WITH(NOLOCK) Where ID = @ID),'Kayıt Silindi',(select top(1) Isim From Depolar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From MailKaliplari Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_Mesaj]
GO
create proc [dbo].[p_Mesaj](
@ID nvarchar(100)
)
as
BEGIN


select 
* 
from Mesajlar WITH(NOLOCK)
Where ID = @ID


END
GO
drop proc [p_MesajKaydet]
GO
create proc [dbo].[p_MesajKaydet](
@KullaniciID nvarchar(100),
@KarsiKullaniciID nvarchar(100)=null,	
@Mesaj nvarchar(max)=null,
@Dosya nvarchar(max)=null
)
as
BEGIN

declare @ID nvarchar(100) = NEWID()
Insert Into Mesajlar
(ID,KullaniciID,KarsiKullaniciID,Mesaj,KayitTarihi,Dosya)
Select @ID,@KullaniciID,@KarsiKullaniciID,@Mesaj,GETDATE(),@Dosya

Select @ID

END
GO
drop proc [p_MesajListesi]
GO
create proc [dbo].[p_MesajListesi](
@KullaniciID nvarchar(100)=''
)
as
BEGIN

	select 
	* 
	from Mesajlar WITH(NOLOCK)
	Where (KullaniciID = @KullaniciID or KarsiKullaniciID = @KullaniciID)
	Order by KayitTarihi asc
	
	Update Mesajlar set GorulmeTarihi = GETDATE() Where KarsiKullaniciID = @KullaniciID and GorulmeTarihi IS NULL
END

GO
drop proc [p_MesajSil]
GO
create proc [dbo].[p_MesajSil](
@ID nvarchar(100)='',
@KullaniciID nvarchar(100)
)
as
BEGIN


Delete From Mesajlar Where ID = @ID 

END
GO
drop proc [p_Parametre]
GO
CREATE proc [dbo].[p_Parametre](
@UyelikID nvarchar(100),
@Kod nvarchar(100)
)
as

select 
Parametreler.ID,
@UyelikID as UyelikID,
ParametrelerStandart.Modul,
ParametrelerStandart.Isim,
ISNULL(Parametreler.Deger,ParametrelerStandart.Deger) as Deger,
ParametrelerStandart.Tip,
ParametrelerStandart.Kategori,
ParametrelerStandart.ID as StandartID
from ParametrelerStandart WITH(NOLOCK)
LEFT OUTER JOIN Parametreler WITH(NOLOCK) ON Parametreler.UyelikID = @UyelikID and Parametreler.Modul = ParametrelerStandart.Modul
and Parametreler.Isim = ParametrelerStandart.Isim
Where 1=1
and Parametreler.Modul = @Kod



GO
drop proc [p_ParametreKaydet]
GO
CREATE proc [dbo].[p_ParametreKaydet](
@UyelikID nvarchar(100),
@Modul nvarchar(100),
@Isim nvarchar(100),
@Deger nvarchar(max)
)
as

IF EXISTS(select * from Parametreler WITH(NOLOCK) Where UyelikID = @UyelikID and Modul = @Modul and Isim = @Isim)
BEGIN
	
	Update Parametreler set 
		Deger = @Deger 
	Where UyelikID = @UyelikID and Modul = @Modul and Isim = @Isim 
	
END
ELSE
BEGIN

	Insert Into Parametreler
	(ID,UyelikID,Modul,Isim,Deger)
	values
	(NEWID(),@UyelikID,@Modul,@Isim,@Deger)

END

GO
drop proc [p_Parametreler]
GO
CREATE proc [dbo].[p_Parametreler](
@UyelikID nvarchar(100)
)
as

select 
Parametreler.ID,
@UyelikID as UyelikID,
ParametrelerStandart.Modul,
ParametrelerStandart.Isim,
ISNULL(Parametreler.Deger,ParametrelerStandart.Deger) as Deger,
ParametrelerStandart.Tip,
ParametrelerStandart.Kategori,
ParametrelerStandart.ID as StandartID
from ParametrelerStandart WITH(NOLOCK)
LEFT OUTER JOIN Parametreler WITH(NOLOCK) ON Parametreler.UyelikID = @UyelikID and Parametreler.Modul = ParametrelerStandart.Modul
and Parametreler.Isim = ParametrelerStandart.Isim
Where 1=1

GO
drop proc [p_Personeller]
GO
create proc [dbo].[p_Personeller](
@ID nvarchar(100)
)
as
BEGIN


select 
* 
from Personeller WITH(NOLOCK)
Where ID = @ID


END

GO
drop proc [p_PersonellerKaydet]
GO
	


create proc [dbo].[p_PersonellerKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@Isim nvarchar(100)=null,
@EMail nvarchar(100)=null,	
@Telefon nvarchar(100)=null,	
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Isim,'Kayıt Eklendi',@Isim,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into Personeller
(ID,UyelikID,Isim,EMail,Telefon,KayitTarihi,KayitYapanKullanici)
Select @ID,@UyelikID,@Isim,@EMail,@Telefon,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Isim,'Kayıt Güncellendi',@Isim,GETDATE(),@KullaniciID)

Update Personeller set 
Isim=@Isim,
EMail=@EMail,
Telefon=@Telefon,
DuzenlemeTarihi=GETDATE(),
DuzenlemeYapanKullanici=@KullaniciID
Where ID = @ID and UyelikID = @UyelikID
END

END

GO
drop proc [p_PersonellerListesi]
GO


create proc [dbo].[p_PersonellerListesi](
@UyelikID nvarchar(100)='GENEL',
@AranacakKelime nvarchar(max)=''
)
as
BEGIN


	select 
	* 
	from Personeller WITH(NOLOCK)
	Where UyelikID =@UyelikID and (EMail like '%'+@AranacakKelime+'%' or Isim like '%'+@AranacakKelime+'%')
	Order by Isim


END
GO
drop proc [p_PersonellerSil]
GO
create proc [dbo].[p_PersonellerSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,(select top(1) Isim From Personeller WITH(NOLOCK) Where ID = @ID),'Kayıt Silindi',(select top(1) Isim From Depolar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From Personeller Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_Plasiyer]
GO
CREATE proc [dbo].[p_Plasiyer](
@ID nvarchar(100)
)
as
BEGIN

select 
* 
from Plasiyerler WITH(NOLOCK)
Where ID = @ID

END
GO
drop proc [p_PlasiyerKaydet]
GO
CREATE proc [dbo].[p_PlasiyerKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@Isim nvarchar(250)=null,	
@Aciklama1 nvarchar(max)=null,
@Aciklama2 nvarchar(max)=null,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Plasiyer','Yeni Plasiyer Eklendi',@Isim,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into Plasiyerler
(ID,UyelikID,Isim,Aciklama1,Aciklama2)
Select @ID,@UyelikID,@Isim,@Aciklama1,@Aciklama2

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Plasiyer','Plasiyer Güncellendi',@Isim,GETDATE(),@KullaniciID)
Update Plasiyerler set 
Isim = @Isim,
Aciklama1 = @Aciklama1,
Aciklama2 = @Aciklama2
Where ID = @ID and UyelikID = @UyelikID
END

END
GO
drop proc [p_PlasiyerListesi]
GO
CREATE proc [dbo].[p_PlasiyerListesi](
@UyelikID nvarchar(100)
)
as
BEGIN

select 
* 
from Plasiyerler WITH(NOLOCK)
Where UyelikID = @UyelikID 
Order by Isim

END
GO
drop proc [p_PlasiyerSil]
GO

CREATE proc [dbo].[p_PlasiyerSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Plasiyer','Palasiyer Silnidi',(select top(1) Isim From Plasiyerler WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From Plasiyerler Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_SatinalmaKayitListesi]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaKayitListesi](
@Kategori1 nvarchar(100)='',
@Kategori2 nvarchar(100)='',
@Kategori3 nvarchar(100)='',
@Kullanici nvarchar(100)='',
@Grupla int=1
)
as
BEGIN

declare @Durumu nvarchar(max)=N'Talep Onaylandı'
if @Grupla = 1
BEGIN

Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler WITH(NOLOCK) ON SatinalmaSubeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and SatinalmaSubeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
)

Select
    --ISNULL((select top(1) Dosya From SatinalmaDosyalar WITH(NOLOCK) Where SatinalmaDosyalar.KayitID = ST.ID),'') as Dosya,
	'' Dosya,
	'' as Dosya,
	S.Kategori1,
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Group by 
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Group by Kategori1 order by Kategori1

END
ELSE IF @Grupla = 0
BEGIN

--Teklifler
Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
)

-- Kayıt Listesi
Select
    --ISNULL((select top(1) Dosya From SatinalmaDosyalar WITH(NOLOCK) Where SatinalmaDosyalar.KayitID = ST.ID),'') as Dosya,
	'' Dosya,
	ST.ID,
	S.Kategori1,
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	S.StokKodu,
	S.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Group by 
ST.ID,
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
S.StokKodu,
S.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi
Order By S.StokKodu
-- Açıklama ve Şube Bilgileri
Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Group by KategoriKodu1,Kategori1 
order by KategoriKodu1


END


END

GO
drop proc [p_SatinalmaKayitListesiExcel]
GO

CREATE PROCEDURE [dbo].[p_SatinalmaKayitListesiExcel](
@Kategori1 nvarchar(100)='',
@Kategori2 nvarchar(100)='',
@Kategori3 nvarchar(100)='',
@Kullanici nvarchar(100)='',
@Grupla int=1
)
as
BEGIN

declare @Durumu nvarchar(max)=N'Talep Onaylandı'
if @Grupla = 1
BEGIN

Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
)

Select
    --ISNULL((select top(1) Dosya From Dosyalar WITH(NOLOCK) Where Dosyalar.KayitID = ST.ID),'') as Dosya,
	'' as Dosya,
	S.Kategori1,
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	SUM(CASE WHEN Subeler.KisaKodu = 'SEL' THEN ST.TalepMiktari ELSE null END) as SEL,
    SUM(CASE WHEN Subeler.KisaKodu = 'SEK' THEN ST.TalepMiktari ELSE null END) as SEK,
    SUM(CASE WHEN Subeler.KisaKodu = 'SDR' THEN ST.TalepMiktari ELSE null END) as SDR,
    SUM(CASE WHEN Subeler.KisaKodu = 'TBB' THEN ST.TalepMiktari ELSE null END) as TBB,
    SUM(CASE WHEN Subeler.KisaKodu = 'GKR' THEN ST.TalepMiktari ELSE null END) as GKR,
    SUM(CASE WHEN Subeler.KisaKodu = 'GSR' THEN ST.TalepMiktari ELSE null END) as GSR,
    SUM(CASE WHEN Subeler.KisaKodu = 'SPR' THEN ST.TalepMiktari ELSE null END) as SPR,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	ST.Aciklama1 as Aciklama
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Group by 
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
ST.Aciklama1
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Group by Kategori1 order by Kategori1

END
ELSE IF @Grupla = 0
BEGIN
--Teklifler
Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
)
-- Kayıt Listesi
Select
    --ISNULL((select top(1) Dosya From Dosyalar WITH(NOLOCK) Where Dosyalar.KayitID = ST.ID),'') as Dosya,
	ST.ID,
	S.Kategori1,
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	SUM(CASE WHEN Subeler.KisaKodu = 'SEL' THEN ST.TalepMiktari ELSE null END) as SEL,
    SUM(CASE WHEN Subeler.KisaKodu = 'SEK' THEN ST.TalepMiktari ELSE null END) as SEK,
    SUM(CASE WHEN Subeler.KisaKodu = 'SDR' THEN ST.TalepMiktari ELSE null END) as SDR,
    SUM(CASE WHEN Subeler.KisaKodu = 'TBB' THEN ST.TalepMiktari ELSE null END) as TBB,
    SUM(CASE WHEN Subeler.KisaKodu = 'GKR' THEN ST.TalepMiktari ELSE null END) as GKR,
    SUM(CASE WHEN Subeler.KisaKodu = 'GSR' THEN ST.TalepMiktari ELSE null END) as GSR,
    SUM(CASE WHEN Subeler.KisaKodu = 'SPR' THEN ST.TalepMiktari ELSE null END) as SPR,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	ST.Aciklama1 as Aciklama
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Group by 
ST.ID,
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
ST.Aciklama1
Order By ST.StokKodu
-- Açıklama ve Şube Bilgileri
Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu or ISNULL(ST.Durumu,'') = '')
and Subeler.KisaKodu IS NOT NULL
--and (ST.KayitTarihi <= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) or ST.SifreliGecis = 1)
Group by KategoriKodu1,Kategori1 
order by KategoriKodu1


END


END

GO
drop proc [p_SatinalmaKayitListesiOnay1]
GO

CREATE PROCEDURE [dbo].[p_SatinalmaKayitListesiOnay1] (
@Kategori1 nvarchar(100)='',
@Kategori2 nvarchar(100)='',
@Kategori3 nvarchar(100)='',
@Kullanici nvarchar(100)='',
@Grupla int=1
)
as
BEGIN

declare @Durumu nvarchar(max)=N'Onayda'
if @Grupla = 1
BEGIN

Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi ,ST.Aciklama1 
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID 
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID 
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
-- test
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN ( 
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
)

Select
	'' Dosya,
	S.Kategori1,
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	C.CariAdi as SecilenCariAdi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.SecilenCariID
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
Group by 
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
C.CariAdi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
Group by Kategori1 order by Kategori1
END
ELSE IF @Grupla = 0
BEGIN

Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi ,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
)

Select
    ISNULL((select top(1) Dosya From SatinalmaDosyalar WITH(NOLOCK) Where SatinalmaDosyalar.KayitID = ST.ID),'') as Dosya,
    ST.ID,
	S.Kategori1, 
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	C.CariAdi as SecilenCariAdi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.SecilenCariID
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
Group by 
ST.ID,
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
C.CariAdi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
Group by Kategori1 order by Kategori1

END
END
GO
drop proc [p_SatinalmaKayitListesiOnay2]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaKayitListesiOnay2](
@Kategori1 nvarchar(100)='',
@Kategori2 nvarchar(100)='',
@Kategori3 nvarchar(100)='',
@Kullanici nvarchar(100)='',
@Grupla int=1
)
as
BEGIN

declare @Durumu nvarchar(max)=N'Onay1'

if @Grupla = 1
BEGIN
Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi ,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and (S.Kategori1 = @Kategori1 or @Kategori1 = '')
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
)

Select
	'' Dosya,
	S.Kategori1,
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	C.CariAdi + '('+LEFT(C.CariKodu,3)+')' as SecilenCariAdi,
	ST.BitisNoktasi,
	NULL as Bakiye
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.SecilenCariID
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and (S.Kategori1 = @Kategori1 or @Kategori1 = '')
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by 
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
C.CariAdi,
C.CariKodu,
ST.BitisNoktasi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and (S.Kategori1 = @Kategori1 or @Kategori1 = '')
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by Kategori1 order by Kategori1
END
ELSE IF @Grupla = 0
BEGIN

Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi ,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and (S.Kategori1 = @Kategori1 or @Kategori1 = '')
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
)

Select
    ISNULL((select top(1) Dosya From SatinalmaDosyalar WITH(NOLOCK) Where SatinalmaDosyalar.KayitID = ST.ID),'') as Dosya,
    ST.ID,
	S.Kategori1, 
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	C.CariAdi + '('+LEFT(C.CariKodu,3)+')' as SecilenCariAdi,
	ST.BitisNoktasi,
	ISNULL((
	  Select top(1) w_StokBakiyeleri.Bakiye from w_StokBakiyeleri 
	  Where w_StokBakiyeleri.ID = S.ID
	  and w_StokBakiyeleri.SubeKodu = ST.SubeKodu collate SQL_Latin1_General_CP1_CI_AS
	 ),0) as Bakiye
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.SecilenCariID
Where ST.Silindi = 0 
and (ST.Durumu = @Durumu)
and (S.Kategori1 = @Kategori1 or @Kategori1 = '')
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by 
ST.ID,
ST.SubeKodu,
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
C.CariAdi,
C.CariKodu,
ST.BitisNoktasi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and (S.Kategori1 = @Kategori1 or @Kategori1 = '')
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by Kategori1 order by Kategori1
END
END

GO
drop proc [p_SatinalmaKayitListesiOnay3]
GO

CREATE PROCEDURE [dbo].[p_SatinalmaKayitListesiOnay3](
@Kategori1 nvarchar(100)='',
@Kategori2 nvarchar(100)='',
@Kategori3 nvarchar(100)='',
@Kullanici nvarchar(100)='',
@Grupla int=1
)
as
BEGIN

declare @Durumu nvarchar(max)=N'Onay2'


if @Grupla = 1
BEGIN
Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi ,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
--and ST.SubeKodu IN (
--select Subeler.Kod from SatinalmaYetkilerSube 
--left outer join SatinalmaSubeler ON SatinalmaSubeler.ID = SatinalmaYetkilerSube.MenuID
--where Kullanici = @Kullanici and Yetki = 1
--)
)

Select
	'' Dosya,
	S.Kategori1,
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	C.CariAdi as SecilenCariAdi,
	ST.BitisNoktasi,
	ISNULL((
	  Select top(1) w_StokBakiyeleri.Bakiye from w_StokBakiyeleri 
	  Where w_StokBakiyeleri.ID = S.ID
	  and w_StokBakiyeleri.SubeKodu = ST.SubeKodu collate SQL_Latin1_General_CP1_CI_AS
	 ),0) as Bakiye
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.SecilenCariID
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
--and ST.SubeKodu IN (
--select Subeler.Kod from SatinalmaYetkilerSube 
--left outer join SatinalmaSubeler ON SatinalmaSubeler.ID = SatinalmaYetkilerSube.MenuID
--where Kullanici = @Kullanici and Yetki = 1
--)
Group by 
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.SubeKodu,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
C.CariAdi,
ST.BitisNoktasi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
--and ST.SubeKodu IN (
--select Subeler.Kod from SatinalmaYetkilerSube 
--left outer join SatinalmaSubeler ON SatinalmaSubeler.ID = SatinalmaYetkilerSube.MenuID
--where Kullanici = @Kullanici and Yetki = 1
--)
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
--and ST.SubeKodu IN (
--select Subeler.Kod from SatinalmaYetkilerSube 
--left outer join SatinalmaSubeler ON SatinalmaSubeler.ID = SatinalmaYetkilerSube.MenuID
--where Kullanici = @Kullanici and Yetki = 1
--)
Group by Kategori1 order by Kategori1
END
ELSE IF @Grupla = 0
BEGIN

Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi ,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
--and ST.SubeKodu IN (
--select Subeler.Kod from SatinalmaYetkilerSube 
--left outer join SatinalmaSubeler ON SatinalmaSubeler.ID = SatinalmaYetkilerSube.MenuID
--where Kullanici = @Kullanici and Yetki = 1
--)
)

Select
    --ISNULL((select top(1) Dosya From Dosyalar WITH(NOLOCK) Where Dosyalar.KayitID = ST.ID),'') as Dosya,
	'' Dosya,
    ST.ID,
	S.Kategori1, 
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	C.CariAdi as SecilenCariAdi,
	ST.BitisNoktasi,
	ISNULL((
	  Select top(1) w_StokBakiyeleri.Bakiye from w_StokBakiyeleri 
	  Where w_StokBakiyeleri.ID = S.ID
	  and w_StokBakiyeleri.SubeKodu = ST.SubeKodu collate SQL_Latin1_General_CP1_CI_AS
	 ),0) as Bakiye
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.SecilenCariID
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
--and ST.SubeKodu IN (
--select Subeler.Kod from SatinalmaYetkilerSube 
--left outer join SatinalmaSubeler ON SatinalmaSubeler.ID = SatinalmaYetkilerSube.MenuID
--where Kullanici = @Kullanici and Yetki = 1
--)
Group by 
ST.ID,
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.SubeKodu,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
C.CariAdi,
ST.BitisNoktasi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
--and ST.SubeKodu IN (
--select Subeler.Kod from SatinalmaYetkilerSube 
--left outer join SatinalmaSubeler ON SatinalmaSubeler.ID = SatinalmaYetkilerSube.MenuID
--where Kullanici = @Kullanici and Yetki = 1
--)
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
--and ST.SubeKodu IN (
--select Subeler.Kod from SatinalmaYetkilerSube 
--left outer join SatinalmaSubeler ON SatinalmaSubeler.ID = SatinalmaYetkilerSube.MenuID
--where Kullanici = @Kullanici and Yetki = 1
--)
Group by Kategori1 order by Kategori1

END

END

GO
drop proc [p_SatinalmaKayitListesiOnay4]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaKayitListesiOnay4](
@Kategori1 nvarchar(100)='',
@Kategori2 nvarchar(100)='',
@Kategori3 nvarchar(100)='',
@Kullanici nvarchar(100)='',
@Grupla int=1
)
as
BEGIN

declare @Durumu nvarchar(max)=N'Onay3'
if @Grupla = 1
BEGIN
Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi ,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
)

Select
	'' Dosya,
	S.Kategori1,
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	C.CariAdi as SecilenCariAdi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.SecilenCariID
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by 
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
C.CariAdi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by Kategori1 order by Kategori1
END
ELSE IF @Grupla = 0
BEGIN

Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi ,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
)

Select
    ISNULL((select top(1) Dosya From SatinalmaDosyalar WITH(NOLOCK) Where SatinalmaDosyalar.KayitID = ST.ID),'') as Dosya,
    ST.ID,
	S.Kategori1, 
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	C.CariAdi as SecilenCariAdi,
	ST.BitisNoktasi,
	ISNULL((
	  Select top(1) w_StokBakiyeleri.Bakiye from w_StokBakiyeleri 
	  Where w_StokBakiyeleri.ID = S.ID
	  and w_StokBakiyeleri.SubeKodu = ST.SubeKodu collate SQL_Latin1_General_CP1_CI_AS
	 ),0) as Bakiye
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.SecilenCariID
Where ST.Silindi = 0 
and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by 
ST.ID,
ST.SubeKodu,
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
C.CariAdi,
ST.BitisNoktasi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by Kategori1 order by Kategori1
END
END

GO
drop proc [p_SatinalmaKayitListesiOnay5]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaKayitListesiOnay5](
@Kategori1 nvarchar(100)='',
@Kategori2 nvarchar(100)='',
@Kategori3 nvarchar(100)='',
@Kullanici nvarchar(100)='',
@Grupla int=1
)
as
BEGIN

declare @Durumu nvarchar(max)=N'Onay4'
if @Grupla = 1
BEGIN
Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi ,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
)

Select
	'' Dosya,
	S.Kategori1,
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	C.CariAdi as SecilenCariAdi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.SecilenCariID
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by 
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
C.CariAdi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by Kategori1 order by Kategori1
END
ELSE IF @Grupla = 0
BEGIN

Select 
	ST.ID,ST.CariID,S.StokKodu,LEFT(C.CariAdi,15)+'...' as CariAdi, ST.Fiyat,ST.DovizBirimi ,ST.Aciklama1
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
Where ST.Silindi = 0 and CAST(GETDATE() as DATE) between ST.Baslangic and ST.Bitis
and ST.CariID IS NOT NULL
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and S.ID IN (
Select
	DISTINCT S.ID
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
)

Select
    ISNULL((select top(1) Dosya From SatinalmaDosyalar WITH(NOLOCK) Where SatinalmaDosyalar.KayitID = ST.ID),'') as Dosya,
    ST.ID,
	S.Kategori1, 
	S.Kategori2,
	S.Kategori3,
	S.ID as StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.OlcuBirimi,
	SUM(ST.TalepMiktari) as TalepMiktari,
	ST.SecilenTeklifID,
	ST.SecilenCariID,
	ST.SecilenFiyat,
	ST.SecilenDovizBirimi,
	C.CariAdi as SecilenCariAdi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.SecilenCariID
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by 
ST.ID,
S.Kategori1,
S.Kategori2,
S.Kategori3,
S.ID,
ST.StokKodu,
ST.StokAdi,
ST.OlcuBirimi,
ST.SecilenTeklifID,
ST.SecilenCariID,
ST.SecilenFiyat,
ST.SecilenDovizBirimi,
C.CariAdi
Order By ST.StokKodu

Select
	ST.ID as ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokKodu,
	ST.TalepMiktari,
	ST.Aciklama1,
	ST.Aciklama2,
	ST.Aciklama3
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and S.Kategori1 = @Kategori1
and (S.Kategori2 = @Kategori2 or @Kategori2 = '')
and (S.Kategori3 = @Kategori3 or @Kategori3 = '')
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Order by ST.StokKodu

Select
	Kategori1,count(*) as Miktar
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where ST.Silindi = 0 and (ST.Durumu = @Durumu)
and Subeler.KisaKodu IS NOT NULL
and ST.Durumu <> ST.BitisNoktasi
Group by Kategori1 order by Kategori1
END
END

GO
drop proc [p_SatinalmaTalepDetayi]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaTalepDetayi](
@StokKodu nvarchar(max)=''
)
as
BEGIN
	
	select 
	ST.ID,
	Subeler.KisaKodu as SubeKodu,
	ST.StokID,
	ST.StokKodu,
	ST.StokAdi,
	ST.TalepMiktari,
	ST.OlcuBirimi,
	ST.Aciklama1,
	ST.Aciklama2,
	ISNULL(ST.Aciklama3,'') as Aciklama3,
	ST.KayitTarihi,
	ST.KayitYapanKullanici,
	(Select top(1) Dosya From SatinalmaDosyalar WITH(NOLOCK) Where Modul = 'Talep' and KayitID = ST.ID Order by ID Desc ) DosyaLinki,
	(Select top(1) Isim From SatinalmaDosyalar WITH(NOLOCK) Where Modul = 'Talep' and KayitID = ST.ID Order by ID Desc ) DosyaAdi,
	(Select top(1) ID From SatinalmaDosyalar WITH(NOLOCK) Where Modul = 'Talep' and KayitID = ST.ID Order by ID Desc ) DosyaID
	from SatinalmaTalepleri ST WITH(NOLOCK)
	LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
	Where Silindi = 0 and ST.StokKodu = @StokKodu
	and (Durumu = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')
	and Subeler.KisaKodu IS NOT NULL

END

GO
drop proc [p_SatinalmaTalepDetayiOnay1]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaTalepDetayiOnay1](
@StokKodu nvarchar(max)=''
)
as
BEGIN

select 
ST.ID,
Subeler.KisaKodu as SubeKodu,
ST.StokKodu,
ST.StokAdi,
ST.TalepMiktari,
ST.OlcuBirimi,
ST.Aciklama1,
ST.Aciklama2,
ISNULL(ST.Aciklama3,'') as Aciklama3,
ST.KayitTarihi,
ST.KayitYapanKullanici,

ISNULL((select top(1) LEFT(S2.CariAdi,20) from SatinalmaTalepleri S1 WITH(NOLOCK) LEFT OUTER JOIN w_Cariler S2 WITH(NOLOCK) ON S2.ID = S1.SecilenCariID Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By S1.ID desc),'') as SonAlisCarisi,
(select top(1) DuzenlemeTarihi from SatinalmaTalepleri S1 WITH(NOLOCK) Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By ID desc) as SonAlisTarihi,
ISNULL((select top(1) SecilenFiyat from SatinalmaTalepleri S1 WITH(NOLOCK) Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By ID desc),0) as SonAlisFiyati,
ISNULL((select top(1) S2.KisaKodu from SatinalmaTalepleri S1 WITH(NOLOCK) LEFT OUTER JOIN SatinalmaSubeler S2 WITH(NOLOCK) ON S2.Kod = S1.SubeKodu Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By S1.ID desc),'') as SonAlisSubesi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where Silindi = 0 and ST.StokKodu = @StokKodu
and (Durumu = N'Onayda')
and Subeler.KisaKodu IS NOT NULL

END
GO
drop proc [p_SatinalmaTalepDetayiOnay2]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaTalepDetayiOnay2](
@StokKodu nvarchar(max)=''
)
as
BEGIN

select 
ST.ID,
Subeler.KisaKodu as SubeKodu,
ST.StokKodu,
ST.StokAdi,
ST.TalepMiktari,
ST.OlcuBirimi,
ST.Aciklama1,
ST.Aciklama2,
ISNULL(ST.Aciklama3,'') as Aciklama3,
ST.KayitTarihi,
ST.KayitYapanKullanici,

ISNULL((select top(1) LEFT(S2.CariAdi,20) from SatinalmaTalepleri S1 WITH(NOLOCK) LEFT OUTER JOIN w_Cariler S2 WITH(NOLOCK) ON S2.ID = S1.SecilenCariID Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By S1.ID desc),'') as SonAlisCarisi,
(select top(1) DuzenlemeTarihi from SatinalmaTalepleri S1 WITH(NOLOCK) Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By ID desc) as SonAlisTarihi,
ISNULL((select top(1) SecilenFiyat from SatinalmaTalepleri S1 WITH(NOLOCK) Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By ID desc),0) as SonAlisFiyati,
ISNULL((select top(1) S2.KisaKodu from SatinalmaTalepleri S1 WITH(NOLOCK) LEFT OUTER JOIN SatinalmaSubeler S2 WITH(NOLOCK) ON S2.Kod = S1.SubeKodu Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By S1.ID desc),'') as SonAlisSubesi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where Silindi = 0 and ST.StokKodu = @StokKodu
and (Durumu = N'Onay1')
and Subeler.KisaKodu IS NOT NULL

END
GO
drop proc [p_SatinalmaTalepDetayiOnay3]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaTalepDetayiOnay3](
@StokKodu nvarchar(max)=''
)
as
BEGIN

select 
ST.ID,
Subeler.KisaKodu as SubeKodu,
ST.StokKodu,
ST.StokAdi,
ST.TalepMiktari,
ST.OlcuBirimi,
ST.Aciklama1,
ST.Aciklama2,
ISNULL(ST.Aciklama3,'') as Aciklama3,
ST.KayitTarihi,
ST.KayitYapanKullanici,

ISNULL((select top(1) LEFT(S2.CariAdi,20) from SatinalmaTalepleri S1 WITH(NOLOCK) LEFT OUTER JOIN w_Cariler S2 WITH(NOLOCK) ON S2.ID = S1.SecilenCariID Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By S1.ID desc),'') as SonAlisCarisi,
(select top(1) DuzenlemeTarihi from SatinalmaTalepleri S1 WITH(NOLOCK) Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By ID desc) as SonAlisTarihi,
ISNULL((select top(1) SecilenFiyat from SatinalmaTalepleri S1 WITH(NOLOCK) Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By ID desc),0) as SonAlisFiyati,
ISNULL((select top(1) S2.KisaKodu from SatinalmaTalepleri S1 WITH(NOLOCK) LEFT OUTER JOIN SatinalmaSubeler S2 WITH(NOLOCK) ON S2.Kod = S1.SubeKodu Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By S1.ID desc),'') as SonAlisSubesi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where Silindi = 0 and ST.StokKodu = @StokKodu
and (Durumu = N'Onay2')
and Subeler.KisaKodu IS NOT NULL

END
GO
drop proc [p_SatinalmaTalepDetayiOnay4]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaTalepDetayiOnay4](
@StokKodu nvarchar(max)=''
)
as
BEGIN

select 
ST.ID,
Subeler.KisaKodu as SubeKodu,
ST.StokKodu,
ST.StokAdi,
ST.TalepMiktari,
ST.OlcuBirimi,
ST.Aciklama1,
ST.Aciklama2,
ISNULL(ST.Aciklama3,'') as Aciklama3,
ST.KayitTarihi,
ST.KayitYapanKullanici,

ISNULL((select top(1) LEFT(S2.CariAdi,20) from SatinalmaTalepleri S1 WITH(NOLOCK) LEFT OUTER JOIN w_Cariler S2 WITH(NOLOCK) ON S2.ID = S1.SecilenCariID Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By S1.ID desc),'') as SonAlisCarisi,
(select top(1) DuzenlemeTarihi from SatinalmaTalepleri S1 WITH(NOLOCK) Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By ID desc) as SonAlisTarihi,
ISNULL((select top(1) SecilenFiyat from SatinalmaTalepleri S1 WITH(NOLOCK) Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By ID desc),0) as SonAlisFiyati,
ISNULL((select top(1) S2.KisaKodu from SatinalmaTalepleri S1 WITH(NOLOCK) LEFT OUTER JOIN SatinalmaSubeler S2 WITH(NOLOCK) ON S2.Kod = S1.SubeKodu Where S1.Aktarildi = 1 and S1.StokKodu = ST.StokKodu Order By S1.ID desc),'') as SonAlisSubesi
from SatinalmaTalepleri ST WITH(NOLOCK)
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = ST.SubeKodu
Where Silindi = 0 and ST.StokKodu = @StokKodu
and (Durumu = N'Onay3')
and Subeler.KisaKodu IS NOT NULL

END
GO
drop proc [p_SatinalmaTeklifKaldir]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaTeklifKaldir](
@Grupla int=1,
@KayitID int = 0,
@StokKodu nvarchar(max)='',
@SecilenTeklifID int=null,
@SecilenCariID nvarchar(100)='',
@SecilenFiyat decimal(18,8)=0,
@SecilenDovizBirimi nvarchar(100)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN
	
	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,CariID,Fiyat,DovizBirimi,Aciklama1,Aciklama2,Aciklama3,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,N'Satınalma',N'Fiyat Kaldırımı',@SecilenCariID,@SecilenFiyat,@SecilenDovizBirimi,'','','',GETDATE(),@Kullanici 
	From SatinalmaTalepleri WITH(NOLOCK) 
	Where Silindi = 0 and SatinalmaTalepleri.StokKodu = @StokKodu and ID = CASE WHEN @Grupla = 1 THEN ID ELSE @KayitID END
	and (Durumu = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')

	Update SatinalmaTalepleri Set
		SecilenTeklifID = null,
		SecilenCariID = null,
		SecilenFiyat = null,
		SecilenDovizBirimi = null
	Where Silindi = 0 and SatinalmaTalepleri.StokKodu = @StokKodu and ID = CASE WHEN @Grupla = 1 THEN ID ELSE @KayitID END
	and (Durumu = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')

END

GO
drop proc [p_SatinalmaTeklifKaldirSatinalma]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaTeklifKaldirSatinalma](
@Grupla int=1,
@KayitID int = 0,
@StokKodu nvarchar(max)='',
@SecilenTeklifID int=null,
@SecilenCariID nvarchar(100)='',
@SecilenFiyat decimal(18,8)=0,
@SecilenDovizBirimi nvarchar(100)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN
	
	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,CariID,Fiyat,DovizBirimi,Aciklama1,Aciklama2,Aciklama3,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,N'Satınalma',N'Fiyat Kaldırımı',@SecilenCariID,@SecilenFiyat,@SecilenDovizBirimi,'','','',GETDATE(),@Kullanici 
	From SatinalmaTalepleri WITH(NOLOCK) 
	Where Silindi = 0 and SatinalmaTalepleri.StokKodu = @StokKodu and ID = CASE WHEN @Grupla = 1 THEN ID ELSE @KayitID END
	and (Durumu = N'Onay1')

	Update SatinalmaTalepleri Set
		SecilenTeklifID = null,
		SecilenCariID = null,
		SecilenFiyat = null,
		SecilenDovizBirimi = null
	Where Silindi = 0 and SatinalmaTalepleri.StokKodu = @StokKodu and ID = CASE WHEN @Grupla = 1 THEN ID ELSE @KayitID END
	and (Durumu = N'Onay1')

END

GO
drop proc [p_SatinalmaTeklifler]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaTeklifler](
@Cari nvarchar(100)='',
@Stok nvarchar(100)=''
)
as
BEGIN

select 
ST.*,
S.StokKodu,
S.StokAdi,
C.CariKodu,
C.CariAdi
from SatinalmaTeklifleri ST WITH(NOLOCK) 
LEFT OUTER JOIN w_SatinalmaStoklar S ON S.ID = ST.StokID
LEFT OUTER JOIN w_Cariler C ON C.ID = ST.CariID
Where ST.Silindi = 0
and S.StokAdi like '%'+@Stok+'%'
and C.CariAdi like '%'+@Cari+'%'

END
GO
drop proc [p_SatinalmaTeklifSec]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaTeklifSec](
@Grupla int=1,
@KayitID int = 0,
@StokKodu nvarchar(max)='',
@SecilenTeklifID int=null,
@SecilenCariID nvarchar(100)='',
@SecilenFiyat decimal(18,8)=0,
@SecilenDovizBirimi nvarchar(100)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN
	
	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,CariID,Fiyat,DovizBirimi,Aciklama1,Aciklama2,Aciklama3,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,N'Satınalma',N'Fiyat Seçimi',@SecilenCariID,@SecilenFiyat,@SecilenDovizBirimi,'','','',GETDATE(),@Kullanici 
	From SatinalmaTalepleri WITH(NOLOCK) 
	Where Silindi = 0 and SatinalmaTalepleri.StokKodu = @StokKodu and ID = CASE WHEN @Grupla = 1 THEN ID ELSE @KayitID END
	and (Durumu = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')

	Update SatinalmaTalepleri Set
		SecilenTeklifID = @SecilenTeklifID,
		SecilenCariID = @SecilenCariID,
		SecilenFiyat = @SecilenFiyat,
		SecilenDovizBirimi = @SecilenDovizBirimi,
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici
	Where Silindi = 0 and SatinalmaTalepleri.StokKodu = @StokKodu and ID = CASE WHEN @Grupla = 1 THEN ID ELSE @KayitID END
	and (ISNULL(Durumu,'') = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')

END
GO
drop proc [p_SatinalmaTeklifSecSatinalma]
GO
CREATE PROCEDURE [dbo].[p_SatinalmaTeklifSecSatinalma](
@Grupla int=1,
@KayitID int = 0,
@StokKodu nvarchar(max)='',
@SecilenTeklifID int=null,
@SecilenCariID nvarchar(100)='',
@SecilenFiyat decimal(18,8)=0,
@SecilenDovizBirimi nvarchar(100)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN
	
	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,CariID,Fiyat,DovizBirimi,Aciklama1,Aciklama2,Aciklama3,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,N'Satınalma',N'Fiyat Seçimi',@SecilenCariID,@SecilenFiyat,@SecilenDovizBirimi,'','','',GETDATE(),@Kullanici 
	From SatinalmaTalepleri WITH(NOLOCK) 
	Where Silindi = 0 and SatinalmaTalepleri.StokKodu = @StokKodu and ID = CASE WHEN @Grupla = 1 THEN ID ELSE @KayitID END
	and Durumu = N'Onayda'

	Update SatinalmaTalepleri Set
		SecilenTeklifID = @SecilenTeklifID,
		SecilenCariID = @SecilenCariID,
		SecilenFiyat = @SecilenFiyat,
		SecilenDovizBirimi = @SecilenDovizBirimi,
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici
	Where Silindi = 0 and SatinalmaTalepleri.StokKodu = @StokKodu and ID = CASE WHEN @Grupla = 1 THEN ID ELSE @KayitID END
	and (ISNULL(Durumu,'') = N'Onayda')

END
GO
drop proc [p_SiparisKaydetERP]
GO
CREATE PROCEDURE [dbo].[p_SiparisKaydetERP](
@ID int = 0
)
as
BEGIN

IF @ID <> 0
BEGIN

declare @FiyatAciklamalari2 nvarchar(max)='<table border="1" style="padding:2px;border-collapse: collapse;font-size:10px;"><tr><td><strong>İsim</strong></td><td><strong>Fiyat Açıklama</strong></td></tr>'

SELECT top(10) @FiyatAciklamalari2 += '<tr><td>'+
SH.StokAdi+'</td><td>'+
ISNULL((
	select top(1)
	F.Aciklama1 
	from SatinalmaTeklifleri F WITH(NOLOCK) 
	Where F.CariID = SH.SecilenCariID 
	and F.StokID = SH.StokID 
	and F.Fiyat = SH.SecilenFiyat 
	and F.Silindi = 0
	and CAST(GETDATE() as DATE) between F.Baslangic and F.Bitis
),'') 
+'</td></tr>'
FROM SatinalmaTalepleri SH WITH(NOLOCK)
LEFT OUTER JOIN w_Cariler ON w_Cariler.ID = SH.SecilenCariID
LEFT OUTER JOIN w_SatinalmaStoklar ON w_SatinalmaStoklar.ID = SH.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = SH.SubeKodu
Where SH.Silindi = 0 and Durumu = BitisNoktasi --and SH.ID = @ID
and ISNULL((
	select top(1)
	F.Aciklama1 
	from SatinalmaTeklifleri F WITH(NOLOCK) 
	Where F.CariID = SH.SecilenCariID 
	and F.StokID = SH.StokID 
	and F.Fiyat = SH.SecilenFiyat 
	and F.Silindi = 0
	and CAST(GETDATE() as DATE) between F.Baslangic and F.Bitis
),'')  <> ''
set @FiyatAciklamalari2 +='</table>'

SELECT 
AktarimNo  SiparisNo,
GETDATE() Tarih,
w_Cariler.CariKodu CariKodu,
w_Cariler.CariAdi CariAdi,
'' CariTelefon,
'' CariEmail,
w_SatinalmaStoklar.StokKodu StokKodu,
w_SatinalmaStoklar.StokAdi as StokAdi,
w_SatinalmaStoklar.OlcuBirimi OlcuBirimi,
SH.TalepMiktari Miktar,
SH.SecilenFiyat Fiyat,
SH.TalepMiktari*SH.SecilenFiyat Tutar,
 ((SH.TalepMiktari*SH.SecilenFiyat) * (1+w_SatinalmaStoklar.Kdv/100)) - (SH.TalepMiktari*SH.SecilenFiyat) Kdv,
(SH.TalepMiktari*SH.SecilenFiyat
+
 ((SH.TalepMiktari*SH.SecilenFiyat) * (1+w_SatinalmaStoklar.Kdv/100)) - (SH.TalepMiktari*SH.SecilenFiyat) )  AS GenelToplam,
Subeler.Isim as SubeAdi,
Subeler.Isim SubeAdres,
'' as EKALAN,
SH.Aciklama1 as Aciklama,
@FiyatAciklamalari2 FiyatAciklamasi
FROM SatinalmaTalepleri SH WITH(NOLOCK)
LEFT OUTER JOIN w_Cariler ON w_Cariler.ID = SH.SecilenCariID
LEFT OUTER JOIN w_SatinalmaStoklar ON w_SatinalmaStoklar.ID = SH.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = SH.SubeKodu
Where SH.ID = @ID


END
ELSE
BEGIN


IF OBJECT_ID('tempdb..#IDGeciciSepet1') IS NOT NULL
    drop proc #IDGeciciSepet1

--ALTER TABLE #IDGeciciSepet1(
--	 [NO] int
--)

declare @CariID nvarchar(100)
declare @SubeKodu int
declare @SubeKoduEski nvarchar(max)
select 
	top(1) @CariID = SecilenCariID,@SubeKodu = SatinalmaTalepleri.SubeKodu, @SubeKoduEski= Subeler.KisaKodu 
from SatinalmaTalepleri WITH(NOLOCK) 
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = SatinalmaTalepleri.SubeKodu
Where Silindi = 0 and (BitisNoktasi = Durumu or Durumu = 'Onay5') and ISNULL(Aktarildi,'') = 0 

Insert Into #IDGeciciSepet1 ([NO])
select 
ID 
from SatinalmaTalepleri WITH(NOLOCK) 
Where Silindi = 0 and (BitisNoktasi = Durumu or Durumu = 'Onay5') and ISNULL(Aktarildi,'') = 0 and SecilenCariID = @CariID and SubeKodu = @SubeKodu
 
if EXISTS(select * from #IDGeciciSepet1)
BEGIN

Update ID_Sayaclar set BelgeNo = CAST(BelgeNo as int) + 1 Where Tip = 'SatinalmaNo'
declare @SiparisNo nvarchar(max)= CAST(@SubeKoduEski as nvarchar(max))+(select RIGHT('0000000000'+CAST(BelgeNo as nvarchar(max)),7) from ID_Sayaclar WITH(NOLOCK) Where Tip = 'SatinalmaNo')
declare @DepoKodu nvarchar(max)='100'

Insert Into SednaSherwoodMhs.dbo.DemandOwner
(Depot,Remark,PurchaseSecCode,DemandDepot,NonStock,Company,Offer,[Type],Returned,OwnerRemark,AdvLock,RecordUser,RecordDate,DepotId,DemandDepotId,CompanyId,[Status],MailStatus)
Select 
@DepoKodu as Depot,
'' as Remark,
4 as PurchaseSecCode,
100 as DemandDepot,
0 as NonStock,
@SubeKodu as Company,
0 as Offer,
0 as [Type],
0 as Returned,
'YK YAZILIM' as OwnerRemark,
0 as AdvLock,
'YK' as RecordUser,
GETDATE() RecordDate,
2 as DepotId,
1 as DemandDepotId,
2 as CompanyId,
0 as [Status],
-2 as MailStatus

declare @SonID int = (select SCOPE_IDENTITY())

Insert Into SednaSherwoodMhs.dbo.PurchaseTrans
(DemandOwnerId,DemandDate,DemandNumber,OfferDate,OfferNumber,StockId,Unit,quantity,Price1,Discount1,Check1,OrderingDate1,
Amount1,Condition1,Remark,InvoiceId,OrderCheck,MatchInvoiceId,ApprovalTracing,DeliveryDate,VatRatio1,DepotQuantity,
DisAmount1_1,AdvCurrentId,AdvPrice,AdvAmount,LastPrice,PayType,DepositQuantity,NetPrice1,LastDate,LastCurrentId,
LastQuantity,CurrentId1,OrderingNumber1)
Select
@SonID as DemandOwnerId,
CAST(KayitTarihi as DATE) as DemandDate,
SatinalmaTalepleri.ID as DemandNumber,
CAST(GETDATE() as DATE) as OfferDate,
@SiparisNo as OfferNumber,
StokID as StockId,
w_SatinalmaStoklar.BirimID as Unit,
SatinalmaTalepleri.TalepMiktari as quantity,
SecilenFiyat as Price1,
0 as Discount1,
1 as Check1,
CAST(GETDATE() as DATE) as OrderingDate1,
SecilenFiyat*SatinalmaTalepleri.TalepMiktari as Amount1,
0 as Condition1,
'YK YAZILIM' as Remark,
0 as InvoiceId,
1 as OrderCheck,
0 as MatchInvoiceId,
1111 as ApprovalTracing,
CAST(GETDATE() as DATE) as DeliveryDate,
1 as VatRatio1,
SatinalmaTalepleri.TalepMiktari as DepotQuantity,
0 as DisAmount1_1,
3575 as AdvCurrentId,
1 as AdvPrice,
1 as AdvAmount,
SecilenFiyat LastPrice,
'' PayType,
0 DepositQuantity,
1 NetPrice1,
CAST(GETDATE() as DATE) as LastDate,
4181 as LastCurrentId,
SatinalmaTalepleri.TalepMiktari LastQuantity,
SecilenCariID,
@SiparisNo
from IDYAZILIM.dbo.SatinalmaTalepleri
LEFT OUTER JOIN IDYAZILIM.dbo.[w_SatinalmaStoklar] ON [w_SatinalmaStoklar].ID = SatinalmaTalepleri.StokID
where SatinalmaTalepleri.ID IN (select [NO] from #IDGeciciSepet1)


Update SatinalmaTalepleri set Aktarildi = 1,AktarimNo = @SiparisNo Where ID IN (select [NO] from #IDGeciciSepet1)


declare @FiyatAciklamalari nvarchar(max)='<table border="1" style="padding:2px;border-collapse: collapse;font-size:10px;"><tr><td><strong>İsim</strong></td><td><strong>Fiyat Açıklama 1</strong></td><td><strong>Fiyat Açıklama 2</strong></td><td><strong>Fiyat Açıklama 3</strong></td></tr>'

SELECT top(10) @FiyatAciklamalari += '<tr><td>'+
SH.StokAdi+'</td><td>'+
ISNULL((
	select top(1)
	F.Aciklama1 
	from SatinalmaTeklifleri F WITH(NOLOCK) 
	Where F.CariID = SH.SecilenCariID 
	and F.StokID = SH.StokID 
	and F.Fiyat = SH.SecilenFiyat 
	and F.Silindi = 0
	and CAST(GETDATE() as DATE) between F.Baslangic and F.Bitis
),'') 
+'</td><td>'+
ISNULL((
	select top(1)
	F.Aciklama2 
	from SatinalmaTeklifleri F WITH(NOLOCK) 
	Where F.CariID = SH.SecilenCariID 
	and F.StokID = SH.StokID 
	and F.Fiyat = SH.SecilenFiyat 
	and F.Silindi = 0
	and CAST(GETDATE() as DATE) between F.Baslangic and F.Bitis
),'') 
+'</td><td>'+
ISNULL((
	select top(1)
	F.Aciklama3 
	from SatinalmaTeklifleri F WITH(NOLOCK) 
	Where F.CariID = SH.SecilenCariID 
	and F.StokID = SH.StokID 
	and F.Fiyat = SH.SecilenFiyat 
	and F.Silindi = 0
	and CAST(GETDATE() as DATE) between F.Baslangic and F.Bitis
),'') 
+'</td></tr>'
FROM SatinalmaTalepleri SH WITH(NOLOCK)
LEFT OUTER JOIN w_Cariler ON w_Cariler.ID = SH.SecilenCariID
LEFT OUTER JOIN w_SatinalmaStoklar ON w_SatinalmaStoklar.ID = SH.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = SH.SubeKodu
Where SH.Silindi = 0 and Durumu = BitisNoktasi --and SH.ID = @ID
and SH.ID IN (select [NO] from #IDGeciciSepet1)
and ISNULL((
	select top(1)
	F.Aciklama1 
	from SatinalmaTeklifleri F WITH(NOLOCK) 
	Where F.CariID = SH.SecilenCariID 
	and F.StokID = SH.StokID 
	and F.Fiyat = SH.SecilenFiyat 
	and F.Silindi = 0
	and CAST(GETDATE() as DATE) between F.Baslangic and F.Bitis
),'')  <> ''
set @FiyatAciklamalari +='</table>'


SELECT 
AktarimNo  SiparisNo,
GETDATE() Tarih,
w_Cariler.CariKodu CariKodu,
w_Cariler.CariAdi CariAdi,
'' CariTelefon,
'' CariEmail,
w_SatinalmaStoklar.StokKodu StokKodu,
w_SatinalmaStoklar.StokAdi as StokAdi,
w_SatinalmaStoklar.OlcuBirimi OlcuBirimi,
SH.TalepMiktari Miktar,
SH.SecilenFiyat Fiyat,
SH.TalepMiktari*SH.SecilenFiyat Tutar,
 ((SH.TalepMiktari*SH.SecilenFiyat) * (1+w_SatinalmaStoklar.Kdv/100)) - (SH.TalepMiktari*SH.SecilenFiyat) Kdv,
(SH.TalepMiktari*SH.SecilenFiyat
+
 ((SH.TalepMiktari*SH.SecilenFiyat) * (1+w_SatinalmaStoklar.Kdv/100)) - (SH.TalepMiktari*SH.SecilenFiyat) )  AS GenelToplam,
Subeler.Isim as SubeAdi,
Subeler.Isim SubeAdres,
'' as EKALAN,
SH.Aciklama1 as Aciklama,
@FiyatAciklamalari FiyatAciklamasi
FROM SatinalmaTalepleri SH WITH(NOLOCK)
LEFT OUTER JOIN w_Cariler ON w_Cariler.ID = SH.SecilenCariID
LEFT OUTER JOIN w_SatinalmaStoklar ON w_SatinalmaStoklar.ID = SH.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = SH.SubeKodu
Where SH.ID IN (select [NO] from #IDGeciciSepet1)



END
ELSE
BEGIN
	Select 0 Where 1=2
END

END

END
GO
drop proc [p_SonHareketler]
GO
CREATE proc [dbo].[p_SonHareketler](
@UyelikID nvarchar(max)
)
as
BEGIN

select top(100)
Loglar.KayitTarihi as Tarih,
Loglar.Modul,
Loglar.Aciklama1,
Loglar.Aciklama2,
Kullanicilar.Ad +' '+Kullanicilar.Soyad as Kullanici
from Loglar WITH(NOLOCK)
LEFT OUTER JOIN Kullanicilar WITH(NOLOCK) ON Loglar.Kullanici = CAST(Kullanicilar.ID as nvarchar(100))
Where 1=1
and Loglar.UyelikID = @UyelikID
--and Kullanicilar.ID IS NOT NULL
and Loglar.Aciklama2 <> 'Yunus KÖSE'
and Loglar.KayitTarihi >= '2024-05-04' 
order by Loglar.KayitTarihi desc

END
GO
drop proc [p_StokBul]
GO
CREATE proc [dbo].[p_StokBul](
@Barkod nvarchar(100)='',
@UyelikID nvarchar(100)
)
as
BEGIN
select 
Stoklar.*,
G1.Deger as GrupKodu1,
G2.Deger as GrupKodu2,
G3.Deger as GrupKodu3,
G4.Deger as GrupKodu4,
G5.Deger as GrupKodu5,
G6.Deger as GrupKodu6,
C1.Isim as AnaStok
from Stoklar WITH(NOLOCK)
LEFT OUTER JOIN GrupKodlari G1 WITH(NOLOCK) ON G1.ID = Stoklar.GrupKodu1ID
LEFT OUTER JOIN GrupKodlari G2 WITH(NOLOCK) ON G2.ID = Stoklar.GrupKodu2ID
LEFT OUTER JOIN GrupKodlari G3 WITH(NOLOCK) ON G3.ID = Stoklar.GrupKodu3ID
LEFT OUTER JOIN GrupKodlari G4 WITH(NOLOCK) ON G4.ID = Stoklar.GrupKodu4ID
LEFT OUTER JOIN GrupKodlari G5 WITH(NOLOCK) ON G5.ID = Stoklar.GrupKodu5ID
LEFT OUTER JOIN GrupKodlari G6 WITH(NOLOCK) ON G6.ID = Stoklar.GrupKodu6ID
LEFT OUTER JOIN Stoklar C1 WITH(NOLOCK) ON C1.ID = Stoklar.AnaStokID
Where ISNULL(Stoklar.Silindi,0) = 0 and  Stoklar.UyelikID = @UyelikID and Stoklar.ID = @Barkod

END
GO
drop proc [p_Stok]
GO
CREATE proc [dbo].[p_Stok](
@ID nvarchar(100)='',
@UyelikID nvarchar(100)
)
as
BEGIN
IF @ID = ''
BEGIN
	return;
END
Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok','Stok Detaya Girildi',(select top(1) Isim from Stoklar WITH(NOLOCK) Where ID = @ID),GETDATE(),NULL)

select 
Stoklar.*,
G1.Deger as GrupKodu1,
G2.Deger as GrupKodu2,
G3.Deger as GrupKodu3,
G4.Deger as GrupKodu4,
G5.Deger as GrupKodu5,
G6.Deger as GrupKodu6,
C1.Isim as AnaStok
from Stoklar WITH(NOLOCK)
LEFT OUTER JOIN GrupKodlari G1 WITH(NOLOCK) ON G1.ID = Stoklar.GrupKodu1ID
LEFT OUTER JOIN GrupKodlari G2 WITH(NOLOCK) ON G2.ID = Stoklar.GrupKodu2ID
LEFT OUTER JOIN GrupKodlari G3 WITH(NOLOCK) ON G3.ID = Stoklar.GrupKodu3ID
LEFT OUTER JOIN GrupKodlari G4 WITH(NOLOCK) ON G4.ID = Stoklar.GrupKodu4ID
LEFT OUTER JOIN GrupKodlari G5 WITH(NOLOCK) ON G5.ID = Stoklar.GrupKodu5ID
LEFT OUTER JOIN GrupKodlari G6 WITH(NOLOCK) ON G6.ID = Stoklar.GrupKodu6ID
LEFT OUTER JOIN Stoklar C1 WITH(NOLOCK) ON C1.ID = Stoklar.AnaStokID
Where ISNULL(Stoklar.Silindi,0) = 0 and  Stoklar.UyelikID = @UyelikID and Stoklar.ID = @ID

END
GO
drop proc [p_StokFiyat]
GO
CREATE proc [dbo].[p_StokFiyat](
@ID nvarchar(100),
@UyelikID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok Fiyat','Stok Fiyat Detayı','',GETDATE(),NULL)

select 
* 
from StokFiyatlari WITH(NOLOCK)
Where UyelikID = @UyelikID and ID = @ID

END
GO
drop proc [p_StokFiyatKaydet]
GO
CREATE proc [dbo].[p_StokFiyatKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@StokID nvarchar(100)=null,
@CariID nvarchar(max)=null,	
@FiyatGrubu nvarchar(250),
@Tip nvarchar(10),
@Fiyat decimal(18,8),
@BaslangicTarihi datetime,
@BitisTarihi datetime,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok Fiyat','Stok Fiyat Eklendi',@Fiyat,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into StokFiyatlari
(ID,UyelikID,StokID,CariID,FiyatGrubu,Tip,Fiyat,BaslangicTarihi,BitisTarihi,KayitTarihi,KayitYapanKullanici)
Select @ID,@UyelikID,@StokID,@CariID,@FiyatGrubu,@Tip,@Fiyat,@BaslangicTarihi,@BitisTarihi,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok Fiyat','Stok Fiyat Güncellendi',@Fiyat,GETDATE(),@KullaniciID)

Update StokFiyatlari set 
CariID=@CariID,
FiyatGrubu=@FiyatGrubu,
Tip = @Tip,
Fiyat = @Fiyat,
BaslangicTarihi=@BaslangicTarihi,
BitisTarihi=@BitisTarihi,
DuzenlemeTarihi = GETDATE(),
DuzenlemeYapanKullanici = @KullaniciID
Where ID = @ID and UyelikID = @UyelikID and StokID = @StokID
END

END
GO
drop proc [p_StokFiyatListesi]
GO
CREATE proc [dbo].[p_StokFiyatListesi](
@UyelikID nvarchar(100),
@StokID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok Fiyat','Stok Fiyat Listesi','',GETDATE(),NULL)

select 
* 
from StokFiyatlari WITH(NOLOCK)
Where Silindi = 0 and UyelikID = @UyelikID and StokID = @StokID
Order by KayitTarihi desc

END
GO
drop proc [p_StokFiyatSil]
GO
CREATE proc [dbo].[p_StokFiyatSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@StokID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok Fiyat','Stok Fiyat Silindi',(select top(1) Aciklama from StokNotlar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)


Update StokFiyatlari set Silindi = 1,SilinenTarih=GETDATE(),SilenKullanici=@KullaniciID Where ID = @ID and StokID = @StokID

END


GO
drop proc [p_StokHareketi]
GO

CREATE proc [dbo].[p_StokHareketi](
@UyelikID nvarchar(100),
@StokID nvarchar(max),
@ID nvarchar(max)
)
as
BEGIN

Select
*
From StokHareketleri WITH(NOLOCK)
Where Silindi = 0
and UyelikID = @UyelikID
and StokID = @StokID
and ID = @ID
Order by Tarih asc
END

GO
drop proc [p_StokHareketiKaydet]
GO

CREATE proc [dbo].[p_StokHareketiKaydet](
@UyelikID nvarchar(100),
@ID nvarchar(100),
@StokID nvarchar(100),
@Tarih datetime,
@VadeTarihi datetime,
@BelgeNo nvarchar(max),
@HareketTipi nvarchar(max),
@GC nvarchar(10),
@Miktar decimal(18,8),
@Tutar decimal(18,8),
@DovizTipi nvarchar(100),
@Kur decimal(18,8),
@DovizTutar decimal(18,8),
@PlasiyerID nvarchar(100),
@BaglantiID nvarchar(100),
@Baglanti nvarchar(100),
@GrupKodu1ID nvarchar(100),
@GrupKodu2ID nvarchar(100),
@Aciklama nvarchar(500),
@Kullanici nvarchar(max)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok','Stok Hareket Kaydı',(select top(1) Isim from Stoklar WITH(NOLOCK) Where ID = @StokID),GETDATE(),NULL)

IF @ID = ''
BEGIN
	declare @YeniID nvarchar(100) = NEWID()
	Insert Into StokHareketleri
	(ID,UyelikID,StokID,Tarih,VadeTarihi,BelgeNo,HareketTipi,
	GC,Miktar,Tutar,DovizTipi,Kur,DovizTutar,Aciklama,
	PlasiyerID,
	BaglantiID,
	Baglanti,
	GrupKodu1ID,
	GrupKodu2ID,
	Silindi,KayitTarihi,KayitYapanKullanici)
	values
	(@YeniID,@UyelikID,@StokID,@Tarih,@VadeTarihi,@BelgeNo,@HareketTipi,
	@GC,@Miktar, @Tutar,@DovizTipi,@Kur,@DovizTutar,@Aciklama,
	CASE WHEN @PlasiyerID = '' THEN NULL ELSE @PlasiyerID END,
	CASE WHEN @BaglantiID = '' THEN NULL ELSE @BaglantiID END,
	@Baglanti,
	CASE WHEN @GrupKodu1ID = '' THEN NULL ELSE @GrupKodu1ID END,
	CASE WHEN @GrupKodu2ID = '' THEN NULL ELSE @GrupKodu2ID END,
	0,GETDATE(),@Kullanici)
	Select @YeniID as ID,'Kayıt başarılı.' Bilgi
END
ELSE
BEGIN
	Update StokHareketleri set 
		Tarih = @Tarih,
		VadeTarihi = @VadeTarihi,
		BelgeNo = @BelgeNo,
		HareketTipi = @HareketTipi,
		GC = @GC,
		Miktar=@Miktar,
		Tutar = @Tutar,
		DovizTipi = @DovizTipi,
		Kur = @Kur,
		DovizTutar = @DovizTutar,
		Aciklama=@Aciklama,
		PlasiyerID = CASE WHEN @PlasiyerID = '' THEN NULL ELSE @PlasiyerID END,
		BaglantiID = CASE WHEN @BaglantiID = '' THEN NULL ELSE @BaglantiID END,
		Baglanti = @Baglanti,
		GrupKodu1ID = CASE WHEN @GrupKodu1ID = '' THEN NULL ELSE @GrupKodu1ID END,
		GrupKodu2ID = CASE WHEN @GrupKodu2ID = '' THEN NULL ELSE @GrupKodu2ID END,
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici
	Where ID = @ID and UyelikID = @UyelikID
	Select @ID as ID,'Kayıt başarılı.' Bilgi

END



ENd
GO
drop proc [p_StokHareketiSil]
GO

CREATE proc [dbo].[p_StokHareketiSil](
@UyelikID nvarchar(100),
@StokID nvarchar(max),
@ID nvarchar(max),
@Kullanici nvarchar(max)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok','Stok Hareket Silindi.',(select top(1) Isim from Stoklar WITH(NOLOCK) Where ID = @StokID),GETDATE(),NULL)

	Update StokHareketleri set 
		Silindi = 1,
		SilinenTarih = GETDATE(),
		SilenKullanici = @Kullanici
	Where UyelikID = @UyelikID and StokID = @StokID and ID = @ID
END

GO
drop proc [p_StokHareketListesi]
GO
CREATE proc [dbo].[p_StokHareketListesi](
@UyelikID nvarchar(100),
@StokID nvarchar(max),
@BaslangicTarihi datetime=null,
@BitisTarihi datetime=null
)
as
BEGIN

	Select
	*
	From StokHareketleri WITH(NOLOCK)
	Where Silindi = 0
	and UyelikID = @UyelikID and StokID = @StokID
	--and Tarih between @BaslangicTarihi and @BitisTarihi
	Order by Tarih asc
END

GO
drop proc [p_StokKaydet]
GO
CREATE proc [dbo].[p_StokKaydet](
@ID nvarchar(100)='',
@UyelikID nvarchar(100)=null,
@Durumu bit,
@Kod nvarchar(100),
@Isim nvarchar(250),
@Aciklama nvarchar(500),
@Barkod nvarchar(200),
@OlcuBirimi nvarchar(20),
@GrupKodu1ID nvarchar(100) = null,
@GrupKodu2ID nvarchar(100) = null,
@GrupKodu3ID nvarchar(100) = null,
@GrupKodu4ID nvarchar(100) = null,
@GrupKodu5ID nvarchar(100) = null,
@GrupKodu6ID nvarchar(100) = null,
@KdvAlis decimal(18,8) = 0,
@KdvSatis decimal(18,8) = 0,
@Otv decimal(18,8) = 0,
@OtvFiyat decimal(18,8) = 0,
@Oiv decimal(18,8) = 0,
@TevkifatPay decimal(18,8) = 0,
@TevkifatPayda decimal(18,8) = 0,
@IskontoSatis1 decimal(18,2),
@VadeGunu int,
@MinimumStok decimal(18,2),
@MaxsimumStok decimal(18,2),
@LimitUyarisi bit,
@LimitDisindaIslemiDurdur bit,
@EksiBakiyeUyarisi bit,
@EksiBakiyedeIslemiDurdur bit,
@StokKilitle bit,
@UreticiFirmaID nvarchar(100) = null,
@MarkaID nvarchar(100) = null,
@ModelID nvarchar(100) = null,
@RenkID nvarchar(100) = null,
@BedenID nvarchar(100) = null,
@KaliteID nvarchar(100) = null,
@KayitYapanKullaniciID nvarchar(100)=null,
@AnaStokID nvarchar(100) =null
)
--WITH ENCRYPTION
as
BEGIN
	IF @ID = ''
	BEGIN

		set @ID = (select NEWID())

		INSERT INTO [dbo].[Stoklar]
           (ID
		   ,[UyelikID]
		   ,[Durumu]
           ,[Kod]
           ,[Isim]
		   ,[Aciklama]
           ,[Barkod]
           ,[OlcuBirimi]
           ,[GrupKodu1ID]
           ,[GrupKodu2ID]
           ,[GrupKodu3ID]
           ,[GrupKodu4ID]
           ,[GrupKodu5ID]
           ,[GrupKodu6ID]
           ,[KdvAlis]
           ,[KdvSatis]
           ,[Oiv]
           ,[Otv]
           ,[OtvFiyat]
           ,[TevkifatPay]
           ,[TevkifatPayda]
           ,[IskontoSatis1]
           ,[VadeGunu]
           ,[MinimumStok]
           ,[MaxsimumStok]
           ,[LimitUyarisi]
           ,[LimitDisindaIslemiDurdur]
           ,[EksiBakiyeUyarisi]
           ,[EksiBakiyedeIslemiDurdur]
           ,[StokKilitle]
           ,[UreticiFirmaID]
           ,[MarkaID]
           ,[ModelID]
           ,[RenkID]
           ,[BedenID]
           ,[KaliteID]
		   ,[AnaStokID]
           ,[KayitTarihi]
           ,[KayitYapanKullanici]
           ,[Silindi]
		   )
		VALUES
           (
		   @ID
		   ,@UyelikID
		   ,@Durumu
           ,@Kod
           ,@Isim
		   ,@Aciklama
           ,@Barkod
           ,@OlcuBirimi
           ,@GrupKodu1ID
           ,@GrupKodu2ID
           ,@GrupKodu3ID
           ,@GrupKodu4ID
           ,@GrupKodu5ID
           ,@GrupKodu6ID
           ,@KdvAlis
           ,@KdvSatis
           ,@Oiv
           ,@Otv
           ,@OtvFiyat
           ,@TevkifatPay
           ,@TevkifatPayda
           ,@IskontoSatis1
           ,@VadeGunu
           ,@MinimumStok
           ,@MaxsimumStok
           ,@LimitUyarisi
           ,@LimitDisindaIslemiDurdur
           ,@EksiBakiyeUyarisi
           ,@EksiBakiyedeIslemiDurdur
           ,@StokKilitle
           ,@UreticiFirmaID
           ,@MarkaID
           ,@ModelID
           ,@RenkID
           ,@BedenID
           ,@KaliteID
		   ,@AnaStokID
		   ,GETDATE()
           ,@KayitYapanKullaniciID
		   ,0)
	Select @ID,'' BİLGİ
	END
	ELSE
	BEGIN

	

	   UPDATE [dbo].[Stoklar]
	   SET [Durumu] = @Durumu
		  ,[Kod] = @Kod
		  ,[Isim] = @Isim
		  ,Aciklama=@Aciklama
		  ,[Barkod] = @Barkod
		  ,[OlcuBirimi] = @OlcuBirimi
		  ,[GrupKodu1ID] = @GrupKodu1ID
		  ,[GrupKodu2ID] = @GrupKodu2ID
		  ,[GrupKodu3ID] = @GrupKodu3ID
		  ,[GrupKodu4ID] = @GrupKodu4ID
		  ,[GrupKodu5ID] = @GrupKodu5ID
		  ,[GrupKodu6ID] = @GrupKodu6ID
		  ,[KdvAlis] = @KdvAlis
		  ,[KdvSatis] = @KdvSatis
		  ,[Oiv] = @Oiv
		  ,[Otv] = @Otv
		  ,[OtvFiyat] = @OtvFiyat
		  ,[TevkifatPay] = @TevkifatPay
		  ,[TevkifatPayda] = @TevkifatPayda
		  ,[IskontoSatis1] = @IskontoSatis1
		  ,[VadeGunu] = @VadeGunu
		  ,[MinimumStok] = @MinimumStok
		  ,[MaxsimumStok] = @MaxsimumStok
		  ,[LimitUyarisi] = @LimitUyarisi
		  ,[LimitDisindaIslemiDurdur] = @LimitDisindaIslemiDurdur
		  ,[EksiBakiyeUyarisi] = @EksiBakiyeUyarisi
		  ,[EksiBakiyedeIslemiDurdur] = @EksiBakiyedeIslemiDurdur
		  ,[StokKilitle] = @StokKilitle
		  ,[DuzenlemeYapanKullanici] = @KayitYapanKullaniciID
		  ,[DuzenlemeTarihi] = GETDATE()
		  ,[UreticiFirmaID] = @UreticiFirmaID
		  ,[MarkaID] = @MarkaID
		  ,[ModelID] = @ModelID
		  ,[RenkID] = @RenkID
		  ,[BedenID] = @BedenID
		  ,[KaliteID] = @KaliteID
		  ,[AnaStokID]=CASE WHEN @AnaStokID = -1 THEN NULL ELSE @AnaStokID END
		  WHERE UyelikID = @UyelikID and ID = @ID
		Select @ID,'' BİLGİ
	END
END

GO
drop proc [p_StokListesi]
GO
CREATE proc [dbo].[p_StokListesi](
@UyelikID nvarchar(100),
@Kod nvarchar(max)='',
@Isim nvarchar(max)=''
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok','Stok Listesi','',GETDATE(),NULL)


select top(250)
Stoklar.*,
G1.Deger as GrupKoduAdi1,
G2.Deger as GrupKoduAdi2,
G3.Deger as GrupKoduAdi3,
G4.Deger as GrupKoduAdi4,
G5.Deger as GrupKoduAdi5,
G6.Deger as GrupKoduAdi6,
C1.Isim as AnaStok
from Stoklar WITH(NOLOCK)
LEFT OUTER JOIN GrupKodlari G1 WITH(NOLOCK) ON G1.ID = Stoklar.GrupKodu1ID
LEFT OUTER JOIN GrupKodlari G2 WITH(NOLOCK) ON G2.ID = Stoklar.GrupKodu2ID
LEFT OUTER JOIN GrupKodlari G3 WITH(NOLOCK) ON G3.ID = Stoklar.GrupKodu3ID
LEFT OUTER JOIN GrupKodlari G4 WITH(NOLOCK) ON G4.ID = Stoklar.GrupKodu4ID
LEFT OUTER JOIN GrupKodlari G5 WITH(NOLOCK) ON G5.ID = Stoklar.GrupKodu5ID
LEFT OUTER JOIN GrupKodlari G6 WITH(NOLOCK) ON G6.ID = Stoklar.GrupKodu6ID
LEFT OUTER JOIN Stoklar C1 WITH(NOLOCK) ON C1.ID = Stoklar.AnaStokID
Where ISNULL(Stoklar.Silindi,0) = 0 and  Stoklar.UyelikID = @UyelikID 
and (
   Stoklar.Kod like '%'+@Kod+'%'
and Stoklar.Isim like '%'+@Isim+'%'
)

END
GO
drop proc [p_StokNot]
GO
CREATE proc [dbo].[p_StokNot](
@ID nvarchar(100),
@UyelikID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok','Stok Not Detayı','',GETDATE(),NULL)

select 
* 
from StokNotlar WITH(NOLOCK)
Where ID = @ID

END

GO
drop proc [p_StokNotKaydet]
GO
CREATE proc [dbo].[p_StokNotKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@StokID nvarchar(100)=null,
@Aciklama nvarchar(max)=null,	
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok','Stok Not Eklendi',@Aciklama,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into StokNotlar
(ID,StokID,Aciklama,KayitTarihi,KayitYapanKullanici)
Select @ID,@StokID,@Aciklama,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok','Stok Not Güncellendi',@Aciklama,GETDATE(),@KullaniciID)

Update StokNotlar set 
Aciklama = @Aciklama,
KayitTarihi = GETDATE(),
KayitYapanKullanici = @KullaniciID
Where ID = @ID and StokID = @StokID
END

END
GO
drop proc [p_StokNotListesi]
GO
CREATE proc [dbo].[p_StokNotListesi](
@UyelikID nvarchar(100),
@StokID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok','Stok Not Listesi','',GETDATE(),NULL)

select 
* 
from StokNotlar WITH(NOLOCK)
Where StokID = @StokID
Order by KayitTarihi desc

END
GO
drop proc [p_StokNotSil]
GO
CREATE proc [dbo].[p_StokNotSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@StokID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok','Stok Not Silindi',(select top(1) Aciklama from StokNotlar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)


Delete From StokNotlar Where ID = @ID and StokID = @StokID

END
GO
drop proc [p_StokSayimKaydet]
GO
CREATE proc [dbo].[p_StokSayimKaydet](
@ID nvarchar(100)='',
@UyelikID nvarchar(100)='',
@Tarih datetime,
@Firma nvarchar(100),
@Sube nvarchar(100),
@Depo nvarchar(100),
@Barkod nvarchar(100)='',
@Stok nvarchar(100)='',
@Miktar decimal(18,8)=0,
@Kullanici nvarchar(100)
)
as
BEGIN

IF NOT EXISTS(Select 
				* 
				from Stoklar WITH(NOLOCK) 
				Where Stoklar.UyelikID = @UyelikID and Stoklar.Silindi = 0 
				and (Stoklar.Barkod = @Barkod or Stoklar.Kod = @Stok))
BEGIN
	Select 'UYARI! Stok bulunamadı.' as Bilgi
	return;
END

IF @Barkod <> ''
BEGIN

	set @Stok = (Select top(1) Stoklar.ID from Stoklar WITH(NOLOCK) Where Stoklar.UyelikID = @UyelikID and Stoklar.Silindi = 0 
	and Stoklar.Barkod = @Barkod)

END


if	@ID=''	
BEGIN
	IF NOT EXISTS(select * from StokSayimlari WITH(NOLOCK) 
		Where Silindi = 0 and UyelikID = @UyelikID and Tarih = @Tarih and Firma = @Firma and Sube = @Sube and Depo = @Depo and Stok = @Stok)
	BEGIN
	PRINT('Insert')

		set @ID = (Select NEWID())
		Insert Into StokSayimlari
		(ID,UyelikID,Tarih,Firma,Sube,Depo,Barkod,Stok,Miktar,KayitTarihi,KayitYapanKullanici,Silindi)
		values
		(@ID,@UyelikID,@Tarih,@Firma,@Sube,@Depo,@Barkod,@Stok,@Miktar,GETDATE(),@Kullanici,0)

	END
	ELSE
	BEGIN
	
	PRINT('Update 2')
		Update StokSayimlari Set 
			Miktar = @Miktar,
			DuzenlemeTarihi=GETDATE(),
			DuzenlemeYapanKullanici = @Kullanici
		Where Silindi = 0 and UyelikID = @UyelikID and Tarih = @Tarih and Firma = @Firma and Sube = @Sube and Depo = @Depo and Stok = @Stok

	END
END
ELSE
BEGIN
	PRINT('Update')
	Update StokSayimlari Set 
		Miktar = @Miktar,
		DuzenlemeTarihi=GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici
	Where ID = @ID
END

Select 'Sayım kaydedildi.' as Bilgi
return;

END

GO
drop proc [p_StokSayimListesi]
GO
CREATE proc [dbo].[p_StokSayimListesi](
@UyelikID nvarchar(100)='',
@Tarih datetime,
@Firma nvarchar(100),
@Sube nvarchar(100),
@Depo nvarchar(100),
@Kullanici nvarchar(100)
)
as
BEGIN

Select
top(500)
*,
S.ID as StokID,
S.Kod as StokKodu,
S.Isim as StokAdi
From StokSayimlari WITH(NOLOCK)
LEFT OUTER JOIN Stoklar S WITH(NOLOCK) ON S.Silindi = 0 and S.UyelikID = StokSayimlari.UyelikID 
and CAST(S.ID as nvarchar(100)) = StokSayimlari.Stok
Where StokSayimlari.Silindi = 0
and StokSayimlari.Tarih = @Tarih
and StokSayimlari.Firma = @Firma
and StokSayimlari.Sube = @Sube
and StokSayimlari.Depo = @Depo
order by StokSayimlari.KayitTarihi desc

END
GO
drop proc [p_StokSayimSil]
GO
CREATE proc [dbo].[p_StokSayimSil](
@ID nvarchar(100),
@Kullanici nvarchar(100)
)
as
BEGIN

Update StokSayimlari set Silindi = 1,SilinenTarih=GETDATE(),SilenKullanici = @Kullanici Where ID = @ID

END
GO
drop proc [p_StokSerileri]
GO
CREATE proc [dbo].[p_StokSerileri](
@UyelikID nvarchar(100),
@StokID nvarchar(100)
)
as
BEGIN

select 
* 
from StokSerileri WITH(NOLOCK) 
Where UyelikID = @UyelikID and StokID = @StokID

END
GO
drop proc [p_StokSil]
GO
CREATE proc [dbo].[p_StokSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Stok','Stok Silindi',(Select top(1) Isim From Stoklar WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Update Stoklar set
Silindi = 1,
SilinenTarih=GETDATE(),
SilenKullanici = @KullaniciID
Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_StopSaleAcentaIsimleri]
GO
CREATE PROCEDURE [dbo].[p_StopSaleAcentaIsimleri]
as
BEGIN
SELECT 
LTRIM(RTRIM(A.[AgencyCode])) Kod
FROM SednaSherwooD.[dbo].[Agency] A(NOLOCK) 
Where LTRIM(RTRIM(A.[AgencyCode])) NOT LIKE 'C-%'
and LTRIM(RTRIM(A.[AgencyCode])) NOT LIKE 'INFO-%'
and LTRIM(RTRIM(A.[AgencyCode])) NOT LIKE 'GRP-%'
Group by LTRIM(RTRIM(A.[AgencyCode]))

END

GO
drop proc [p_Sube]
GO
CREATE proc [dbo].[p_Sube](
@ID nvarchar(100)
)
as
BEGIN


select 
* 
from Subeler WITH(NOLOCK)
Where ID = @ID


END
GO
drop proc [p_SubeKaydet]
GO
CREATE proc [dbo].[p_SubeKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@Kod nvarchar(200)=null,	
@Isim nvarchar(max)=null,
@Adres nvarchar(max)=null,
@Telefon nvarchar(max)=null,
@KullaniciID nvarchar(100)
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Eklendi',@Isim,GETDATE(),@KullaniciID)

set @ID = NEWID()
Insert Into Subeler
(ID,UyelikID,Kod,Isim,Adres,Telefon,KayitTarihi,KayitYapanKullanici)
Select @ID,@UyelikID,@Kod,@Isim,@Adres,@Telefon,GETDATE(),@KullaniciID

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,@Kod,'Kayıt Güncellendi',@Isim,GETDATE(),@KullaniciID)

Update Subeler set 
Kod=@Kod,
Isim=@Isim,
Adres=@Adres,
Telefon=@Telefon,
DuzenlemeTarihi=GETDATE(),
DuzenlemeYapanKullanici=@KullaniciID
Where ID = @ID and UyelikID = @UyelikID
END

END

GO
drop proc [p_SubeListesi]
GO
CREATE proc [dbo].[p_SubeListesi](
@UyelikID nvarchar(100)='GENEL',
@AranacakKelime nvarchar(max)=''
)
as
BEGIN


	select 
	* 
	from Subeler WITH(NOLOCK)
	Where UyelikID =@UyelikID and (Firma like '%'+@AranacakKelime+'%' or Kod like '%'+@AranacakKelime+'%' or Isim like '%'+@AranacakKelime+'%')
	Order by Isim


END

GO
drop proc [p_SubeSil]
GO
CREATE proc [dbo].[p_SubeSil](
@ID nvarchar(100)='',
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,(select top(1) Kod From Subeler WITH(NOLOCK) Where ID = @ID),'Kayıt Silindi',(select top(1) Isim From Subeler WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From Subeler Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [p_SubeYetkiKaydet]
GO
CREATE PROCEDURE [dbo].[p_SubeYetkiKaydet](
@kullanici nvarchar(100) = '',
@MenuID int = 0,
@Yetki bit = '0'
)
as
BEGIN

IF EXISTS(select * from YetkilerSube WITH(NOLOCK) Where Kullanici = @kullanici and MenuID = @MenuID)
BEGIN
	Update YetkilerSube Set Yetki = @Yetki Where  Kullanici = @kullanici and MenuID = @MenuID
END
ELSE
BEGIN
	Insert Into YetkilerSube (Kullanici,MenuID,Yetki) values (@kullanici,@MenuID,@Yetki)
END

END
GO
drop proc [p_TalebiGeriyeGonder]
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonder](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Satınalma Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')

	Update SatinalmaTalepleri Set
		Durumu = N'Satınalma Red',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama,
		SecilenCariID = NULL,
		SecilenDovizBirimi = '',
		SecilenFiyat = 0,
		SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderID]    
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderID](
@KayitID nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Satınalma Red',@Aciklama,StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and ID = @KayitID
	and (Durumu = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')

	Update SatinalmaTalepleri Set
		Durumu = N'Satınalma Red',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama,
		SecilenCariID = null,
		SecilenDovizBirimi = '',
		SecilenFiyat = 0,
		SecilenTeklifID = null
	Where Silindi = 0 and ID = @KayitID
	and (Durumu = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderOnay1] 
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderOnay1](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Cost Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onayda')

	Update SatinalmaTalepleri Set
		Durumu = N'Cost Red',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama
		--SecilenCariID = 0,
		--SecilenDovizBirimi = '',
		--SecilenFiyat = 0,
		--SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onayda')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderOnay2] 
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderOnay2](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Cost Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay1')

	Update SatinalmaTalepleri Set
		Durumu = N'Cost Red',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama
		--SecilenCariID = 0,
		--SecilenDovizBirimi = '',
		--SecilenFiyat = 0,
		--SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay1')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderOnay3] 
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderOnay3](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Genel Müdür Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay2')

	Update SatinalmaTalepleri Set
		Durumu = N'Genel Müdür Red',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama
		--SecilenCariID = 0,
		--SecilenDovizBirimi = '',
		--SecilenFiyat = 0,
		--SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay2')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderOnay4]  
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderOnay4](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Yönetim Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay3')

	Update SatinalmaTalepleri Set
		Durumu = N'Yönetim Red',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama
		--SecilenCariID = 0,
		--SecilenDovizBirimi = '',
		--SecilenFiyat = 0,
		--SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay3')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderOnay5]  
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderOnay5](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Yönetim Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay4')

	Update SatinalmaTalepleri Set
		Durumu = N'Yönetim Red',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama
		--SecilenCariID = 0,
		--SecilenDovizBirimi = '',
		--SecilenFiyat = 0,
		--SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay4')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderSatinalmaya]  
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderSatinalmaya](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Satınalma Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onayda' or ISNULL(Durumu,'') = '')

	Update SatinalmaTalepleri Set
		Durumu = N'Talep Onaylandı',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama
		--SecilenCariID = 0,
		--SecilenDovizBirimi = '',
		--SecilenFiyat = 0,
		--SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onayda' or ISNULL(Durumu,'') = '')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderSatinalmaya2]   
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderSatinalmaya2](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Cost Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay1' or ISNULL(Durumu,'') = '')

	Update SatinalmaTalepleri Set
		Durumu = N'Talep Onaylandı',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama
		--SecilenCariID = 0,
		--SecilenDovizBirimi = '',
		--SecilenFiyat = 0,
		--SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay1' or ISNULL(Durumu,'') = '')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderSatinalmaya3]  
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderSatinalmaya3](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Genel Müdür Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay2' or ISNULL(Durumu,'') = '')

	Update SatinalmaTalepleri Set
		Durumu = N'Talep Onaylandı',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama
		--SecilenCariID = 0,
		--SecilenDovizBirimi = '',
		--SecilenFiyat = 0,
		--SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay2' or ISNULL(Durumu,'') = '')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderSatinalmaya4]   
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderSatinalmaya4](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Yönetim Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay3' or ISNULL(Durumu,'') = '')

	Update SatinalmaTalepleri Set
		Durumu = N'Talep Onaylandı',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama
		--SecilenCariID = 0,
		--SecilenDovizBirimi = '',
		--SecilenFiyat = 0,
		--SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay3' or ISNULL(Durumu,'') = '')

END

GO
drop proc [dbo].[p_TalebiGeriyeGonderSatinalmaya5]  
GO
CREATE PROCEDURE [dbo].[p_TalebiGeriyeGonderSatinalmaya5](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Aciklama nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Yönetim Red',@Aciklama,@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay4' or ISNULL(Durumu,'') = '')

	Update SatinalmaTalepleri Set
		Durumu = N'Talep Onaylandı',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		Aciklama2 = ISNULL(Aciklama2,'')+'<br/>'+@Aciklama
		--SecilenCariID = 0,
		--SecilenDovizBirimi = '',
		--SecilenFiyat = 0,
		--SecilenTeklifID = null
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay4' or ISNULL(Durumu,'') = '')

END

GO
drop proc [dbo].[p_TalepKaydet] 
GO
CREATE PROCEDURE [dbo].[p_TalepKaydet](
@StokID nvarchar(100),
@SubeKodu nvarchar(10),
@Miktar decimal(18,2),
@Aciklama1 nvarchar(max),
@Kullanici nvarchar(100)
)
as
BEGIN

declare @StokKodu nvarchar(100)=(select top(1) StokKodu From w_Stoklar Where ID = @StokID)
declare @StokAdi nvarchar(100)=(select top(1) StokAdi From w_Stoklar Where ID = @StokID)
declare @OlcuBirimi nvarchar(100)=(select top(1) OlcuBirimi From w_Stoklar Where ID = @StokID)
declare @Bakiye decimal(18,2)=0
declare @SonAlisFiyati decimal(18,2)=0

declare @Tarih datetime = CAST('2024-01-01' as DATE) --(Select DATEADD(DAY,-3,GETDATE()))

if 1=2
BEGIN

	Insert Into SatinalmaTalepleri
	(StokID,SubeKodu,StokKodu,StokAdi,OlcuBirimi,Bakiye,SonAlisFiyati,TalepMiktari,TalepMiktariYedek,Aciklama1,Silindi,KayitTarihi,KayitYapanKullanici)
	values
	(@StokID,@SubeKodu,@StokKodu,@StokAdi,@OlcuBirimi,@Bakiye,@SonAlisFiyati,@Miktar,@Miktar,@Aciklama1,0,GETDATE(),@Kullanici)

	Select SCOPE_IDENTITY() as [Data],N'Başarılı' Bilgi

END
ELSE
BEGIN

IF EXISTS (Select * from SatinalmaTalepleri WITH(NOLOCK) Where ISNULL(Aktarildi,0) = 0  and SubeKodu = @SubeKodu and StokID = @StokID and KayitTarihi >= @Tarih and ISNULL(Durumu,'') <> 'Tamamlandı' and ISNULL(Durumu,'') NOT LIKE '%Red')
BEGIN
	PRINT('Uyarı!')
	Select 0 as [Data],'UYARI! '+@StokAdi+' Talep süreçte olduğundan dolayı tekrar açılamadı.' as Bilgi
	return;
END
PRINT('2')
Insert Into SatinalmaTalepleri
(StokID,SubeKodu,StokKodu,StokAdi,OlcuBirimi,Bakiye,SonAlisFiyati,TalepMiktari,TalepMiktariYedek,Aciklama1,Silindi,KayitTarihi,KayitYapanKullanici)
values
(@StokID,@SubeKodu,@StokKodu,@StokAdi,@OlcuBirimi,@Bakiye,@SonAlisFiyati,@Miktar,@Miktar,@Aciklama1,0,GETDATE(),@Kullanici)

Select SCOPE_IDENTITY() as [Data],'Başarılı' Bilgi
END

END
GO
drop proc [dbo].[p_TalepKaydet2]  
GO
CREATE PROCEDURE [dbo].[p_TalepKaydet2](
@StokID nvarchar(100),
@SubeKodu nvarchar(10),
@Miktar decimal(18,2),
@Aciklama1 nvarchar(max),
@Kullanici nvarchar(100)
)
as
BEGIN

declare @StokKodu nvarchar(100)=(select top(1) StokKodu From w_Stoklar Where ID = @StokID)
declare @StokAdi nvarchar(100)=(select top(1) StokAdi From w_Stoklar Where ID = @StokID)
declare @OlcuBirimi nvarchar(100)=(select top(1) OlcuBirimi From w_Stoklar Where ID = @StokID)
declare @Bakiye decimal(18,2)=0
declare @SonAlisFiyati decimal(18,2)=0

declare @Tarih datetime = CAST('2024-01-01' as DATE) --(Select DATEADD(DAY,-3,GETDATE()))

if @StokKodu like '09%' or @StokKodu like '13%'
BEGIN

	Insert Into SatinalmaTalepleri
	(StokID,SubeKodu,StokKodu,StokAdi,OlcuBirimi,Bakiye,SonAlisFiyati,TalepMiktari,TalepMiktariYedek,Aciklama1,Silindi,KayitTarihi,KayitYapanKullanici,SifreliGecis)
	values
	(@StokID,@SubeKodu,@StokKodu,@StokAdi,@OlcuBirimi,@Bakiye,@SonAlisFiyati,@Miktar,@Miktar,@Aciklama1,0,GETDATE(),@Kullanici,1)

	Select SCOPE_IDENTITY() as [Data],N'Başarılı' Bilgi

END
ELSE
BEGIN

IF EXISTS (Select * from SatinalmaTalepleri WITH(NOLOCK) Where ISNULL(Aktarildi,0) = 0  and SubeKodu = @SubeKodu and StokID = @StokID and KayitTarihi >= @Tarih and ISNULL(Durumu,'') <> 'Tamamlandı' and ISNULL(Durumu,'') NOT LIKE '%Red')
BEGIN
	PRINT('Uyarı!')
	Select 0 as [Data],'UYARI! '+@StokAdi+' Talep süreçte olduğundan dolayı tekrar açılamadı.' as Bilgi
	return;
END
PRINT('2')
Insert Into SatinalmaTalepleri
(StokID,SubeKodu,StokKodu,StokAdi,OlcuBirimi,Bakiye,SonAlisFiyati,TalepMiktari,TalepMiktariYedek,Aciklama1,Silindi,KayitTarihi,KayitYapanKullanici,SifreliGecis)
values
(@StokID,@SubeKodu,@StokKodu,@StokAdi,@OlcuBirimi,@Bakiye,@SonAlisFiyati,@Miktar,@Miktar,@Aciklama1,0,GETDATE(),@Kullanici,1)

Select SCOPE_IDENTITY() as [Data],'Başarılı' Bilgi
END

END
GO
drop proc [dbo].[p_Taleplerim]   
GO
CREATE PROCEDURE [dbo].[p_Taleplerim](
@KullaniciAdi nvarchar(max),
@Tarih1 datetime,
@Tarih2 datetime
)
as
BEGIN

select 
T.Id, 
ISNULL((select top(1) Dosya From SatinalmaDosyalar WITH(NOLOCK) Where SatinalmaDosyalar.KayitID = T.ID),'') as Dosya, 
CASE 
WHEN (T.Durumu = T.BitisNoktasi) THEN 'Onay4'
ELSE Durumu
END Durumu,
Subeler.KisaKodu as SubeKodu,
w_Stoklar.Kategori1,
w_Stoklar.Kategori2,
w_Stoklar.Kategori3,
T.StokKodu,
T.StokAdi,
T.OlcuBirimi,
T.TalepMiktari,
T.TalepMiktariYedek,
T.Aciklama1,
T.Aciklama2,
T.KayitTarihi,
T.KayitYapanKullanici,
ISNULL(T.SecilenFiyat,0) as SecilenFiyat
from SatinalmaTalepleri as T WITH(NOLOCK)
LEFT OUTER JOIN w_Stoklar ON w_Stoklar.ID = T.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = T.SubeKodu

Where T.Silindi = 0 
and T.KayitYapanKullanici = (CASE WHEN @KullaniciAdi = 'erayerdinc' or @KullaniciAdi = 'admin' or @KullaniciAdi = 'gulsumcetin' or @KullaniciAdi = 'sibeldemirdelen' THEN T.KayitYapanKullanici ELSE @KullaniciAdi END)
and KayitTarihi between @Tarih1 and @Tarih2


END
GO
drop proc [dbo].[p_TalepOnayaGonder] 
GO
CREATE PROCEDURE [dbo].[p_TalepOnayaGonder](
@Grupla int = 1,
@StokKodu nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'TalepOnayaGonder',N'Talep Onaya Gönderildi',@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')
	and SecilenCariID IS NOT NULL

	Update SatinalmaTalepleri Set
		Durumu = N'Onayda',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Talep Onaylandı' or ISNULL(Durumu,'') = '')
	and SecilenCariID IS NOT NULL

END

GO
drop proc [dbo].[p_TalepOnayla] 
GO

CREATE PROCEDURE [dbo].[p_TalepOnayla](
@KayitID int=0,
@Miktar decimal(18,8)=0,
@Aciklama1 nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select @KayitID,'TalepOnayi',N'Talep Onaylandı',@Aciklama1,GETDATE(),@Kullanici

	Update SatinalmaTalepleri Set
		TalepMiktari = @Miktar,
		Aciklama1=@Aciklama1,
		Durumu = N'Talep Onaylandı'	
	Where ID = @KayitID

END
GO
drop proc [dbo].[p_TalepOnayla1] 
GO
CREATE PROCEDURE [dbo].[p_TalepOnayla1](
@Grupla int=1,
@StokKodu nvarchar(max)='',
@Kullanici nvarchar(100)='',
@BitisNoktasi nvarchar(max)='',
@OnaySeviyesi nvarchar(max)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Onay1',N'Satınalma Onayladı',@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onayda')
	and SecilenCariID IS NOT NULL

	Update SatinalmaTalepleri Set
		Durumu = @OnaySeviyesi,
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici,
		BitisNoktasi = @BitisNoktasi
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onayda')
	and SecilenCariID IS NOT NULL

END

GO
drop proc [dbo].[p_TalepOnayla2] 
GO
CREATE PROCEDURE [dbo].[p_TalepOnayla2](
@Grupla int=1,
@StokKodu nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Onay2',N'Cost Onayladı',@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay1')
	and SecilenCariID IS NOT NULL

	Update SatinalmaTalepleri Set
		Durumu = N'Onay2',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay1')
	and SecilenCariID IS NOT NULL

END

GO
drop proc [dbo].[p_TalepOnayla3] 
GO
CREATE PROCEDURE [dbo].[p_TalepOnayla3](
@Grupla int=1,
@StokKodu nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Onay3',N'Genel Müdür Onayladı',@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay2')
	and SecilenCariID IS NOT NULL

	Update SatinalmaTalepleri Set
		Durumu = N'Onay3',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay2')
	and SecilenCariID IS NOT NULL

END

GO
drop proc [dbo].[p_TalepOnayla4] 
GO
CREATE PROCEDURE [dbo].[p_TalepOnayla4](
@Grupla int=1,
@StokKodu nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Onay4',N'Yönetim Onayladı',@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay3')
	and SecilenCariID IS NOT NULL

	Update SatinalmaTalepleri Set
		Durumu = N'Onay4',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay3')
	and SecilenCariID IS NOT NULL

END

GO
drop proc [dbo].[p_TalepOnayla5]  
GO
CREATE PROCEDURE [dbo].[p_TalepOnayla5](
@Grupla int=1,
@StokKodu nvarchar(max)='',
@Kullanici nvarchar(100)=''
)
as
BEGIN

	Insert Into SatinalmaTalepHareketleri
	(TalepID,Modul,Durumu,Aciklama1,KayitTarihi,KayitYapanKullanici)
	Select 
		SatinalmaTalepleri.ID,'Onay4',N'Yönetim Onayladı',@StokKodu,GETDATE(),@Kullanici
	From SatinalmaTalepleri WITH(NOLOCK)
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay4')
	and SecilenCariID IS NOT NULL

	Update SatinalmaTalepleri Set
		Durumu = N'Onay5',
		DuzenlemeTarihi = GETDATE(),
		DuzenlemeYapanKullanici = @Kullanici
	Where Silindi = 0 and CASE WHEN @Grupla = 1 THEN StokKodu ELSE StokKodu+CAST(ID as nvarchar(50)) END = @StokKodu
	and (Durumu = N'Onay4')
	and SecilenCariID IS NOT NULL

END

GO
drop proc [dbo].[p_TalepOnayListesi] 
GO
CREATE PROCEDURE [dbo].[p_TalepOnayListesi](
@KullaniciAdi nvarchar(max)
)
as
BEGIN

select 
T.ID,
T.Durumu,
REPLACE(Subeler.Isim,'SHERWOOD ','') as SubeAdi,
w_Stoklar.Kategori1,
w_Stoklar.Kategori2,
w_Stoklar.Kategori3,
T.StokKodu,
T.StokAdi,
ISNULL(w_StokBakiyeleri.Bakiye,0) as Bakiye,
T.OlcuBirimi,
T.TalepMiktari,
T.Aciklama1,
T.Aciklama2,
T.KayitTarihi,
T.KayitYapanKullanici
from SatinalmaTalepleri as T WITH(NOLOCK)
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = T.SubeKodu
LEFT OUTER JOIN w_Stoklar ON w_Stoklar.ID = T.StokID
LEFT OUTER JOIN w_StokBakiyeleri ON w_StokBakiyeleri.ID = T.StokID and w_StokBakiyeleri.SubeKodu = T.SubeKodu collate SQL_Latin1_General_CP1_CI_AS
Where T.Silindi = 0 and T.KayitYapanKullanici = @KullaniciAdi
and ISNULL(Durumu,'') like '%Red%'
and Subeler.KisaKodu IS NOT NULL
and T.KayitYapanKullanici = @KullaniciAdi

END
GO
drop proc [dbo].[p_TalepRaporu] 
GO


CREATE PROCEDURE [dbo].[p_TalepRaporu](
@KullaniciAdi nvarchar(max),
@Sube nvarchar(max)='',
@Tarih1 datetime,
@Tarih2 datetime,
@Kategori1 nvarchar(max)='',
@TamamlananlariGoster  bit = 0
)
as
BEGIN

Select * from (
select
T.Id, 
--ISNULL((select top(1) Dosya From SatinalmaDosyalar WITH(NOLOCK) Where SatinalmaDosyalar.KayitID = T.ID),'') as Dosya, 
'' Dosya,
CASE 
WHEN (T.Durumu = T.BitisNoktasi) THEN N'Tamamlandı'
WHEN (T.Durumu = 'Onay4' and Durumu <> BitisNoktasi) THEN N'Yönetim Onayda'
WHEN (T.Durumu = 'Onay3' and Durumu <> BitisNoktasi) THEN N'COO Onayda'
WHEN (T.Durumu = 'Onay2' and Durumu <> BitisNoktasi) THEN N'Genel Müdür Onayda'
WHEN (T.Durumu = 'Onay1' and Durumu <> BitisNoktasi) THEN N'Cost Onayda'
--WHEN T.SifreliGecis = 0 and T.KayitTarihi >= (select KilitTarihi from ID_Satinalma_Tarihler where tarih  = CAST(GETDATE() as date)) and T.SecilenCariID IS NULL THEN N'Talep Günü Bekleniyor'
WHEN (T.Durumu = N'Onayda') THEN N'Satınalma Onayda'
WHEN (T.Durumu like N'%Red%') THEN Durumu
ELSE 'Fiyat Bekliyor'
END as Durumu,
CASE
WHEN BitisNoktasi = 'Onay4' THEN 'Yönetim'
WHEN BitisNoktasi = 'Onay4' THEN 'COO'
WHEN BitisNoktasi = 'Onay3' THEN 'Genel Müdür'
WHEN BitisNoktasi = 'Onay2' THEN 'Cost'
WHEN BitisNoktasi = 'Onay1' THEN 'Satınalma'
ELSE '' END
as BitisNoktasi,
Subeler.KisaKodu as SubeKodu,
w_Stoklar.Kategori1,
w_Stoklar.Kategori2,
w_Stoklar.Kategori3,
T.StokKodu,
T.StokAdi,
T.OlcuBirimi,
T.TalepMiktari,
T.TalepMiktariYedek,
REPLACE(REPLACE(T.Aciklama1,'''','-'),'\r\n','') as Aciklama1,
REPLACE(REPLACE(T.Aciklama2,'''','-'),'\r\n','') as Aciklama2,
REPLACE(REPLACE(T.Aciklama3,'''','-'),'\r\n','') as Aciklama3,
T.KayitTarihi,
T.KayitYapanKullanici,
CAST(ISNULL(T.SecilenFiyat,0) as decimal(18,2)) as SecilenFiyat,
w_Cariler.CariAdi as SecilenCariAdi,
AktarimNo as SiparisNo,
(select top(1) KayitYapanKullanici from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay1' Order by ID desc ) SatinalmaOnayi,
(select top(1) KayitTarihi from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay1' Order by ID desc ) SatinalmaOnayTarihi,
(select top(1) KayitYapanKullanici from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay2' Order by ID desc ) CostOnayi,
(select top(1) KayitTarihi from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay2' Order by ID desc ) CostOnayTarihi,
(select top(1) KayitYapanKullanici from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay3' Order by ID desc ) GenelMudurOnayi,
(select top(1) KayitTarihi from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay3' Order by ID desc ) GenelMudurOnayTarihi,
(select top(1) KayitYapanKullanici from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay4' Order by ID desc ) COOOnayi,
(select top(1) KayitTarihi from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay4' Order by ID desc ) COOOnayTarihi,
(select top(1) KayitYapanKullanici from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay5' Order by ID desc ) YonetimOnayi,
(select top(1) KayitTarihi from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay5' Order by ID desc ) YonetimOnayTarihi,
ISNULL(PdfDosyaAdi,AktarimNo) as PdfDosyaAdi,
SifreliGecis
from SatinalmaTalepleri as T WITH(NOLOCK)
LEFT OUTER JOIN w_Stoklar ON w_Stoklar.ID = T.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = T.SubeKodu
LEFT OUTER JOIN w_Cariler ON w_Cariler.ID = T.SecilenCariID
Where T.Silindi = 0 
--and T.StokKodu = '020102018'
and (Subeler.Kod = @Sube or @Sube = '')
and (Kategori1 = @Kategori1 or @Kategori1 = '')
and KayitTarihi between @Tarih1 and @Tarih2
--and T.KayitYapanKullanici = (CASE WHEN @KullaniciAdi = 'erayerdinc' or @KullaniciAdi = 'admin' THEN T.KayitYapanKullanici ELSE @KullaniciAdi END)
--and Subeler.ID IN (select YetkilerSube.MenuID from SatinalmaYetkilerSube YetkilerSube  WITH(NOLOCK) where Kullanici = @KullaniciAdi and Yetki = 1)
--and (T.Durumu = N'Onay1')
--and T.Aciklama1 like N'%HARİTA MET%'
--and (KayitTarihi <= '2024-06-01 12:00:00.000' or SifreliGecis = 1)
--and T.Durumu <> T.BitisNoktasi
) YK
Where 1=1
and Durumu <> CASE WHEN @TamamlananlariGoster = 1 THEN 'XXXXX' ELSE N'Tamamlandı' END
--and Durumu <> N'Yönetim Onayda'
--and Durumu <> N'Tamamlandı'
--and Durumu <> N'Genel Müdür Onayda'
--and Durumu <> N'Cost Onayda'
--and Durumu not like N'%Red%'
--and Durumu <> N'Talep Günü Bekleniyor'
--and Durumu <> N'Satınalma Onayda'

END
GO
drop proc [dbo].[p_TalepRaporuExcel] 
GO
CREATE PROCEDURE [dbo].[p_TalepRaporuExcel](
@KullaniciAdi nvarchar(max),
@Sube nvarchar(max)='',
@Tarih1 datetime,
@Tarih2 datetime,
@Kategori1 nvarchar(max)='',
@TamamlananlariGoster  bit = 0
)
as
BEGIN

select
T.Id, 
ISNULL((select top(1) Dosya From SatinalmaDosyalar WITH(NOLOCK) Where SatinalmaDosyalar.KayitID = T.ID),'') as Dosya, 
CASE 
WHEN (T.Durumu = T.BitisNoktasi) THEN 'Tamamlandı'
WHEN T.Durumu = 'Onay3' THEN 'Yönetim'
WHEN T.Durumu = 'Onay2' THEN 'Genel Müdür'
WHEN T.Durumu = 'Onay1' THEN 'Cost'
ELSE 'Satınalma'
END as Durumu,

CASE
WHEN BitisNoktasi = 'Onay4' THEN 'Yönetim'
WHEN BitisNoktasi = 'Onay3' THEN 'Genel Müdür'
WHEN BitisNoktasi = 'Onay2' THEN 'Cost'
WHEN BitisNoktasi = 'Onay1' THEN 'Satınalma'
ELSE '' END
as BitisNoktasi,
Subeler.KisaKodu as SubeKodu,
w_Stoklar.Kategori1,
w_Stoklar.Kategori2,
w_Stoklar.Kategori3,
T.StokKodu,
T.StokAdi,
T.OlcuBirimi,
REPLACE(CAST(T.TalepMiktari as nvarchar(max)),'.',',') as TalepMiktari,
REPLACE(CAST(ISNULL(T.SecilenFiyat,0)*T.TalepMiktari as nvarchar(max)),'.',',') as Tutar,
T.TalepMiktariYedek,
REPLACE(REPLACE(T.Aciklama1,'''','-'),'\r\n','') as Aciklama1,
REPLACE(REPLACE(T.Aciklama2,'''','-'),'\r\n','') as Aciklama2,
REPLACE(REPLACE(T.Aciklama3,'''','-'),'\r\n','') as Aciklama3,
T.KayitTarihi,
T.KayitYapanKullanici,
REPLACE(CAST(ISNULL(T.SecilenFiyat,0) as nvarchar(max)),'.',',') as SecilenFiyat,
w_Cariler.CariAdi as SecilenCariAdi,
AktarimNo as SiparisNo,
(select top(1) KayitYapanKullanici from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay1' Order by ID desc ) SatinalmaOnayi,
(select top(1) KayitTarihi from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay1' Order by ID desc ) SatinalmaOnayTarihi,
(select top(1) KayitYapanKullanici from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay2' Order by ID desc ) CostOnayi,
(select top(1) KayitTarihi from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay2' Order by ID desc ) CostOnayTarihi,
(select top(1) KayitYapanKullanici from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay3' Order by ID desc ) GenelMudurOnayi,
(select top(1) KayitTarihi from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay3' Order by ID desc ) GenelMudurOnayTarihi,
(select top(1) KayitYapanKullanici from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay4' Order by ID desc ) YonetimOnayi,
(select top(1) KayitTarihi from SatinalmaTalepHareketleri SH WITH(NOLOCK) Where SH.TalepID = T.ID and Modul = 'Onay4' Order by ID desc ) YonetimOnayTarihi,
ISNULL(PdfDosyaAdi,AktarimNo) as PdfDosyaAdi
from SatinalmaTalepleri as T WITH(NOLOCK)
LEFT OUTER JOIN w_Stoklar ON w_Stoklar.ID = T.StokID
LEFT OUTER JOIN SatinalmaSubeler Subeler WITH(NOLOCK) ON Subeler.Kod = T.SubeKodu
LEFT OUTER JOIN w_Cariler ON w_Cariler.ID = T.SecilenCariID
Where T.Silindi = 0 
--and T.StokKodu = '020102018'
and (Subeler.Kod = @Sube or @Sube = '')
and (Kategori1 = @Kategori1 or @Kategori1 = '')
and KayitTarihi between @Tarih1 and @Tarih2
--and T.KayitYapanKullanici = (CASE WHEN @KullaniciAdi = 'erayerdinc' or @KullaniciAdi = 'admin' THEN T.KayitYapanKullanici ELSE @KullaniciAdi END)
and Subeler.ID IN (select YetkilerSube.MenuID from SatinalmaYetkilerSube YetkilerSube  WITH(NOLOCK) where Kullanici = @KullaniciAdi and Yetki = 1)
--and (T.Durumu = N'Onay1')
--and T.Aciklama1 like N'%HARİTA MET%'
END
GO
drop proc [dbo].[p_Uyelik]   
GO

CREATE proc [dbo].[p_Uyelik](
@ID nvarchar(100)
)
as
BEGIN

select 
* 
from Uyelikler WITH(NOLOCK)
Where Silindi = 0 and ID = @ID
Order by UyelikBitisTarihi

END
GO
drop proc [dbo].[p_UyelikKaydet]  
GO
CREATE proc [dbo].[p_UyelikKaydet](
@ID nvarchar(100)=null,
@Isim nvarchar(200)=null,
@Unvan nvarchar(200)=null,	
@VergiNumarasi nvarchar(11)=null,	
@VergiDairesi nvarchar(100)=null,	
@Adres nvarchar(250)=null,	
@EMail nvarchar(200)=null,	
@Iletisim nvarchar(200)=null,	
@Kullanici nvarchar(100),
@UyelikBaslangicTarihi datetime,
@UyelikBitisTarihi datetime,
@ApiUrl nvarchar(250),
@AcilisSayfasi nvarchar(max)='',
@Resim nvarchar(max)=''
)
as
BEGIN

IF LEN(@VergiNumarasi) < 10 or LEN(@VergiNumarasi) > 11
BEGIN
	Select 0 as ID, 'Vergi numarası 10 veya 11 karakterli olmalıdır!' as Bilgi
	return;
END

IF LEN(@Isim) <= 0
BEGIN
	Select 0 as ID, 'İsim boş olamaz!' as Bilgi
	return;
END

IF LEN(@Unvan) <= 0
BEGIN
	Select 0 as ID, 'Ünvan boş olamaz!' as Bilgi
	return;
END

IF @EMail not like '%@%.%'
BEGIN
	Select 0 as ID, 'E-mail formata uygun değil!' as Bilgi
	return;
END

IF EXISTS(select ID from Uyelikler WITH(NOLOCK) Where Silindi = 0 and VergiNumarasi = @VergiNumarasi and CAST(ID as nvarchar(100)) <> @ID )
BEGIN
	Select 0 as ID, 'Sistemde aynı vergi numarasıyla üyelik mevcut!' as Bilgi
	return;
END

IF EXISTS(select ID from Uyelikler WITH(NOLOCK) Where Silindi = 0 and EMail = @EMail and CAST(ID as nvarchar(100)) <> @ID)
BEGIN
	Select 0 as ID, 'E-Mail sistemde daha önce kullanışmış!' as Bilgi
	return;
END

IF @ID = ''
BEGIN

set @ID = NEWID()

Insert Into Uyelikler
(ID,Isim,Unvan,VergiNumarasi,VergiDairesi,Adres,Iletisim,Email,UyelikBaslangicTarihi,UyelikBitisTarihi,KayitTarihi,KayitYapanKullanici,Silindi,ApiUrl,AcilisSayfasi,Resim)
Select @ID,@Isim,@Unvan,@VergiNumarasi,@VergiDairesi,@Adres,@Iletisim,@Email,CAST(GETDATE() as DATE),DATEADD(MONTH,1,CAST(GETDATE() as DATE)),GETDATE(),@Kullanici,0,'http://api.ykyazilim.com.tr/api/',@AcilisSayfasi,@Resim

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@ID,'Uyelik','Yeni Üyelik Oluşturuldu',@Isim,GETDATE(),null)

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@ID,'Uyelik','Üyelik Güncellendi',@Isim,GETDATE(),@Kullanici)

Update Uyelikler set 
Isim = @Isim,
Unvan = @Unvan,
VergiNumarasi = @VergiNumarasi,
VergiDairesi = @VergiDairesi,
Adres = @Adres,
Iletisim = @Iletisim,
Email = @Email,
UyelikBaslangicTarihi = @UyelikBaslangicTarihi,
UyelikBitisTarihi = @UyelikBitisTarihi,
DuzenlemeTarihi = GETDATE(),
DuzenlemeYapanKullanici = @Kullanici,
ApiUrl = @ApiUrl,
AcilisSayfasi=@AcilisSayfasi,
Resim=@Resim
Where ID = @ID
END

Select @ID as ID, 'Üyelik kaydı başarıyla oluşturulmuştur.' as Bilgi

END
GO
drop proc [dbo].[p_UyelikListesi]  
GO
CREATE proc [dbo].[p_UyelikListesi](
@AranacakKelime nvarchar(max)=''
)
as
BEGIN

select 
* 
from Uyelikler WITH(NOLOCK)
Where Silindi = 0 
and (Isim like '%'+@AranacakKelime+'%' 
or Unvan like '%'+@AranacakKelime+'%' 
or VergiNumarasi like '%'+@AranacakKelime+'%' 
or EMail like '%'+@AranacakKelime+'%')
Order by UyelikBitisTarihi

END
GO
drop proc [dbo].[p_UyelikOdemesiOlustur]  
GO
CREATE proc [dbo].[p_UyelikOdemesiOlustur](
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100),
@Uygulama nvarchar(100),
@Tutar decimal(18,8),
@UzatilacakAy int,
@Durum nvarchar(100),
@OrderID nvarchar(100),
@KrediKartIsim nvarchar(100),
@KrediKartNo nvarchar(100),
@KrediKartSonKullanim nvarchar(100),
@KrediKartCVV nvarchar(100)
)
as
BEGIN

declare @ID nvarchar(100)= (select newid())

Insert Into UyelikOdemeleri 
(
ID,
UyelikID,
KullaniciID,
Uygulama,
Tutar,
Durum,
UzatilacakAy,
OrderID,
KrediKartIsim,
KrediKartNo,
KrediKartSonKullanim,
KrediKartCVV,
SonucKodu,
SonucAciklama
)
values 
(
@ID,
@UyelikID,
@KullaniciID,
@Uygulama,
@Tutar,
@Durum,
@UzatilacakAy,
@OrderID,
@KrediKartIsim,
@KrediKartNo,
@KrediKartSonKullanim,
@KrediKartCVV,
'',
''
)

Select @ID as ID

END
GO
drop proc [dbo].[p_UyelikOdemesiTamamla] 
GO
CREATE proc [dbo].[p_UyelikOdemesiTamamla](
@UyelikID nvarchar(100)=null,
@KullaniciID nvarchar(100)=null,
@Uygulama nvarchar(100),
@OrderID nvarchar(100),
@Durumu nvarchar(max),
@SonucKodu nvarchar(max),
@SonucAciklama nvarchar(max)
)
as
BEGIN

Update UyelikOdemeleri Set 
	Durum=@Durumu,
	SonucKodu=@SonucKodu,
	SonucAciklama=@SonucAciklama
Where OrderID = @OrderID

IF @Durumu = 'Başarılı'
BEGIN
	declare @Ay int = (select top(1) UzatilacakAy From UyelikOdemeleri WITH(NOLOCK) Where OrderID = @OrderID)
	Update Uyelikler set 
		UyelikBitisTarihi = DATEADD(MONTH,@Ay,UyelikBitisTarihi)
	Where ID = (select UyelikOdemeleri.UyelikID From UyelikOdemeleri WITH(NOLOCK) Where UyelikOdemeleri.OrderID = @OrderID)
END

END


GO
drop proc [dbo].[p_UyelikPaketleri]  
GO
CREATE proc [dbo].[p_UyelikPaketleri](
@UyelikID nvarchar(max)
)
as
BEGIN

	Select * from UyelikPaketleri WITH(NOLOCK)

END
GO
drop proc [dbo].[p_UyelikSil]  
GO
CREATE proc [dbo].[p_UyelikSil](
@ID nvarchar(100)='',
@KullaniciID nvarchar(100)
)
as
BEGIN

Update Uyelikler set 
Silindi = 1,
SilinenTarih = GETDATE(),
SilenKullanici = @KullaniciID
Where ID = @ID



END
GO
drop proc [dbo].[p_Ziyaret]   
GO

CREATE proc [dbo].[p_Ziyaret](
@ID nvarchar(100),
@UyelikID nvarchar(100)
)
as
BEGIN

select 
* 
from Ziyaretler WITH(NOLOCK)
Where ID = @ID and UyelikID = @UyelikID

END
GO
drop proc [dbo].[p_ZiyaretKapat]  
GO
CREATE proc [dbo].[p_ZiyaretKapat](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@CariID nvarchar(100)=null,
@TamamlamaAciklamasi nvarchar(500)=null,
@TamamlamaTarihi datetime=null,
@TamamlayanKullaniciID nvarchar(100)=null
)
as
BEGIN


Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Ziyaret','Ziyaret Kapatıldı',@TamamlamaAciklamasi,GETDATE(),@TamamlayanKullaniciID)

Update Ziyaretler set 
TamamlamaAciklamasi=@TamamlamaAciklamasi,
TamamlamaTarihi=@TamamlamaTarihi,
TamamlayanKullaniciID=@TamamlayanKullaniciID,
DuzenlemeTarihi=GETDATE(),
DuzenmaYapanKullanici=@TamamlayanKullaniciID
Where ID = @ID and UyelikID = @UyelikID


END
GO
drop proc [dbo].[p_ZiyaretKaydet]   
GO
CREATE proc [dbo].[p_ZiyaretKaydet](
@ID nvarchar(100)=null,
@UyelikID nvarchar(100)=null,
@CariID nvarchar(100)=null,
@Tarih datetime = null,
@ZiyaretTipi nvarchar(100)=null,
@Aciklama nvarchar(500)=null,
@TamamlamaAciklamasi nvarchar(500)=null,
@TamamlamaTarihi datetime=null,
@TamamlayanKullaniciID nvarchar(100)=null,
@KullaniciID nvarchar(100)=null
)
as
BEGIN

IF @ID = ''
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Ziyaret','Yeni Ziyaret Eklendi',@Aciklama,GETDATE(),@KullaniciID)

Insert Into Ziyaretler
       (UyelikID, CariID, Tarih, KullaniciID, ZiyaretTipi, Aciklama, TamamlamaAciklamasi, TamamlamaTarihi, TamamlayanKullaniciID, KayitTarihi, KayitYapanKullanici,Silindi)
Select @UyelikID,@CariID,@Tarih,@KullaniciID,@ZiyaretTipi,@Aciklama,@TamamlamaAciklamasi,@TamamlamaTarihi,@TamamlayanKullaniciID,   GETDATE(),@KullaniciID,0

END
ELSE
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Ziyaret','Ziyaret Güncellendi',@TamamlamaAciklamasi,GETDATE(),@KullaniciID)

Update Ziyaretler set 
Tarih=@Tarih,
ZiyaretTipi=@ZiyaretTipi,
Aciklama=@Aciklama,
TamamlamaAciklamasi=@TamamlamaAciklamasi,
TamamlamaTarihi=@TamamlamaTarihi,
TamamlayanKullaniciID=@TamamlayanKullaniciID,
DuzenlemeTarihi=GETDATE(),
DuzenmaYapanKullanici=@KullaniciID
Where ID = @ID and UyelikID = @UyelikID

END

END
GO
drop proc [dbo].[p_ZiyaretListesi]   
GO
CREATE proc [dbo].[p_ZiyaretListesi](
@UyelikID nvarchar(100),
@CariID nvarchar(100)=''
)
as
BEGIN

select 
* 
from Ziyaretler WITH(NOLOCK)
Where UyelikID = @UyelikID and CariID like '%'+@CariID+'%'
Order by Tarih

END
GO
drop proc [dbo].[p_ZiyaretSil]  
GO
CREATE proc [dbo].[p_ZiyaretSil](
@ID nvarchar(100),
@UyelikID nvarchar(100),
@KullaniciID nvarchar(100)
)
as
BEGIN

Insert Into Loglar (UyelikID,Modul,Aciklama1,Aciklama2,KayitTarihi,Kullanici) values (@UyelikID,'Ziyaret','Ziyaret Silnidi',(select top(1) Aciklama From Ziyaretler WITH(NOLOCK) Where ID = @ID),GETDATE(),@KullaniciID)

Delete From Ziyaretler Where ID = @ID and UyelikID = @UyelikID

END
GO
