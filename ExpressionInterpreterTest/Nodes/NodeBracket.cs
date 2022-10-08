using ExpressionInterpreterTest.Interpreting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Nodes {
    internal class NodeBracket : Node {

        private Node rhs;

        public NodeBracket(Node rhs) {
            this.rhs = rhs;
        }

        public override Node Eval(IContext ctx) {
            rhs = rhs.Eval(ctx);
            if (rhs is NodeNum)
                return rhs;
            return this;
        }

        public override string ToString() {
            return "(" + rhs.ToString() + ")";
        }
    }
}
