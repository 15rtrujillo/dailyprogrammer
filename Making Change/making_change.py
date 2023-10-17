"""
The country of Examplania has coins that are worth 1, 5, 10, 25, 100, and 500 currency units. At the Zeroth Bank of Examplania, you are trained to make various amounts of money by using as many ¤500 coins as possible, then as many ¤100 coins as possible, and so on down.

For instance, if you want to give someone ¤468, you would give them four ¤100 coins, two ¤25 coins, one ¤10 coin, one ¤5 coin, and three ¤1 coins, for a total of 11 coins.

Write a function to return the number of coins you use to make a given amount of change.

change(0) => 0
change(12) => 3
change(468) => 11
change(123456) => 254

https://www.reddit.com/r/dailyprogrammer/comments/nucsik/20210607_challenge_393_easy_making_change/
"""
def change(amount: int) -> int:
    coins = 0
    for denomination in [500, 100, 25, 10, 5, 1]:
        cur = amount // denomination
        amount -= (cur * denomination)
        coins += cur

    return coins


def main():
    amount = int(input("How much money would you like to withdraw? ¤"))
    print(f"I will give you {change(amount)} coins.")


if __name__ == "__main__":
    main()
