#ifndef NODE_HPP
#define NODE_HPP

#include <memory>

class Node {
private:
    char letter;
    bool terminal;
    std::unique_ptr<Node> next;

public:
    Node(char letter);

    char getLetter() const;

    bool isTerminal() const;

    Node& getNext() const;

    void setTerminal(bool terminal);

    void setNext(std::unique_ptr<Node> newNext);
};

#endif