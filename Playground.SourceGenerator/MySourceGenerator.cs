﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Playground.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class MySourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static context =>
        {
            context.AddSource("MyGenerationAttribute.g.cs",
                $$"""
                       // <auto-generated />
                       using System;
                       
                       namespace Playground
                       {
                           [global::System.Diagnostics.Conditional("__SourceGenerator__DesignTimeOnly__")]
                           [global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false)]
                           internal class MyGenerationAttribute : Attribute
                           {
                           }
                       }
                       """);
        });

        var attrValues = context.SyntaxProvider.ForAttributeWithMetadataName(
            "Playground.MyGenerationAttribute",
            predicate: static (node, cancellationToken) => node is ClassDeclarationSyntax,
            transform: static (context, cancellationToken) => (ClassDeclarationSyntax)context.TargetNode);

        context.RegisterSourceOutput(attrValues, static (context, values) =>
        {
            context.AddSource(Path.GetFileNameWithoutExtension(values.Identifier.Text) + ".g.cs",
                $$"""
                        partial class Generated
                        {
                            // {{values.Identifier.Text}}
                        }
                        """);
        });
    }
}