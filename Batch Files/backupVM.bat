@ECHO OFF
VBoxManage controlvm Turnkey acpipowerbutton
REM 60 seconds to wait for shutdown. Barring issues, this should be more than enough. 
TIMEOUT /T 60

rem figure out the date stamp to append to folder name
for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "datestamp=%YYYY%%MM%%DD%" & set "timestamp=%HH%%Min%%Sec%"

REM make a copy of the backup.
xcopy "C:\Users\David Heng\VirtualBox VMs\Turnkey" "D:\Backup\Backup%datestamp%"

REM assume that 60 seconds is enough. If it takes more time, bump this number up. 
REM WE MUST make the complete copy before turning on the VM. 
TIMEOUT /T 60

REM Turn back on the VM. 
VBoxManage startvm Turnkey