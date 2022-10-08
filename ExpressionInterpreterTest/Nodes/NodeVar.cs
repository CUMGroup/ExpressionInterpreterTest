using ExpressionInterpreterTest.Interpreting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ExpressionInterpreterTest.Nodes {
    internal class NodeVar : Node {

        private string varname;

        public NodeVar(string varname) {
            this.varname = varname;
        }

        public override Node Eval(IContext ctx) {
            return this;
        }

        public override string ToString() {
            return varname;
        }
    }
}
