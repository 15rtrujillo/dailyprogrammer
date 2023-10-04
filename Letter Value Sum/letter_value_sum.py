"""
Challenge
Assign every lowercase letter a value, from 1 for a to 26 for z. Given a string of lowercase letters, find the sum of the values of the letters in the string.

lettersum("") => 0
lettersum("a") => 1
lettersum("z") => 26
lettersum("cab") => 6
lettersum("excellent") => 100
lettersum("microspectrophotometries") => 317

https://www.reddit.com/r/dailyprogrammer/comments/onfehl/20210719_challenge_399_easy_letter_value_sum/
"""


def lettersum(word: str) -> int:
    return sum(ord(letter) - (ord('a') - 1) for letter in word.lower())


def main():
    word = input("Please enter a sequence of letters: ")
    print("Letter Sum:", lettersum(word))


if __name__ == "__main__":
    main()