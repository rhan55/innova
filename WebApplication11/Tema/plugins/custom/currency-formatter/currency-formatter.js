$(() => {
    const moneyInputs = $('input[data-component="money-input"]');

    
    $(document).on('input', 'input[data-component="money-input"]', (event) => {
        handleMoneyInput(event.target)
    })

    moneyInputs.each(function () {
        handleMoneyInput(this);
    })

    function formatMoney(value) {
        // Sadece sayıları alıp, noktalar ve virgülleri temizliyoruz
        value = value.replace(/[^0-9]/g, '');
    
        // En sondaki iki basamağı virgülden sonrası olarak alıyoruz
        const decimalPart = value.slice(-2);
        const integerPart = value.slice(0, -2).replace(/^0+/, '') || '0'; // Soldaki sıfırları kaldırıyoruz
    
        // Tam sayı kısmını binlik formatında yazdırıyoruz
        const formattedIntegerPart = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
    
        // Tam sayı kısmı ve virgül sonrası kısmı birleştiriyoruz
        return formattedIntegerPart + ',' + decimalPart;
    }
    
    // İmleç pozisyonunu almak için fonksiyon
    function getCursorPosition(element) {
        return element.selectionStart;
    }
    
    // İmleç pozisyonunu ayarlamak için fonksiyon
    function setCursorPosition(element, position) {
        element.setSelectionRange(position, position);
    }
    
    // Giriş yapılırken çalışacak ana fonksiyon
    function handleMoneyInput(element) {
        const input = element;
        const originalCursorPosition = getCursorPosition(input);
        let value =$(element).val();
    
        // Sadece sayılar olacak şekilde temizliyoruz
        value = value.replace(/[^0-9]/g, '');
    
        // Formatlı değeri alıyoruz
        const formattedValue = formatMoney(value);
    
        // Formatlanmış değeri input alanına koyuyoruz
        input.value = formattedValue;
    
        // İmlecin pozisyonunu koruyoruz
        const newCursorPosition = originalCursorPosition + (input.value.length - value.length);
        setCursorPosition(input, newCursorPosition);
    }
})

