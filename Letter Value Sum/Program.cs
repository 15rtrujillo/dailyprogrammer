/*
Challenge
Assign every lowercase letter a value, from 1 for a to 26 for z. Given a string of lowercase letters, find the sum of the values of the letters in the string.

lettersum("") => 0
lettersum("a") => 1
lettersum("z") => 26
lettersum("cab") => 6
lettersum("excellent") => 100
lettersum("microspectrophotometries") => 317
Optional bonus challenges
Use the enable1 word list for the optional bonus challenges.

microspectrophotometries is the only word with a letter sum of 317. Find the only word with a letter sum of 319.

How many words have an odd letter sum?

There are 1921 words with a letter sum of 100, making it the second most common letter sum. What letter sum is most common, and how many words have it?

zyzzyva and biodegradabilities have the same letter sum as each other (151), and their lengths differ by 11 letters. Find the other pair of words with the same letter sum whose lengths differ by 11 letters.

cytotoxicity and unreservedness have the same letter sum as each other (188), and they have no letters in common. Find a pair of words that have no letters in common, and that have the same letter sum, which is larger than 188. (There are two such pairs, and one word appears in both pairs.)

The list of word { geographically, eavesdropper, woodworker, oxymorons } contains 4 words. Each word in the list has both a different number of letters, and a different letter sum. The list is sorted both in descending order of word length, and ascending order of letter sum. What's the longest such list you can find?
https://www.reddit.com/r/dailyprogrammer/comments/onfehl/20210719_challenge_399_easy_letter_value_sum/
*/

using System.Collections.Specialized;

class LetterValueSum
{
    static Dictionary<string, int> lettersums = new Dictionary<string, int>();

    static int LetterSum(string letters)
    {
        int sum = 0;
        foreach (char c in letters.ToCharArray())
        {
            sum += c - ('a' - 1);
        }
        return sum;
    }

    static void ReadFile(string fileName)
    {
        StreamReader reader = new StreamReader(fileName);
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            int sum = LetterSum(line);
            lettersums.Add(line, sum);
        }
    }

    static KeyValuePair<string, int> GetLetterSum319()
    {
        return lettersums.First(pair => pair.Value == 319);
    }

    static int GetOddLetterSums()
    {
        var oddQuery = from pair in lettersums
        where pair.Value % 2 != 0
        select pair;

        return oddQuery.ToList().Count;
    }

    static KeyValuePair<int, int> MostCommonSum()
    {
        Dictionary<int, int> keyFrequency = new Dictionary<int, int>();

        foreach (var lettersum in lettersums)
        {
            if (keyFrequency.ContainsKey(lettersum.Value))
            {
                keyFrequency[lettersum.Value] = keyFrequency[lettersum.Value] + 1;
            }

            else
            {
                keyFrequency.Add(lettersum.Value, 1);
            }
        }

        var sortedKeyFrequency = from pair in keyFrequency
        orderby pair.Value descending select pair;

        return sortedKeyFrequency.ToList()[0];
    }

    static void Main(string[] args)
    {
        // Original challenge
        Console.Write("Please enter a string of letters: ");
        string? letters = Console.ReadLine();
        if (letters == null)
        {
            return;
        }
        Console.WriteLine($"Letter Sum: {LetterSum(letters.ToLower())}");

        // For the bonuses, we first need to read in the file and sum all the words
        ReadFile("enable1.txt");

        /* 
        Bonus 1
        microspectrophotometries is the only word with a letter sum of 317. Find the only word with a letter sum of 319.
        */
        var sum319 = GetLetterSum319();
        Console.WriteLine($"{sum319.Key} has is the only word with a letter sum of {sum319.Value}.");

        /*
        Bonus 2
        How many words have an odd letter sum?
        */
        int oddLetterSums = GetOddLetterSums();
        Console.WriteLine($"There are {oddLetterSums} words with odd letter sums.");

        /*
        Bonus 3
        There are 1921 words with a letter sum of 100, making it the second most common letter sum. What letter sum is most common, and how many words have it?
        */
        KeyValuePair<int, int> mostCommon = MostCommonSum();
        Console.WriteLine($"The most common sum is {mostCommon.Key} which occurs {mostCommon.Value} times.");

        /*
        Bonus 4
        zyzzyva and biodegradabilities have the same letter sum as each other (151), and their lengths differ by 11 letters. Find the other pair of words with the same letter sum whose lengths differ by 11 letters.
        */

        /*
        Bonus 5
        cytotoxicity and unreservedness have the same letter sum as each other (188), and they have no letters in common. Find a pair of words that have no letters in common, and that have the same letter sum, which is larger than 188. (There are two such pairs, and one word appears in both pairs.)
        */
        
        /*
        Bonus 6
        The list of word { geographically, eavesdropper, woodworker, oxymorons } contains 4 words. Each word in the list has both a different number of letters, and a different letter sum. The list is sorted both in descending order of word length, and ascending order of letter sum. What's the longest such list you can find?
        */
    }
}