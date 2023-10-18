#include "node.hpp"

Node::Node(char letter) : letter(letter) {
    terminal = false;
}

char Node::getLetter() const {
    return letter;
}

bool Node::isTerminal() const {
    return terminal;
}

Node& Node::getNext() const {
    return *next;
}

void Node::setTerminal(bool terminal) {
    this->terminal = terminal;
}

void Node::setNext(std::unique_ptr<Node> newNext) {
    next = std::move(newNext);
}