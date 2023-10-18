#ifndef ERROR_NODE_HPP
#define ERROR_NODE_HPP

#include "node.hpp"

class ErrorNode : public Node {
public:
    ErrorNode(): Node(0) {};
};

#endif