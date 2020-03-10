@echo off
setlocal
                     
SET buildPathName= "%1"

if exist "%buildPathName%" (
	@echo This folder is exist
	exit/b
)

mkdir "%buildPathName%"   

cd src/BackendApi
dotnet build -o "../../%buildPathName%/BackendApi"

cd ../RequestClient
dotnet build -o "../../%buildPathName%/RequestClient"           
 
cd ../..
copy start.cmd "%buildPathName%"
copy stop.cmd "%buildPathName%"

cd config
copy host.json "../%buildPathName%/RequestClient"