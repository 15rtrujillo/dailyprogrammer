/*
A palindrome is a whole number that's the same when read backward in base 10, such as 12321 or 9449.

Given a positive whole number, find the smallest palindrome greater than the given number.

nextpal(808) => 818
nextpal(999) => 1001
nextpal(2133) => 2222
For large inputs, your solution must be much more efficient than incrementing and checking each subsequent number to see if it's a palindrome. Find nextpal(339) before posting your solution. Depending on your programming language, it should take a fraction of a second.

https://www.reddit.com/r/dailyprogrammer/comments/n3var6/20210503_challenge_388_intermediate_next/
*/

class NextPalindrome
{
    static int NextPal(int number)
    {
        // Immiediately break out if the number is 10 or less
        if (number < 11)
        {
            return 11;
        }



        return 0;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("This program will find the next palindrome number.");
        while (true)
        {
            Console.Write("Please enter a number: ");
            string? line = Console.ReadLine();
            if (line == null)
            {
                continue;
            }

            int number;
            try
            {
                number = Convert.ToInt32(line);
                if (number >= 0)
                {
                    break;
                }

                else
                {
                    Console.WriteLine("You need to enter a positive number.");
                }
            }

            catch
            {
                Console.WriteLine("You need to enter a number. Please try again.");
            }
        }

        Console.WriteLine($"The next palindrome is {NextPal(number)}");
    }
}