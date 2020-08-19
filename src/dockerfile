FROM microsoft/aspnetcore
WORKDIR /app
COPY bin/Release/netcoreapp2.2/publish/ .
ENTRYPOINT ["dotnet", "hello.dll"]