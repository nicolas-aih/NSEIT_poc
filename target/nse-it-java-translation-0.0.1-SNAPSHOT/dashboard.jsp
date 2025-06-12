<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<!DOCTYPE html>
<html>
<head>
    <title>Dashboard</title>
    <style>
        body { font-family: Arial, sans-serif; }
        .dashboard-box { width: 400px; margin: 100px auto; border: 1px solid #ccc; padding: 20px; border-radius: 8px; text-align: center; }
        .success { color: green; }
    </style>
</head>
<body>
<div class="dashboard-box">
    <h2>Welcome to the Dashboard</h2>
    <div class="success">
        <%= request.getAttribute("message") != null ? request.getAttribute("message") : "" %>
    </div>
    <p>You are now logged in.</p>
</div>
</body>
</html> 