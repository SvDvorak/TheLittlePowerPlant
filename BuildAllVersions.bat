@echo OFF

REM --- Builds and publishes project for Web, Win, Linux & OSX ---
REM Input parameter: release name, for example "r4"
REM Make sure that set variable paths are correct

set buildFolder=F:\Personal\Programming\The Little Power Plant That Could\Build
set zipExe=C:\Program Files\7-Zip\7z.exe
set dropboxFolder=F:\Personal\Dropbox\Public\TheLittlePowerPlant
start "" "C:\Program Files\Unity\Editor\Unity.exe" -batchmode -openproject "" -quit -buildWebPlayer "%buildFolder%\Web\TheLittlePowerPlant" -buildWindowsPlayer "%buildFolder%\Win\TheLittlePowerPlant" -buildLinux32Player "%buildFolder%\Linux\TheLittlePowerPlant" -buildOSXPlayer "%buildFolder%\OSX\TheLittlePowerPlant"
echo "Wait until all projects have been built then press enter"
pause

start "" "%zipExe%" a "%buildFolder%\TheLittlePowerPlant_win_%1.zip" "%buildFolder%\Win\*"
start "" "%zipExe%" a "%buildFolder%\TheLittlePowerPlant_linux_%1.zip" "%buildFolder%\Linux\*"
start "" "%zipExe%" a "%buildFolder%\TheLittlePowerPlant_osx_%1.zip" "%buildFolder%\OSX\*"

echo "Wait until all zip folders have been created then press enter"
pause

copy "%buildFolder%\TheLittlePowerPlant_win_%1.zip" "%dropboxFolder%\TheLittlePowerPlant_win_%1.zip"
copy "%buildFolder%\TheLittlePowerPlant_linux_%1.zip" "%dropboxFolder%\TheLittlePowerPlant_linux_%1.zip"
copy "%buildFolder%\TheLittlePowerPlant_osx_%1.zip" "%dropboxFolder%\TheLittlePowerPlant_osx_%1.zip"
robocopy /NP /NJH /NJS "%buildFolder%\Web\TheLittlePowerPlant" "%dropboxFolder%\TheLittlePowerPlant_%1"

pause