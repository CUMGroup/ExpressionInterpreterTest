using ExpressionInterpreterTest.Interpreting;
using ExpressionInterpreterTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Nodes {
    internal class NodeFunction : Node {

        public IFunctionProvider FuncProvider { get; private set; }
        private Node args;

        public NodeFunction(Node args, IFunctionProvider func) {
            this.args = args;
            this.FuncProvider = func;
        }

        public override Node Eval(IContext ctx) {
            args = args.Eval(ctx);
            if (args is NodeNum)
                return FuncProvider.Eval((NodeNum)args);
            return this;
        }

        public override string ToString() {
            return FuncProvider.ToString(args);
        }

        // override object.Equals
        public override bool Equals(object obj) {
            return obj is NodeFunction funct && funct.FuncProvider.ToString().Equals(FuncProvider.ToString()) && funct.args.Equals(args);
        }

        // override object.GetHashCode
        public override int GetHashCode() { 
            return base.GetHashCode();
        }
    }
}
