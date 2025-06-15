# DummyFoldersSync
The following utility syncs (copies) folders and files from source folder to destination folder.
Only the files that do not exist in the destination folder will be copied. Existence check is naive - by file name.

## Build (.NET)
Run from terminal,  
For debug:  
```powershell
dotnet build
```  
For release (publish):  
```powershell
dotnet publish -c Release -r win-x64
```  
or with additional flags...  
```powershell
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false
```

#### Prerequisites
.NET 8.x
