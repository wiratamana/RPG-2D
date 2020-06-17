<?php include("mysql2.php") ?>
<link rel="stylesheet" href="order.css">

<html>

		<head>
				<title> ユーザー一覧 </title>

				<div class="header">
				  <h1>ユーザー一覧</h1>
				</div>

		</head>


		<body>
      <br>

      <?php
        if (mysql::connectToMySQL() == false) {
          echo "Failed to connect to mysql";
          die("Failed to connect to mysql");
        }

        mysql::printConfirmation();
      ?>

		</body>

</html>
