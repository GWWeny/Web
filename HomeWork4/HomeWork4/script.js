document.getElementById('btnGenerate').addEventListener('click', function () {
    // 生成1到100之间的随机数
    var randomNumber = Math.floor(Math.random() * 100) + 1;
    // 显示随机数
    document.getElementById('randomNumber').textContent = randomNumber;
});