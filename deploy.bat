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
		copy /y "jarתexeԴ�ļ�\src\*" "%copyDir%\src"
		copy /y "jarתexeԴ�ļ�\app.manifest" "%copyDir%\app.manifest"
		copy /y "jarתexeԴ�ļ�\cfg.properties" "%copyDir%\cfg.properties"
		copy /y "��������\buildexe.exe" "%copyDir%\buildexe.exe"
		echo �Ѹ��������ļ���%copyDir%�ļ����У�
		goto end
	) else (
		if %dmode%==r (
			if defined version (
				set fileName=%fileName%-%version%
			)
			7z a -t7z -mx9 "%dest%\!fileName!.7z" ".\jarתexeԴ�ļ�\src" ".\jarתexeԴ�ļ�\app.manifest" ".\jarתexeԴ�ļ�\cfg.properties" ".\��������\buildexe.exe"
			echo �����а������%dest%\!fileName!.7z��
			goto end
		) else (
			echo ����
			goto help
		)
	)
) else (
	echo ��ָ��ģʽ��
	goto help
)
:help
echo �÷���
echo deploy ģʽ [�汾��]
echo ģʽ��
echo t --- ����ģʽ��ֻ�Ḵ�������ļ����û������ļ����Թ��û����ԣ�����ָ���汾��
echo r --- ����ģʽ��ѹ�������ļ����û������ļ��У�����ָ���汾������Ϊѹ���ļ���׺
:end