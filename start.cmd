@echo off
setlocal                         

cd BackendApi
start dotnet BackendApi.dll
                                                        
cd ../RequestClient
start dotnet RequestClient.dll