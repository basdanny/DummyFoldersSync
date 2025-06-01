# DummyFoldersSync
The following utility syncs (copies) folders and files from source folder to destination folder.
Only the files that do not exist in the destination folder will be copied. Existence check is naive - by file name.

## Build (.NET)
Run from terminal,  
For debug:  
`dotnet build`  
For release (publish):  
`dotnet publish -c Release -r win-x64`  
or with additional flags...  
`dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained false`

#### Prerequisites
.NET 8.x
