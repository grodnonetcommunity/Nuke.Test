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
using static Nuke.Common.IO.SerializationTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static GitTasks;

partial class Build
{
    [Parameter] int? Sprint;
    [Parameter] string NewVersion;

    AbsolutePath BuildVariablesFilePath => EnvironmentInfo.BuildProjectDirectory / $"{nameof(BuildVariables)}.cs";

    AbsolutePath AssemblyInfoProps => SolutionDirectory / "AssemblyInfo.props";

    AbsolutePath WebAppDirectory => SolutionDirectory / "webapp";

    Target UpdateVersion => _ => _
        .Requires(() => Sprint)
        .Requires(() => NewVersion)
        .Executes(() =>
        {
            if (!GitHasCleanWorkingCopy(RootDirectory))
            {
                Logger.Error("There are uncommitted changes");
                return;
            }

            UpdateSprint();
            UpdateAssemblyInfo();

            var jsonFiles = new[]
            {
                WebAppDirectory / "package.json",
                WebAppDirectory / "package-lock.json",
            };

            foreach (var jsonFile in jsonFiles)
            {
                UpdateJson(jsonFile);
            }

            var changes = new[]
                {
                    AssemblyInfoProps,
                    BuildVariablesFilePath,
                }
                .Concat(jsonFiles)
                .Select(p => p.ToRelative(RootDirectory).ToString())
                .ToList();

            GitAdd(RootDirectory, changes);
            GitCommit(RootDirectory, $"build(version): update version to {NewVersion} and sprint {Sprint}");
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

    void UpdateJson(string path)
    {
        var json = JsonDeserializeFromFile<dynamic>(path);
        json.version = BuildVariables.Version;
        JsonSerializeToFile(json, path);
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
                .WithTriviaFrom(node.ExpressionBody.Expression);
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
