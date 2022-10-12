using ExpressionInterpreterTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest.Interpreting {
    internal interface IContext {

        double CallFunction(String funcname, double[] args);
        double GetVariable(String varname);
        double SetVariable(String varname, double val);

    }
}
