@echo off
rem F:
rem cd F:/workspace/MahjongMaster/Assets/StreamingAssets/DB/
rem del /f/q *.db

rem sqlite3 F:/workspace/MahjongMaster/Assets/StreamingAssets/DB/DB.db<F:/workspace/MahjongMaster/Assets/Design/Excels/DB.sql
rem @echo %1
rem @echo %2 
rem sqlite3 %1 < %2
%3 %1 < %2
@pause..