using System.Buffers;

public class FolderSynchronization {

    readonly string SourceFolderPath;

    readonly string ReplicaFolderPath;

    readonly Logger Logger;

    public FolderSynchronization(string sourcePath, string replicaPath, string logPath){

        if(!Directory.Exists(sourcePath))
            throw new ArgumentException("Source file path does not exist!");
        
        if(!Directory.Exists(replicaPath))
            throw new ArgumentException("Replica file path does not exist!");

        this.Logger = new Logger(logPath);
        this.SourceFolderPath = sourcePath;
        this.ReplicaFolderPath = replicaPath;
    }

    private void DeleteFile(string path){
        File.Delete(path);
        this.Logger.LogOperation(this.SourceFolderPath, this.ReplicaFolderPath, path, Logger.Operation.Delete);
    }

    private void CopyFile(string sourceFilePath, string replicaFilePath){
        bool fileExisted = File.Exists(replicaFilePath);

        File.Copy(sourceFilePath, replicaFilePath, true);

        // if file did not exist in replica folder the file creation should be logged
        if(!fileExisted)
            this.Logger.LogOperation(this.SourceFolderPath, this.ReplicaFolderPath, replicaFilePath, Logger.Operation.Create);

        this.Logger.LogOperation(this.SourceFolderPath, this.ReplicaFolderPath, replicaFilePath, Logger.Operation.Copy);
    }

    public void SyncFiles(){
        this.DeleteFiles(this.SourceFolderPath, this.ReplicaFolderPath);
        this.CopyFiles(this.SourceFolderPath, this.ReplicaFolderPath);
    }

    private void CopyFiles(string sourceFolder, string replicaFolder){

        foreach(string filePath in Directory.GetFiles(sourceFolder)){
            // when the contents of a folder inside the source folder need to be copied, the folder needs to be created before copying
            if(!Directory.Exists(replicaFolder))    
                Directory.CreateDirectory(replicaFolder);
            this.CopyFile(filePath, filePath.Replace(sourceFolder, replicaFolder));
        }

        // call recursively for subFolders, adding to the replica path the new folder
        string [] subFolders = Directory.GetDirectories(sourceFolder);
        foreach(string folder in subFolders)
            CopyFiles(folder, folder.Replace(sourceFolder, replicaFolder)); 
    }

    private void DeleteFiles(string sourceFolder, string replicaFolder){

        foreach(string filePath in Directory.GetFiles(replicaFolder)){

            string sourceFilePath = filePath.Replace(replicaFolder, sourceFolder);

            // if file exists in replica but not in source, then it was deleted from source and should be deleted in replica
            if(!File.Exists(sourceFilePath)) 
                this.DeleteFile(filePath);
        }

        // if folder exists in replica but not in source, then it was deleted from source and should be deleted in replica
        if(!Directory.Exists(sourceFolder))
            Directory.Delete(replicaFolder);
        else{
            // call recursively for subFolders, adding to the source path the new folder
            string [] subFolders = Directory.GetDirectories(replicaFolder);
            foreach(string folder in subFolders)
                DeleteFiles(folder.Replace(replicaFolder, sourceFolder), folder); 
        }
    }


}