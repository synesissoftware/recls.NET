@ECHO OFF

REM Copyright (c) 2009-2017, Matthew Wilson and Synesis Software. All rights
REM reserved.

SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

IF ERRORLEVEL 1 (

	ECHO This script requires extensions and delayed expansion

	EXIT /B 1
)

SET ScriptDir=%~dp0


IF {%1} == {} (

:usage
	ECHO USAGE: %~dpn0^(%~x0^) { --help ^| Clean ^| Debug ^| Release ^| Package } [ ^<signing-private-key-path^> ]

	IF {%1} == {} (

		EXIT /B 0
	) ELSE (

		EXIT /B 1
	)
)

IF /I {%1} == {--help} (

	GOTO usage
)
IF /I {%1} == {Clean} (

	GOTO clean
)
IF /I {%1} == {Debug} (

	GOTO build
)
IF /I {%1} == {Release} (

	GOTO build
)
IF /I {%1} == {Package} (

	GOTO build_and_package
)

ECHO Configuration '%1' not recognised; use --help for usage 1>&2
EXIT /B 1



:clean

msbuild.exe %ScriptDir%recls.NET.sln /t:Clean /p:Configuration=Debug
msbuild.exe %ScriptDir%recls.NET.sln /t:Clean /p:Configuration=Release

GOTO :EOF



:build

msbuild.exe %ScriptDir%recls.NET.sln /t:Build /p:Configuration=%1

GOTO :EOF



:build_and_package

IF {%2} == {} (

	ECHO Package option requires ^<signing-private-key-path^>; use --help for usage 1>&2

	EXIT /B 1
)

IF NOT EXIST "%~2" (

	ECHO Given key path "%~2" does not exist

	EXIT /B 1
)

msbuild.exe %ScriptDir%recls.NET.sln /t:Build /p:Configuration=Release

CALL %ScriptDir%scripts\sign_assemblies.cmd %2

IF NOT DEFINED NUGET (

	ECHO Environment variable NUGET is not defined 1>&2

	EXIT /B 1
)

%NUGET% pack %ScriptDir%buildsrc\nuget\recls.NET.nuspec

GOTO :EOF


