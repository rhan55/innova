declare @UyelikID nvarchar(100)=N'68854af6-504f-48c7-b64e-eccbf881db80'
declare @KullaniciID nvarchar(100)=N'45b83a26-d48d-4b59-bd12-92cd2788c093'
INSERT [dbo].[Uyelikler] 
([ID], [Isim], [Unvan], [VergiNumarasi], [VergiDairesi], [Adres], [Iletisim], [EMail], [UyelikBaslangicTarihi], 
[UyelikBitisTarihi], [KayitTarihi], [KayitYapanKullanici], [DuzenlemeTarihi], [DuzenlemeYapanKullanici], [Silindi], 
[SilinenTarih], [SilenKullanici], [ApiUrl], [AcilisSayfasi], [Resim]) 
Select 
@UyelikID, N'YENİ ŞİRKET', N'YENİ ŞİRKET', N'1234567891', N'ANTALYA KURUMLAR', 
N'adresiniz', N'5355089134', N'info@ykyazilim.com.tr', 
CAST(GETDATE() AS DateTime), CAST(DATEADD(MONTH,6,GETDATE()) AS DateTime),
CAST(GETDATE() AS DateTime), N'', CAST(GETDATE() AS DateTime), NULL, 0, NULL, NULL, 
N'http://api.ykyazilim.com.tr/api/', N'', N''
Where (select COUNT(*) from Uyelikler WITH(NOLOCK) Where ID = @UyelikID) = 0

INSERT [dbo].[Kullanicilar] ([ID], [UyelikID], [KullaniciAdi], [Parola], [Ad], [Soyad], [Telefon], [Adres], [Il], [Ilce], 
[Aktif], [Aciklama1], [Aciklama2], [Aciklama3], [Tarih], [Resim], [KayitTarihi], [KayitYapanKullanici], [DuzenlemeTarihi], 
[DuzenlemeYapanKullanici], [Silindi], [SilinenTarih], [SilenKullanici], [Onay], [SicilNo], [PersonelParola]) 
Select 
@KullaniciID, @UyelikID, N'info@domain.com', N'12345', 
N'Yunus', N'KÖSE', N'5355089134', N'Kızılarık Mh 2751 Sk. 16/6', N'df6e3618-83c5-48c6-961f-9f953cad222c', N'Muratpaşa', 
1, N'YK YAZILIM', N'admin', N'Genel Müdür', NULL, 
N'/Uploads/Avatarlar/45b83a26-d48d-4b59-bd12-92cd2788c093_4d3c713e-1591-447d-9ead-4974a114c738.JPG', 
CAST(GETDATE() AS DateTime), null, 
CAST(GETDATE() AS DateTime), null, 0, NULL, NULL, 1, NULL, NULL
Where (select COUNT(*) from [Kullanicilar] WITH(NOLOCK) Where ID = @KullaniciID) = 0


INSERT [dbo].[GrupKodlari] ([ID], [UyelikID], [Kod], [Deger], [UstID], [Aktif]) 
Select YK1.* from (
Select N'e2b0b109-d750-457c-b8ad-079885766c41' ID, N'68854af6-504f-48c7-b64e-eccbf881db80'UyelikID, N'Crm_ZiyaretTipi'Kod, N'Telefon Açılacak'Deger, NULL UstID, 1 Aktif	union all
Select N'7ec86fa1-95eb-44a1-a9ce-1005301651cc', N'68854af6-504f-48c7-b64e-eccbf881db80', N'Crm_ZiyaretTipi', N'Mail Atılacak', NULL, 1   						 union all
Select N'12120a9e-923b-4a95-b943-7d728f9d2a53', N'68854af6-504f-48c7-b64e-eccbf881db80', N'Crm_ZiyaretTipi', N'Teklif Gönderilecek', NULL, 1					 union all
Select N'4ba7cafb-21df-4c6b-ac8a-875780cd8224', N'68854af6-504f-48c7-b64e-eccbf881db80', N'Crm_ZiyaretTipi', N'Ziyaret Yapılacak', NULL, 1						 union all
Select N'2198b77d-0238-4804-a701-c13fe564e6d0', N'68854af6-504f-48c7-b64e-eccbf881db80', N'Crm_ZiyaretTipi', N'Genel Hatırlatma', NULL, 0						 union all
Select N'30b5371d-e9f5-45db-b116-ab9ef4ece4c8', N'68854af6-504f-48c7-b64e-eccbf881db80', N'Ulke', N'Türkiye', NULL, 1											 union all
Select N'34d72e49-8291-41e2-b773-5c249665d6a3', N'68854af6-504f-48c7-b64e-eccbf881db80', N'Il', N'Antalya', NULL, 1												 union all
Select N'95810c48-2672-4765-a48c-485d88c751c7', N'68854af6-504f-48c7-b64e-eccbf881db80', N'Ilce', N'Muratpaşa', N'34d72e49-8291-41e2-b773-5c249665d6a3', 1		 union all
Select N'0f38634e-119e-496f-9df3-2147d3bce6ef', N'68854af6-504f-48c7-b64e-eccbf881db80', N'Mahalle', N'Kızılarık', N'95810c48-2672-4765-a48c-485d88c751c7', 1	 union all
Select N'05740601-5928-4530-b559-31a6675b2e9c', N'68854af6-504f-48c7-b64e-eccbf881db80', N'ÖnTanımlıDil', N'Türkçe', NULL, 1									 union all
Select N'8f59892c-5ee1-47a0-aba1-cff833f4287f', N'68854af6-504f-48c7-b64e-eccbf881db80', N'DovizBirimi', N'TL', NULL, 1											 union all
Select N'1a5f353a-86f1-4500-9cd5-04857009a355', N'68854af6-504f-48c7-b64e-eccbf881db80', N'StokHareketTipi', N'Evrak', NULL, 1									 union all
Select N'344c4722-8f5e-418b-9840-8508c70dfeb8', N'68854af6-504f-48c7-b64e-eccbf881db80', N'CariHareketTipi', N'Evrak', NULL, 1									 union all
Select N'd54797ac-7285-4092-8181-9a3dc5582aa6', N'68854af6-504f-48c7-b64e-eccbf881db80', N'GorevTipi', N'GENEL', NULL, 0										 union all
Select N'22a07cf2-0e56-473e-bb8c-7156c593043f', N'68854af6-504f-48c7-b64e-eccbf881db80', N'AnaSayfaTakvimDurumlari', N'Beklemede', NULL, 1						 union all
Select N'3d711f32-7616-4000-b21a-9d90ceb5dd5f', N'68854af6-504f-48c7-b64e-eccbf881db80', N'AnaSayfaTakvimDurumlari', N'Yapıldı', NULL, 1						 union all
Select N'4b2626eb-90dc-469b-8ef5-aac6ff4cf05e', N'68854af6-504f-48c7-b64e-eccbf881db80', N'AnaSayfaTakvimDurumlari', N'Ertelendi', NULL, 1						 union all
Select N'6f86d84d-60e3-42af-841a-b3a40a840cdf', N'68854af6-504f-48c7-b64e-eccbf881db80', N'AnaSayfaTakvimDurumlari', N'İptal', NULL, 1							 union all
Select N'164c1dbb-6a11-4828-a3a4-e6698eb45670', N'68854af6-504f-48c7-b64e-eccbf881db80', N'AnaSayfaTakvimDurumlari', N'Onaylandı', NULL, 1	
) YK1
LEFT OUTER JOIN GrupKodlari G1 WITH(NOLOCK) ON G1.ID  = YK1.ID
Where G1.ID IS NULL




INSERT [dbo].[MailKaliplari] ([ID], [UyelikID], [Kod], [Isim], [Icerik], [KayitTarihi], [KayitYapanKullanici]) 
Select YK1.* From (
Select N'e02fc514-6013-47cd-aaf4-c9ad33e30507' as ID, N'68854af6-504f-48c7-b64e-eccbf881db80' UyelikID, N'Destek_Gorev_Tamamlama' Kod, N'Talebiniz Güncellenmiştir' Isim, 
N'&lt;p&gt;Merhaba; &lt;br /&gt;Aşağıdaki talebiniz g&amp;uuml;ncellenmiştir. &lt;br /&gt;&lt;br /&gt;Kullanıcı : {Isim} &lt;br /&gt;Talep ID : {KayitNo} &lt;br /&gt;Durum : {Durumu} &lt;br /&gt;A&amp;ccedil;ıklama : {Aciklama}&lt;br /&gt;&lt;br /&gt;İyi G&amp;uuml;nler.&lt;/p&gt;' as Icerik, 
CAST(GETDATE() AS DateTime) as KayitTarihi, @KullaniciID as Kullanici union all
Select N'40e7abf1-76e2-4ea5-aa55-ed9751d195db', N'68854af6-504f-48c7-b64e-eccbf881db80', N'Test_Mail', N'Test Mail', N'&lt;h1&gt;Test maili burada&lt;/h1&gt;&lt;p&gt;Bu bir test mailidir&lt;/p&gt;', 
CAST(GETDATE() AS DateTime), @KullaniciID  union all
Select N'f78f0977-8760-4fa4-aba9-fb42f44cd889', N'68854af6-504f-48c7-b64e-eccbf881db80', N'Destek_Gorev_Yeni', N'Yeni Talebiniz Açılmıştır', N'Merhaba;
<br><br>Yeni talebiniz a&#231;ılmıştır, ayrıntıları aşağıdaki gibidir.

<br><br>
Kullanıcı : {Isim}
<br>Tarih : {Tarih}
 <br>A&#231;ıklama : {Aciklama}

<br><br>
İyi G&#252;nler.

', CAST(GETDATE() AS DateTime), @KullaniciID
) YK1
LEFT OUTER JOIN [MailKaliplari] G1 WITH(NOLOCK) ON G1.ID  = YK1.ID
Where G1.ID IS NULL

INSERT [dbo].[Menuler] ([ID], [Menu], [UstID], [icon], [url], [Sira], [Aktif]) 
Select YK1.* From (
Select N'1e2b517c-2aa3-4a6c-8d67-01e605cde2ba' ID, N'Renkler' Menu, N'c3b90aac-17d3-4a72-990e-d7f7313ab061' as UstID, NULL icon, N'/Tanimlamalar/GrupKodu?grupKodu=Renkler' [url], 8 as sira, NULL as Aktif union all
Select N'cf1444df-16ce-4f3d-8467-055d5ca77697', N'Kasa Listesi', N'3c2482ee-faa6-4afe-a435-458729359650', NULL, N'/Tanimlamalar/KasaListe', 999, 1 union all
Select N'c71798da-7ea1-459f-adb2-069040d4c15f', N'Görev Ekle', N'5caa9948-f1c6-485f-866f-0ab77a22a865', NULL, N'/Gorev/GorevEkle', 1, NULL union all
Select N'1ff19826-abfa-4287-8bad-072730a442eb', N'Parametreler', N'996a04a2-109b-4c08-b3b4-b9c51bffc4c7', NULL, N'/Parametre/ParametreListesi', 1, NULL union all
Select N'01f3e12d-df13-40f7-8fe4-08e05de7b1a8', N'Ziyaret Tipleri', N'3bc5066b-441c-446a-977f-59d260727d28', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=Crm_ZiyaretTipi', 999, NULL union all
Select N'5caa9948-f1c6-485f-866f-0ab77a22a865', N'Görevler', N'2e8eb534-88c1-4b5a-bb71-5c7482815909', NULL, NULL, 3, NULL union all
Select N'141748c7-a257-47cd-a8e2-0bc6fd1ffc7e', N'Cari Grup Kodu 6', N'22448d0e-15d8-4a25-8c66-280e700d2ed8', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=CariGrupKod6', 7, NULL union all
Select N'92f1e4d3-a15b-439c-9b81-0d037d7b585e', N'Banka Hesabı Listesi', N'12e742ba-e071-4d0e-acde-8113a813aa59', NULL, N'/Tanimlamalar/BankaHesabiListe', 999, 1 union all
Select N'526ae9f1-8649-4673-8180-1014061b003a', N'Stok Grup Kodu 2', N'c3b90aac-17d3-4a72-990e-d7f7313ab061', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=StokGrupKod2', 3, NULL union all
Select N'85c91c48-bb30-465a-a105-10ba9fd547e0', N'Tanımlamalar', NULL, N'ki-duotone ki-text fs-2', NULL, 120, 1 union all
Select N'242f3450-5076-4417-90da-111b8a01cf51', N'Bankalar', N'd2377ac7-c725-474d-82e2-890c564671ee', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=Bankalar', 999, 1 union all
Select N'a01db815-a8fe-4bcf-9d60-11f575e92507', N'Mal Kabul', N'99394e88-400a-4c9b-89fd-3c898bb41883', NULL, NULL, 1, NULL union all
Select N'cbc0c711-561e-4026-aa76-1281841514b5', N'Genel Müdür Onayı', N'bbddbf6c-c674-466e-8d9a-bbef2aebc384', NULL, N'/Satinalma/YKSatinalma/Onay4', 4, NULL union all
Select N'ea142767-d50f-4545-8432-14615b7a54d5', N'Stok Grup Kodu 1', N'c3b90aac-17d3-4a72-990e-d7f7313ab061', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=StokGrupKod1', 2, NULL union all
Select N'3bc2a3b7-4675-40d7-aecf-16200a418736', N'Fatura Listesi', N'0a292e76-1efa-4b05-8d7b-f19bee40e2eb', NULL, N'/SatinalmaFatura/Liste/?Tip=AF', 999, 1 union all
Select N'18a490ab-36e6-4714-89b5-1634125d76d5', N'Banka', NULL, N'ki-duotone ki-bank fs-2', NULL, 999, 1 union all
Select N'd1782f6e-dd0e-4577-b1cb-1886f01fee56', N'Genel Müdür Yardımcısı Onayı', N'bbddbf6c-c674-466e-8d9a-bbef2aebc384', NULL, N'/Satinalma/YKSatinalma/Onay3', 3, NULL union all
Select N'43737efb-ec8b-42a5-8dd4-1922b0aa1b82', N'Liste', N'eab22fd0-121c-45fc-b979-47ac15cf0c91', NULL, N'/SatisTeklifi/Liste/?Tip=ST', 999, 1 union all
Select N'3ef9b850-3d5b-4f4c-bfd5-195bc157df34', N'Red Taleplerim', N'2d1c36a7-7301-4a43-997c-7bf3910f6c1e', NULL, N'/Satinalma/YKSatinalma/TalepOnayi', 3, NULL union all
Select N'c0fad745-bf89-4c32-b284-19fb688f8724', N'Satınalma', N'99394e88-400a-4c9b-89fd-3c898bb41883', NULL, NULL, 3, NULL union all
Select N'45dcc4df-7de0-4abe-962b-1d575e8ae297', N'Yeni Talep Belgeli', N'2d1c36a7-7301-4a43-997c-7bf3910f6c1e', NULL, N'/Satinalma/YKSatinalma/YeniTalepBelgeli/?Tip=Satinalma', -9, 1 union all
Select N'eb46fabc-c497-4529-8acf-24abb45f7730', N'Kullanıcı Listesi', N'9eaf836e-06ad-4b08-8170-690604b3ade3', NULL, N'/Kullanici/Liste', 2, NULL union all
Select N'89ebd818-5589-48b2-9c60-25ad48c5fe0a', N'Departman onayı', N'bbddbf6c-c674-466e-8d9a-bbef2aebc384', NULL, N'/Satinalma/YKSatinalma/Onay2', 2, NULL union all
Select N'43e66bad-3f9d-4a36-9c05-25bff27a8ef6', N'Satış İrsaliyesi', N'3c9c3adf-ae89-4c50-90fd-89fc7e40c671', NULL, NULL, 999, 1 union all
Select N'ba1babd6-594e-49d2-b0bf-2633ac9eabfd', N'B2B', NULL, N'ki-duotone ki-setting-2 fs-2', NULL, 110, 0 union all
Select N'22448d0e-15d8-4a25-8c66-280e700d2ed8', N'Cari Tanımlamaları', N'85c91c48-bb30-465a-a105-10ba9fd547e0', NULL, NULL, 3, NULL union all
Select N'58151b1c-7de6-49f4-9777-2ab7c1a35b4f', N'Cari Liste', N'6be04247-f925-41dd-bcb8-3e772aa2a512', NULL, N'/Cari/Liste', 2, NULL union all
Select N'e8e8f1a9-0a09-478f-814a-2b4b793b56fc', N'Yeni Talep', N'2d1c36a7-7301-4a43-997c-7bf3910f6c1e', NULL, N'/Satinalma/YKSatinalma/YeniTalep', 1, NULL union all
Select N'951a32f0-0269-41c8-bbe9-2fd12cde5ad4', N'Stok', NULL, N'ki-duotone ki-parcel-tracking fs-2', NULL, 30, 1 union all
Select N'84e23f5c-0603-47cc-9b91-30b9494b4e17', N'Stok', N'2e8eb534-88c1-4b5a-bb71-5c7482815909', NULL, NULL, 2, NULL union all
Select N'31e480f9-6560-4642-9762-33e9e6dcac8d', N'Yeni Satınalma Teklifi', N'c3bb5d84-3d8b-4e02-a235-755f2656e3b7', NULL, N'/SatinalmaTeklifi/Detay/?Tip=AT', 999, 1 union all
Select N'a2d4941a-dc93-4ba0-88f3-384abfd43625', N'Destek', N'ba2ab428-7e79-41f1-9a67-d648c199e914', N'', N'/D/Destek/AnaSayfa', 1, 1 union all
Select N'19a34913-f79d-4597-bd56-3a2b425b67b4', N'Depo Transferi', N'951a32f0-0269-41c8-bbe9-2fd12cde5ad4', NULL, N'/Belge/Liste/?Tip=DT', 2, 1 union all
Select N'6f74d6ea-5ce5-4854-84e8-3bb7a4da2f22', N'Döviz Birimleri', N'd2377ac7-c725-474d-82e2-890c564671ee', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=DovizBirimi', 3, 1 union all
Select N'99394e88-400a-4c9b-89fd-3c898bb41883', N'Satınalma', NULL, N'ki-duotone ki-basket-ok fs-2', NULL, 90, 1 union all
Select N'6be04247-f925-41dd-bcb8-3e772aa2a512', N'Cari', N'2e8eb534-88c1-4b5a-bb71-5c7482815909', N'', NULL, 1, NULL union all
Select N'38b18b11-c45a-4ad4-98c4-404f989b0a7e', N'Yeni Satış Talebi', N'31500c4a-2328-43ba-964d-b45f441717b8', NULL, N'/SatisTalebi/Detay/?Tip=STL', 999, 1 union all
Select N'20d6a5d0-0902-4a5a-98a8-4201a48dc6c3', N'Mail Ayarları', N'996a04a2-109b-4c08-b3b4-b9c51bffc4c7', NULL, N'/Parametre/MailAyarlari', 2, NULL union all
Select N'928d7eb4-365d-4365-8f4f-420b54c483dd', N'Satınalma Sözleşmeleri', N'c0fad745-bf89-4c32-b284-19fb688f8724', NULL, N'/Satinalma/YKSatinalma/CariSOzlesmeleri', 3, NULL union all
Select N'a93e04eb-b2de-4530-a5d4-43910b48ec31', N'Alış İrsaliyesi', N'a01db815-a8fe-4bcf-9d60-11f575e92507', NULL, N'/Belge/Liste/?Tip=AI', 1, NULL union all
Select N'3c2482ee-faa6-4afe-a435-458729359650', N'Kasa', NULL, N'ki-duotone ki-cube-2 fs-2', NULL, 999, 1 union all
Select N'ee2c2f13-629e-4b73-b1c1-46fc77777d22', N'Satınalma İrsaliyesi', N'3c9c3adf-ae89-4c50-90fd-89fc7e40c671', NULL, NULL, 999, 1 union all
Select N'eab22fd0-121c-45fc-b979-47ac15cf0c91', N'Satış Teklifi', N'1809459e-6cc9-495b-8e1b-b162313ffe42', NULL, NULL, 999, 1 union all
Select N'f4e32666-e638-4156-95b3-498bc3e5e453', N'Cari Hareket Tipi', N'22448d0e-15d8-4a25-8c66-280e700d2ed8', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=CariHareketTipi', 1, NULL union all
Select N'a68642f9-9a5a-4aef-a741-4f4537b039af', N'Yeni Satınalma İrsaliyesi', N'ee2c2f13-629e-4b73-b1c1-46fc77777d22', NULL, N'/SatinalmaIrsaliyesi/Detay/?Tip=AI', 999, 1 union all
Select N'cc4e01a5-c1d9-4500-89c9-4fe7ebc0f21f', N'Cari', NULL, N'ki-duotone ki-user fs-2', NULL, 20, 1 union all
Select N'53b79ead-56e6-46cd-82a8-52367f90d315', N'Satınalma Siparişi', N'10436d14-67b5-4ce8-80d8-5c689b7bb965', NULL, NULL, 999, 1 union all
Select N'9a78fbdb-6d59-404a-8a28-53737c387d21', N'Depolar', N'd2377ac7-c725-474d-82e2-890c564671ee', NULL, N'/Depo/Liste', 2, 1 union all
Select N'3880cedc-8c16-4783-8ae9-539926aeadcf', N'Yeni Satınalma Faturası', N'0a292e76-1efa-4b05-8d7b-f19bee40e2eb', NULL, N'/SatinalmaFatura/Detay/?Tip=AF', 999, 1 union all
Select N'a95bd013-3ea8-4da8-ab60-570763ecbfb2', N'Satınalma Fiyat Girişi', N'c0fad745-bf89-4c32-b284-19fb688f8724', NULL, N'/Satinalma/YKSatinalma/Satinalma', 1, NULL union all
Select N'36989579-a038-488f-a55d-57119f18d2d9', N'Taleplerim', N'2d1c36a7-7301-4a43-997c-7bf3910f6c1e', NULL, N'/Satinalma/YKSatinalma/Taleplerim', 2, NULL union all
Select N'6ef6e746-5f98-4085-8e8d-574f2223afb1', N'Stok Grup Kodu 3', N'c3b90aac-17d3-4a72-990e-d7f7313ab061', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=StokGrupKod3', 4, NULL union all
Select N'2e370180-c976-45e2-8aa4-5b10ce119ef6', N'Satış Siparişi Listesi', N'b422f138-9b95-4fea-8cc9-e4cab589e7ab', NULL, N'/SatisSiparisi/Liste/?Tip=SS', 999, 1 union all
Select N'10436d14-67b5-4ce8-80d8-5c689b7bb965', N'Sipariş', NULL, N'ki-duotone ki-basket fs-2 ', NULL, 60, 1 union all
Select N'2e8eb534-88c1-4b5a-bb71-5c7482815909', N'Ana Kayıtlar', NULL, N'ki-duotone ki-home-2 fs-2', NULL, 10, 1 union all
Select N'5a493bc9-53f7-44b1-85c2-6204bb7558db', N'Mail', NULL, N'ki-duotone ki-messages fs-2 ', NULL, 999, 1 union all
Select N'39375252-4117-46b4-ad55-65e0803da28e', N'Yeni Ziyaret Planla', N'7cae1caa-f416-4279-a42f-b40b59414eb1', NULL, N'~/Ziyaret/Ziyaret/Ziyaret', 2, 1 union all
Select N'7b3ad6b6-ad5b-4331-b04a-681d0db80236', N'Projeler', N'd2377ac7-c725-474d-82e2-890c564671ee', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=Projeler', 7, 1 union all
Select N'9eaf836e-06ad-4b08-8170-690604b3ade3', N'Kullanıcı Yönetimi', N'85c91c48-bb30-465a-a105-10ba9fd547e0', NULL, NULL, 1, NULL union all
Select N'8d6ad668-02a8-4043-9a85-6928e03da4d9', N'Yeni Satış İrsaliyesi', N'43e66bad-3f9d-4a36-9c05-25bff27a8ef6', NULL, N'/SatisIrsaliyesi/Detay/?Tip=SI', 999, 1 union all
Select N'a880549b-69d1-468f-a33f-69aea40dfcdd', N'Satınalma Raporu', N'dd2f712c-0b99-41d2-ac5e-976a2da2b23c', NULL, N'/Satinalma/YKSatinalma/TalepRaporu', 1, NULL union all
Select N'fd0cc63a-c09f-4088-8d10-6b22b6104aca', N'Stok Grup Kodu 6', N'c3b90aac-17d3-4a72-990e-d7f7313ab061', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=StokGrupKod6', 7, NULL union all
Select N'8369848f-7359-4e0b-91fa-6db030c4d780', N'İller', N'd2377ac7-c725-474d-82e2-890c564671ee', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=Il', 4, 1 union all
Select N'c5593890-3294-4ed4-8a51-6fbc44430152', N'Cari Grup Kodu 2', N'22448d0e-15d8-4a25-8c66-280e700d2ed8', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=CariGrupKod2', 3, NULL union all
Select N'771ea4bb-49e7-4b47-b4ee-70905ff9b8dc', N'Yeni Talep Listesi Belgeli', N'2d1c36a7-7301-4a43-997c-7bf3910f6c1e', NULL, N'/Satinalma/YKSatinalma/YeniTalepBelgeliListe/?Tip=Satinalma', -8, 1 union all
Select N'e9bd274f-0765-46ad-b11d-715e9b1b2d88', N'Satış Personeli Ekle', N'6ed74064-87c6-4716-9833-e1a30ed483c3', NULL, N'/SatisPersoneli/Ekle', 1, NULL union all
Select N'5cbf9675-6abf-4de9-b211-74340214a055', N'Tanimlama Ekle', N'85c91c48-bb30-465a-a105-10ba9fd547e0', NULL, N'/Tanimlamalar/Ekle', 999, 1 union all
Select N'c3bb5d84-3d8b-4e02-a235-755f2656e3b7', N'Satınalma Teklifi', N'1809459e-6cc9-495b-8e1b-b162313ffe42', NULL, NULL, 999, 1 union all
Select N'35b17349-7f4f-453f-8900-76b0995e31c5', N'Yeni Satış Fatura', N'9b2899c1-205e-4032-8cdc-8f26720caf7f', NULL, N'/SatisFatura/Detay/?Tip=SF', 999, 1 union all
Select N'dd01a517-5815-41d2-8d05-772d6f643740', N'Yeni Cari Hareket Kaydı', N'cc4e01a5-c1d9-4500-89c9-4fe7ebc0f21f', NULL, N'/Cari/YeniCariHareketKaydi', 1, NULL union all
Select N'258f1bd7-8e6c-46e9-b5f5-784627f1f999', N'Görev Listesi', N'5caa9948-f1c6-485f-866f-0ab77a22a865', NULL, N'/Gorev/GorevListe', 2, NULL union all
Select N'8a3e3d06-3f36-4598-9d5d-7b127c1fe9d7', N'Ziyaret Tipi', N'66780d18-28cc-4c97-aafd-f5458bde0d5d', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=Crm_ZiyaretTipi', 1, NULL union all
Select N'85d23ff2-bcfa-453a-83f9-7b4eeb684aa3', N'Liste', N'1f10d0e6-03f5-4e50-8bbf-88b1e1033a56', NULL, N'/SatinalmaTalebi/Liste/?Tip=ATL', 999, 1 union all
Select N'2d1c36a7-7301-4a43-997c-7bf3910f6c1e', N'Talep', N'99394e88-400a-4c9b-89fd-3c898bb41883', NULL, NULL, 2, NULL union all
Select N'8c9cd3b1-28ab-4dc7-897c-7cd90d505821', N'Yeni Satınalma Talebi', N'1f10d0e6-03f5-4e50-8bbf-88b1e1033a56', NULL, N'/SatinalmaTalebi/Detay/?Tip=ATL', 999, 1 union all
Select N'd9834fb7-62ee-4ebd-a21b-7cf81d8f6432', N'Döviz', N'85c91c48-bb30-465a-a105-10ba9fd547e0', NULL, NULL, 999, 1 union all
Select N'12e742ba-e071-4d0e-acde-8113a813aa59', N'Banka Hesapları', N'18a490ab-36e6-4714-89b5-1634125d76d5', NULL, N'', 999, 1 union all
Select N'56c9f9d2-1d0f-4e31-a240-82c585b2474c', N'Ülkeler', N'd2377ac7-c725-474d-82e2-890c564671ee', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=Ulke', 5, 1 union all
Select N'3c1fb264-b879-439e-afbe-870b1047a80b', N'Cari Grup Kodu 4', N'22448d0e-15d8-4a25-8c66-280e700d2ed8', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=CariGrupKod4', 5, NULL union all
Select N'4c704014-3a35-44d8-a192-87cde1dcf8e6', N'Stok Ekle', N'84e23f5c-0603-47cc-9b91-30b9494b4e17', NULL, N'/Stok/Ekle', 1, NULL union all
Select N'1f10d0e6-03f5-4e50-8bbf-88b1e1033a56', N'Satınalma Talebi', N'fcc10e54-89b8-474b-b8f5-9f1352b1fab3', NULL, NULL, 999, 1 union all
Select N'd2377ac7-c725-474d-82e2-890c564671ee', N'Genel Tanımlamalar', N'85c91c48-bb30-465a-a105-10ba9fd547e0', N'ki-duotone ki-address-book fs-2', NULL, 2, NULL union all
Select N'2c5932a2-787c-402b-8132-89dd26e28ed3', N'Yeni Kullanıcı', N'9eaf836e-06ad-4b08-8170-690604b3ade3', NULL, N'/Kullanici/Ekle', 1, NULL union all
Select N'3c9c3adf-ae89-4c50-90fd-89fc7e40c671', N'İrsaliye', NULL, N'ki-duotone ki-questionnaire-tablet fs-2', NULL, 70, 1 union all
Select N'c14b7648-10aa-46ab-84ba-8be7d2a8f5ac', N'Liste', N'31500c4a-2328-43ba-964d-b45f441717b8', NULL, N'/SatisTalebi/Liste/?Tip=STL', 999, 1 union all
Select N'a90e66f7-5f7e-4f4b-b5a4-8df922b244a0', N'Satınalma Onayı', N'bbddbf6c-c674-466e-8d9a-bbef2aebc384', NULL, N'/Satinalma/YKSatinalma/Onay1', 1, NULL union all
Select N'112fd30c-6a4c-4475-b190-8e3651ca45ef', N'Satış İrsaliyesi Listesi', N'43e66bad-3f9d-4a36-9c05-25bff27a8ef6', NULL, N'/SatisIrsaliyesi/Liste/?Tip=SI', 999, 1 union all
Select N'9b2899c1-205e-4032-8cdc-8f26720caf7f', N'Satış Faturası', N'4307e889-3ce4-4d6b-9b85-b37ab5150fa3', NULL, NULL, 999, 1 union all
Select N'7c3c0f75-97a2-4788-830d-966624c76bc0', N'Cari Grup Kodu 1', N'22448d0e-15d8-4a25-8c66-280e700d2ed8', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=CariGrupKod1', 2, NULL union all
Select N'dd2f712c-0b99-41d2-ac5e-976a2da2b23c', N'Rapor', N'99394e88-400a-4c9b-89fd-3c898bb41883', NULL, NULL, 5, NULL union all
Select N'2298a7fa-6926-47a8-814b-9837d02968dc', N'Yeni Stok Hareket Kaydı', N'951a32f0-0269-41c8-bbe9-2fd12cde5ad4', NULL, N'/Stok/YeniStokHareketKaydi', 1, NULL union all
Select N'4baf9145-3d58-436e-9ea6-990b7856f3f4', N'Stok Grup Kodu 5', N'c3b90aac-17d3-4a72-990e-d7f7313ab061', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=StokGrupKod5', 6, NULL union all
Select N'2c0eb690-aa23-445d-a93b-9ef5e24590c6', N'Satınalma Fiyat Teklif Listesi', N'c0fad745-bf89-4c32-b284-19fb688f8724', NULL, N'/Satinalma/YKSatinalma/Teklifler', 2, NULL union all
Select N'fcc10e54-89b8-474b-b8f5-9f1352b1fab3', N'Talep', NULL, N'ki-duotone ki-plus-square fs-2 ', NULL, 40, 1 union all
Select N'de8b1b7d-2922-43c6-bc3e-a258586463b9', N'Yeni Satış Siparişi', N'b422f138-9b95-4fea-8cc9-e4cab589e7ab', NULL, N'/SatisSiparisi/Detay/?Tip=SS', 999, 1 union all
Select N'ec21b9ca-450a-4b11-99ce-a718c26e241d', N'Mail Kalıpları Listesi', N'5a493bc9-53f7-44b1-85c2-6204bb7558db', NULL, N'/MailKaliplari/Liste', 999, 1 union all
Select N'896e48d2-639c-4139-b1e4-a7777896131c', N'Satış Personeli Listesi', N'6ed74064-87c6-4716-9833-e1a30ed483c3', NULL, N'/SatisPersoneli/Liste', 2, NULL union all
Select N'cbccf70b-6c42-40ae-ae1d-a803f173da99', N'Banka Hesabı Ekle', N'12e742ba-e071-4d0e-acde-8113a813aa59', NULL, N'/Tanimlamalar/BankaHesabiEkle', 999, 1 union all
Select N'47e1db12-85d8-4288-98a5-ae0ac4b66a88', N'Satınalma İrsaliye Listesi', N'ee2c2f13-629e-4b73-b1c1-46fc77777d22', NULL, N'/SatinalmaIrsaliyesi/Liste/?Tip=AI', 999, 1 union all
Select N'38243b86-d3c7-4999-8a2a-aeb3c80c71be', N'Kaliteler', N'c3b90aac-17d3-4a72-990e-d7f7313ab061', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=Kaliteler', 11, NULL union all
Select N'3f858e59-3a76-4098-886a-b031bd4a6586', N'Markalar', N'c3b90aac-17d3-4a72-990e-d7f7313ab061', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=Markalar', 10, NULL union all
Select N'1809459e-6cc9-495b-8e1b-b162313ffe42', N'Teklif ', NULL, N'ki-duotone ki-delivery-3 fs-2', NULL, 50, 1 union all
Select N'4307e889-3ce4-4d6b-9b85-b37ab5150fa3', N'Fatura', NULL, N'ki-duotone ki-book fs-2', NULL, 80, 1 union all
Select N'7cae1caa-f416-4279-a42f-b40b59414eb1', N'CRM', NULL, N'ki-duotone ki-screen fs-2', NULL, 115, 1 union all
Select N'31500c4a-2328-43ba-964d-b45f441717b8', N'Satış Talebi', N'fcc10e54-89b8-474b-b8f5-9f1352b1fab3', NULL, NULL, 999, 1 union all
Select N'aaec5fcf-685b-4059-95e1-b927d956bd9b', N'Yönetim Onayı', N'bbddbf6c-c674-466e-8d9a-bbef2aebc384', NULL, N'/Satinalma/YKSatinalma/Onay5', 5, NULL union all
Select N'996a04a2-109b-4c08-b3b4-b9c51bffc4c7', N'Ayarlar', NULL, N'ki-duotone ki-setting-2 fs-2', NULL, 130, 1 union all
Select N'bbddbf6c-c674-466e-8d9a-bbef2aebc384', N'Onay', N'99394e88-400a-4c9b-89fd-3c898bb41883', NULL, NULL, 4, NULL union all
Select N'4f95b3de-2351-4798-90ef-bdcde277f71f', N'Cari Grup Kodu 5', N'22448d0e-15d8-4a25-8c66-280e700d2ed8', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=CariGrupKod5', 6, NULL union all
Select N'73f27091-6642-4f25-a0a6-c05d2c129027', N'Stok Listesi', N'84e23f5c-0603-47cc-9b91-30b9494b4e17', NULL, N'/Stok/Liste', 2, NULL union all
Select N'6818277b-5c00-44df-bbf1-c53d054301f9', N'Stok Hareket Tipi', N'c3b90aac-17d3-4a72-990e-d7f7313ab061', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=StokHareketTipi', 1, NULL union all
Select '5b88198f-194b-4c60-9c9c-c5db0d94ab36', N'Yeni satınalma Siparişi', N'53b79ead-56e6-46cd-82a8-52367f90d315', NULL, N'/SatinalmaSiparisi/Detay/?Tip=AS', 999, 1 union all
Select N'9ccf7517-bbf2-471c-ac3a-c5ffb2b36cfc', N'Bedenler', N'c3b90aac-17d3-4a72-990e-d7f7313ab061', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=Bedenler', 9, NULL union all
Select N'ba2ab428-7e79-41f1-9a67-d648c199e914', N'Destek Sistemi', NULL, N'ki-duotone ki-delivery-24 fs-2', NULL, 100, 1 union all
Select N'e0d3534e-9265-49fa-9c10-d66d1c2b5a6d', N'Liste', N'c3bb5d84-3d8b-4e02-a235-755f2656e3b7', NULL, N'/SatinalmaTeklifi/Liste/?Tip=AT', 999, 1 union all
Select N'e2bf6e36-f5db-4db6-9377-d6f0ae5ad2a1', N'Görev Tipi', N'66780d18-28cc-4c97-aafd-f5458bde0d5d', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=GorevTipi', 2, 1 union all
Select N'775b75ad-d7c3-4ce8-9897-d74f72e66bb2', N'Cari Grup Kodu 3', N'22448d0e-15d8-4a25-8c66-280e700d2ed8', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=CariGrupKod3', 4, NULL union all
Select N'2fc68eca-fd50-4fee-8bdc-d7552e17e483', N'Ziyaret Takibi', N'7cae1caa-f416-4279-a42f-b40b59414eb1', NULL, N'~/Ziyaret/Ziyaret/AnaSayfa', 1, 1 union all
Select N'4a7b5067-ebf0-40a5-96ae-d7b28611b796', N'Kasa Ekle', N'3c2482ee-faa6-4afe-a435-458729359650', NULL, N'/Tanimlamalar/KasaEkle', 999, 1 union all
Select N'c3b90aac-17d3-4a72-990e-d7f7313ab061', N'Stok Tanımlamaları', N'85c91c48-bb30-465a-a105-10ba9fd547e0', NULL, NULL, 4, NULL union all
Select N'51b19120-0b79-4aa3-98d0-dc2552797f40', N'Stok Sayım', N'951a32f0-0269-41c8-bbe9-2fd12cde5ad4', NULL, N'/Stok/Sayim', 5, 1 union all
Select N'1da5d037-720b-4351-9b8f-dd03479ea243', N'Stok Hareket Listesi', N'951a32f0-0269-41c8-bbe9-2fd12cde5ad4', NULL, N'/Stok/HareketListesi', 2, NULL union all
Select N'e72037fd-21cf-4ee4-b81b-dd6c6fdf20fa', N'Mail Ekle', N'5a493bc9-53f7-44b1-85c2-6204bb7558db', NULL, N'/MailKaliplari/Ekle', 999, 1 union all
Select N'df921643-d386-4d38-a09a-dd85c21246df', N'Takvim Durumları', N'd2377ac7-c725-474d-82e2-890c564671ee', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=AnaSayfaTakvimDurumlari', 6, 1 union all
Select N'836d060c-a1ed-4e62-9e7f-df9f77240b44', N'Stok Grup Kodu 4', N'c3b90aac-17d3-4a72-990e-d7f7313ab061', NULL, N'/Tanimlamalar/GrupKodu?grupKodu=StokGrupKod4', 5, NULL union all
Select N'5eab73c5-97fa-453c-b153-e12ecb043beb', N'Satınalma Siparişi Listesi ', N'53b79ead-56e6-46cd-82a8-52367f90d315', NULL, N'/SatinalmaSiparisi/Liste/?Tip=AS', 999, 1 union all
Select N'6ed74064-87c6-4716-9833-e1a30ed483c3', N'Satış Personelleri', N'85c91c48-bb30-465a-a105-10ba9fd547e0', NULL, NULL, 5, NULL union all
Select N'e50449f5-6239-4712-a2c7-e3ec84cd7d7a', N'Yeni Satış Teklifi', N'eab22fd0-121c-45fc-b979-47ac15cf0c91', NULL, N'/SatisTeklifi/Detay/?Tip=ST', 999, 1 union all
Select N'b422f138-9b95-4fea-8cc9-e4cab589e7ab', N'Satış Siparişi', N'10436d14-67b5-4ce8-80d8-5c689b7bb965', NULL, NULL, 999, 1 union all
Select N'8e3b2f4a-2238-4ae2-83ea-e528d7d6964b', N'Logo B2B', N'ba1babd6-594e-49d2-b0bf-2633ac9eabfd', NULL, N'/B2BLogo/B2BLogo/AnaSayfa', 999, NULL union all
Select N'4a992577-aae1-424c-bd41-e5758e418aaa', N'Fatura Listesi', N'9b2899c1-205e-4032-8cdc-8f26720caf7f', NULL, N'/SatisFatura/Liste/?Tip=SF', 1, 1 union all
Select N'528cf2f1-369a-49aa-9088-e62833c4b91d', N'Cari Hareket Listesi', N'cc4e01a5-c1d9-4500-89c9-4fe7ebc0f21f', NULL, N'/Cari/HareketListesi', 2, NULL union all
Select N'12680518-371d-4f4e-b428-ea141f748d29', N'Şubeler', N'd2377ac7-c725-474d-82e2-890c564671ee', NULL, N'/Sube/Liste', 1, 1 union all
Select N'0a292e76-1efa-4b05-8d7b-f19bee40e2eb', N'Satınalma Faturası', N'4307e889-3ce4-4d6b-9b85-b37ab5150fa3', NULL, NULL, 999, 1 union all
Select N'7a6a90ad-6794-4fa6-8fba-f29a129bf1b3', N'Cari Ekle', N'6be04247-f925-41dd-bcb8-3e772aa2a512', NULL, N'/Cari/Ekle', 1, NULL union all
Select N'66780d18-28cc-4c97-aafd-f5458bde0d5d', N'CRM', N'85c91c48-bb30-465a-a105-10ba9fd547e0', NULL, NULL, 6, 1 union all
Select N'5377fa97-cedc-461e-8c44-fca57570e4fc', N'Döviz Birimi Ekle', N'd9834fb7-62ee-4ebd-a21b-7cf81d8f6432', NULL, N'/Tanimlamalar/DovizBirimiEkle', 999, 1 union all
Select N'701fc11c-6f47-4aa2-9ca1-ffdc553cc32d', N'Döviz Birimi Listesi', N'd9834fb7-62ee-4ebd-a21b-7cf81d8f6432', NULL, N'/Tanimlamalar/DovizBirimiListe', 999, 1
) YK1
LEFT OUTER JOIN Menuler WITH(NOLOCK) ON Menuler.ID = YK1.ID
Where Menuler.ID IS NULL


INSERT [dbo].[ParametrelerStandart] ([ID], [Modul], [Isim], [Deger], [Tip], [Kategori]) 
Select YK1.* from (
Select N'efbfb8d5-e00e-42bc-8434-025a855a165e' ID, N'EMail' Modul, N'Host' Isim, N'mail.ykyazilim.com.tr' Deger, N'text' Tip, N'Genel E-Mail' Kategori union all
Select N'a12cfac5-1f05-4a65-9216-15bd316a4dc5', N'AktivasyonMailIcerigi', N'Aktivasyon Mail İçeriği', N'<h2>Merhaba {ad} {soyad},</h2><br/><strong>Hesabınızı doğrulayarak YK ERP’yi kullanmaya başlayın.</strong><br/><br/>Hesabınızı doğrulamak için <a href="https://app.ykyazilim.com.tr/Aktivasyon/KullaniciOnayla/?kullaniciid={id}">tıklayın</a>', N'textarea', N'Genel' union all
Select N'845c60b5-3979-401f-8d69-16868f9b88a9', N'Stok', N'Stok Kodu - Ön Değeri', N'YK-', N'text', N'Genel' union all
Select N'15e751dc-f9a9-4a0a-9fe2-2b448af5b9bb', N'EMail', N'Port', N'587', N'text', N'Genel E-Mail' union all
Select N'b0a9afc8-1766-498e-a3b2-315a976bdfcc', N'Stok', N'Otomatik Stok Kodu Üret', N'1', N'checkbox', N'Genel' union all
Select N'07160d1c-fb6b-4269-ae99-3aa78ff2b74b', N'AnaSayfaAktifCari', N'Aktif Cari Gözüksün', N'0', N'checkbox', N'Ana Sayfa' union all
Select N'7222e51a-cf4e-4875-a281-3ce5b91a77c7', N'EMail', N'SSL', N'0', N'checkbox', N'Genel E-Mail' union all
Select N'44433082-ad05-4eee-88a5-46c85cc404d4', N'AnaSayfaAktifStok', N'Aktif Stok Gözüksün', N'0', N'checkbox', N'Ana Sayfa' union all
Select N'21f5da95-866d-413f-9671-4d2dc15b0a71', N'AnaSayfaAktifSiparis', N'Aktif Sipariş Gözüksün', N'0', N'checkbox', N'Ana Sayfa' union all
Select N'b7404295-b43e-4fb4-8905-62e32692cdf5', N'EMail', N'KullaniciAdi', N'info@ykyazilim.com.tr', N'text', N'Genel E-Mail' union all
Select N'f95f0138-9013-4e6f-9eb3-7133243b8885', N'EMail', N'Isim', N'YK YAZILIM', N'text', N'Genel E-Mail' union all
Select N'37c5cef8-fb5d-4ed3-92af-72f635e43972', N'EMail', N'Parola', N'', N'text', N'Genel E-Mail' union all
Select N'a2ed298f-9aa7-499e-a39f-888f5279800a', N'Stok', N'Varsayılan Kdv Oranı', N'20', N'text', N'Fiyat' union all
Select N'37086db3-c47b-48c8-b83c-915254d0968e', N'AnaSayfaBekleyenGorevSayisi', N'Bekleyen Görev Sayısı Gözüksün', N'0', N'checkbox', N'Ana Sayfa' union all
Select N'9a30cf98-2963-4057-9bfe-9952ceef8f1d', N'AnaSayfaTakvim', N'Takvim Gözüksün', N'0', N'checkbox', N'Ana Sayfa' union all
Select N'3ec921f1-699b-48bc-bfad-a0049f4769d4', N'Cari', N'Otomatik Cari Kodu Üret', N'1', N'checkbox', N'Genel' union all
Select N'bb7a0290-f7e0-49a8-bf74-a72bf515d234', N'NetsisDatabase', N'Netsis Database', N'', N'text', N'Genel' union all
Select N'1ab0c5f8-8250-4da2-b786-e0e8600e09cf', N'AnaSayfaSonAktiviteler', N'Son Aktiviteler Gözüksün', N'0', N'checkbox', N'Ana Sayfa' union all
Select N'de5e7885-3884-4049-bc0b-ee5d88199ea9', N'Cari', N'Cari Kodu - Ön Değeri', N'120-', N'text', N'Genel' union all
Select N'de5e7885-3884-4049-bc0b-ee5d88199ea8', N'Uygulama', N'Uygulama', N'', N'text', N'Genel'
) YK1
LEFT OUTER JOIN ParametrelerStandart WITH(NOLOCK) ON ParametrelerStandart.ID = YK1.ID
Where ParametrelerStandart.ID IS NULL

INSERT [dbo].[Subeler] ([ID], [UyelikID], [Firma], [Kod], [Isim], [Adres], [Telefon], [KayitTarihi], [KayitYapanKullanici])
Select 
N'87fb7d65-6e06-4147-a43e-0f2aa394e23c', @UyelikID, N'MERKEZ', N'MERKEZ', N'MERKEZ', N'', N'',GETDATE(), @KullaniciID
Where (select COUNT(*) from [Subeler] WITH(NOLOCK) Where ID = N'87fb7d65-6e06-4147-a43e-0f2aa394e23c') = 0


INSERT [dbo].[UyelikPaketleri] ([ID], [Isim], [Ay], [Tutar], [ResimUrl], [Aciklama]) 
Select YK1.* From (
Select N'09cf5637-326a-41ed-aefe-6b3e7e34c1cf'ID, N'Aylık Eko Paket (1 Aylık)' Isim, 1 Ay, CAST(200.00 AS Decimal(18, 2)) Tutar, NULL ResimUrl, N'<strong></strong>' Aciklama union all
Select N'24bd361b-47d8-470f-8687-8013ee08d90b', N'Yıllık Eko Paket (12 Paket) %16,6 İndirim', 12, CAST(2000.00 AS Decimal(18, 2)), NULL, N'<strong></strong>'
) YK1 
LEFT OUTER JOIN UyelikPaketleri WITH(NOLOCK) ON UyelikPaketleri.ID = YK1.ID
Where UyelikPaketleri.ID IS NULL

