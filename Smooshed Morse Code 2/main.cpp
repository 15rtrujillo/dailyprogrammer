#include <chrono>
#include <fstream>
#include <future>
#include <iostream>
#include <map>
#include <string>
#include <vector>

#include "node.hpp"
#include "error_node.hpp"

#define LONGEST_CODE 4

std::string morseCode =
".- -... -.-. -.. . ..-. --. .... .. .--- -.- .-.. -- -. --- .--. --.- .-. "
"... - ..- ...- .-- -..- -.-- --..";
std::map<char, std::string> lettersToCode;

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
		lettersToCode.insert(std::pair<char, std::string>(letter++, code));
	}
	/*
	for (auto iter = lettersToCode.begin(); iter != lettersToCode.end(); ++iter)
	{
		std::cout << iter->first << ": " << iter->second << std::endl;
	}
	*/
}

// std::unique_ptr<Node> consumeNextCode(std::string& morse, int currentIndex, std::vector<char>& letters)
// void consumeNextCode(std::string& morse, int currentIndex, std::vector<char>& letters)
std::unique_ptr<Node> consumeNextCode(std::string& morse, int currentIndex, int usedLetters)
{
	int remainingCharacters = morse.length() - currentIndex;
	int longestCodeToCheck = remainingCharacters < LONGEST_CODE ? remainingCharacters : LONGEST_CODE;
	for (int i = 0; i < longestCodeToCheck; ++i)
	{
		size_t codeLength = longestCodeToCheck - i;

		// Get the next sequence that matches the code length
		std::string currentSequence = morse.substr(currentIndex, codeLength);

		for (auto iter = lettersToCode.begin(); iter != lettersToCode.end(); ++iter)
		{
			// We want to match the longest codes first
			if (iter->second.length() != codeLength)
			{
				continue;
			}

			// Attempt to match the current sequence with a letter
			if (currentSequence != iter->second)
			{
				continue;
			}

			// Make sure we haven't used this letter before
			int bitMask = 1 << (25 - (iter->first - 'a'));
			if ((usedLetters & bitMask) != 0)
			{
				continue;
			}
			/*
			bool letterUsed = false;
			for (auto charIter = letters.begin(); charIter != letters.end(); ++charIter)
			{
				if (iter->first == *charIter)
				{
					letterUsed = true;
					break;
				}
			}

			if (letterUsed)
			{
				continue;
			}
			*/

			// If we did find a match, we can create a new node with the letter.
			std::unique_ptr<Node> newNode = std::make_unique<Node>(iter->first);

			// Add the letter to the list of used letters
			usedLetters ^= bitMask;
			// letters.push_back(iter->first);

			// We need to determine if this node is termianl.
			// If it is, we just want to return it.
			if (currentIndex + codeLength == morse.length())
			{
				newNode->setTerminal(true);
				return std::move(newNode);
				//return;
			}

			std::unique_ptr<Node> nextNode = consumeNextCode(morse, currentIndex + codeLength, usedLetters);
			// std::unique_ptr<Node> nextNode = consumeNextCode(morse, currentIndex + codeLength, letters);
			// consumeNextCode(morse, currentIndex + codeLength, letters);

			// We need to check if we were given an error node. If we were, we need to look for more matches.
			// if (letters.back() == 0)
			if (nextNode->getLetter() == 0)
			{
				usedLetters ^= bitMask;
				// Pop the error
				// letters.pop_back();
				// Pop the letter that was tried
				// letters.pop_back();
				continue;
			}

			// If we didn't get an error node, this means that we can attach the newNode to the current node and return the current node.
			else
			{
				newNode->setNext(std::move(nextNode));
				return std::move(newNode);
				// return;
			}
		}
	}

	// We didn't find any matches, return an error node
	return std::make_unique<ErrorNode>();
	// letters.push_back(0);
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
	/*
	std::vector<char> permutation;

	consumeNextCode(morse, 0, permutation);

	for (char letter : permutation)
	{
		std::cout << letter;
	}
	std::cout << std::endl;
	*/
	// Bonus
	std::cout << "Bonus" << std::endl;
	// std::vector<std::vector<char>> outputs;
	std::vector<std::unique_ptr<Node>> outputs;
	std::vector<std::future<std::unique_ptr<Node>>> futures;


	std::ifstream inFileStream("smorse2_bonus.in");
	std::string line;

	auto start = std::chrono::high_resolution_clock::now();
	while (std::getline(inFileStream, line))
	{
		int usedLetters = 0;
		futures.push_back(std::async(consumeNextCode, line, 0, 0));
		// outputs.push_back(consumeNextCode(line, 0, usedLetters));
		// outputs.push_back(letters);
	}

	// Get the futures
	for (int i = 0; i < futures.size(); ++i)
	{
		outputs.push_back(futures.at(i).get());
	}

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

	return 0;
}