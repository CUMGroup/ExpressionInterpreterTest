using ExpressionInterpreterTest.Interpreting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Nodes {
    internal abstract class Node {
        public abstract double Eval(IContext ctx);

        public abstract override String ToString();
    }
}
