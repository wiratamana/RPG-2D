<?php
    include("mysql.php");

    if (connectToMySQL() == false) {
        die("1");
    }

    $name = $_POST[NAME];
    $gender = $_POST[GENDER];
    $deviceUID = $_POST[DEVICE_UID];
    $birthday = $_POST[BIRTHDAY];
    $salt = $_POST[SALT];
    $hash = $_POST[HASH];

    if (isRowExist(TABLE_NAME, NAME, $name) == true) {
        die("2");
    }

    insertUser($name, $deviceUID, $gender, $birthday, $salt, $hash);

    echo "0";
?>
