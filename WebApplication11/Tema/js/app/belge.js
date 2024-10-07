function kaydet(controllerName, tip) {
    // Belge numarası ve cari seçimi kontrolleri
    if ($("#BelgeNo").val() === "" && tip === "AI") {
        alert("Lütfen belge numarası yazınız.");
        return;
    }

    var _data = {
        Tip: tip,
        ID: '',
        BelgeNo: '',
        Tarih: '',
        CariID: '',
        Kalemler: kalemler
    };

   
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
    if (belgeEkleCompenentValue.CariVarMi) {
        _data.CariID = $("#CariID option:selected").val();
    }

    if (belgeEkleCompenentValue.CikisDepoVarMi) {
        _data.DepoCikisID = $("#DepoCikisID option:selected").val()
    }

    if (belgeEkleCompenentValue.SatisPersoneliVarMi) {
        _data.SatisPersonelID = $("#SatisPersonelID option:selected").val()
    }

    if (belgeEkleCompenentValue.DurumuVarMi) {
        _data.SatisPersonelID = $("#Durumu option:selected").val()
    }

    _data.Tip = tip;
    _data.ID = $("#ID").val();
    _data.BelgeNo = $('#BelgeNo').val();
    _data.Tarih = $('#Tarih').val();
    _data.Aciklama = $('#Aciklama').val();
    _data.Kalemler = kalemler;

    // AJAX isteği
    $.ajax({
        url: `/${controllerName}/Kaydet`,
        data: JSON.stringify(_data),
        contentType: "application/json",
        dataType: "json",
        type: "POST",
        success: function (data) {
            alert("Belge Kaydedildi!");
            window.open(`/${controllerName}/Duzenle?id=` + data.Data + "&Tip=" + tip, "_parent");
        },
        error: function (msg) {
            console.log(msg);
        }
    });
}