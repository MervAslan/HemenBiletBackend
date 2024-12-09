document.addEventListener("DOMContentLoaded", () => {
    const registerModal = document.getElementById("registerModal");
    const openRegister = document.getElementById("openRegister");
    const closeModal = document.querySelector(".close");

    // Modalı Açma
    openRegister.addEventListener("click", (e) => {
        e.preventDefault(); // Sayfa yenilenmesini engelle
        registerModal.style.display = "block"; // Modal görünür hale gelir
    });

    // Modalı Kapatma
    closeModal.addEventListener("click", () => {
        registerModal.style.display = "none";
    });

    // Modal dışında bir yere tıklanınca kapat
    window.addEventListener("click", (e) => {
        if (e.target === registerModal) {
            registerModal.style.display = "none";
        }
    });
});
