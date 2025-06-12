<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<!DOCTYPE html>
<html>
<head>
    <title>Quick Login</title>
    <style>
        body { font-family: Arial, sans-serif; }
        .login-box { width: 300px; margin: 100px auto; border: 1px solid #ccc; padding: 20px; border-radius: 8px; }
        .login-box h2 { text-align: center; }
        .form-group { margin-bottom: 15px; }
        .form-group label { display: block; }
        .form-group input { width: 100%; padding: 8px; }
        .btn { width: 100%; padding: 10px; background: #1a237e; color: #fff; border: none; border-radius: 4px; }
        .error { color: red; text-align: center; }
    </style>
</head>
<body>
<div class="login-box">
    <h2>Quick Login</h2>
    <form method="post" action="login">
        <div class="form-group">
            <label for="userId">User Id</label>
            <input type="text" id="userId" name="userId" required />
        </div>
        <div class="form-group">
            <label for="password">Password</label>
            <input type="password" id="password" name="password" required />
        </div>
        <input type="submit" class="btn" value="Login" />
    </form>
    <div class="error">
        <%= request.getAttribute("error") != null ? request.getAttribute("error") : "" %>
    </div>
    <div style="text-align:center; margin-top:10px;">
        <a href="#">Forgot Password?</a> | <a href="#">Change Password?</a>
    </div>
</div>
</body>
</html> 