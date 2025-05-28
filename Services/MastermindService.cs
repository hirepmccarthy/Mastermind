namespace Mastermind.Services
{
    public class MastermindService
    {
        private int NumberAttempts => 10;
        private int NumberDigits => 4;
        private int LowerBound => 1;
        private int UpperBound => 6;
        private bool TrackGuesses => false;

        public string[] Answer { get; set; }
        public List<string> Guesses { get; set; }

        public MastermindService() { }

        public void Run()
        {
            Guesses = new List<string>();
            GenerateAnswer();

            Console.Clear();
            Console.WriteLine($"Welcome to Mastermind!\n");
            Console.WriteLine($"You have {NumberAttempts} attempts to guess a {NumberDigits} digit number. Each digit is between {LowerBound} and {UpperBound}.");
            Console.WriteLine("After each attempt, the hint will display a (-) for every digit that is correct but in the wrong position, and a (+) for every digit that is both correct AND in the correct position.\nGood luck!\n");

            int attemptCount = 1;
            while (true)
            {
                if (attemptCount > NumberAttempts)
                {
                    Console.WriteLine($"\nSorry, you lose. The answer was {String.Join(string.Empty, Answer)}.");
                    break;
                }

                Console.WriteLine($"Attempt #{attemptCount}");
                string? guess = Console.ReadLine();

                if (guess == null || guess.Length < NumberDigits)
                {
                    Console.WriteLine($"Guess must be {NumberDigits} digits.\n");
                    continue;
                }

                // Can be configured to track previous guesses
                if (TrackGuesses && Guesses.Contains(guess))
                {
                    Console.WriteLine($"You've already guessed {guess}.\n");
                    continue;
                }
                Guesses.Add(guess);

                string[] guessArray = ConvertGuessToArray(guess);
                int correctDigits = CorrectDigit(guessArray);
                int correctPosition = CorrectPosition(guessArray);

                if (correctPosition == NumberDigits)
                {
                    Console.WriteLine("Congratulations, you win!");
                    break;
                }

                string hintDisplay = string.Empty;
                for (int i = 0; i < correctPosition; i++)
                {
                    hintDisplay = hintDisplay + "+";
                }
                // Correct positions are counted in both checks, so the number of correct digits
                // should be the difference between the two.
                for (int j = 0; j < (correctDigits - correctPosition); j++)
                {
                    hintDisplay = hintDisplay + "-";
                }
                Console.WriteLine($"Hint: {hintDisplay}\n");

                attemptCount++;
            }
        }

        private void GenerateAnswer()
        {
            Answer = new string[NumberDigits];

            Random rnd = new Random();
            for (int i = 0; i < NumberDigits; i++)
            {
                // Linq doesn't support chars so need to convert to individual strings
                Answer[i] = rnd.Next(LowerBound, UpperBound).ToString();
            }
        }

        private string[] ConvertGuessToArray(string guess)
        {
            return guess.ToCharArray().Select(x => x.ToString()).ToArray();
        }

        private int CorrectDigit(string[] guess)
        {
            if (guess.Length != NumberDigits)
                return 0;

            if (guess.Length != Answer.Length)
                return 0;

            return guess.Where(x => Answer.Distinct().Contains(x)).Count();
        }

        private int CorrectPosition(string[] guess)
        {
            if (guess.Length != NumberDigits)
                return 0;

            if (guess.Length != Answer.Length)
                return 0;

            int returnVal = 0;
            for (int i = 0; i < NumberDigits; i++)
            {
                if (guess[i] == Answer[i])
                    returnVal++;
            }

            return returnVal;
        }
    }
}
