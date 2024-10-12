
function kaydet(controllerName, tip) {
    // Belge numarası ve cari seçimi kontrolleri
    if ($("#BelgeNo").val() === "" && tip === "AI") {
        alert("Lütfen belge numarası yazınız.");
        return;
    }

    // Kalemler verisinin toplanması
    var kalemler = [];
    $('#TabloKalemler .tablo-satir').each(function () {
        var satirID = $(this).attr("data-ID2");
        var kalemId = $(this).attr("data-id");
        if (satirID) {
            kalemler.push({
                ID: kalemId,
                StokID: $("#StokID_" + satirID).val(),
                Seri: $("#Seri_" + satirID).val(),
                Miktar: $("#Miktar_" + satirID).val(),
                Fiyat: $("#Fiyat_" + satirID).val(),
                KdvOrani: $('#KdvOrani_' + satirID).val(),
                IskontoOrani1: $('#IskontoOrani1_' + satirID).val()
            });
        }
    });

    // Kalemler boşsa uyarı ver
    if (kalemler.length === 0) {
        alert("Lütfen kalem giriniz.");
        return;
    }

    // AJAX isteği için veri hazırlığı
    var _data = {
        Tip: tip,
        ID: $("#ID").val(),
        BelgeNo: $('#BelgeNo').val(),
        Tarih: $('#Tarih').val(),
        Aciklama: $('#Aciklama').val(),
        Kalemler: kalemler // Kalemler burada atanıyor
    };

    // Optional olarak eklenecek alanlar (belgeEkleCompenentValue kontrollerine bağlı)
    if (belgeEkleCompenentValue.CariVarMi) {
        _data.CariID = $("#CariID option:selected").val();
    }

    if (belgeEkleCompenentValue.CikisDepoVarMi) {
        _data.DepoCikisID = $("#DepoCikisID option:selected").val();
    }

    if (belgeEkleCompenentValue.SatisPersoneliVarMi) {
        _data.SatisPersonelID = $("#SatisPersonelID option:selected").val();
    }

    if (belgeEkleCompenentValue.DurumuVarMi) {
        _data.Durumu = $("#Durumu option:selected").val(); // Düzeltme: Durumu'ya atanmalı
    }

    // AJAX isteği
    $.ajax({
        url: `/${controllerName}/Kaydet`,
        data: JSON.stringify(_data),
        contentType: "application/json",
        dataType: "json",
        type: "POST",
        success: function (data) {
            alert("Belge Kaydedildi!");
            window.location.href = `/${controllerName}/Duzenle?id=` + data.Data + "&Tip=" + tip; // Aynı sekmede yönlendirme
        },
        error: function (msg) {
            console.log(msg);
        }
    });
}

$(".deleteBtn").click(function () {
    $(this).closest("tr").remove();
    ToplamTutarHesapla();
    KdvTutarHesapla();
    IskontoTutarHesapla();
    AraToplamHesapla();
});

function SatirSil(SatirID) {
    $("#TR_" + SatirID).remove();
    ToplamTutarHesapla();
    KdvTutarHesapla();
    IskontoTutarHesapla();
    AraToplamHesapla();
}


//Binlik ayıracı eklemek için---------
function TutarHesapla(miktarid, fiyatid, tutarid) {
    var kdvInput = $('#KdvOrani_' + miktarid.split('_')[1]);
    var iskontoInput = $('#IskontoOrani1_' + miktarid.split('_')[1]);

    var miktar = parseFloat($("#" + miktarid).val().replace(/\./g, '').replace(',', '.')); // Tüm noktaları kaldır, ilk virgülü noktaya çevir
    var fiyat = parseFloat($("#" + fiyatid).val().replace(/\./g, '').replace(',', '.'));   // Tüm noktaları kaldır, ilk virgülü noktaya çevir

    if (isNaN(miktar)) miktar = 0;  // Eğer miktar bir sayı değilse, 0 yap
    if (isNaN(fiyat)) fiyat = 0;    // Eğer fiyat bir sayı değilse, 0 yap

    var tutar = miktar * fiyat;  // Tutarı hesapla

    var kdv = parseFloat($(kdvInput).val());
    var iskonto = parseFloat($(iskontoInput).val());

    if (isNaN(kdv)) kdv = 0;       // Eğer KDV bir sayı değilse, 0 yap
    if (isNaN(iskonto)) iskonto = 0; // Eğer iskonto bir sayı değilse, 0 yap

    // Tutar hesaplaması KDV ve İskonto uygulanarak
    var finalTutar = tutar - (tutar * iskonto / 100);

    // Formatla ve inputa yaz
    $("#" + tutarid).val(formatNumberWithSeparator(finalTutar));

    // Diğer hesaplamaları yap
    ToplamTutarHesapla();
    KdvTutarHesapla();
    IskontoTutarHesapla();
    AraToplamHesapla();
}

function formatNumberWithSeparator(number) {
    // Sayıyı sabit ondalık ve binlik ayracı ekleyerek formatla
    return number.toLocaleString('tr-TR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
//---------Binlik ayıracı eklemek için
function ToplamTutarHesapla() {
    var toplamTutar = 0;
    $('[data-kt-element="tutar-data"]').each(function (index, input) {
       
        var kdvData = parseFloat($(input).closest('[data-ID2]').find('[data-kt-element="kdv-data"]').val());
        if ($(input).val().includes(',')) {
            toplamTutar = toplamTutar + parseFloat($(input).val().replace(".", "").replace(",", ".")) + (kdvData * parseFloat($(input).val().replace(".", "").replace(",", ".")) / 100);
        } else {
            toplamTutar = toplamTutar + parseFloat($(input).val()) + (kdvData * parseFloat($(input).val()));
        }
    });
    console.log(toplamTutar);

    $('[data-kt-element="toplam-tutar"]').text(formatNumberWithSeparator(toplamTutar));
}

function KdvTutarHesapla() {
    var kdvTutari = 0;
    $('[data-kt-element="kdv-data"]').each(function (index, input) {

        var kdvId = $(input).attr('id');
        var kdv = parseFloat($(input).val());
        var tutar = parseFloat($("#Tutar_" + kdvId.split("_")[1]).val());

        kdvTutari = kdvTutari + kdv * tutar / 100;
    });

    $('[data-kt-element="kdv-tutari"]').text(formatNumberWithSeparator(kdvTutari));
}

function IskontoTutarHesapla() {
    var iskontoTutari = 0;
    $('[data-kt-element="iskonto-data"]').each(function (index, input) {
        var iskontoId = $(input).attr('id');
        var iskonto = parseFloat($(input).val());
        var miktar = parseFloat($("#Miktar_" + iskontoId.split("_")[1]).val());
        var fiyat = parseFloat($("#Fiyat_" + iskontoId.split("_")[1]).val());

        iskontoTutari = iskontoTutari + iskonto * miktar * fiyat / 100;
    });

    $('[data-kt-element="iskonto-tutari"]').text(formatNumberWithSeparator(iskontoTutari));
}

function AraToplamHesapla() {
    var araToplam = 0;
    $('[data-kt-element="iskonto-data"]').each(function (index, input) {
        var iskontoId = $(input).attr('id');
        var miktar = parseFloat($("#Miktar_" + iskontoId.split("_")[1]).val());
        var fiyat = parseFloat($("#Fiyat_" + iskontoId.split("_")[1]).val());
        araToplam = araToplam + miktar * fiyat;
    });

    $('[data-kt-element="ara-toplam-tutari"]').text(formatNumberWithSeparator(araToplam));
}

$(function () {

    const CariIDSelect = $('#CariID').select2({
        ajax: {
            url: "/Cari/SelectListe",
            data: function (params) {
                var query = {
                    search: params.term,
                }

                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (data) {
                var results = [];
                if (data && data.length > 0) {
                    data.forEach(function (item) {
                        results.push({ id: item.ID, text: item.Isim })
                    });
                }

                return {
                    results: results
                };
            }
        },
        delay: 250,
        minimumInputLength: 3,
        language: {
            placeholder: function () {
                return "Arama yapmak için en az 3 karakter girin";
            },
            inputTooShort: function () {
                return "En az lütfen en az 3 karakter girin";
            }
        }
    });


});

function StokAramaOlustur(stokid) {
    $(function () {

        const StokIDSelect = $('#' + stokid).select2({
            ajax: {
                url: "/Stok/SelectListe",
                data: function (params) {
                    var query = {
                        search: params.term,
                    }

                    // Query parameters will be ?search=[term]&type=public
                    return query;
                },
                processResults: function (data) {
                    var results = [];
                    if (data && data.length > 0) {
                        data.forEach(function (item) {
                            results.push({ id: item.ID, text: item.Isim })
                        });
                    }

                    return {
                        results: results
                    };
                }
            },
            delay: 250,
            minimumInputLength: 3,
            language: {
                placeholder: function () {
                    return "Arama yapmak için en az 3 karakter girin";
                },
                inputTooShort: function () {
                    return "En az lütfen en az 3 karakter girin";
                }
            }
        });


    });
}


function YeniSatirAc(Tip, IsMobile) {
    var YeniID = IDGuidOlustur().replace(/-/g, "");
    var deger = "";

    if (IsMobile == "1") {
        // Mobile view için yeni satır oluşturma
        deger += `<div class="row pb-4 tablo-satir" data-ID=''  data-ID2='${YeniID}' id='TR_${YeniID}' name='TR_${YeniID}'>`;

        // Ürün Seçimi
        deger += `
            <div class="col-12 col-lg-2 d-flex flex-column">
                <label style="font-weight: bold; margin-bottom: 5px;">Ürün</label>
                <select class='form-select form-select-solid p-2 rounded-1' name='StokID_${YeniID}' id='StokID_${YeniID}' style='width:100%;' data-control='select2' data-placeholder='Lütfen Seçim Yapınız'>
                </select>
            </div>
        `;

        // Seri Girişi
        if (Tip === 'DT') {
            deger += `
                <div class="col-12 col-lg-1 d-flex flex-column">
                    <label style="font-weight: bold; margin-bottom: 5px;">Seri</label>
                    <input type='text' name='Seri_${YeniID}' id='Seri_${YeniID}' value='' class='form-control p-2 me-10' />
                </div>
            `;
        } else {
            deger += `
                <div class="col-12 col-lg-1 d-flex flex-column">
                    <label style="font-weight: bold; margin-bottom: 5px;">Seri</label>
                    <select class='form-select form-select-solid p-2 rounded-1' name='Seri_${YeniID}' id='Seri_${YeniID}' data-control='select2' data-placeholder='Lütfen Seçim Yapınız'>
                    </select>
                </div>
            `;
        }

        // Diğer Bilgiler (Birim, KDV, Miktar, Fiyat, İskonto, Tutar)
        deger += `
            <div class="col-12 col-lg-1 d-flex flex-column mb-2 mb-lg-0">
                <label style="font-weight: bold; margin-bottom: 5px;">Birim</label>
                <input type='text' name='OlcuBirimi_${YeniID}' id='OlcuBirimi_${YeniID}' value='Adet' class='form-control p-2 me-10' />
            </div>
            <div class="col-12 col-lg-1 d-flex flex-column mb-2 mb-lg-0">
                <label style="font-weight: bold; margin-bottom: 5px;">KDV Oranı</label>
                <input type='text' data-component='money-input' data-kt-element="kdv-data" onchange="TutarHesapla('Miktar_${YeniID}','Fiyat_${YeniID}','Tutar_${YeniID}'); formatNumberWithSeparator('Miktar_${YeniID}');" name='KdvOrani_${YeniID}' id='KdvOrani_${YeniID}' value='0' class='form-control p-2 me-10' style='text-align:right;' onchange='TutarHesapla("Miktar_${YeniID}","Fiyat_${YeniID}","Tutar_${YeniID}");'/>
            </div>
            <div class="col-12 col-lg-1 d-flex flex-column mb-2 mb-lg-0">
                <label style="font-weight: bold; margin-bottom: 5px;">Miktar</label>
                <input type='text' data-component='money-input' onchange="TutarHesapla('Miktar_${YeniID}','Fiyat_${YeniID}','Tutar_${YeniID}'); formatNumberWithSeparator('Miktar_${YeniID}');" name='Miktar_${YeniID}' id='Miktar_${YeniID}' value='1' class='form-control p-2 me-10' style='text-align:right;' onchange='TutarHesapla("Miktar_${YeniID}","Fiyat_${YeniID}","Tutar_${YeniID}"); formatNumberWithSeparator("Miktar_${YeniID}");'/>
            </div>
            <div class="col-12 col-lg-2 d-flex flex-column mb-2 mb-lg-0">
                <label style="font-weight: bold; margin-bottom: 5px;">Fiyat</label>
                <input type='text' data-component='money-input' onchange="TutarHesapla('Miktar_${YeniID}','Fiyat_${YeniID}','Tutar_${YeniID}'); formatNumberWithSeparator('Miktar_${YeniID}');" name='Fiyat_${YeniID}' id='Fiyat_${YeniID}' value='0' class='form-control p-2 me-10' style='text-align:right;' onchange='TutarHesapla("Miktar_${YeniID}","Fiyat_${YeniID}","Tutar_${YeniID}"); formatNumberWithSeparator("Fiyat_${YeniID}");'/>
            </div>
            <div class="col-12 col-lg-1 d-flex flex-column mb-2 mb-lg-0">
                <label style="font-weight: bold; margin-bottom: 5px;">İskonto Oranı</label>
                <input type='text' data-component='money-input' data-kt-element="iskonto-data" onchange="TutarHesapla('Miktar_${YeniID}','Fiyat_${YeniID}','Tutar_${YeniID}'); formatNumberWithSeparator('Miktar_${YeniID}');" name='IskontoOrani1_${YeniID}' id='IskontoOrani1_${YeniID}' value='0' class='form-control p-2 me-10' style='text-align:right;' onchange='TutarHesapla("Miktar_${YeniID}","Fiyat_${YeniID}","Tutar_${YeniID}");'/>
            </div>
            <div class="col-12 col-lg-2 d-flex flex-column pb-2 mb-2 mb-lg-0">
                <label style="font-weight: bold; margin-bottom: 5px;">Tutar</label>
                <input type='text' data-component='money-input' data-kt-element="tutar-data" name='Tutar_${YeniID}' id='Tutar_${YeniID}' value='0' class='form-control p-2 me-10' style='text-align:right;' readonly />
            </div>
        `;

        
        deger += `
                <div class="col-12 col-lg-1 d-flex flex-column align-items-end justify-content-end pt-4 pt-lg-0">
                    <div>
                        <button type="button" class="deleteBtn btn-sm" onclick='SatirSil("${YeniID}")' data-ID2='${YeniID}'>Sil</button>
                    </div>
                </div>
        `;
        deger += `</div>`;
    }
   
    $('#TabloKalemler').append(deger);
    $("#StokID_" + YeniID).select2();
    StokAramaOlustur("StokID_" + YeniID);
}

// Select2 ile stok arama fonksiyonu

// Stok seçimi için select2 fonksiyonunu başlatan fonksiyon
function initStokSelect(stokID) {
    $('#' + stokID).select2({
        ajax: {
            url: "/Stok/SelectListe",
            data: function (params) {
                return {
                    search: params.term
                };
            },
            processResults: function (data) {
                var results = [];
                data.forEach(function (item) {
                    results.push({ id: item.ID, text: item.Isim });
                });
                return {
                    results: results
                };
            }
        },
        delay: 250,
        minimumInputLength: 3,
        language: {
            placeholder: "Arama yapmak için en az 3 karakter girin",
            inputTooShort: function () {
                return "En az lütfen 3 karakter girin";
            }
        }
    });
}

// Stok seçildiğinde seri numaralarını AJAX ile getiren fonksiyon
function initStokSeriChange(stokID, seriID) {
    console.log(stokID);
    $('#' + stokID).on('change', function () {
        $.ajax({
            url: '/Stok/SelectStokSeriListe',
            data: JSON.stringify({ StokID: this.value }),
            type: 'POST',
            contentType: 'application/json',
            success: function (data) {
                $('#' + seriID).empty();
                var options = '';
                data.forEach(function (seri) {
                    options += "<option value='" + seri.SeriNo + "'>" + seri.SeriNo + "</option>";
                });
                $('#' + seriID).append(options);
            },
            error: function (error) {
                console.error("Seri numaralarını alırken hata oluştu:", error);
            }
        });
    });
}
