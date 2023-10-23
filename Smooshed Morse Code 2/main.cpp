/*
A permutation of the alphabet is a 26-character string in which each of the letters a through z appears once.

Given a smooshed Morse code encoding of a permutation of the alphabet, find the permutation it encodes, or any other permutation that produces the same encoding (in general there will be more than one). It's not enough to write a program that will eventually finish after a very long period of time: run your code through to completion for at least one example.

Examples
smalpha(".--...-.-.-.....-.--........----.-.-..---.---.--.--.-.-....-..-...-.---..--.----..")
    => "wirnbfzehatqlojpgcvusyxkmd"
smalpha(".----...---.-....--.-........-----....--.-..-.-..--.--...--..-.---.--..-.-...--..-")
    => "wzjlepdsvothqfxkbgrmyicuna"
smalpha("..-...-..-....--.---.---.---..-..--....-.....-..-.--.-.-.--.-..--.--..--.----..-..")
    => "uvfsqmjazxthbidyrkcwegponl"
Again, there's more than one valid output for these inputs.

Optional bonus 1
Here's a list of 1000 inputs. How fast can you find the output for all of them? A good time depends on your language of choice and setup, so there's no specific time to aim for.
https://www.reddit.com/r/dailyprogrammer/comments/cn6gz5/20190807_challenge_380_intermediate_smooshed/
*/

#define MULTITHREADING true
#define LONGEST_CODE 4

#include <chrono>
#include <fstream>
#if MULTITHREADING
#include <future>
#endif
#include <iostream>
#include <string>
#include <unordered_map>
#include <vector>

#include "node.hpp"
#include "error_node.hpp"

std::string morseCode =
".- -... -.-. -.. . ..-. --. .... .. .--- -.- .-.. -- -. --- .--. --.- .-. "
"... - ..- ...- .-- -..- -.-- --..";
std::unordered_map<std::string, char> codeToLetters;

void populateMap()
{
	char letter = 'a';
	int lastSpaceIndex = 0;
	for (int i = 0; i < 26; ++i)
	{
		int spaceIndex = morseCode.find(' ', lastSpaceIndex);
		std::string code =
			morseCode.substr(lastSpaceIndex, spaceIndex - lastSpaceIndex);
		lastSpaceIndex = spaceIndex + 1;
		codeToLetters.insert(std::pair<std::string, char>(code, letter++));
	}
}

std::unique_ptr<Node> consumeNextCode(std::string& morse, int currentIndex, int usedLetters)
{
	int remainingCharacters = morse.length() - currentIndex;
	int longestCodeToCheck = remainingCharacters < LONGEST_CODE ? remainingCharacters : LONGEST_CODE;
	for (int i = 0; i < longestCodeToCheck; ++i)
	{
		size_t codeLength = longestCodeToCheck - i;

		// Get the next sequence that matches the code length
		std::string currentSequence = morse.substr(currentIndex, codeLength);

		// Check to see if the sequence exists in the map
		if (codeToLetters.count(currentSequence) == 0) {
			continue;
		}

		// Make sure we haven't used this letter before
		char currentLetter = codeToLetters[currentSequence];

		int bitMask = 1 << (25 - (currentLetter - 'a'));
		if ((usedLetters & bitMask) != 0)
		{
			continue;
		}

		// If we did find a match, we can create a new node with the letter.
		std::unique_ptr<Node> newNode = std::make_unique<Node>(currentLetter);

		// Add the letter to the list of used letters
		usedLetters ^= bitMask;

		// We need to determine if this node is termianl.
		// If it is, we just want to return it.
		if (currentIndex + codeLength == morse.length())
		{
			newNode->setTerminal(true);
			return std::move(newNode);
		}

		// Try to grab the next letter in the sequence
		std::unique_ptr<Node> nextNode = consumeNextCode(morse, currentIndex + codeLength, usedLetters);

		// We need to check if we were given an error node. If we were, we need to look for more matches.
		if (nextNode->getLetter() == 0)
		{
			usedLetters ^= bitMask;
			continue;
		}

		// If we didn't get an error node, this means that we can attach the newNode to the current node and return the current node.
		else
		{
			newNode->setNext(std::move(nextNode));
			return std::move(newNode);
		}
	}

	// We didn't find any matches, return an error node
	return std::make_unique<ErrorNode>();
}

int main() {
	{
		populateMap();
		std::cout << "Please enter a string of smooshed morse code: ";

		std::string morse;
		std::getline(std::cin, morse);

		int usedLetters = 0;

		std::unique_ptr<Node> permutation = consumeNextCode(morse, 0, usedLetters);

		Node* currentNode = permutation.get();
		while (currentNode != nullptr)
		{
			std::cout << currentNode->getLetter();
			currentNode = &currentNode->getNext();
		}
		std::cout << std::endl;
	}

	// Bonus
	std::cout << "Bonus" << std::endl;
#if MULTITHREADING
	std::vector<std::future<std::unique_ptr<Node>>> futures;
#endif
	std::vector<std::unique_ptr<Node>> outputs;

	std::ifstream inFileStream("smorse2_bonus.in");
	std::string line;

	auto start = std::chrono::high_resolution_clock::now();

	while (std::getline(inFileStream, line))
	{
#if MULTITHREADING
		futures.push_back(std::async(consumeNextCode, line, 0, 0));
#else
		outputs.push_back(consumeNextCode(line, 0, 0));
#endif
	}

#if MULTITHREADING
	// Get the futures
	for (int i = 0; i < futures.size(); ++i)
	{
		outputs.push_back(futures.at(i).get());
	}
#endif

	auto stop = std::chrono::high_resolution_clock::now();

	auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start);

	// Print out the results lol
	for (int i = 0; i < outputs.size(); ++i)
	{
		Node* currentNode = outputs.at(i).get();
		while (currentNode != nullptr)
		{
			std::cout << currentNode->getLetter();
			currentNode = &currentNode->getNext();
		}
		std::cout << std::endl;
	}

	std::cout << "Time taken to find all inputs: " << duration.count() << " milliseconds" << std::endl;

	std::cin.get();

	return 0;
}
