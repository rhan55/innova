
drop view [w_CalismaKontrolu1]
GO
create view [dbo].[w_CalismaKontrolu1]
as
select top(99999999)Tarih,COUNT(Saat) CalismaSaati,SUM(IslemSayisi)GirisSayisi From (
select  top(99999999)
 Tarih = CAST(KayitTarihi as DATE)
,Saat = FORMAT(KayitTarihi,'HH')
,IslemSayisi = COUNT(*)
from Loglar 
Where Aciklama2 like '%İlayda%'
Group by CAST(KayitTarihi as DATE),FORMAT(KayitTarihi,'HH')
order by CAST(KayitTarihi as DATE) desc, FORMAT(KayitTarihi,'HH')
) YK1
Group by Tarih 
Order by Tarih desc
GO
drop view [w_Cariler]
GO

CREATE view [dbo].[w_Cariler]
as
select
ID as ID,
UyelikID,
Kod as CariKodu,
Isim as CariAdi,
Silindi
from Cariler WITH(NOLOCK) Where 1=1
GO
drop view [w_Depolar]
GO

create view [dbo].[w_Depolar]
as
select 
*
from Depolar WITH(NOLOCK)
GO
drop view [w_SatinalmaStoklar]
GO



CREATE view [dbo].[w_SatinalmaStoklar]
as
select 
S.ID as ID,
REPLACE(CAST(S.ID as nvarchar(100)),'-','') as IDTresiz,
K1.Deger as Kategori1,
K2.Deger as Kategori2,
K3.Deger as Kategori3,
S.Kod as StokKodu,
S.Isim as StokAdi,
S.OlcuBirimi as BirimID,
S.KdvSatis as Kdv,
S.OlcuBirimi as OlcuBirimi,
CAST(0 as decimal(18,2)) as TalepMiktari,
0 as Bakiye,
'' Aciklama1,
'' Aciklama2,
K1.Deger as KategoriKodu1,
K2.Deger as KategoriKodu2,
K3.Deger as KategoriKodu3
from Stoklar S WITH(NOLOCK)
LEFT OUTER JOIN GrupKodlari K1 WITH(NOLOCK) ON K1.ID = S.GrupKodu1ID
LEFT OUTER JOIN GrupKodlari K2 WITH(NOLOCK) ON K2.ID = S.GrupKodu2ID
LEFT OUTER JOIN GrupKodlari K3 WITH(NOLOCK) ON K3.ID = S.GrupKodu3ID
Where S.Silindi = 0
GO
drop view [w_StokBakiyeleri]
GO

create view [dbo].[w_StokBakiyeleri]
as
select 
ID as ID,
0 as SubeKodu,
Kod as StokKodu,
Isim as StokAdi,
0 as Bakiye
From Stoklar WITH(NOLOCK)
GO
drop view [w_Stoklar]
GO

CREATE view [dbo].[w_Stoklar]
as
select 
S.ID as ID,
K1.Deger as Kategori1,
K2.Deger as Kategori2,
K3.Deger as Kategori3,
S.Kod as StokKodu,
S.Isim as StokAdi,
S.OlcuBirimi as BirimID,
S.KdvSatis as Kdv,
S.OlcuBirimi as OlcuBirimi,
CAST(0 as decimal(18,2)) as TalepMiktari,
0 as Bakiye,
'' Aciklama1,
'' Aciklama2,
K1.Deger as KategoriKodu1,
K2.Deger as KategoriKodu2,
K3.Deger as KategoriKodu3
from Stoklar S WITH(NOLOCK)
LEFT OUTER JOIN GrupKodlari K1 WITH(NOLOCK) ON K1.ID = S.GrupKodu1ID
LEFT OUTER JOIN GrupKodlari K2 WITH(NOLOCK) ON K2.ID = S.GrupKodu2ID
LEFT OUTER JOIN GrupKodlari K3 WITH(NOLOCK) ON K3.ID = S.GrupKodu3ID
Where S.Silindi = 0
GO
drop view [w_Ziyaretler]
GO
CREATE view [dbo].[w_Ziyaretler]
as
Select
*
from Ziyaretler WITH(NOLOCK) 

GO
