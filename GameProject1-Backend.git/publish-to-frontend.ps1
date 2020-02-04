Remove-Item .\..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend -Recurse
mkdir ..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend


dotnet publish ./Regulus.Project.GameProject1.PlayUser -o ..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend


rd ..\GameProject1-FrontEnd.git\Assets\Project\RemotingCode\ -Recurse
mkdir ..\GameProject1-FrontEnd.git\Assets\Project\RemotingCode
xcopy .\Regulus.Project.GameProject1.Game\*.cs ..\GameProject1-FrontEnd.git\Assets\Project\RemotingCode /s 



Remove-Item .\Regulus.Project.GameProject1.Protocol\*.cs -Recurse

dotnet tool install --global  Regulus.Application.Protocol.CodeWriter

Regulus.Application.Protocol.CodeWriter Regulus.Project.GameProject1.Protocol "..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend\Regulus.Project.GameProject1.Data.dll" ".\Regulus.Project.GameProject1.Protocol"

dotnet tool uninstall --global  Regulus.Application.Protocol.CodeWriter

dotnet publish .\Regulus.Project.GameProject1.Protocol -o ..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend

