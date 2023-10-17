/*
For the purpose of today's challenge, a Roman numeral is a non-empty string of the characters M, D, C, L, X, V, and I, each of which has the value 1000, 500, 100, 50, 10, 5, and 1. The characters are arranged in descending order, and the total value of the numeral is the sum of the values of its characters. For example, the numeral MDCCXXVIIII has the value 1000 + 500 + 2x100 + 2x10 + 5 + 4x1 = 1729.

This challenge uses only additive notation for roman numerals. There's also subtractive notation, where 9 would be written as IX. You don't need to handle subtractive notation (but you can if you want to, as an optional bonus).

Given two Roman numerals, return whether the first one is less than the second one:

numcompare("I", "I") => false
numcompare("I", "II") => true
numcompare("II", "I") => false
numcompare("V", "IIII") => false
numcompare("MDCLXV", "MDCLXVI") => true
numcompare("MM", "MDCCCCLXXXXVIIII") => false
You only need to correctly handle the case where there are at most 1 each of D, L, and V, and at most 4 each of C, X, and I. You don't need to validate the input, but you can if you want. Any behavior for invalid inputs like numcompare("V", "IIIIIIIIII") is fine - true, false, or error.

Try to complete the challenge without actually determining the numerical values of the inputs.

https://www.reddit.com/r/dailyprogrammer/comments/oe9qnb/20210705_challenge_397_easy_roman_numeral/
 */

import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

class RomanNumeralComparison {
    final static HashMap<Character, Integer> numeralMap = new HashMap<Character, Integer>();

    static {
        numeralMap.put('M', 1000);
        numeralMap.put('D', 500);
        numeralMap.put('C', 100);
        numeralMap.put('L', 50);
        numeralMap.put('X', 10);
        numeralMap.put('V', 5);
        numeralMap.put('I', 1);
    }

    private static boolean numCompare(String numeral1, String numeral2) {
        // I had help with this :\
        // I remembered that streams existed, but I wouldn't have figured this out.
        int value1 = numeral1.chars()
            .mapToObj(c -> (char)c)
            .mapToInt(numeralMap::get)
            .sum();

        // This is how I would have done it.
        int value2 = 0;
        for (char c : numeral2.toCharArray()) {
            value2 += numeralMap.get(c);
        }

        return value1 < value2;
    }

    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        System.out.print("Please input a Roman numeral: ");
        String numeral1 = scanner.nextLine().toUpperCase();

        System.out.print("Please input a Roman numeral: ");
        String numeral2 = scanner.nextLine().toUpperCase();

        System.out.println(numeral1 + " is less than " + numeral2 + ": " + numCompare(numeral1, numeral2));
    }
}
