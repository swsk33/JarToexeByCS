@echo off
set destDir=C:\Users\%username%\Downloads\j2ecs
mkdir "%destDir%"
mkdir "%destDir%\src"
cd %~dp0
copy /y "��������\buildexe.exe" "%destDir%\buildexe.exe"
copy /y "jarתexeԴ�ļ�\app.manifest" "%destDir%\app.manifest"
copy /y "jarתexeԴ�ļ�\cfg.properties" "%destDir%\cfg.properties"
copy /y "jarתexeԴ�ļ�\src\*" "%destDir%\src\"
echo ���������ļ���ɣ�