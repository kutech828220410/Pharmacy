@echo off
title SunloginKeeper
cls

set runAppPath="C:\Users\hsonds01\Desktop\智能藥庫系統(VM Server)\智能藥庫系統(VM Server).exe"

set _interval=10

set runAppFolder=''
set _processName=''
set _processNameExt=''
if '%runAppPath%'=='' (goto end)

for %%a in (%runAppPath%) do (
set runAppFolder=%%~dpa
set _processName=%%~na
set _processNameExt=%%~nxa
)
::echo %runAppPath%
::echo %runAppFolder%
::echo %_processName%
::echo %_processNameExt%
goto checkstart
 
:checkstart


:startApp

pushd %runAppFolder%

echo %date:~0,10% %time:~0,8%: %runAppPath%

start "" %runAppPath%

popd
 
:checkend

choice /t %_interval% /d y /n >nul
goto checkstart
 
:end
echo end.
pause