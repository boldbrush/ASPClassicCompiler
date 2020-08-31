# ASP Classic Compiler

[Original code](https://weblogs.asp.net/lichen/lessons-from-the-asp-classic-compiler-project) by Li Chen.

Prototype port to .NET Core by bijoutrouvaille @ BoldBrush.


## Running Instructions

1. Download .NET Core SDK: https://dotnet.microsoft.com/download/dotnet-core.
2. For a basic test that runs a sample website in `aspclassiccompiler/asp-sample`:
    - Open a linux shell
    - cd into the project folder
    - `make run` for a bare-bone ASP page test. 
    - view the asp website at `http://localhost:6001/index.asp`.
3. To customize port and web root:
    - create a folder on your system and put some ASP pages in it
    - assign its path to $myWebRoot shell variable
    - cd into the project folder
    - `make run port=8765 root="$myWebRoot"` 
    - view your custom ASP site at `http://localhost:8765/my-file.asp`.

## What is supported

Very few things, but all the ASP built-in objects are there. Session, cookies, Application.StaticObjects and Application.Contents may behave in funny ways.

You need to explicitly specify the path to a file in the URL. The server currently will not look for, say, `index.asp` on `/my/path/` and will exit with error.

## License

Open the source code under the Apache 2.0 license. 
