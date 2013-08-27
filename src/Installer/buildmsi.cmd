
"C:\Program Files (x86)\WiX Toolset v3.6\bin\candle.exe" -dVersion=1.2.0.0 -dSource=C:\Users\cdyck\Documents\GitHub\BCAAImport C:\Users\cdyck\Documents\GitHub\bca-toolset\src\Installer\BCADataAdviceToolset.wxs


"C:\Program Files (x86)\WiX Toolset v3.6\bin\light.exe" BCADataAdviceToolset.wixobj -ext WixNetFxExtension -cultures:en-us -out BCADataAdviceToolset_1.2.0.0.msi
