1 Create new Environment variable with name "HashGoDir", give variabe value as shown below based on your HashGo release Directory
C:\Users\HP\source\HashGo\HashGo.Wpf.App\bin\Release\net8.0-windows10.0.19041.0
2. Open command prompt (Windows+R, type cmd)
3. cd C:\Program Files (x86)\WiX Toolset v3.14\bin
4. heat.exe dir "C:\Users\HP\source\HashGo\HashGo.Wpf.App\bin\Release\net8.0-windows10.0.19041.0" -dr "INSTALLFOLDER" -cg HashGoComponents -gg -sf -scom -sfrag -srd -sreg -svb6  -var "env.HashGoDir" -out "C:\Users\HP\source\HashGo\HashGo.Installer\Fragment_HashGo.wxs"