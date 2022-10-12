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

        public override double Eval(IContext ctx) {
            return op(lhs.Eval(ctx), rhs.Eval(ctx));
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
