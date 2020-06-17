<?php
    include("mysql.php");

    if (connectToMySQL() == false) {
        die("1");
    }

    $name = $_POST[NAME];

    if (isRowExist(TABLE_NAME, NAME, $name) == false) {
        die("2");
    }

    $hash = getRowValue(TABLE_NAME, HASH, NAME."='{$name}'");
    $salt = getRowValue(TABLE_NAME, SALT, NAME."='{$name}'");
    $gender = getRowValue(TABLE_NAME, GENDER, NAME."='{$name}'");
    $birthday = getRowValue(TABLE_NAME, BIRTHDAY, NAME."='{$name}'");
    $result = "{$hash}|{$salt}|{$name}|{$gender}|{$birthday}";

    echo "0|{$result}";
?>
