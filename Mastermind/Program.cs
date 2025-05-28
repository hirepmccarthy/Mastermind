using Mastermind.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        // Moving all the logic to a non-static class
        new MastermindService().Run();
    }
}