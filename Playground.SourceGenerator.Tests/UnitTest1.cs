using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Playground.SourceGenerator.Tests;

public class UnitTest1(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Test1()
    {
        // Set up the source generator.
        var sourceGenerator = new MySourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            generators: new[] { sourceGenerator.AsSourceGenerator() },
            driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true)
        );

        // Create a compilation and add a source code.
        var compilation = CompilationHelper.CreateCompilation();
        compilation = compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(
            path: "Program.cs",
            text: $$"""
                    using System;
                    Console.WriteLine("Hello");
                    """));
        compilation = compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(
            path: "Class1.cs",
            text: $$"""
                    using System;
                    
                    namespace ConsoleApp1;

                    [Playground.MyGeneration]
                    class A
                    {
                    }
                    """));
        // Run the source generator.
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var updatedCompilation, out var sourceGeneratorDiagnostics);

        // Diagnostics: Source Generator / Analyzer
        outputHelper.WriteDiagnostics("Diagnostics: Source Generator / Analyzer", sourceGeneratorDiagnostics);
        Assert.Empty(sourceGeneratorDiagnostics.Where(x => x.Severity >= DiagnosticSeverity.Warning));

        // Diagnostics: Compiler
        var diagnostics = updatedCompilation.GetDiagnostics();
        outputHelper.WriteDiagnostics("Diagnostics: Compiler", diagnostics);
        Assert.Empty(diagnostics.Where(x => x.Severity >= DiagnosticSeverity.Warning));

        // Validate the results generated by the source generator.
        var sourceGeneratorResult = driver.GetRunResult();
        outputHelper.WriteSyntaxTrees("Generated", sourceGeneratorResult.GeneratedTrees);
    }
}