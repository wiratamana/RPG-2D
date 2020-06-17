<?php

  class mysql {
  	const DATABASE_NAME = "unityapp";
  	const TABLE_ITEMS = "users";

    const TABLE_ORDER_COUNTER = "order_counter";
    const ORDER_COUNTER_COLUMN_MAX_NUMBER = "max_number";

    const CUSTOMER_NAME = "customerName";

    const TABLE_ORDER_ITEM = "order_item";
    const ORDER_ITEM_COLUMN_ORDER_NUMBER = "order_number";
    const ORDER_ITEM_COLUMN_ITEM_NAME = "item_name";
    const ORDER_ITEM_COLUMN_ORDER_COUNT = "order_count";

    const TABLE_ORDER_MAIN = "order_main";
    const ORDER_MAIN_COLUMN_ORDER_NUMBER = "order_number";
    const ORDER_MAIN_COLUMN_TIME = "time";
    const ORDER_MAIN_COLUMN_NAME = "name";

  	public static $con;

  	function connectToMySQL() {
  		self::$con = mysqli_connect("localhost", "root", "root", self::DATABASE_NAME);

  		if (mysqli_connect_errno()) {
  			return false;
  		}

  		return true;
  	}

  	function printTableHeader() {
  		echo "<tr>";
  		echo "<th>アイテム名</th>";
  		echo "<th>アイテム説明</th>";
  		echo "<th>価格</th>";
  		echo "<th>個数</th>";
  		echo "</tr>";
  	}

  	function printTableData() {
  		$result = mysqli_query(self::$con, "SELECT * FROM " . self::TABLE_ITEMS) or die("ERROR");

  		while ($row = mysqli_fetch_array($result)) {
  			echo "<tr>";
  			echo "<td>" . $row['name'] . "</td>";
  			echo "<td>" . $row['description'] . "</td>";
  			echo "<td>" . $row['price'] . "</td>";
  			echo "<td><input type=\"number\" id=\"" . $row['name'] . "\" name=\"" . $row['name'] . "\" value=\"0\" min=\"0\" max=\"99\"></td>";
  			echo "</tr>";
  		}
  	}

    function printConfirmation() {
      $result = mysqli_query(self::$con, "SELECT * FROM " . self::TABLE_ITEMS) or die("ERROR");

      echo "<table id=\"customers\">";

      echo "<tr>";
      echo "<th>ユーザー名</th>";
      echo "<th>機器ID</th>";
      echo "<th>性別</th>";
      echo "<th>誕生日</th>";
      echo "</tr>";


      $num = 1;
      $grandTotal = 0;
      while ($row = mysqli_fetch_array($result)) {
        $name = $row['name'];
        $deviceUID = $row['deviceUID'];
        $gender = $row['gender'];
        $birthday = $row['birthday'];

        echo "<tr>";
        echo "<td>" . $name . "</td>";
        echo "<td>" . $deviceUID . "</td>";
        echo "<td>" . $gender . "</td>";
        echo "<td>" . $birthday . "</td>";
        echo "</tr>";

        $num++;
        $grandTotal += $totalPrice;
      }
    }

    function applyPurchase() {
      $result = mysqli_query(self::$con, "SELECT * FROM " . self::TABLE_ITEMS) or die("ERROR");

      echo "<form action=\"result.php\" method=\"post\">";

      while ($row = mysqli_fetch_array($result)) {
        $name = $row['name'];
        $price = $row['price'];
        $number = $_POST[$name];
        $totalPrice = $number * $price;

        if ($number == "0") {
          echo "<input type=\"hidden\" name=" . $name . " value=\"0\">";
          continue;
        }

        echo "<input type=\"hidden\" name=" . $name . " value=" . $number . ">";
        echo "<input type=\"hidden\" name=ITEM_NAME_" . $name . " value=" . $name . ">";
        echo "<input type=\"hidden\" name=ITEM_COUNT_" . $name . " value=" . $number . ">";
      }

      echo "<input type=\"text\" id=\"customerName\" name=\"customerName\" placeholder=\"お名前\"><br>";
      echo "<div class=\"left\">";
      echo        "<input type=\"submit\" class=\"button\" value=\"注文\">";
      echo  "</div>";
      echo  "<div class=\"right\">";
      echo    "<input type=\"button\" class=\"button\" value=\"戻る\" onclick=\"history.back()\">";
      echo  "</div>";
      echo  "</form>";
    }

    function getOrderCounter() {
      $result = mysqli_query(self::$con, "SELECT " . self::ORDER_COUNTER_COLUMN_MAX_NUMBER . " FROM " . self::TABLE_ORDER_COUNTER) or die("ERROR");
      while ($row = mysqli_fetch_array($result)) {
        return $row[self::ORDER_COUNTER_COLUMN_MAX_NUMBER];
      }

      return -1;
    }

    function setOrderCounter($value) {
      mysqli_query(self::$con, "UPDATE " . self::TABLE_ORDER_COUNTER . " SET " . self::ORDER_COUNTER_COLUMN_MAX_NUMBER . " = " . $value);
    }

    function insert_order_item($order_number, $item_name, $order_count) {
      mysqli_query(self::$con, "INSERT INTO " . self::TABLE_ORDER_ITEM . " ("
                                        . self::ORDER_ITEM_COLUMN_ORDER_NUMBER . ", "
                                        . self::ORDER_ITEM_COLUMN_ITEM_NAME . ", "
                                        . self::ORDER_ITEM_COLUMN_ORDER_COUNT . ")
                              VALUES ('" . $order_number . "', '"
                                         . $item_name . "', '"
                                         . $order_count . "');")
            or die("FUCK!! - insert_order_item");

    }

    function insert_order_main($order_number, $time, $name) {
      mysqli_query(self::$con, "INSERT INTO " . self::TABLE_ORDER_MAIN . " ("
                                        . self::ORDER_MAIN_COLUMN_ORDER_NUMBER . ", "
                                        . self::ORDER_MAIN_COLUMN_TIME . ", "
                                        . self::ORDER_MAIN_COLUMN_NAME . ")
                              VALUES ('" . $order_number . "', '"
                                         . $time . "', '"
                                         . $name . "');")
            or die("FUCK!! - insert_order_main");

    }

    function result_verifyValue() {
      $name = $_POST[self::CUSTOMER_NAME];
      if ($name == "") {
        echo "<script> alert(\"お名前欄にあなたの名前を入力してください。\");history.go(-1);</script>";
        return false;
      }

      return true;
    }

    function result_sendDataToMySQL() {
      $orderCounter = self::getOrderCounter();
      $orderCounter += 1;
      $now = date("Y-m-d H:i:s");
      $customerName = $_POST[self::CUSTOMER_NAME];

      $result = mysqli_query(self::$con, "SELECT * FROM " . self::TABLE_ITEMS) or die("ERROR");

      while ($row = mysqli_fetch_array($result)) {
        $name = $row['name'];
        $number = $_POST[$name];

        if ($number == "0") {
          continue;
        }

        //echo "Item Name : " . $name . "<br>";
        //echo "Nubmer : " . $number . "<br>";
        self::insert_order_item($orderCounter, $name, $number);
      }

      self::setOrderCounter($orderCounter);
      self::insert_order_main($orderCounter, $now, $customerName);

      return true;
    }

    function result_printCustomerNameAndOrderID() {
      if (self::connectToMySQL() == false) {
                echo "<script> alert(\"MySQLにログインできませんでした。\");history.go(-1);</script>";
      }

      $orderCounter = self::getOrderCounter();
      $orderCounter += 1;
      echo "<p>" . $_POST[mysql::CUSTOMER_NAME] . "様の注文を受け付けました　注文ID：" . $orderCounter . "</p>";
    }
  }


?>
