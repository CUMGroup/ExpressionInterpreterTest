using ExpressionInterpreterTest.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Util {
    internal interface IFunctionProvider {

        Node DifferentiateFunction(Node arg);
        double Eval(double arg);
        String ToString(Node arg);
    }
}
