rd Assets\Project\Plugins\Backend /q /s
mkdir Assets\Project\Plugins\Backend

rem copy ..\GameProject1-Backend\Game\bin\Debug\ Assets\Project\Plugins\Backend
copy ..\GameProject1-Backend.git\Data\bin\Debug\*.* Assets\Project\Plugins\Backend
copy ..\GameProject1-Backend.git\PlayUser\bin\Debug\*.* Assets\Project\Plugins\Backend
rem copy ..\GameProject1-Backend\PlayUser\bin\Debug\*.* Assets\Project\Plugins\Backend



rd Assets\Project\RemotingCode\ /q /s
mkdir Assets\Project\RemotingCode

xcopy ..\GameProject1-Backend.git\Game\*.cs Assets\Project\RemotingCode /s 



pause