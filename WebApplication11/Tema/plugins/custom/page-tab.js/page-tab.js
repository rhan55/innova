////    // Sekme yapısı başlangıcı

////    document.addEventListener("DOMContentLoaded", () => {
////        loadTabsFromStorage(); // Sayfa yüklendiğinde sekmeleri geri yükler
////        if (window !== window.parent) {
////            document.querySelector('#kt_app_sidebar').classList.add('hidden');
////            document.querySelector('#kt_app_header').classList.add('hidden');
////        }

////        // Anasayfa sekmesini sadece sayfa ilk yüklendiğinde ekle
////        if (!document.querySelector('#tabs li[data-url="/YK/Anasayfa"]')) {
////            createHomeTab(); // Anasayfa için özel bir fonksiyon
////        }
////    });

////    function createHomeTab() {
////        const homeTabId = 'tab-home';

////        // Anasayfa sekmesini oluştur
////        document.querySelector('#tabs').innerHTML += `
////    <li data-url="/YK/Anasayfa" id="${homeTabId}" style="padding: 5px 10px; border: 1px solid #ccc; border-radius: 3px; cursor: pointer; display: flex; align-items: center;">
////        <a href="javascript:void(0)" onclick="selectTab('${homeTabId}')" style="text-decoration: none; color: black;">Anasayfa</a>
////    </li>
////`;

////        document.querySelector('#tabContents').innerHTML += `
////    <div id="content-${homeTabId}" class="tab-content" style="padding: 10px; border: 1px solid #ddd; margin-top: 10px;">
////        <iframe src="/YK/Anasayfa" style="width: 100%; height: calc(100vh - 112px); border: none;" onload="preventReload('${homeTabId}', '/YK/Anasayfa')"></iframe>
////    </div>
////`;

////        selectTab(homeTabId);
////    }
////    function preventReload(tabId, url) {
////        // Anasayfa sekmesi tekrar yükleniyorsa openTab çağrısını önler
////        if (document.querySelector(`#tabs li[data-url="${url}"]`) && tabId === 'tab-home') {
////            return;
////        }
////        openTab('Anasayfa', url);
////    }

////    function openTab(title, url) {
////        let tabId = 'tab-' + new Date().getTime();

////        // Eğer Anasayfa açıkken yeni bir sekme açılıyorsa Anasayfa'yı kapat
////        const homeTab = document.querySelector('#tabs li[data-url="/YK/Anasayfa"]');
////        if (homeTab && url !== '/YK/Anasayfa') {
////            closeTab(homeTab.id);
////        }

////        // Aynı sekme zaten açık mı kontrol et
////        if (document.querySelector(`#tabs li[data-url="${url}"]`)) {
////            selectTab(tabId);
////            return;
////        }

////        // Yeni sekme oluştur ve içeriğini ekle
////        document.querySelector('#tabs').innerHTML += `
////    <li data-url="${url}" id="${tabId}" style="padding: 5px 10px; border: 1px solid #ccc; border-radius: 3px; cursor: pointer; display: flex; align-items: center;">
////        <a href="javascript:void(0)" onclick="selectTab('${tabId}')" style="text-decoration: none; color: black;">${title}</a>
////        <button onclick="closeTab('${tabId}')" style="background: none; border: none; font-size: 16px; cursor: pointer; margin-left: 5px;">×</button>
////    </li>
////`;

////        document.querySelector('#tabContents').innerHTML += `
////    <div id="content-${tabId}" class="tab-content" style="display: none; padding: 10px; border: 1px solid #ddd; margin-top: 10px;">
////        <iframe src="${url}" style="width: 100%; height: calc(100vh - 112px); border: none;"></iframe>
////    </div>
////`;

////        selectTab(tabId);
////        saveTabsToStorage(); // Sekme açıldıktan sonra LocalStorage'a kaydet
////    }

////    function selectTab(tabId) {
////        document.querySelectorAll('.tab-content').forEach(content => content.style.display = 'none');
////        document.querySelector(`#content-${tabId}`).style.display = 'block';

////        document.querySelectorAll('#tabs li').forEach(tab => tab.style.backgroundColor = '');
////        document.querySelector(`#${tabId}`).style.backgroundColor = '#f0f0f0';
////    }

////    function closeTab(tabId) {
////        document.querySelector(`#content-${tabId}`).remove();
////        document.querySelector(`#${tabId}`).remove();

////        if (!document.querySelector('#tabs .active') && document.querySelector('#tabs li')) {
////            selectTab(document.querySelector('#tabs li').id);
////        }

////        saveTabsToStorage(); // Sekme kapatıldığında LocalStorage'ı güncelle
////    }

////    function saveTabsToStorage() {
////        const tabs = Array.from(document.querySelectorAll('#tabs li')).map(tab => ({
////            title: tab.querySelector('a').innerText,
////            url: tab.getAttribute('data-url')
////        }));
////        localStorage.setItem('openTabs', JSON.stringify(tabs));
////    }

////    function loadTabsFromStorage() {
////        const savedTabs = JSON.parse(localStorage.getItem('openTabs'));
////        if (savedTabs) {
////            savedTabs.forEach(tab => {
////                if (tab.url !== '/YK/Anasayfa') { // Anasayfa dışındaki sekmeleri yükler
////                    openTab(tab.title, tab.url);
////                }
////            });
////        }
////    }
////    // Sekme yapısı bitişi