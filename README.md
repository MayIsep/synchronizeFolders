# synchronizeFolders
A program that synchronizes two folders: source and replica. The program should maintain a full, identical copy of the source folder at the replica folder.

The program is based on the following rules:
- Synchronization must be one-way: after the synchronization content of the
replica folder should be modified to exactly match content of the source
folder;
- Synchronization should be performed periodically;
- File creation/copying/removal operations should be logged to a file and to the
console output;
- Folder paths, synchronization interval and log file path should be provided using
the command line arguments;

# How to run the app
To run the app open the Powershell command line in the project's folder (synchronizeFolders) and type the following commands:

    cd synchronizeFoldersApp
    dotnet build
    # replace the examples next to dotnet run for the actual arguments
    dotnet run SourceFolderPath ReplicaFolderPath TimeInterval LogFilePath
