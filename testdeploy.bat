@echo off
set destDir=C:\Users\%username%\Downloads\j2ecs
mkdir "%destDir%"
mkdir "%destDir%\src"
cd %~dp0
copy /y "构建程序\buildexe.exe" "%destDir%\buildexe.exe"
copy /y "jar转exe源文件\app.manifest" "%destDir%\app.manifest"
copy /y "jar转exe源文件\cfg.properties" "%destDir%\cfg.properties"
copy /y "jar转exe源文件\src\*" "%destDir%\src\"
echo 部署所需文件完成！