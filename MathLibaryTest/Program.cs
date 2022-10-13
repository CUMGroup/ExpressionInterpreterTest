using AngouriMath;
using BenchmarkDotNet.Running;
using MathLibaryTest;

//BenchmarkRunner.Run<Benchmark>();
Entity expr = "(x+13)^2*(x-2.72)*(x+1.12)*x^2*(x-14)";
Entity expr1 = "(x+13)^2*(x-2.72)*(x+1.12)*x^2*(x-14)";
Console.WriteLine(new Entity.Equalsf(expr, expr1).Simplify());