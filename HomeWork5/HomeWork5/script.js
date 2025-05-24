document.getElementById('infoForm').addEventListener('submit', function (event) {
    event.preventDefault(); // 阻止表单的默认提交行为

    // 获取用户输入的信息
    var name = document.getElementById('name').value;
    var email = document.getElementById('email').value;
    var phone = document.getElementById('phone').value;
    var address = document.getElementById('address').value;
    var age = document.getElementById('age').value;

    // 在网页上显示用户输入的信息
    document.getElementById('displayName').textContent = '姓名：' + name;
    document.getElementById('displayEmail').textContent = '邮箱：' + email;
    document.getElementById('displayPhone').textContent = '电话：' + phone;
    document.getElementById('displayAddress').textContent = '地址：' + address;
    document.getElementById('displayAge').textContent = '年龄：' + age;
});