@ECHO OFF

SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

IF ERRORLEVEL 1 (

	ECHO This script requires extensions and delayed expansion

	EXIT /B 1
)

IF NOT DEFINED SS_SN_32 (

	IF NOT DEFINED SS_SN_64 (

		ECHO Neither SS_SN_32 nor SS_SN_64 environment variables are defined

		EXIT /B 1
	)
)

IF {%1} == {} (

	ECHO USAGE: %~dpn0^(%~x0^) ^<key-path^>

	EXIT /B 1
)

SET KeyPath_=%1
SET KeyPath=%KeyPath_:"=%

SET ScriptDir=%~dp0
SET ProjectRootDir=!ScriptDir!..\
SET SrcDir=!ProjectRootDir!src\

CALL %ScriptDir%perform_strong_signing.cmd %SrcDir%recls.Core\bin\Release\recls.NET.Core.dll %KeyPath%
