/*
For the purpose of this challenge, Morse code represents every letter as a sequence of 1-4 characters, each of which is either . (dot) or - (dash). The code for the letter a is .-, for b is -..., etc. The codes for each letter a through z are:

.- -... -.-. -.. . ..-. --. .... .. .--- -.- .-.. -- -. --- .--. --.- .-. ... - ..- ...- .-- -..- -.-- --..
Normally, you would indicate where one letter ends and the next begins, for instance with a space between the letters' codes, but for this challenge, just smoosh all the coded letters together into a single string consisting of only dashes and dots.

Examples
smorse("sos") => "...---..."
smorse("daily") => "-...-...-..-.--"
smorse("programmer") => ".--..-.-----..-..-----..-."
smorse("bits") => "-.....-..."
smorse("three") => "-.....-..."
An obvious problem with this system is that decoding is ambiguous. For instance, both bits and three encode to the same string, so you can't tell which one you would decode to without more information.

Optional bonus challenges
For these challenges, use the enable1 word list. It contains 172,823 words. If you encode them all, you would get a total of 2,499,157 dots and 1,565,081 dashes.

The sequence -...-....-.--. is the code for four different words (needing, nervate, niding, tiling). Find the only sequence that's the code for 13 different words.

autotomous encodes to .-..--------------..-..., which has 14 dashes in a row. Find the only word that has 15 dashes in a row.

Call a word perfectly balanced if its code has the same number of dots as dashes. counterdemonstrations is one of two 21-letter words that's perfectly balanced. Find the other one.

protectorate is 12 letters long and encodes to .--..-.----.-.-.----.-..--., which is a palindrome (i.e. the string is the same when reversed). Find the only 13-letter word that encodes to a palindrome.

--.---.---.-- is one of five 13-character sequences that does not appear in the encoding of any word. Find the other four.

https://www.reddit.com/r/dailyprogrammer/comments/cmd1hb/20190805_challenge_380_easy_smooshed_morse_code_1/
*/

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map.Entry;
import java.util.Scanner;

class SmooshedMorseCode {

    private final static String morse = ".- -... -.-. -.. . ..-. --. .... .. .--- -.- .-.. -- -. --- .--. --.- .-. ... - ..- ...- .-- -..- -.-- --..";

    private final static HashMap<Character, String> morseCode = new HashMap<Character, String>();
    private final static HashMap<String, String> codedWords = new HashMap<String, String>();

    static {
        char letter = 'a';
        for (String code : morse.split(" ")) {
            morseCode.put(letter++, code);
        }
    }

    private static String sMorse(String word) {
        StringBuilder morseBuilder = new StringBuilder();

        for (char letter : word.toCharArray()) {
            morseBuilder.append(morseCode.get(letter));
        }

        return morseBuilder.toString();
    }

    private static void readFile(String fileName) {
        try {
            File file = new File(fileName);
            FileReader reader = new FileReader(file);
            BufferedReader buffReader = new BufferedReader(reader);

            String line = buffReader.readLine();
            while (line != null) {
                codedWords.put(line, sMorse(line));

                line = buffReader.readLine();
            }
        } catch (Exception ex) {
            System.out.println("Error reading from enable1.txt file.");
            System.exit(-1);
        }
    }

    private static String bonus1() {
        /*
        The sequence -...-....-.--. is the code for four different words (needing, nervate, niding, tiling). Find the only sequence that's the code for 13 different words.
        */
        HashMap<String, Integer> codeOccurrences = new HashMap<String, Integer>();

        for (String sequence : codedWords.values()) {
            if (codeOccurrences.containsKey(sequence)) {
                codeOccurrences.put(sequence, codeOccurrences.get(sequence) + 1);
            } else {
                codeOccurrences.put(sequence, 1);
            }
        }

        for (Entry<String, Integer> occurrence : codeOccurrences.entrySet()) {
            if (occurrence.getValue() == 13) {
                return occurrence.getKey();
            }
        }

        return null;
    }

    private static Entry<String, String> bonus2() {
        /*
        autotomous encodes to .-..--------------..-..., which has 14 dashes in a row. Find the only word that has 15 dashes in a row.
        */

        for (Entry<String, String> entry : codedWords.entrySet()) {
            String sequence = entry.getValue();
            // Skip codes that are too short
            if (sequence.length() < 15) {
                continue;
            }

            int longestDashCount = 0;
            int currentDashCount = 0;
            char[] sequenceChars = sequence.toCharArray();
            for (int i = 0; i < sequence.length(); ++i) {
                char c = sequenceChars[i];
                // If we reach a . and there is less than 15 characters left, we know that this isn't the sequence
                if (c == '.' && (sequence.length() - i) < 15) {
                    break;
                } else if (c == '.') {
                    if (currentDashCount > longestDashCount) {
                        longestDashCount = currentDashCount;
                    }
                    currentDashCount = 0;
                } else if (c == '-') {
                    ++currentDashCount;
                }
            }

            if (currentDashCount > longestDashCount) {
                longestDashCount = currentDashCount;
            }

            if (longestDashCount == 15) {
                return entry;
            }
        }

        return null;
    }

    private static Entry<String, String> bonus3() {
        /*
        Call a word perfectly balanced if its code has the same number of dots as dashes. counterdemonstrations is one of two 21-letter words that's perfectly balanced. Find the other one.
        */
        for (Entry<String, String> entry : codedWords.entrySet()) {
            if (entry.getKey().length() != 21) {
                continue;
            }

            if (entry.getKey().equals("counterdemonstrations")) {
                continue;
            }

            int dotsCount = 0;
            int dashCount = 0;
            for (char c : entry.getValue().toCharArray()) {
                if (c == '.') {
                    ++dotsCount;
                } else if (c == '-') {
                    ++dashCount;
                }
            }

            if (dotsCount == dashCount) {
                return entry;
            }
        }

        return null;
    }

    private static Entry<String, String> bonus4() {
        /*
        protectorate is 12 letters long and encodes to .--..-.----.-.-.----.-..--., which is a palindrome (i.e. the string is the same when reversed). Find the only 13-letter word that encodes to a palindrome.
        */
        for (Entry<String, String> entry : codedWords.entrySet()) {
            if (entry.getKey().length() != 13) {
                continue;
            }

            boolean palindrome = true;
            char[] sequence = entry.getValue().toCharArray();
            for (int i = 0; i < sequence.length / 2; ++i) {
                if (sequence[i] != sequence[sequence.length - i - 1]) {
                    palindrome = false;
                    break;
                }
            }

            if (palindrome) {
                return entry;
            }
        }

        return null;
    }

    private static ArrayList<String> bonus5() {
        /*
        --.---.---.-- is one of five 13-character sequences that does not appear in the encoding of any word. Find the other four.
        */

        // Generate permutations of length 13.
        ArrayList<String> permutations = new ArrayList<String>();
        for (int i = 0; i < Math.pow(2, 13); ++i) {
            char[] perm = { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' };
            int index = 0;
            for (int j = i; j > 0; j /= 2) {
                boolean dot = j % 2 == 0;
                perm[index++] = dot ? '.' : '-';
            }

            permutations.add(new String(perm));
        }

        // Now check if they exist in any of the coded sequences.
        for (Iterator<String> permIter = permutations.iterator(); permIter.hasNext();) {
            String permutation = permIter.next();

            for (String sequence : codedWords.values()) {
                if (sequence.contains(permutation)) {
                    permIter.remove();
                    break;
                }
            }
        }

        return permutations;
    }

    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        System.out.print("Please input a string of alpha characters: ");
        String word = scanner.nextLine();

        System.out.println("The morse code is: " + sMorse(word));

        readFile("..\\enable1.txt");

        System.out.println("\nBonus 1");
        String bonus1 = bonus1();
        System.out.println(bonus1 + " is the only sequence that appears for 13 different words");
        System.out.print("Those words are: ");
        StringBuilder wordsBuilder = new StringBuilder();
        for (Entry<String, String> entry : codedWords.entrySet()) {
            if (entry.getValue().equals(bonus1)) {
                wordsBuilder.append(entry.getKey());
                wordsBuilder.append(", ");
            }
        }
        wordsBuilder.setLength(wordsBuilder.length() - 2);
        System.out.println(wordsBuilder.toString());

        System.out.println("\nBonus 2");
        Entry<String, String> bonus2 = bonus2();
        System.out.println(bonus2.getKey() + " is the only word with a sequence that contains 15 dashes in a row: " + bonus2.getValue());

        System.out.println("\nBonus 3");
        Entry<String, String> bonus3 = bonus3();
        System.out.println(bonus3.getKey() + " is the only word besides counterdemonstrations that has 21 letters and has an encoding that is perfectly balanced: " + bonus3.getValue());

        System.out.println("\nBonus 4");
        Entry<String, String> bonus4 = bonus4();
        System.out.println(bonus4.getKey() + " is the only word that is 13 letters and encodes to a palindrome: " + bonus4.getValue());

        System.out.println("\nBonus 5");
        ArrayList<String> bonus5 = bonus5();
        System.out.println("There are five 13-character sequences that do not appear in the encoding of any word.\nThey are:");
        for (String sequence : bonus5) {
            System.out.println(sequence);
        }
    }
}
