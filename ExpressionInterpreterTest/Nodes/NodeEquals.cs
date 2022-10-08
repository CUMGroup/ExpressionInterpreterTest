using ExpressionInterpreterTest.Interpreting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Nodes {
    internal class NodeEquals : Node {

        private Node lhs;
        private Node rhs;

        public NodeEquals(Node lhs, Node rhs) {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public override Node Eval(IContext ctx) {
            lhs = lhs.Eval(ctx);
            rhs = rhs.Eval(ctx);
            return this;
        }

        public override string ToString() {
            return lhs.ToString() + " = " + rhs.ToString();
        }
    }
}
