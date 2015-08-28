<!DOCTYPE html>
<head>
    <html lang="zh-CN"><head><meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">

        <meta name="description" content="">
        <meta name="author" content="">
        <link rel="icon" href="http://v3.bootcss.com/favicon.ico">
        <title>Automatic Mail Sender GUI</title>

    </head>

    <body>

    <?php
    include ("ConnectDatabase.php");
    function send_post($url, $post_data)
    {
    $postdata = http_build_query($post_data);
    $options = array(
            'http' => array(

            'method' => 'POST',

            'header' => 'Content-type:application/x-www-form-urlencoded',

            'content' => $postdata,

            'timeout' => 15 * 60
        )

    );
    $context = stream_context_create($options);
    $result = file_get_contents($url, false, $context);
    return $result;
}


    foreach ($_POST['sub'] as $result)
    {
        $mailaddress = mysql_fetch_array(mysql_query("SELECT * FROM tbl_name WHERE COL1 = '$result'"));

        if ($mailaddress['item'] == "")
        {

         $post_data = array(
             'from' => 'no-reply@accounts.google.com',
             'to' => $mailaddress['COL2'],
             'subject' => '湛江一中IT社面试通知',
             'html' => $result.constant("DETAILS")
         );
        send_post('http://hs.stcula.com:800/mailer',$post_data);
		echo '发送邮件成功，发给了  '.$result.'  邮件地址为:'.$mailaddress['COL2']. "<br />";
		mysql_query("UPDATE tbl_name SET item='Yes' WHERE COL1 ='$result'");
        }
        else
        {
            echo "The Person ".$result."  ".$mailaddress['COL2']." exists something wrong.Please check your select.";

        }


    }



    ?>

    </body>
    </html>
