using ExpressionInterpreterTest.Interpreting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Nodes {
    internal class NodeNum : Node {

        private double num;

        public NodeNum(double num) {
            this.num = num;
        }

        public double GetValue() {
            return num;
        }

        public override double Eval(IContext ctx) {
            return num;
        }

        public override string ToString() {
            return num + "";
        }
    }
}
