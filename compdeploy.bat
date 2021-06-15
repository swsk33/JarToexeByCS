@echo off
cd %~dp0
7z a -t7z -mx9 "C:\Users\%username%\Downloads\j2ecs.7z" ".\构建程序\buildexe.exe" ".\jar转exe源文件\app.manifest" ".\jar转exe源文件\cfg.properties" ".\jar转exe源文件\src"
echo 部署压缩包完成！