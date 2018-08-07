using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nuke.Common;
using static Nuke.Common.IO.PathConstruction;

partial class Build
{
    [Parameter]
    int? Sprint;

    AbsolutePath BuildVariablesFilePath => EnvironmentInfo.BuildProjectDirectory / $"{nameof(BuildVariables)}.cs";

    Target UpdateVersion => _ => _
        .Requires(() => Sprint)
        .Executes(() =>
        {
            UpdateSprint();
        });

    void UpdateSprint()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(BuildVariablesFilePath));

        syntaxTree = syntaxTree.WithRootAndOptions(SprintRewriter.RewriteSprint(syntaxTree.GetRoot(), Sprint.Value), new CSharpParseOptions());

        File.WriteAllText(BuildVariablesFilePath, syntaxTree.ToString(), Encoding.UTF8);
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
            if (node.Identifier.Text != nameof(BuildVariables.Sprint))
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
