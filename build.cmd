@echo off
setlocal
                     
SET buildPathName= "%1"

if exist "%buildPathName%" (
	@echo This folder is exist
	exit/b
)

mkdir "%buildPathName%"

cd "%buildPathName%"

mkdir config   
cd ..
cd src/BackendApi
dotnet publish -o "../../%buildPathName%/BackendApi"

cd ../RequestClient
dotnet publish -o "../../%buildPathName%/RequestClient"           
 
cd ../..
copy start.cmd "%buildPathName%"
copy stop.cmd "%buildPathName%"

cd config
copy host.json "../%buildPathName%/config"