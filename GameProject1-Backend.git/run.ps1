md bin
cd bin
dotnet publish .\..\Regulus.Project.GameProject1.Play\ -o ./
copy .\..\Assets\server\*.* 
dotnet tool install --tool-path .\tool Regulus.Application.Server 
.\tool\Regulus.Application.Server.exe launchprotocol 53001 Regulus.Project.GameProject1.Protocol.dll Regulus.Project.GameProject1.Play.dll Regulus.Project.GameProject1.Play.Server
dotnet tool uninstall --tool-path .\tool Regulus.Application.Server 
cd ..
rd bin -Recurse
