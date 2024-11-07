using System.IO.Enumeration;
using System.Text.Json;

public class Logger {

    readonly string LogPath;

    public enum Operation {
        Create,
        Copy,
        Delete
    }

    public class LogObject
    {
        public DateTimeOffset TimeStamp { get; set; }

        public required string File { get; set; }

        public required string Operation {get; set;}  

        public required string Source { get; set; }

        public required string Replica { get; set; }    
    }

    public Logger(string logPath){
        if(!File.Exists(logPath))
            throw new ArgumentException("Log file path does not exist!");
        this.LogPath = logPath;
    }

    public void LogOperation(string sourcePath, string replicaPath, string filePath, Operation op){
        string msg;
        string opToStr = "";

        switch(op)
        {
            case Operation.Create:
                opToStr = "CREATE";
                break;
            case Operation.Copy:
                opToStr = "COPY";
                break;
            case Operation.Delete:
                opToStr = "DELETE";
                break; 
        }

        LogObject logJson = new LogObject
        {
            TimeStamp = DateTime.Now,
            File = filePath,
            Source = sourcePath,
            Replica = replicaPath,
            Operation = opToStr
        };
        
        msg = JsonSerializer.Serialize(logJson) + Environment.NewLine;
            
        Console.WriteLine(msg);
        using (StreamWriter sw = new StreamWriter(this.LogPath, true))
        {
            sw.WriteLine(msg);
        }
    }
}
