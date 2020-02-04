
FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS gameproject1-run

WORKDIR /app

COPY / /app/source/

RUN dotnet publish ./source/Regulus.Project.GameProject1.Play/ -o /app/bin
RUN cp -r ./source/Assets/server/* ./bin

RUN dotnet tool install --tool-path /app/bin Regulus.Application.Server

WORKDIR /app/bin

  
ENTRYPOINT ["./Regulus.Application.Server","launchprotocol","47536" ,"Regulus.Project.GameProject1.Protocol.dll" ,"Regulus.Project.GameProject1.Play.dll" , "Regulus.Project.GameProject1.Play.Server" ]


