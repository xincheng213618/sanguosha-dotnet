using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class FrameInvocationAnalysis
{
    public static void Run(string root, string output, SyntaxTree[] trees,
        CSharpCompilation compilation, JsonSerializerOptions json)
    {
        var matches = new List<object>();
        var rejected = new Dictionary<string, int>(StringComparer.Ordinal);
        var seen = 0;
        foreach (var tree in trees)
        {
            var calls = tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>()
                .Where(call => CallName(call) == "HasRuntimeSkillInstance").ToArray();
            if (calls.Length == 0) continue;
            var model = compilation.GetSemanticModel(tree);
            foreach (var call in calls)
            {
                seen++;
                if (call.ArgumentList.Arguments.Count != 3 ||
                    call.ArgumentList.Arguments[1].Expression is not MemberAccessExpressionSyntax skill ||
                    call.ArgumentList.Arguments[2].Expression is not MemberAccessExpressionSyntax instance ||
                    skill.Name.Identifier.ValueText != "SkillId" || instance.Name.Identifier.ValueText != "SkillInstanceId" ||
                    skill.Expression is not IdentifierNameSyntax frame ||
                    !SyntaxFactory.AreEquivalent(frame, instance.Expression))
                { Reject("not-the-same-simple-frame-receiver"); continue; }
                if (model.GetTypeInfo(frame).Type?.ToDisplayString() != "CardGame.Core.ProgramSkillFrame")
                { Reject("receiver-is-not-ProgramSkillFrame"); continue; }
                var symbol = model.GetSymbolInfo(frame).Symbol;
                if (symbol is not (ILocalSymbol or IParameterSymbol) ||
                    !SymbolEqualityComparer.Default.Equals(symbol, model.GetSymbolInfo(instance.Expression).Symbol))
                { Reject("receiver-symbol-not-confirmed"); continue; }
                if (model.GetSymbolInfo(call).Symbol is not IMethodSymbol { Name: "HasRuntimeSkillInstance" } target)
                { Reject("callee-unresolved"); continue; }
                var position = tree.GetLineSpan(call.Span).StartLinePosition;
                var ownerExpression = call.ArgumentList.Arguments[0].Expression;
                var directOwner = OwnerIndex(ownerExpression, frame, model, allowSeatAlias: false);
                var aliasEvidence = new List<string>();
                var aliasedOwner = !directOwner && Initializer(ownerExpression, model) is { } ownerInitializer &&
                    OwnerIndex(ownerInitializer, frame, model, allowSeatAlias: true);
                if (aliasedOwner) aliasEvidence.Add(ownerExpression + " = " + Initializer(ownerExpression, model));
                matches.Add(new
                {
                    File = Path.GetRelativePath(root, tree.FilePath).Replace('\\', '/'),
                    AbsoluteFile = tree.FilePath,
                    Start = call.SpanStart,
                    Length = call.Span.Length,
                    Line = position.Line + 1,
                    Column = position.Character + 1,
                    InvocationText = call.ToString(),
                    FrameExpression = frame.ToString(),
                    ReceiverText = call.Expression is MemberAccessExpressionSyntax member ? member.Expression.ToString() : "",
                    FrameType = "CardGame.Core.ProgramSkillFrame",
                    Callee = target.ToDisplayString(),
                    OwnerExpression = ownerExpression.ToString(),
                    OwnerIsFrameSeatIndex = directOwner,
                    OwnerAliasFrameSeatIndex = aliasedOwner,
                    OwnerAliasEvidence = aliasEvidence
                });
            }
        }
        var result = new
        {
            Scope = "CardGame.Core + Characters/**/Runtime source context; no source modifications",
            Selection = "Exactly 3 arguments; SkillId and SkillInstanceId on the same local/parameter identifier; both semantic symbols equal; receiver type exactly CardGame.Core.ProgramSkillFrame; bound callee confirmed",
            Seen = seen,
            Count = matches.Count,
            Rejected = rejected,
            Calls = matches
        };
        var path = Path.Combine(output, "frame-invocation-consumers.json");
        File.WriteAllText(path, JsonSerializer.Serialize(result, json), new UTF8Encoding(false));
        Console.WriteLine($"Frame invocation report: {matches.Count}/{seen} calls; {path}");
        void Reject(string reason) => rejected[reason] = rejected.GetValueOrDefault(reason) + 1;
    }

    private static bool OwnerIndex(ExpressionSyntax expression, IdentifierNameSyntax frame, SemanticModel model, bool allowSeatAlias)
    {
        if (expression is not ElementAccessExpressionSyntax index || index.ArgumentList.Arguments is not [var seat]) return false;
        var key = seat.Expression;
        if (allowSeatAlias && Initializer(key, model) is { } seatInitializer) key = seatInitializer;
        return key is MemberAccessExpressionSyntax member && member.Name.Identifier.ValueText == "OwnerSeat" &&
            SyntaxFactory.AreEquivalent(frame, member.Expression) &&
            SymbolEqualityComparer.Default.Equals(model.GetSymbolInfo(frame).Symbol, model.GetSymbolInfo(member.Expression).Symbol);
    }

    private static ExpressionSyntax? Initializer(ExpressionSyntax expression, SemanticModel model)
    {
        if (expression is not IdentifierNameSyntax || model.GetSymbolInfo(expression).Symbol is not ILocalSymbol symbol ||
            symbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is not VariableDeclaratorSyntax { Initializer.Value: { } value } declaration)
            return null;
        // Initializer aliases are useful only while their local has no writes or ref/out escape.
        var scope = declaration.Ancestors().FirstOrDefault(node => node is BaseMethodDeclarationSyntax or LocalFunctionStatementSyntax);
        if (scope is null || scope.DescendantNodes().Any(node => node switch
            {
                AssignmentExpressionSyntax assignment => SymbolEqualityComparer.Default.Equals(model.GetSymbolInfo(assignment.Left).Symbol, symbol),
                PrefixUnaryExpressionSyntax prefix when prefix.IsKind(SyntaxKind.PreIncrementExpression) || prefix.IsKind(SyntaxKind.PreDecrementExpression) => SymbolEqualityComparer.Default.Equals(model.GetSymbolInfo(prefix.Operand).Symbol, symbol),
                PostfixUnaryExpressionSyntax postfix when postfix.IsKind(SyntaxKind.PostIncrementExpression) || postfix.IsKind(SyntaxKind.PostDecrementExpression) => SymbolEqualityComparer.Default.Equals(model.GetSymbolInfo(postfix.Operand).Symbol, symbol),
                ArgumentSyntax argument when argument.RefKindKeyword.Kind() is SyntaxKind.RefKeyword or SyntaxKind.OutKeyword => SymbolEqualityComparer.Default.Equals(model.GetSymbolInfo(argument.Expression).Symbol, symbol),
                _ => false
            })) return null;
        return value;
    }

    private static string? CallName(InvocationExpressionSyntax call) => call.Expression switch
    {
        MemberAccessExpressionSyntax member => member.Name.Identifier.ValueText,
        IdentifierNameSyntax name => name.Identifier.ValueText,
        _ => null
    };
}
