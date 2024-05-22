using Microsoft.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace Playground.SourceGenerator.Tests;

public static class OutputHelperExtensions
{
    public static void WriteSyntaxTrees(this ITestOutputHelper outputHelper, string label, IEnumerable<SyntaxTree> syntaxTrees)
    {
        outputHelper.WriteLine($"Generated:{Environment.NewLine}========================================");
        outputHelper.WriteLine(string.Join("========================================" + Environment.NewLine,
            syntaxTrees.Select(x =>
                x.FilePath + Environment.NewLine +
                "----------------------------------------" + Environment.NewLine +
                x.ToString() + Environment.NewLine +
                "----------------------------------------")));
        outputHelper.WriteLine("");
    }

    public static void WriteDiagnostics(this ITestOutputHelper outputHelper, string label, IEnumerable<Diagnostic> diagnostics)
    {
        outputHelper.WriteLine($"{label}:{Environment.NewLine}========================================");
        outputHelper.WriteLine(string.Join(Environment.NewLine, diagnostics.Select(x => x.ToString())));
        outputHelper.WriteLine("");
    }
}