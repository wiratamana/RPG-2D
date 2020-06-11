<?php
    include("mysql.php");

    if (connectToMySQL() == false) {
        die("1");
    }

    $name = $_POST[NAME];
    $gender = $_POST[GENDER];
    $birthday = $_POST[BIRTHDAY];
    $salt = $_POST[SALT];
    $hash = $_POST[HASH];

    if (isRowExist(TABLE_NAME, NAME, $name) == false) {
        die("2");
    }

    $hash = getRowValue(TABLE_NAME, HASH, NAME."='{$name}'");
    $salt = getRowValue(TABLE_NAME, SALT, NAME."='{$name}'");
    $result = "{$hash}_{$salt}";

    echo "0_{$result}";
?>
