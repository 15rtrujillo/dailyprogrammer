#include <chrono>
#include <fstream>
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

void populateMap() {
  char letter = 'a';
  int lastSpaceIndex = 0;
  for (int i = 0; i < 26; ++i) {
    int spaceIndex = morseCode.find(' ', lastSpaceIndex);
    std::string code =
        morseCode.substr(lastSpaceIndex, spaceIndex - lastSpaceIndex);
    lastSpaceIndex = spaceIndex + 1;
    lettersToCode.insert(std::pair<char, std::string>(letter++, code));
  }
  /*
  for (auto iter = lettersToCode.begin(); iter != lettersToCode.end(); ++iter) {
      std::cout << iter->first << ": " << iter->second << std::endl;
  }
  */
}

std::unique_ptr<Node> consumeNextCode(std::string& morse, int currentIndex, std::vector<char>& usedLetters) {
    for (int i = 0; i < LONGEST_CODE; ++i) {
        size_t codeLength = LONGEST_CODE - i;

        // If the code is too long, we don't even need to check it.
        if (codeLength > morse.length() - currentIndex) {
            continue;
        }
        
        // Get the next sequence that matches the code length
        std::string currentSequence = morse.substr(currentIndex, codeLength);
        
        for (auto iter = lettersToCode.begin(); iter != lettersToCode.end(); ++iter) {
            // We want to match the longest codes first
            if (iter->second.length() < codeLength) {
                continue;
            }

            // Attempt to match the current sequence with a letter
            if (currentSequence != iter->second) {
                continue;
            }
            
            // Make sure we haven't used this letter before
            bool letterUsed = false;
            for (auto charIter = usedLetters.begin(); charIter != usedLetters.end(); ++charIter) {
                if (iter->first == *charIter) {
                    letterUsed = true;
                    break;
                }
            }

            if (letterUsed) {
                continue;
            }

            // If we did find a match, we can create a new node with the letter.
            std::unique_ptr<Node> newNode = std::make_unique<Node>(iter->first);

            // Add the letter to the list of used letters
            usedLetters.push_back(iter->first);

            // We need to determine if this node is termianl.
            // If it is, we just want to return it.
            if (currentIndex + codeLength >= morse.length()) {
                newNode->setTerminal(true);
                return std::move(newNode);
            }
            
            std::unique_ptr<Node> nextNode = consumeNextCode(morse, currentIndex + codeLength, usedLetters);

            // We need to check if we were given an error node. If we were, we need to look for more matches.
            if (nextNode->getLetter() == 0) {
                usedLetters.pop_back();
                continue;
            }

            // If we didn't get an error node, this means that we can attach the newNode to the current node and return the current node.
            else {
                newNode->setNext(std::move(nextNode));
                return std::move(newNode);
            }
        }
    }

    // We didn't find any matches, return an error node
    return std::make_unique<ErrorNode>();
}

int main() {
    populateMap();
    std::cout << "Please enter a string of smooshed morse code: ";

    std::string morse;
    std::getline(std::cin, morse);

    std::vector<char> usedLetters;

    std::unique_ptr<Node> permutation = consumeNextCode(morse, 0, usedLetters);

    Node* currentNode = permutation.get();
    while (currentNode != nullptr) {
        std::cout << currentNode->getLetter();
        currentNode = &currentNode->getNext();
    }
    std::cout << std::endl;

    // Bonus
    std::cout << "Bonus" << std::endl;
    std::vector<std::unique_ptr<Node>> permutations;
    
    std::ifstream inFileStream("smorse2_bonus.in");
    std::string line;

    auto start = std::chrono::high_resolution_clock::now();
    while (std::getline(inFileStream, line)) {
        usedLetters.clear();
        permutations.push_back(consumeNextCode(line, 0, usedLetters));
    }
    auto stop = std::chrono::high_resolution_clock::now();

    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start);

    // Print out the results lol
    for (int i = 0; i < permutations.size(); ++i) {
        Node* nodePtr = permutations.at(i).get();
        while (nodePtr != nullptr) {
            std::cout << nodePtr->getLetter();
            nodePtr = &nodePtr->getNext();
        }
        std::cout << std::endl;
    }

    std::cout << "Time taken to find all inputs: " << duration.count() << " milliseconds" << std::endl;
}