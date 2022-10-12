using BenchmarkDotNet.Running;
using ExpressionInterpreterTest;
using ExpressionInterpreterTest.Interpreting;
using ExpressionInterpreterTest.Parsing;
/*
Console.WriteLine("Hello World!");
var lib = new ContextLibrary();
while (true) {
    string inp = Console.ReadLine();
    Console.WriteLine("= " + Parser.Eval(inp, lib));
}*/

BenchmarkRunner.Run<Benchmark>();