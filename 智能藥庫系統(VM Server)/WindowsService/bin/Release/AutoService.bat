SC QUERY my_process > NUL
IF ERRORLEVEL 1060 GOTO NOTEXIST
GOTO EXIST

:NOTEXIST
ECHO not exist my_process service
cd %~dp0\
InstallUtil.exe WindowsService.exe /i
net start my_process
GOTO END

:EXIST
ECHO exist my_process service
cd %~dp0\
InstallUtil.exe WindowsService.exe /u
GOTO END

:END
pause