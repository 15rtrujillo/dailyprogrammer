def lettersum(word: str) -> int:
    return sum(ord(letter) - (ord('a') - 1) for letter in word.lower())


def main():
    word = input("Please enter a sequence of letters: ")
    print("Letter Sum:", lettersum(word))


if __name__ == "__main__":
    main()