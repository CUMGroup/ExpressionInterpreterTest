using AngouriMath;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibaryTest {
    public class Benchmark {

        [Benchmark]
        public Entity CheckEqualityHard() {

            Entity expr = "(x+13)^2*(x-2.72)*(x+1.12)*x^2*(x-14)";
            //expr = expr.Expand();
            Entity expr1 = "x^7 + 10.4*x^6 - 217.246*x^5 - 2090.56*x^4 + 4379.65*x^3 + 7207.78*x^2";
            var res = new Entity.Equalsf(expr, expr1).Simplify();
            return res;
        }

        [Benchmark]
        public Entity CheckEqualityEasy() {
            Entity expr = "x^5-27*x^4+7.5*x^3-12*x^2+x-2.72";
            Entity expr1 = "x^5-27*x^4+7.5*x^3-12*x^2+x-2.72";
            var res = new Entity.Equalsf(expr, expr1).Simplify();
            return res;
        }

        [Benchmark]
        public Entity Calculate() {
            Entity expr = "x^5-27*x^4+7.5*x^3-12*x^2+x-2.72";
            return expr.Substitute("x", Random.Shared.NextDouble() * 1000 - 500);
        }

    }
}
