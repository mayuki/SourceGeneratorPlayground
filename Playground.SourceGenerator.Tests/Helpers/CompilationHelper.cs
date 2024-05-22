using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

// ReSharper disable once CheckNamespace
namespace Playground.SourceGenerator.Tests;

internal class CompilationHelper
{
    public static Compilation CreateCompilation()
    {
        var assemblyName = Guid.NewGuid().ToString();
        var compilationOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication);

        var references = ReferenceAssemblies.Net.Net80.ResolveAsync(LanguageNames.CSharp, default).GetAwaiter().GetResult();
        return CSharpCompilation.Create(assemblyName, Array.Empty<SyntaxTree>(), references, compilationOptions);
    }
}