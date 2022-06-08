using System;


namespace dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            string appName = "Number guesser";
            int appVersion = 1;
            string appAuthor = "Patryk Ledzion";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[{appName}] Version: 0.0.{appVersion} - Author: {appAuthor}");
            Console.ResetColor();
            
            bool playAgain = true;

            do
            {
                Game();
                char choice = '0';
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Would you like to play again? (Y/N)");
                Console.ResetColor();
                GetInput(ref choice);

                switch(choice)
                {
                    case 'Y':
                        playAgain = true;
                        break;

                    case 'N':
                        playAgain = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("INcorrect choice!");
                        Console.ResetColor();
                        break;
                }
            } while (playAgain == true);
            

            Console.ReadKey();
        }

        static void Game()
        {

            Random rand = new Random();
            int drawnNumber = rand.Next(50);
            int guessedNumber = 0;
            do
            {
                Console.WriteLine("Enter a number: ");
                GetInput(ref guessedNumber);
                if (guessedNumber > drawnNumber)
                {
                    Console.WriteLine($"{guessedNumber} is greater than drawn number! Try again!");

                }
                else if (guessedNumber < drawnNumber)
                {
                    Console.WriteLine($"{guessedNumber} is lower than drawn number! Try again!");
                }

            }
            while (guessedNumber != drawnNumber);

            Console.WriteLine($"Congrats! The number was {drawnNumber}!");
        }
        
        static bool GetInput(ref int value)
        {
            try
            {
                value = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                value = 0;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Niepoprawny typ danych!");
                Console.ResetColor();
                return false;
            }

            return true;
        }
        static bool GetInput(ref char value)
        {
            try
            {
                value = Convert.ToChar(Console.ReadLine());
            }
            catch (Exception)
            {
                value = '0';
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Niepoprawny typ danych!");
                Console.ResetColor();
                return false;
            }

            return true;
        }
    }
}
