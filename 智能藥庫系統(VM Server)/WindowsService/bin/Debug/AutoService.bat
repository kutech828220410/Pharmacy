SC QUERY my_process > NUL
IF ERRORLEVEL 1060 GOTO NOTEXIST
GOTO EXIST

:NOTEXIST
ECHO not exist my_process service
%~dp0\InstallUtil.exe %~dp0\WindowsService.exe
net start my_process
GOTO END

:EXIST
ECHO exist my_process service
%~dp0\InstallUtil.exe %~dp0\WindowsService.exe /u
GOTO END

:END
pause