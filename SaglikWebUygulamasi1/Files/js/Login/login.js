<script>
document.getElementById("loginButton").onclick = function() {
    // AJAX kullanarak sunucu tarafındaki Login action'ını çağırma
    $.ajax({
        url: '/Default/Login', // Action'ın URL'sini doğru olarak belirtin
        type: 'POST',
        data: { username: 'kullaniciAdi', password: 'sifre' }, // Kullanıcı adı ve şifreyi uygun şekilde belirtin
        success: function(result) {
            // Başarılı cevap geldiğinde, gelen verileri değerlendirin (isteğe bağlı)
            console.log(result);
        },
        error: function(error) {
            // Hata durumunda işlemleri gerçekleştirin (isteğe bağlı)
            console.error(error);
        }
    });
};
</script>
