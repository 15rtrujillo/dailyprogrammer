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

    static KeyValuePair<string, int> Bonus1()
    {
        return lettersums.First(pair => pair.Value == 319);
    }

    static int Bonus2()
    {
        var oddQuery = from pair in lettersums
        where pair.Value % 2 != 0
        select pair;

        return oddQuery.ToList().Count;
    }

    static KeyValuePair<int, int> Bonus3()
    {
        Dictionary<int, int> keyFrequency = new Dictionary<int, int>();

        // Create a dictionary that tracks how many times a specific lettersum appears
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

        // Order the dictionary
        var sortedKeyFrequency = from pair in keyFrequency
        orderby pair.Value descending select pair;

        return sortedKeyFrequency.ToList()[0];
    }

    static List<Tuple<string, string>> Bonus4()
    {
        List<Tuple<string, string>> elevenLetters = new List<Tuple<string, string>>();
        List<string> words = lettersums.Keys.ToList();

        /* lul inefficient
        for (int i = 0; i < words.Count; ++i)
        {
            for (int j = i + 1; j < words.Count; ++j)
            {
                if (Math.Abs(words[i].Length - words[j].Length) == 11)
                {
                    elevenLetters.Add(new Tuple<string, string>(words[i], words[j]));
                }
            }
        }
        */

        Dictionary<int, List<string>> wordsByLength = new Dictionary<int, List<string>>();

        // Separate the words out into "buckets"
        foreach (string word in words)
        {
            int length = word.Length;

            if (!wordsByLength.ContainsKey(length))
            {
                wordsByLength[length] = new List<string>();
            }
            
            wordsByLength[length].Add(word);
        }

        // Compare the appropriate buckets
        foreach (var keyValuePair in wordsByLength)
        {
            if (wordsByLength.ContainsKey(keyValuePair.Key + 11))
            {
                foreach (var word1 in keyValuePair.Value)
                {
                    foreach (var word2 in wordsByLength[keyValuePair.Key + 11])
                    {
                        elevenLetters.Add(new Tuple<string, string>(word1, word2));
                    }
                }
            }
        }

        return elevenLetters;
    }

    static List<Tuple<string, string>> Bonus5()
    {
        // Make a list of words that have a lettersum higher than 188.
        var query = from pair in lettersums
        where pair.Value > 188
        select pair.Key;

        List<string> words = query.ToList();

        // Make pairs of words that have the same lettersum.
        Dictionary<int, List<string>> wordsByLettersum = new Dictionary<int, List<string>>();

        // Sort words by their lettersum
        foreach (string word in words)
        {
            int lettersum = lettersums[word];
            if (!wordsByLettersum.ContainsKey(lettersum))
            {
                wordsByLettersum[lettersum] = new List<string>();
            }

            wordsByLettersum[lettersum].Add(word);
        }

        // Make the pairs
        List<Tuple<string, string>> matchingLettersums = new List<Tuple<string, string>>();

        foreach (List<string> matchingWords in wordsByLettersum.Values)
        {
            for (int i = 0; i < matchingWords.Count; ++i)
            {
                for (int j = i + 1; j < matchingWords.Count; ++j)
                {
                    matchingLettersums.Add(new Tuple<string, string>(matchingWords[i], matchingWords[j]));
                }
            }
        }

        // Rule out the pairs that have common letters.
        for (int i = matchingLettersums.Count - 1; i >= 0; --i)
        {
            var pair = matchingLettersums[i];
            foreach (char letter in pair.Item1)
            {
                if (pair.Item2.Contains(letter))
                {
                    matchingLettersums.RemoveAt(i);
                    break;
                }
            }
        }

        return matchingLettersums;
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
        ReadFile("..\\enable1.txt");

        /* 
        Bonus 1
        microspectrophotometries is the only word with a letter sum of 317. Find the only word with a letter sum of 319.
        */
        var sum319 = Bonus1();
        Console.WriteLine($"{sum319.Key} is the only word with a letter sum of {sum319.Value}.");

        /*
        Bonus 2
        How many words have an odd letter sum?
        */
        int oddLetterSums = Bonus2();
        Console.WriteLine($"There are {oddLetterSums} words with odd letter sums.");

        /*
        Bonus 3
        There are 1921 words with a letter sum of 100, making it the second most common letter sum. What letter sum is most common, and how many words have it?
        */
        KeyValuePair<int, int> mostCommon = Bonus3();
        Console.WriteLine($"The most common sum is {mostCommon.Key} which occurs {mostCommon.Value} times.");

        /*
        Bonus 4
        zyzzyva and biodegradabilities have the same letter sum as each other (151), and their lengths differ by 11 letters. Find the other pair of words with the same letter sum whose lengths differ by 11 letters.
        */

        // Find all the words that differ by 11 letters
        var elevenDifference = Bonus4();

        // See if their lettersums match
        foreach (var combo in elevenDifference)
        {
            if (lettersums[combo.Item1] == lettersums[combo.Item2])
            {
                Console.WriteLine($"{combo.Item1} and {combo.Item2}'s lengths differ by 11 letters and both have the letter sum of {lettersums[combo.Item1]}.");
            }
        }

        /*
        Bonus 5
        cytotoxicity and unreservedness have the same letter sum as each other (188), and they have no letters in common. Find a pair of words that have no letters in common, and that have the same letter sum, which is larger than 188. (There are two such pairs, and one word appears in both pairs.)
        */
        
        List<Tuple<string, string>> matchingLettersums = Bonus5();
        foreach (var pair in matchingLettersums)
        {
            Console.WriteLine($"{pair.Item1} and {pair.Item2} have no letters in common and have a lettersum of {lettersums[pair.Item1]}, which is greater than 188.");
        }

        /*
        Bonus 6
        The list of word { geographically, eavesdropper, woodworker, oxymorons } contains 4 words. Each word in the list has both a different number of letters, and a different letter sum. The list is sorted both in descending order of word length, and ascending order of letter sum. What's the longest such list you can find?
        */
    }
}