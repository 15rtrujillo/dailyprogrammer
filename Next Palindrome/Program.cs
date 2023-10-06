/*
A palindrome is a whole number that's the same when read backward in base 10, such as 12321 or 9449.

Given a positive whole number, find the smallest palindrome greater than the given number.

nextpal(808) => 818
nextpal(999) => 1001
nextpal(2133) => 2222
For large inputs, your solution must be much more efficient than incrementing and checking each subsequent number to see if it's a palindrome. Find nextpal(3^39) before posting your solution. Depending on your programming language, it should take a fraction of a second.

https://www.reddit.com/r/dailyprogrammer/comments/n3var6/20210503_challenge_388_intermediate_next/
*/

class NextPalindrome
{
    static int[] GetNumberArray(int number, int digits)
    {
        int[] numberArray = new int[digits];
        for (int i = 0; i < digits; ++i)
        {
            int mask = (int)Math.Pow(10, digits-i-1);
            int digit = number / mask;
            number -= mask * digit;
            numberArray[i] = digit;
        }

        return numberArray;
    }

    static int ReconstructNumber(int[] digits)
    {
        int number = 0;
        for (int i = 0; i < digits.Length; ++i)
        {
            number += (int)Math.Pow(10, i) * digits[digits.Length-i-1];
        }

        return number;
    }

    static int NextPal(int number)
    {
        // Going to try to solve this without strings
        // Immediately break out if the number is 10 or less
        if (number < 11)
        {
            return 11;
        }

        // Increment the number so that we don't end up finding the same palindrome.
        int nextPalindrome = number + 1;

        // Determine how many digits we have
        int digits = (int)Math.Log10(nextPalindrome) + 1;

        // If we have an odd number of digits
        if (digits % 2 != 0)
        {
            // Conver the number into an array
            int[] numberArray = GetNumberArray(nextPalindrome, digits);

            // Get the index of the middle element
            int midpoint = (int)Math.Floor(digits / 2.0);

            // Create an array from everything on the left-hand side of the midpoint
            // This will take everything except the midpoint itself.
            int[] lhs = numberArray.Take(midpoint).ToArray();
            // Create an array from everything on the right-hand side of the midpoint
            // This will take everything except the midpoint itself.
            int[] rhs = numberArray.Skip(midpoint+1).Take(midpoint).ToArray();

            // If the right-hand side is greater than the left, we will increase the value stored at the midpoint by 1.
            if (ReconstructNumber(lhs) < ReconstructNumber(rhs))
            {
                numberArray[midpoint] += 1;
            }

            // We now need to make the right-hand side the inverse of the left-hand side
            lhs = lhs.Reverse().ToArray();

            lhs.CopyTo(numberArray, midpoint + 1);

            nextPalindrome = ReconstructNumber(numberArray);
        }

        // We have an even number of digits
        else
        {
            // Conver the number into an array
            int[] numberArray = GetNumberArray(nextPalindrome, digits);

            int half = numberArray.Length / 2;

            // Create an array from everything on the left-hand side.
            int[] lhs = numberArray.Take(half).ToArray();

            // Create an array from everything on the right-hand side.
            int[] rhs = numberArray.Skip(half).Take(half).ToArray();

            // If the right-hand side is greater than the inverse of the left-hand side, we need to add 1 to the left-hand side.
            if (ReconstructNumber(rhs) > ReconstructNumber(lhs.Reverse().ToArray()))
            {
                lhs = GetNumberArray(ReconstructNumber(lhs) + 1, lhs.Length);
            }

            // Now create the new number.
            // Copy over the left-hand side
            lhs.CopyTo(numberArray, 0);
            // Copy over the inverse of the left-hand side to replace the original right-hand side
            lhs.Reverse().ToArray().CopyTo(numberArray, lhs.Length);

            nextPalindrome = ReconstructNumber(numberArray);
        }

        return nextPalindrome;
    }

    static bool CheckPalindrome(int number)
    {
        // Don't care about using strings here, this isn't part of the challenge.
        string num = number.ToString();
        int half = num.Length / 2;
        for (int i = 0; i < half; ++i)
        {
            if (num[i] != num[num.Length - i - 1])
            {
                return false;
            }
        }

        return true;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("This program will find the next palindrome number.");

        int number;
        while (true)
        {
            Console.Write("Please enter a number: ");
            string? line = Console.ReadLine();
            if (int.TryParse(line, out number))
            {
                if (number >= 0)
                {
                    break;
                }
            }

            else
            {
                Console.WriteLine("You need to enter a positive number. Please try again.");
            }
        }

        int nextPalindrome = (int)Math.Pow(3, 39); // NextPal(number);
        Console.WriteLine($"The next palindrome is {nextPalindrome}");
        if (CheckPalindrome(nextPalindrome))
        {
            Console.WriteLine("After checking, this is indeed a palindrome.");
        }

        else
        {
            Console.WriteLine("There must be an error somewhere, this is not a palindrome.");
        }
    }
}