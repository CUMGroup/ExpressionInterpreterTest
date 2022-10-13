using AngouriMath;
using BenchmarkDotNet.Running;
using MathLibaryTest;

//BenchmarkRunner.Run<Benchmark>();
Entity expr = "x^5-27*x^4+7.5*x^3-12*x^2+x-2.72";
Console.WriteLine(expr.Substitute("x", 1_000_000_000_000_000_000).Simplify());