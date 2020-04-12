@echo off
setlocal 

cd nats
start nats-server.exe                        

cd ../BackendApi
start dotnet BackendApi.dll
                                                        
cd ../RequestClient
start dotnet RequestClient.dll

cd ../JobLogger
start dotnet JobLogger.dll