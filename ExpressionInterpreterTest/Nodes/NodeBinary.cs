using ExpressionInterpreterTest.Interpreting;
using ExpressionInterpreterTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Nodes {
    internal class NodeBinary : Node {

        private Node lhs, rhs;
        private readonly Func<double, double, double> op;
        public TokenType OperationType { get; private set; }

        public NodeBinary(Node lhs, Node rhs, Func<double, double, double> op, TokenType operationType) {
            this.lhs = lhs;
            this.rhs = rhs;
            this.op = op;
            this.OperationType = operationType;
        }

        public override Node Eval(IContext ctx) {
            lhs = lhs.Eval(ctx);
            rhs = rhs.Eval(ctx);
            if (lhs is NodeNum && rhs is NodeNum)
                return new NodeNum(op(((NodeNum)lhs).GetValue(), ((NodeNum)rhs).GetValue()));

            if (lhs is not NodeNum && rhs is not NodeNum)
                return this;
            
            // left is number, right is expression
            if(lhs is NodeNum && rhs is not NodeNum) {
                var numNode = (NodeNum)lhs;
                var other = rhs;
                // num  op  other
                if (numNode.GetValue() == 0) {
                    switch(OperationType) {
                        case TokenType.ADD:
                            return other;  // 0 + other = other  
                        case TokenType.SUB:
                            return new NodeUnary(rhs, a => -a, TokenType.SUB);  // 0 - other = -other
                        case TokenType.MULT:
                        case TokenType.DIV:
                        case TokenType.POWER:
                            return numNode; // 0 */^ other = 0
                    }
                }
                if(numNode.GetValue() == 1) {
                    switch (OperationType) {
                        case TokenType.MULT:
                            return other;  // 1 * other = other
                        case TokenType.POWER:
                            return numNode; // 1 ^other = 1
                    }
                }
            }

            // right is number, left is expression
            if(lhs is not NodeNum && rhs is NodeNum) {
                var numNode = (NodeNum)rhs;
                var other = lhs;
                // other  op   num
                if (numNode.GetValue() == 0) {
                    switch (OperationType) {
                        case TokenType.ADD:
                        case TokenType.SUB:
                            return other;  // other +- 0 = other
                        case TokenType.MULT:
                            return numNode; // other * 0 = 0
                        case TokenType.POWER:
                            return new NodeNum(1); // other ^0 = 1
                        case TokenType.DIV: // other / 0
                            throw new Exception("Division by 0");
                    }
                }
                if (numNode.GetValue() == 1) {
                    switch (OperationType) {
                        case TokenType.MULT:
                        case TokenType.DIV:
                        case TokenType.POWER:
                            return other;  // other */^ 1 = other
                    }
                }
            }
            return this;
        }

        public override string ToString() {
            var opString = OperationType switch {
                TokenType.ADD => "+",
                TokenType.SUB => "-",
                TokenType.MULT => "*",
                TokenType.DIV => "/",
                TokenType.POWER => "^",
                _ => " ",
            };
            return lhs.ToString() + " " + opString + " " + rhs.ToString();
        }
    }
}
