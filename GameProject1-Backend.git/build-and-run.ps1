$msbuildPath = & ".\tool\Resolve-MSBuild.ps1"

#& $msbuildPath

if (test-path "bin"  ) {
	Remove-Item "bin" -Recurse
}

mkdir "bin"
mkdir "bin/server"
copy "assets/server/*.*" "bin/server"

copy "regulus/tool/server/bin/debug/*.*" "bin/server"
copy "data/bin/debug/*.*" "bin/server"
copy "play/bin/debug/*.*" "bin/server"

cd "./bin/server"
& "./Regulus.Application.Server.exe" # "launchini" "config.ini"

