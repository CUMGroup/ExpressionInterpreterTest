using BenchmarkDotNet.Attributes;
using CommandLine;
using ExpressionInterpreterTest.Interpreting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionInterpreterTest {
    public class Benchmark {

        [Params(10, 100, 1000)]
        public double MaxIter { get; set; }

        [Benchmark]
        public void CheckEquality() {
            IContext ctx = new ContextLibrary();
            for (int i = 0; i < MaxIter; ++i) {
                double x = Random.Shared.NextDouble() * 1000 - 500;
                ctx.SetVariable("x", x);
                ExpressionInterpreterTest.Parsing.Parser.Eval("x^5-27*x^4+7.5*x^3-12*x^2+x-2.72", ctx);
            }
        }

    }
}
