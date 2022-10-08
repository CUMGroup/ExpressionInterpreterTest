using ExpressionInterpreterTest.Interpreting;
using ExpressionInterpreterTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Nodes {
    internal class NodeUnary : Node {

        private Node rhs;
        private Func<double, double> op;
        private TokenType OperationType;

        public NodeUnary(Node rhs, Func<double, double> op, TokenType OperationType) {
            this.rhs = rhs;
            this.op = op;
            this.OperationType = OperationType;
        }

        public override Node Eval(IContext ctx) {
            rhs = rhs.Eval(ctx);
            if (rhs is NodeNum)
                return new NodeNum(op(((NodeNum)rhs).GetValue()));
            return this;
        }

        public override string ToString() {
            var opString = OperationType switch {
                TokenType.ADD => "+",
                TokenType.SUB => "-",
                _ => "",
            };
            return opString + rhs.ToString();
        }
    }
}
