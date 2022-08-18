using System.Collections;

namespace PentonimoSolver;

public static class Helpers
{

    public static void UpdatePercentageAndPrint(int nom, int denom, ref double lastPercentage, TimeSpan timeElapsed)
    {
        var percentage = Math.Round((nom / (double) denom), 5);
        if (percentage > lastPercentage)
        {
            Interlocked.Exchange(ref lastPercentage, percentage);
            Console.WriteLine($"{Math.Round(percentage * 100, 3)}% complete at {timeElapsed}");
        }
    }
}