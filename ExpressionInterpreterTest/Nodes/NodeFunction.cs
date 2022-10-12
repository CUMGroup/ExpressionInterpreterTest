using ExpressionInterpreterTest.Interpreting;
using ExpressionInterpreterTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Nodes {
    internal class NodeFunction : Node {

        private Node[] args;
        private readonly String functionname;

        public NodeFunction(Node[] args, String functionname) {
            this.args = args;
            this.functionname = functionname;
        }

        public override double Eval(IContext ctx) {
            double[] argVals = new double[args.Length];
            for (int i = 0; i < args.Length; ++i) {
                argVals[i] = args[i].Eval(ctx);
            }
            return ctx.CallFunction(functionname, argVals);
        }

        public override string ToString() {
            return "";//FuncProvider.ToString(args);
        }
    }
}
