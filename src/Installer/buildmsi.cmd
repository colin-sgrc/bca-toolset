
"C:\Program Files (x86)\WiX Toolset v3.9\bin\candle.exe" -dVersion=1.3.0.0 -dSource=C:\code\github\bca-toolset\bin C:\code\github\bca-toolset\src\Installer\BCADataAdviceToolset.wxs


"C:\Program Files (x86)\WiX Toolset v3.9\bin\light.exe" BCADataAdviceToolset.wixobj -ext WixNetFxExtension -cultures:en-us -out BCADataAdviceToolset_1.3.0.0.msi
