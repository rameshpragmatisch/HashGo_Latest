
del /s C:\_Work\HashGo\Trunk\hashgo\HashGo.Wpf.App\bin\Debug\net8.0-windows10.0.19041.0\*.pdb


heat.exe dir "C:\_Work\HashGo\Trunk\hashgo\HashGo.Wpf.App\bin\Debug\net8.0-windows10.0.19041.0" -dr "INSTALLFOLDER" -cg HashGoComponents -gg -sf -scom -sfrag -srd -sreg -svb6  -var "env.HashGoDir" -out "C:\_Work\HashGo\Trunk\hashgo\HashGo.Installer\Fragment_HashGo.wxs"

