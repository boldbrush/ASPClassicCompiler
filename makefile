# install the .NET Core SDK here
# https://dotnet.microsoft.com/download/dotnet-core
#
# example: make run port=6001 root=/path/to/asp/files
run: root?=./aspclassiccompiler/AspWebServer/asp-sample
run: port?=6001
run: 
	dotnet run --project ./aspclassiccompiler/AspWebServer/AspWebServer.csproj --WebRoot=$(root) --Urls="http://localhost:$(port)"
