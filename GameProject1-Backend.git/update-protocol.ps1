Remove-Item .\Regulus.Project.GameProject1.Protocol\*.cs -Recurse

dotnet tool install --global  Regulus.Application.Protocol.CodeWriter

Regulus.Application.Protocol.CodeWriter Regulus.Project.GameProject1.Protocol "..\GameProject1-FrontEnd.git\Assets\Project\Plugins\Backend\Regulus.Project.GameProject1.Data.dll" ".\Regulus.Project.GameProject1.Protocol"

dotnet tool uninstall --global  Regulus.Application.Protocol.CodeWriter