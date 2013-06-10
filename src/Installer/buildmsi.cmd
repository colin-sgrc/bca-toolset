  "C:\Program Files (x86)\WiX Toolset v3.6\bin\candle.exe" -dVersion=1.1.0.0 -dSource=D:\sgrc\projects\BCAAImport D:\sgrc\projects\rdkb\bcaa\Installer\BCADataAdviceToolset.wxs


"C:\Program Files (x86)\WiX Toolset v3.6\bin\light.exe" BCADataAdviceToolset.wixobj -ext WixNetFxExtension -cultures:en-us -out BCADataAdviceToolset.msi
