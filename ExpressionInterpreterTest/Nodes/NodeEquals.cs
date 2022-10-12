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

        public override double Eval(IContext ctx) {
            throw new NotImplementedException();
        }

        public override string ToString() {
            return lhs.ToString() + " = " + rhs.ToString();
        }
    }
}
