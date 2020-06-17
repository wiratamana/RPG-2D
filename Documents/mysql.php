<?php
    include("mysql_account.php");

    function connectToMySQL() {
        $con = mysqli_connect(IP, ID, PW, DB);

        if (mysqli_connect_errno()) {
            return false;
        }

        return true;
    }

    function isRowExist($tableName, $columnName, $value) {
        $con = mysqli_connect(IP, ID, PW, DB);
        $query = "SELECT {$columnName} FROM {$tableName} WHERE {$columnName}='{$value}'";
        $result = mysqli_query($con, $query);
        return mysqli_num_rows($result) > 0;
    }

    function getRowValue($tableName, $columnName, $condition) {
        $con = mysqli_connect(IP, ID, PW, DB);
        $query = "SELECT {$columnName} FROM {$tableName} WHERE {$condition}";
        $result = mysqli_query($con, $query);
        if (mysqli_num_rows($result) < 1) {
            return "Failed";
        }

        $info = mysqli_fetch_assoc($result);

        return $info[$columnName];
    }

    function insertUser($name, $deviceUID, $gender, $birthday, $salt, $hash) {
        $con = mysqli_connect(IP, ID, PW, DB);

        $tableName = TABLE_NAME;
        $col_name = NAME;
        $col_deviceUID = DEVICE_UID;
        $col_gender = GENDER;
        $col_birthday = BIRTHDAY;
        $col_salt = SALT;
        $col_hash = HASH;

        $query = "INSERT INTO {$tableName}
                    ({$col_name}, {$col_deviceUID}, {$col_gender}, {$col_birthday}, {$col_salt}, {$col_hash})
                    VALUES ('{$name}', '{$deviceUID}', '{$gender}', '{$birthday}', '{$salt}', '{$hash}');";

        if (!mysqli_query($con, $query)) {
            die("3");
        }
    }

    function searchUser($keyword)
    {
        $con = mysqli_connect(IP, ID, PW, DB);

        $tableName = TABLE_NAME;
        $col_name = NAME;
        $col_gender = GENDER;
        $col_birthday = BIRTHDAY;

        $query = "SELECT `{$col_name}`,`{$col_gender}`,`{$col_birthday}` FROM {$tableName}
                  WHERE `{$col_name}` LIKE '%{$keyword}%';";

        $result = mysqli_query($con, $query);
        if (mysqli_num_rows($result) < 1) {
              return "2";
        }

        $returnvalue = "0|";
        while ($info = mysqli_fetch_assoc($result)) {
            $result_name = $info[$col_name];
            $result_gender = $info[$col_gender];
            $result_birthday = $info[$col_birthday];
            $returnvalue = "{$returnvalue}|{$result_name},{$result_gender},{$result_birthday}";
        }

        return "{$returnvalue}";
    }
?>
