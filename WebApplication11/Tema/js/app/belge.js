
function kaydet(controllerName, tip) {
    // Belge numarası ve cari seçimi kontrolleri
    if ($("#BelgeNo").val() === "" && tip === "AI") {
        alert("Lütfen belge numarası yazınız.");
        return;
    }

    // Kalemler verisinin toplanması
    var kalemler = [];
    $('#TabloKalemler tbody tr').each(function () {
        var satirID = $(this).attr("data-ID2");
        if (satirID) {
            kalemler.push({
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
    console.log(Tip);
    var deger = "";
    if (IsMobile == "1") {

        deger += "<tr data-ID='' data-ID2='" + YeniID + "' id='TR_" + YeniID + "' name='TR_" + YeniID + "'>                                                                                                             ";
        deger += "     <td colspan='4'>                                                                                                                               ";
        deger += "         <table style='width:100%;margin-bottom:15px;'>                                                                                             ";
        deger += "             <tr>                                                                                                                                   ";
        deger += "                 <td colspan='3'>                                                                                                                   ";
        deger += "                     <select class='form-select form-select-solid p-2 rounded-1' name='StokID_" + YeniID + "' id='StokID_" + YeniID + "' style='width:100%;'          ";
        deger += "                             aria-label='Floating label select example' data-control='select2' data-placeholder='Lütfen Seçim Yapınız'>             ";
        deger += "                                                                                   ";
        deger += "                     </select>                                                                                                                      ";
        deger += "                                                                                                                                                    ";
        deger += "                 </td>                                                                                                                              ";
        deger += "                 <td>                                                                                                                               ";
        if (Tip == Tip) {
            deger += "                         <input type='text' name='Seri_" + YeniID + "' id='Seri_" + YeniID + "' value='' class='form-control p-2 me-10' />                         ";
        } else if (Tip == 'DT') {
            deger += "                         <input type='text' name='Seri_" + YeniID + "' id='Seri_" + YeniID + "' value='' class='form-control p-2 me-10' />                         ";
        } else {
            deger += "                         <select class='form-select form-select-solid p-2 rounded-1' name='Seri_" + YeniID + "' id='Seri_" + YeniID + ")'                              ";

            deger += "                                 aria-label='Floating label select example' data-control='select2' data-placeholder='Lütfen Seçim Yapınız'>         ";
            deger += "                         </select>                                                                                                                  ";
        }
        deger += "                 </td>                                                                                                                              ";
        deger += "             </tr>                                                                                                                                  ";
        deger += "             <tr>        ";


        deger += "                 <td><strong>Birim</strong></td>                                                                                                    ";
        deger += "                 <td><strong>KDV Oranı</strong></td>                                                                                                    ";
        deger += "                 <td style='text-align: right;'><strong>Miktar</strong></td>                                                                        ";
        deger += "                 <td style='text-align: right;'><strong>Fiyat</strong></td>                                                                         ";
        deger += "                 <td style='text-align: right;'><strong>İskonto Oranı</strong></td>                                                                         ";
        deger += "                 <td style='text-align: right;'><strong>Tutar</strong></td>                                                                         ";
        deger += "             </tr>                                                                                                                                  ";
        deger += "             <tr>                                                                                                                                   ";

        deger += "                 <td><input type='text' name='OlcuBirimi_" + YeniID + "' id='OlcuBirimi_" + YeniID + "' value='Adet' class='form-control p-2 me-10' /></td>                   ";
        deger += "                 <td><input type='text' data-component='money-input' data-kt-element='iskonto-data' name='IskontoOrani1_" + YeniID + "' id='IskontoOrani1_" + YeniID + "' value='0' onchange='TutarHesapla(\"Miktar_" + YeniID + "\",\"Fiyat_" + YeniID + "\",\"Tutar_" + YeniID + "\");' class='form-control ' style='text-align:right;' /></td>              ";
        deger += "                 <td><input type='text' data-component='money-input' data-kt-element='kdv-data' name='KdvOrani_" + YeniID + "' id='KdvOrani_" + YeniID + "' value='0' onchange='TutarHesapla(\"Miktar_" + YeniID + "\",\"Fiyat_" + YeniID + "\",\"Tutar_" + YeniID + "\");' class='form-control ' style='text-align:right;' /></td>              ";
        deger += "                 <td><input type='text' data-component='money-input' name='Miktar_" + YeniID + "' id='Miktar_" + YeniID + "' value='1' onchange='TutarHesapla(\"Miktar_" + YeniID + "\",\"Fiyat_" + YeniID + "\",\"Tutar_" + YeniID + "\"); formatNumberWithSeparator(\"Miktar_" + YeniID + "\");' class='form-control me-10' style='text-align:right;' /></td>              ";
        deger += "                 <td><input type='text' data-component='money-input' name='Fiyat_" + YeniID + "' id='Fiyat_" + YeniID + "' value='0' onchange='TutarHesapla(\"Miktar_" + YeniID + "\",\"Fiyat_" + YeniID + "\",\"Tutar_" + YeniID + "\"); formatNumberWithSeparator(\"Miktar_" + YeniID + "\");'  style='text-align:right;' /></td>                 ";
        deger += "                 <td><input type='text' data-component='money-input' data-kt-element='iskonto-data' name='IskontoOrani1_" + YeniID + "' id='IskontoOrani1_" + YeniID + "' value='0' onchange='TutarHesapla(\"Miktar_" + YeniID + "\",\"Fiyat_" + YeniID + "\",\"Tutar_" + YeniID + "\");' class='form-control ' style='text-align:right;' /></td>              ";
        deger += "<td style='max-width:150px;'><input type='text' data-component='money-input' data-kt-element='tutar-data' name='Tutar_" + YeniID + "' id='Tutar_" + YeniID + "' value='0' onchange='TutarHesapla(\"Miktar_" + YeniID + "\", \"Fiyat_" + YeniID + "\", \"Tutar_" + YeniID + "\"); formatNumberWithSeparator(\"Tutar_" + YeniID + "\");' class='form-control p-2 me-10' style='text-align:right;' readonly /></td>";
        deger += "             </tr>";
        deger += "         </table>";
        deger += "     </td><td><button type='button' class='deleteBtn btn-sm pt-2' onclick='SatirSil(\"" + YeniID + "\")' data-ID2='" + YeniID + "'>Sil</button></td>";
        deger += " </tr>";


    } else {

        deger += "<tr data-ID='' data-ID2='" + YeniID + "' id='TR_" + YeniID + "' name='TR_" + YeniID + "'>";
        deger += "      <td style='max-width:150px;' id='TD_" + YeniID + "' name='TD_" + YeniID + "'>";
        deger += "      </td>";
        deger += "      <td style='max-width:100px;'>";
        if (Tip == Tip) {
            deger += "              <input type='text' name='Seri_" + YeniID + "' id='Seri_" + YeniID + "' value='' class='form-control p-2 me-10' />";
        } else if (Tip == "DT") {
            deger += "              <input type='text' name='Seri_" + YeniID + "' id='Seri_" + YeniID + "' value='' class='form-control p-2 me-10' />";
        } else {
            deger += "              <select class='form-select form-select-solid p-2 rounded-1' name='Seri_" + YeniID + "' id='Seri_" + YeniID + "'";
            deger += "                      aria-label='Floating label select example' data-control='select2' data-placeholder='Lütfen Seçim Yapınız'>";
            deger += "              </select>";
            deger += "";
        }
        deger += "      </td>";

        deger += "      <td style='max-width:100px;'><input type='text' name='OlcuBirimi_" + YeniID + "' id ='OlcuBirimi_" + YeniID + "' value='Adet' class='form-control p-2 me-10' /></td > ";
        deger += "      <td style='max-width:100px;'><input type='text' data-component='money-input' data-kt-element='kdv-data' name='KdvOrani_" + YeniID + "' id='KdvOrani_" + YeniID + "' value='0' onchange='TutarHesapla(\"Miktar_" + YeniID + "\",\"Fiyat_" + YeniID + "\",\"Tutar_" + YeniID + "\");' class='form-control p-2 me-10' style='text-align:right;' /></td>              ";
        deger += "      <td style='max-width:100px;'><input type='text' data-component='money-input' name='Miktar_" + YeniID + "' id='Miktar_" + YeniID + "' value='1' onchange='TutarHesapla(\"Miktar_" + YeniID + "\",\"Fiyat_" + YeniID + "\",\"Tutar_" + YeniID + "\"); formatNumberWithSeparator(\"Miktar_" + YeniID + "\");' class='form-control p-2 me-10' style='text-align:right;' /></td>";
        deger += "      <td style='max-width:150px;'><input type='text' data-component='money-input' name='Fiyat_" + YeniID + "' id = 'Fiyat_" + YeniID + "' value = '0' onchange='TutarHesapla(\"Miktar_" + YeniID + "\",\"Fiyat_" + YeniID + "\",\"Tutar_" + YeniID + "\");  formatNumberWithSeparator(\"Fiyat_" + YeniID + "\");' class='form-control p-2 me-10' style = 'text-align:right;' /></td > ";
        deger += "      <td><input type='text' data-kt-element='iskonto-data' data-component='money-input' name='IskontoOrani1_" + YeniID + "' id='IskontoOrani1_" + YeniID + "' value='0' onchange='TutarHesapla(\"Miktar_" + YeniID + "\",\"Fiyat_" + YeniID + "\",\"Tutar_" + YeniID + "\");' class='form-control p-2 me-10' style='text-align:right;' /></td>              ";
        deger += "<td style='max-width:150px;'><input type='text' data-component='money-input' data-kt-element='tutar-data' name='Tutar_" + YeniID + "' id='Tutar_" + YeniID + "' value='0' onchange='TutarHesapla(\"Miktar_" + YeniID + "\", \"Fiyat_" + YeniID + "\", \"Tutar_" + YeniID + "\"); formatNumberWithSeparator(\"Tutar_" + YeniID + "\");' class='form-control p-2 me-10' style='text-align:right;' readonly /></td>";
        deger += "<td><button type='button' class='deleteBtn btn-sm pt-2' onclick='SatirSil(\"" + YeniID + "\")' data-ID2='" + YeniID + "'>Sil</button></td></tr>";
    }
    var newSelect = $("<select class='form-select form-select-solid p-2 rounded-1 ' name='StokID_" + YeniID + "' id='StokID_" + YeniID + "' style='min-width:150px;max-width:350px;' aria-label='Floating label select example' data-control='select2' data-placeholder='Lütfen Seçim Yapınız' ></select>");

    // Yeni satır eklenmesi ve select2'nin aktif edilmesi
    $('#TabloKalemler tbody').append(deger);
    $("#TD_" + YeniID).append(newSelect);

    newSelect.select2();
    StokAramaOlustur("StokID_" + YeniID + "");
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
