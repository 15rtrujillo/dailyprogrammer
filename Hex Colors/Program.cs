/*
Challenge
Given three integers between 0 and 255, corresponding to the red, green, and blue channel values of a color, find the hex string for that color. You may use anything built into your programming language, such as for base conversion, but you can also do it manually.

Examples
hexcolor(255, 99, 71) => "#FF6347"  (Tomato)
hexcolor(184, 134, 11) => "#B8860B"  (DarkGoldenrod)
hexcolor(189, 183, 107) => "#BDB76B"  (DarkKhaki)
hexcolor(0, 0, 205) => "#0000CD"  (MediumBlue)

Optional bonus: color blending
Given a list of hex color strings, produce the hex color string you get from averaging their RGB values together. You'll need to round channel values to integers.

blend({"#000000", "#778899"}) => "#3C444C" - Pretty sure this is a typo and the red should be 3B
blend({"#E6E6FA", "#FF69B4", "#B0C4DE"}) => "#DCB1D9"

https://www.reddit.com/r/dailyprogrammer/comments/a0lhxx/20181126_challenge_369_easy_hex_colors/
*/

using System.Text;

class HexColors
{
    static readonly Dictionary<int, char> hexLetters = new Dictionary<int, char>();

    static HexColors()
    {
        hexLetters.Add(10, 'A');
        hexLetters.Add(11, 'B');
        hexLetters.Add(12, 'C');
        hexLetters.Add(13, 'D');
        hexLetters.Add(14, 'E');
        hexLetters.Add(15, 'F');
    }

    static int HexToInt(string hex)
    {
        int integer = 0;
        int digits = hex.Length;

        for (int i = 0; i < digits; ++i)
        {
            char charDigit = hex[i];
            int digit;
            if (charDigit >= '0' && charDigit <= '9')
            {
                digit = charDigit - '0';
            }

            else if (charDigit >= 'A' && charDigit <= 'F')
            {
                digit = hexLetters.First(pair => pair.Value == charDigit).Key;
            }

            else
            {
                throw new FormatException("Hexadecimal number is not formatted correctly");
            }

            int exponent = digits - i - 1;
            int placeValueMask = digit * (int)Math.Pow(16, exponent);

            integer += placeValueMask;
        }

        return integer;
    }

    static string Blend(string[] blendColors)
    {
        StringBuilder newColorBuilder = new StringBuilder("#");

        int hexLength = blendColors[0].Length;

        for (int i = 1; i < hexLength; i += 2)
        {
            int sum = 0;
            foreach (string color in blendColors)
            {
                sum += HexToInt(color.Substring(i, 2));
            }
            int average = sum / blendColors.Length;
            newColorBuilder.Append(IntToHex(average).PadLeft(2, '0'));
        }

        return newColorBuilder.ToString();
    }

    static string IntToHex(int integer)
    {   
        string hex = "";
        while (integer > 0)
        {
            int digit = integer % 16;
            integer /= 16;
            if (digit > 9)
            {
                hex = hexLetters[digit] + hex;
            }

            else
            {
                hex = digit + hex;
            }
        }

        return hex;
    }

    static string HexColor(int[] rgb)
    {
        StringBuilder hex = new StringBuilder("#");
        foreach (int color in rgb)
        {
            hex.Append(IntToHex(color).PadLeft(2, '0'));
        }

        return hex.ToString();
    }

    static void Main(String[] args)
    {
        int[] rgb;
        while (true)
        {
            Console.Write("Please enter an RGB value.\nThis will be three whole numbers from 0-255 separated by commas: ");
            string? input = Console.ReadLine();
            if (input == null)
            {
                continue;
            }

            string[] rbgValues = input.Split(",");

            try
            {
                rgb = (from value in (from stringValue in rbgValues
                select int.Parse(stringValue.Trim()))
                where value >= 0 && value <= 255
                select value).ToArray();

                if (rgb.Length == 3)
                {
                    break;
                }
            }
            catch (FormatException) {}

            Console.WriteLine("Please enter three integers from 0-255 separated by commas.");
        }

        Console.WriteLine($"Your color is {HexColor(rgb)} in hexadecimal.");

        // Can't be bothered to verify that they only contain A-F
        string[] toBlend;
        while (true)
        {
            Console.Write("Please enter multiple hex colors separated by commas to blend them.\nYour hex colors should have a # followed by 6 hexadecimal digits: ");
            string? line = Console.ReadLine();
            if (line == null)
            {
                continue;
            }

            string[] possibleColors = line.Split(",");

            toBlend = (from value in (from possibleColor in possibleColors
            where possibleColor.Trim().StartsWith("#") && possibleColor.Trim().Length == 7
            select possibleColor.Trim().ToUpper())
            select value).ToArray();

            if (toBlend.Length > 1)
            {
                break;
            }

            Console.WriteLine("Please enter two hex colors separated by commas.");
        }

        Console.WriteLine($"Blending those {toBlend.Length} colors together results in {Blend(toBlend)}");
    }
}
