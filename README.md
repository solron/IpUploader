# IP Uploader

IP Loader saves the public ip of the machine it is run from, and uploads it to a ssh host.<br>
Where only me, Google and NSA has access to it.<br>

It is basicly the exact same tool as Public IP Tool, but this is for .net core 3 instead of .net framwork.<br>

It is sending a webrequest to https://soltveit.org/pages/myip/ and take the ip from<br>
the response.<br>

This is how the response looks like:<br>
```
92.250.85.90
```
After the program got the ip address it saves it as a text file (public-ip.txt), and upload it to the
server and path specified in the config file.<br>

How the output file looks like:
```
Time: 2019-10-04 21:46:32
IPv4: 92.221.70.95
```

## How to use

To run this every hour, put this in your crontab:<br>
```
0 */1 * * * ( cd /home/user/folder && ./ip-tool.sh )
```

Example of ip-tool.sh<br>
```
#!/bin/sh
dotnet IpUploader.dll
```
Remember to make the scipt executable.<br>
```
chmod +x ip-tool.sh
```
## The code
There is now a simple config instead of hard coded connection info. It must be in the same folder as the ip uploader
is run from. You can use hostname or ip address. It both works.<br>

Example of the config.txt. There is also a copy in the project root folder.<br>
```
my.server.com
ronny
Password
/home/ronny/public-ip
```

The public ip is optained by sending a web request to https://soltveit.org/pages/myip/
```
public static string MyIP()
{
    string externalip = new WebClient().DownloadString("https://soltveit.org/pages/myip/");

    return externalip;
}
```

Added this php script to my web server:<br>

<strong>ip.php</strong><br>
```
<?php
echo $_SERVER['REMOTE_ADDR'];
?>
```

A successful run:<br>
![alt public ip screenshot](https://soltveit.org/files/iptool.jpg)

## TODO
- Encrypt password in config.txt<br>