public class Program
{
    public async static Task Main(string[] args)
    {
        try{

            if(args.Length == 0)
                throw new ArgumentException("Program's arguments need to be inputted in the command line next to dotnet run");

            int seconds;
            if(!int.TryParse(args[2], out seconds))
                throw new ArgumentException("Interval is not a valid number!");

            FolderSynchronization sync = new FolderSynchronization(args[0], args[1], args[3]);

            // assuming that the time interval given represents a quantity of seconds

            TimeSpan syncInterval = TimeSpan.FromSeconds(seconds);
            PeriodicTimer timer = new PeriodicTimer(syncInterval);
            Console.WriteLine($"Starting folders synchronization\nThe folders will be synchronized in time intervals of {seconds} seconds");
            while (await timer.WaitForNextTickAsync()) 
            {   
                sync.SyncFiles();
            }
        }
        catch(Exception e){
            Console.WriteLine(e.ToString());
        }
    }
}
