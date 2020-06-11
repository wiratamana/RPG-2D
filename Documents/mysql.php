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
?>
