# DynamicIPChecker

C# Console application that checks your dynamic public IP address then sends an email notification if your IP address changes.

Configuration options in app.config, I use Google for the SMTP because it's free and easy to use.

`   <add key="UserName" value="" />
    <add key="Password" value="" />
    <add key="SMTP_Host" value="smtp.gmail.com" />
    <add key="SMTP_Port" value="587" />
    <add key="From_Address" value="" />
    <add key="To_Address" value="" />
`

I run the .exe as a scheduled task every 10 minutes (even when logged off) so if my ISP decides to change my IP when I am away from my machine I get a notification. 
