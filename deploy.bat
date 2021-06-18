@echo off
setlocal enabledelayedexpansion
cd %~dp0
set dmode=%1
set version=%2
set dest=C:\Users\%username%\Downloads
set copyDir=%dest%\j2ecs
set fileName=j2ecs
if defined dmode (
	if %dmode%==t (
		mkdir %copyDir%
		mkdir %copyDir%\src
		copy /y "jar转exe源文件\src\*" "%copyDir%\src"
		copy /y "jar转exe源文件\app.manifest" "%copyDir%\app.manifest"
		copy /y "jar转exe源文件\cfg.properties" "%copyDir%\cfg.properties"
		copy /y "构建程序\buildexe.exe" "%copyDir%\buildexe.exe"
		echo 已复制所需文件至%copyDir%文件夹中！
		goto end
	) else (
		if %dmode%==r (
			if defined version (
				set fileName=%fileName%-%version%
			)
			7z a -t7z -mx9 "%dest%\!fileName!.7z" ".\jar转exe源文件\src" ".\jar转exe源文件\app.manifest" ".\jar转exe源文件\cfg.properties" ".\构建程序\buildexe.exe"
			echo 部署发行版完成至%dest%\!fileName!.7z！
			goto end
		) else (
			echo 错误！
			goto help
		)
	)
) else (
	echo 请指定模式！
	goto help
)
:help
echo 用法：
echo deploy 模式 [版本号]
echo 模式：
echo t --- 测试模式，只会复制所需文件至用户下载文件夹以供用户测试，无需指定版本号
echo r --- 发行模式，压缩所需文件至用户下载文件夹，可以指定版本号以作为压缩文件后缀
:end