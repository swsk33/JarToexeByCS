@echo off
cd %~dp0
set fileName=j2ecs
set suffix=%1
if defined suffix (
	set fileName=%fileName%-%suffix%
)
7z a -t7z -mx9 "C:\Users\%username%\Downloads\%fileName%.7z" ".\��������\buildexe.exe" ".\jarתexeԴ�ļ�\app.manifest" ".\jarתexeԴ�ļ�\cfg.properties" ".\jarתexeԴ�ļ�\src"
echo ����ѹ������ɣ�