using System;
using System.Diagnostics;
using System.Threading;


/// <summary>
/// Console program to verbally announce elapsed time.
/// Command line argument is the number of seconds between announcements.
/// </summary>
class Program {
    static int Main(string[] args) {
        /// Check for new announcement every SLEEP_MSEC.
        const int SLEEP_MSEC = 500;
        const double DEFAULT_SEC_BETWEEN_ANNOUNCEMENTS = 5.0;
        var secBetweenAnnouncements = DEFAULT_SEC_BETWEEN_ANNOUNCEMENTS;

        Console.Write("\nHit the space bar to exit.");
        // Look for seconds between announcements on the command line.
        if (args.Length > 0) {
            try {
                secBetweenAnnouncements = Convert.ToDouble(args[0]);
            }
            catch (Exception e) when (e is FormatException || e is OverflowException) {
                // Couldn't parse the command line argument.
                Console.WriteLine($"Unable to convert '{args[0]}' to a double.");
                secBetweenAnnouncements = DEFAULT_SEC_BETWEEN_ANNOUNCEMENTS;
            }
        }
        Debug.Write($"\nTime between announcements = {secBetweenAnnouncements}");
        var announcer = new TimeAnnouncer(secBetweenAnnouncements);
        // Stopwatch gives the elapsed time.
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        // Keep track of whether the user has typed a space bar - to quit the program.
        bool spaceTyped = false;
        ConsoleKeyInfo keyInfo;
        while (spaceTyped == false) {
            // Get the elapsed time.
            TimeSpan ts = stopwatch.Elapsed;
            announcer.AnnounceIfTime((int)ts.TotalSeconds);
            Thread.Sleep(SLEEP_MSEC);
            while (Console.KeyAvailable) {
                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Spacebar) {
                    spaceTyped = true;
                }
            }
        }
        return 0;
    }
}
