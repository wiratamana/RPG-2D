<?php
    include("mysql.php");

    if (connectToMySQL() == false) {
        die("1");
    }

    $keyword = $_POST[KEYWORD];

    $result = searchUser($keyword);
    echo "{$result}";
?>
