﻿using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nuke.Common;
using Nuke.Common.IO;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.IO.XmlTasks;

partial class Build
{
    [Parameter] int? Sprint;
    [Parameter] string NewVersion;

    AbsolutePath BuildVariablesFilePath => EnvironmentInfo.BuildProjectDirectory / $"{nameof(BuildVariables)}.cs";

    AbsolutePath AssemblyInfoProps => SolutionDirectory / "AssemblyInfo.props";

    Target UpdateVersion => _ => _
        .Requires(() => Sprint)
        .Requires(() => NewVersion)
        .Executes(() =>
        {
            UpdateSprint();
            UpdateAssemblyInfo();
        });

    void UpdateSprint()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(BuildVariablesFilePath));

        syntaxTree = syntaxTree.WithRootAndOptions(SprintRewriter.RewriteSprint(syntaxTree.GetRoot(), Sprint.Value), new CSharpParseOptions());

        File.WriteAllText(BuildVariablesFilePath, syntaxTree.ToString(), Encoding.UTF8);
    }

    void UpdateAssemblyInfo()
    {
        XmlPoke(AssemblyInfoProps, "/Project/PropertyGroup/Version", BuildVariables.Version);
        XmlPoke(AssemblyInfoProps, "/Project/PropertyGroup/AssemblyVersion", BuildVariables.Version);
        XmlPoke(AssemblyInfoProps, "/Project/PropertyGroup/FileVersion", BuildVariables.FileVersion);
    }

    private class SprintRewriter : CSharpSyntaxRewriter
    {
        readonly int Sprint;

        public SprintRewriter(int sprint, bool visitIntoStructuredTrivia = false)
            : base(visitIntoStructuredTrivia)
        {
            this.Sprint = sprint;
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (node.Identifier.Text != nameof(global::BuildVariables.Sprint))
            {
                return base.VisitPropertyDeclaration(node);
            }

            var sprintLiteralExpressionSyntax = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(Sprint))
                .WithLeadingTrivia(node.ExpressionBody.GetLeadingTrivia())
                .WithTrailingTrivia(node.ExpressionBody.GetTrailingTrivia());
            var sprintExpressionBody = node.ExpressionBody.WithExpression(sprintLiteralExpressionSyntax);

            return node.WithExpressionBody(sprintExpressionBody);
        }

        public static SyntaxNode RewriteSprint(SyntaxNode node, int sprint)
        {
            var visitor = new SprintRewriter(sprint);
            return visitor.Visit(node);
        }
    }
}
