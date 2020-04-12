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
mkdir nats
cd ..

cd src/BackendApi
dotnet publish -o "../../%buildPathName%/BackendApi"

cd ../RequestClient
dotnet publish -o "../../%buildPathName%/RequestClient" 
          
cd ../JobLogger
dotnet publish -o "../../%buildPathName%/JobLogger"  

cd ../..
copy start.cmd "%buildPathName%"
copy stop.cmd "%buildPathName%"

cd config
copy host.json "../%buildPathName%/config"

cd ../nats-server
copy nats-server.exe "../%buildPathName%/nats"