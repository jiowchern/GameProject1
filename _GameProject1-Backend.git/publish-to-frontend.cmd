rd ..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend /q /s
mkdir ..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend


copy Data\bin\Debug\*.*  ..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend
copy PlayUser\bin\Debug\*.*  ..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend




rd ..\GameProject1-FrontEnd.git\Assets\Project\RemotingCode\ /q /s
mkdir ..\GameProject1-FrontEnd.git\Assets\Project\RemotingCode

xcopy Game\*.cs ..\GameProject1-FrontEnd.git\Assets\Project\RemotingCode /s 


cd "Regulus\Tool\GhostProviderGenerator\bin\Debug" 
Regulus.Application.Protocol.Generator.exe "..\..\..\..\..\data\bin\debug\Regulus.Project.GameProject1.Data.dll" "..\..\..\..\..\..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend\Regulus.Project.GameProject1.Protocol.dll"

pause