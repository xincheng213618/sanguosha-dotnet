using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

internal static class HistoryQueryAnalysis
{
    static readonly string[] Excluded = ["Contracts", "DependencyAdapters", "StateContracts", "NativeOwners"];
    static readonly HashSet<string> Filters = ["Where", "Any", "Count", "LongCount", "First", "FirstOrDefault", "Single", "SingleOrDefault", "Last", "LastOrDefault"];
    static readonly Regex Time = new(@"\b(?:ActualTurn(?:Number|Ordinal|OwnerSeat)?|TurnNumber|RoundNumber|CurrentRound|ActualPhase|RoundStartedEvent|RoundStartingEvent|TurnStartedEvent|TurnEndedEvent|ActualDiscardRecoveryPhase\w*|SinceLastBoundary)\b", RegexOptions.Compiled);
    static readonly Regex Owner = new(@"\b(?:OwnerSeat|SourceSeat|PlayerSeat|KillerSeat|HelpedSeat|UserSeat)\b", RegexOptions.Compiled);
    static readonly Regex NativeKey = new(@"\b\w*(?:FrameId|ResolutionId|DamageId|ChangeId|ActionId|UseId|WindowId|EntryId|GrantId)\b", RegexOptions.Compiled);
    static readonly Regex OpaqueReceipt = new(@"\.(?:Qualification|Receipt|Origin|Protection)\s*==", RegexOptions.Compiled);
    static string Name(InvocationExpressionSyntax i) => i.Expression switch { MemberAccessExpressionSyntax m => m.Name.Identifier.ValueText, SimpleNameSyntax s => s.Identifier.ValueText, _ => "" };
    static ExpressionSyntax Pipeline(ExpressionSyntax node)
    {
        while (true)
        {
            if (node.Parent is ParenthesizedExpressionSyntax p) { node = p; continue; }
            if (node.Parent is MemberAccessExpressionSyntax m && m.Expression == node)
            { node = m; if (m.Parent is InvocationExpressionSyntax call && call.Expression == m) node = call; continue; }
            return node;
        }
    }
    public static void Run(string root, string output, SyntaxTree[] trees, CSharpCompilation compilation, JsonSerializerOptions json)
    {
        var timer = Stopwatch.StartNew();
        var models = new ConcurrentDictionary<SyntaxTree, SemanticModel>();
        SemanticModel Model(SyntaxTree t) => models.GetOrAdd(t, t => compilation.GetSemanticModel(t));
        var rows = new List<Query>();
        foreach (var tree in trees)
        foreach (var call in tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            if (Name(call) is not ("ProgramEventHistory" or "CompleteProgramEventHistory")) continue;
            var method = call.Ancestors().OfType<MethodDeclarationSyntax>().FirstOrDefault();
            var scope = (SyntaxNode?)method ?? call.Ancestors().FirstOrDefault(n => n is PropertyDeclarationSyntax or ConstructorDeclarationSyntax or AccessorDeclarationSyntax) ?? call;
            var file = Path.GetRelativePath(root, tree.FilePath).Replace('\\', '/');
            var start = scope.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
            var typed = call.Expression.DescendantNodesAndSelf().OfType<GenericNameSyntax>().FirstOrDefault(g => g.Identifier.ValueText == "ProgramEventHistory");
            var q = new Query(call, scope, method, file, start, Name(call), typed?.TypeArgumentList.Arguments.ToString());
            q.Model = Model(tree);
            q.MethodSymbol = method is null ? null : q.Model.GetDeclaredSymbol(method);
            q.Callee = q.Model.GetSymbolInfo(call).Symbol?.ToDisplayString() ?? "unresolved";
            q.Scope = file.StartsWith("src/Characters/", StringComparison.Ordinal) ? "characters" : "core";
            q.ExcludedFromPrimary = q.Scope != "characters" || Excluded.Any(s => file.Contains(s, StringComparison.OrdinalIgnoreCase));
            Expand(q);
            rows.Add(q);
        }
        // Bind only call names belonging to query-bearing methods, rather than all repository calls.
        var methodSymbols = rows.Where(q => q.MethodSymbol is not null).Select(q => q.MethodSymbol!.OriginalDefinition).ToHashSet(SymbolEqualityComparer.Default);
        var names = methodSymbols.Select(m => m!.Name).ToHashSet(StringComparer.Ordinal);
        var callers = new Dictionary<ISymbol, List<CallContext>>(SymbolEqualityComparer.Default);
        foreach (var tree in trees)
        foreach (var call in tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>().Where(c => names.Contains(Name(c))))
        {
            if (Model(tree).GetSymbolInfo(call).Symbol is not IMethodSymbol target || !methodSymbols.Contains(target.OriginalDefinition)) continue;
            if (!callers.TryGetValue(target.OriginalDefinition, out var list)) callers[target.OriginalDefinition] = list = [];
            list.Add(new(Path.GetRelativePath(root, tree.FilePath).Replace('\\', '/'), call.GetLocation().GetLineSpan().StartLinePosition.Line + 1,
                call.Ancestors().OfType<MethodDeclarationSyntax>().FirstOrDefault()?.Identifier.ValueText ?? "property/initializer", call.ToString(), call, Model(tree)));
        }
        foreach (var q in rows)
        {
            var context = q.MethodSymbol is null ? [] : callers.GetValueOrDefault(q.MethodSymbol.OriginalDefinition) ?? [];
            q.Callers = context;
            q.ProofContext = IsProofContext(q.Method) || context.Count > 0 && context.All(c => IsProofContext(c.Call.Ancestors().OfType<MethodDeclarationSyntax>().FirstOrDefault()));
            Classify(q);
            if (q.Category != "c") q.CDisposition = "not-applicable";
        }
        var primary = rows.Where(q => !q.ExcludedFromPrimary).ToArray();
        object Counts(Query[] items) => new
        {
            queries = items.Length, typed = items.Count(q => q.Typed), completeOrNonGeneric = items.Count(q => !q.Typed),
            classes = new[] { "a", "b", "c" }.Select(category => new
            {
                category, queries = items.Count(q => q.Category == category), typed = items.Count(q => q.Category == category && q.Typed),
                completeOrNonGeneric = items.Count(q => q.Category == category && !q.Typed),
                methods = items.Where(q => q.Category == category).Select(q => q.MethodId).Distinct().Count(),
                deduplicatedMethodLines = items.Where(q => q.Category == category).DistinctBy(q => q.MethodId).Sum(q => q.MethodLines)
            }),
            deduplicatedMethods = items.Select(q => q.MethodId).Distinct().Count(),
            deduplicatedMethodLines = items.DistinctBy(q => q.MethodId).Sum(q => q.MethodLines),
            onlyAMethods = items.GroupBy(q => q.MethodId).Count(g => g.All(q => q.Category == "a")),
            onlyAMethodLines = items.GroupBy(q => q.MethodId).Where(g => g.All(q => q.Category == "a")).Sum(g => g.First().MethodLines),
            mixedMethods = items.GroupBy(q => q.MethodId).Count(g => g.Select(q => q.Category).Distinct().Count() > 1),
            aStrictMajority = items.Count(q => q.Category == "a") * 2 > items.Length
        };
        void Write(string name, object value) => File.WriteAllText(Path.Combine(output, name), JsonSerializer.Serialize(value, json), new UTF8Encoding(false));
        Write("queries.json", rows.Select(q => new
        {
            q.File, q.Line, q.MethodId, q.MethodLines, method = q.Method?.Identifier.ValueText ?? "property/initializer", q.Scope, q.ExcludedFromPrimary,
            q.Accessor, q.Typed, q.EventType, q.Callee, q.Category, q.CDisposition, q.ProofContext, q.Reasons, q.OwnFrameEvidence,
            pipelines = q.Expressions.Select(x => x.ToString()).Distinct(), aliases = q.Aliases.ToArray(),
            callers = q.Callers.Take(3).Select(c => new { c.File, c.Line, c.Method, c.Expression }), callerCount = q.Callers.Count
        }));
        Write("methods.json", primary.GroupBy(q => q.MethodId).Select(g => new
        {
            methodId = g.Key, g.First().File, g.First().StartLine, g.First().MethodLines,
            categories = g.Select(q => q.Category).Distinct().Order(), queries = g.Select(q => new { q.Line, q.Accessor, q.EventType, q.Category, q.CDisposition, q.Reasons })
        }));
        Write("samples.json", new[] { "a", "b", "c" }.ToDictionary(c => c, c => primary.Where(q => q.Category == c).DistinctBy(q => q.MethodId).Take(3).Select(q => new
        { q.File, q.Line, q.MethodId, q.EventType, q.CDisposition, q.Reasons, q.OwnFrameEvidence, pipelines = q.Expressions.Take(3).Select(x => x.ToString()) }).ToArray()));
        Write("summary.json", new
        {
            generatedUtc = DateTime.UtcNow, scope = "Primary: Characters/**/Runtime, excluding paths/files containing Contracts, DependencyAdapters, StateContracts, NativeOwners. Core separately reported. One query is one actual accessor InvocationExpression; declarations and dependency type names are excluded.",
            counting = "Method lines are per enclosing method span, deduplicated within each class. Classes may share methods; onlyAMethodLines is the non-overlapping all-a subset. Complete/alias sources are one site, not one per terminal use.",
            basis = new[] { "a requires an event filter/exact-event constructor tied to typed ProgramSkillFrame.Id (or a caller-proven frame-id parameter), and a local existence/uniqueness/receipt lookup. Names Paid/Proof never determine class.", "b requires no own-frame/native-key ambiguity and explicit turn/round, owner-wide or typed unbounded event-history scope.", "c includes mixed own/global alias uses, unresolved scalar identities, helper escapes, unbounded raw Complete history and own-frame aggregates carrying outcomes. Classification is not a deletion decision." },
            primary = Counts(primary), charactersIncludingExcluded = Counts(rows.Where(q => q.Scope == "characters").ToArray()), core = Counts(rows.Where(q => q.Scope == "core").ToArray()),
            ambiguousReasons = primary.Where(q => q.Category == "c").SelectMany(q => q.Reasons).GroupBy(x => x).OrderByDescending(g => g.Count()).Select(g => new { reason = g.Key, queries = g.Count() }),
            cDisposition = primary.Where(q => q.Category == "c").GroupBy(q => q.CDisposition).ToDictionary(g => g.Key, g => new { queries = g.Count(), methods = g.Select(q => q.MethodId).Distinct().Count(), methodLines = g.DistinctBy(q => q.MethodId).Sum(q => q.MethodLines) }),
            textCountReconciliation = new { allRuntimeTypedMatchingPhysicalLines = characterTextLines(trees, root, false), primaryTypedMatchingPhysicalLines = characterTextLines(trees, root, true), note = "The quoted 3604 counts matching physical lines. Several calls can share one line; regex occurrence totals also include one generic method declaration, which is not an InvocationExpression." },
            syntaxAccessorTokens = trees.Sum(t => t.GetRoot().DescendantTokens().Count(token => token.ValueText is "ProgramEventHistory" or "CompleteProgramEventHistory")),
            elapsedSeconds = timer.Elapsed.TotalSeconds
        });
        Console.WriteLine($"History report ready: {primary.Length} primary queries; a={primary.Count(q => q.Category == "a")}, b={primary.Count(q => q.Category == "b")}, c={primary.Count(q => q.Category == "c")}; a strict majority={primary.Count(q => q.Category == "a") * 2 > primary.Length}; {timer.Elapsed.TotalSeconds:F1}s.");
    }
    static bool IsProofContext(MethodDeclarationSyntax? method) => method is not null && method.AttributeLists.SelectMany(a => a.Attributes).Any(a =>
        Regex.IsMatch(a.ToString(), "Program(Continuation|DyingProof|TriggerProof|RecoveryParent)") ||
        a.ToString().Contains("ProgramRuntimeHook", StringComparison.Ordinal) && Regex.IsMatch(a.ToString(), "AfterSkillLoss|AfterOwnerDeath"));
    static int characterTextLines(SyntaxTree[] trees, string root, bool primary) => trees.Where(t =>
        t.FilePath.StartsWith(Path.Combine(root, "src", "Characters") + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) &&
        (!primary || !Excluded.Any(s => t.FilePath.Contains(s, StringComparison.OrdinalIgnoreCase))))
        .Sum(t => t.GetText().Lines.Count(line => Regex.IsMatch(line.ToString(), @"\bProgramEventHistory\s*<[^>]+>\s*\(")));
    static void Expand(Query q)
    {
        var first = Pipeline(q.Node); q.Expressions.Add(first);
        var queue = new Queue<ExpressionSyntax>(); queue.Enqueue(first);
        var seen = new HashSet<ISymbol>(SymbolEqualityComparer.Default);
        while (queue.TryDequeue(out var expression))
        {
            if (expression.Parent is not EqualsValueClauseSyntax { Parent: VariableDeclaratorSyntax variable }) continue;
            var symbol = q.Model.GetDeclaredSymbol(variable);
            if (symbol is null || !seen.Add(symbol)) continue;
            q.Aliases.Add(variable.Identifier.ValueText);
            if (seen.Count > 8) { q.Escaped = true; break; }
            foreach (var id in q.Container.DescendantNodes().OfType<IdentifierNameSyntax>().Where(i => i.Identifier.ValueText == variable.Identifier.ValueText))
            {
                if (!SymbolEqualityComparer.Default.Equals(q.Model.GetSymbolInfo(id).Symbol, symbol)) continue;
                var use = Pipeline(id);
                if (use.Parent is ArgumentSyntax { Parent.Parent: InvocationExpressionSyntax staticUse }) use = staticUse;
                if (use == id) q.Escaped = true;
                q.Expressions.Add(use); queue.Enqueue(use);
            }
        }
    }
    static void Classify(Query q)
    {
        var predicates = q.Expressions.SelectMany(e => e.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>()).Where(i => Filters.Contains(Name(i)))
            .SelectMany(i => i.ArgumentList.Arguments.Select(a => a.Expression).OfType<LambdaExpressionSyntax>()).DistinctBy(l => l.Span).ToArray();
        var self = predicates.Where(p => RequiredOwn(p.Body, q)).ToArray();
        foreach (var p in self) q.OwnFrameEvidence.Add(p.ToString());
        var exact = q.Expressions.SelectMany(e => e.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>()).Where(i => Name(i) == "Contains")
            .SelectMany(i => i.ArgumentList.Arguments).Any(a => OwnEvent(a.Expression, q));
        if (exact) q.OwnFrameEvidence.Add("Contains an exact event constructor anchored to this ProgramSkillFrame.Id");
        var hasOwn = self.Length > 0 || exact;
        var text = string.Join("\n", q.Expressions.Select(e => e.ToString()));
        var terminals = q.Expressions.SelectMany(e => e.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>()).Select(Name).Distinct().ToArray();
        var aggregate = terminals.Any(n => n is "Sum" or "Min" or "Max" or "Average" or "GroupBy");
        if (q.Escaped) { q.Category = "c"; q.Reasons.Add("history alias escapes or has an untracked use"); return; }
        if (hasOwn)
        {
            if (aggregate) { q.Category = "c"; q.CDisposition = "other"; q.Reasons.Add("own-frame history also computes an aggregate outcome"); return; }
            if (q.Aliases.Count > 0 && q.Expressions.Skip(1).Any(e => Time.IsMatch(e.ToString()) && !e.DescendantNodesAndSelf().OfType<LambdaExpressionSyntax>().Any(l => RequiredOwn(l.Body, q))))
            { q.Category = "c"; q.Reasons.Add("alias mixes own-frame receipt and temporal/global consumption"); return; }
            q.Category = "a"; q.Reasons.Add("required own-program-frame event key plus existence/uniqueness/receipt lookup"); return;
        }
        if (predicates.Any(p => p.Body.DescendantNodesAndSelf().OfType<BinaryExpressionSyntax>().Any(b => OwnSide(b, q))))
        { q.Category = "c"; q.Reasons.Add("own-frame comparison is not required on every predicate branch"); return; }
        if (NativeKey.IsMatch(text) || OpaqueReceipt.IsMatch(text))
        { q.Category = "c"; q.Reasons.Add("event/parent/native identifier is not proven to be this program frame"); return; }
        if (Time.IsMatch(text) || Owner.IsMatch(text) || q.Typed && predicates.Length == 0 && terminals.Any(n => n is "Any" or "Count" or "LongCount" or "Sum" or "Min" or "Max" or "Distinct" or "GroupBy"))
        { q.Category = "b"; q.Reasons.Add("cross-frame typed history with temporal, owner-wide or global rule consumption"); return; }
        q.Category = "c"; q.Reasons.Add(q.Typed ? "typed query purpose or predicate remains unresolved" : "unbounded Complete/non-generic history lacks a resolved rule scope");
    }
    static bool RequiredOwn(SyntaxNode node, Query q)
    {
        if (node is ParenthesizedExpressionSyntax p) return RequiredOwn(p.Expression, q);
        if (node is BinaryExpressionSyntax b)
        {
            if (b.IsKind(SyntaxKind.LogicalAndExpression)) return RequiredOwn(b.Left, q) || RequiredOwn(b.Right, q);
            if (b.IsKind(SyntaxKind.LogicalOrExpression)) return RequiredOwn(b.Left, q) && RequiredOwn(b.Right, q);
            return b.IsKind(SyntaxKind.EqualsExpression) && OwnSide(b, q);
        }
        return false;
    }
    static bool OwnSide(BinaryExpressionSyntax b, Query q) =>
        IsEventKey(b.Left, q) && OwnId(b.Right, q) || IsEventKey(b.Right, q) && OwnId(b.Left, q) ||
        OwnEvent(b.Left, q) && IsEventValue(b.Right, q) || OwnEvent(b.Right, q) && IsEventValue(b.Left, q);
    static bool IsEventKey(ExpressionSyntax expression, Query q) => expression is MemberAccessExpressionSyntax m &&
        m.Name.Identifier.ValueText is "FrameId" or "ProgramFrameId" or "ProgramSkillFrameId" or "ParentProgramFrameId" or "OwnerFrameId" or "OriginFrameId" or "ParentFrameId" &&
        m.Expression.DescendantNodesAndSelf().OfType<IdentifierNameSyntax>().Any(id => q.Model.GetSymbolInfo(id).Symbol is IParameterSymbol p &&
            p.DeclaringSyntaxReferences.Any(r => r.GetSyntax().Ancestors().Any(a => a is LambdaExpressionSyntax)));
    static bool IsEventValue(ExpressionSyntax expression, Query q) => q.Model.GetTypeInfo(expression).Type is INamedTypeSymbol type &&
        type.AllInterfaces.Any(i => i.Name == "IGameEvent");
    static bool OwnEvent(ExpressionSyntax expression, Query q)
    {
        if (expression is not BaseObjectCreationExpressionSyntax created || !IsEventValue(expression, q) || created.ArgumentList is null) return false;
        var ctor = q.Model.GetSymbolInfo(created).Symbol as IMethodSymbol;
        return created.ArgumentList.Arguments.Select((a, i) => (a, i)).Any(x => ctor is not null && x.i < ctor.Parameters.Length &&
            ctor.Parameters[x.i].Name is "FrameId" or "ProgramFrameId" or "ProgramSkillFrameId" or "OwnerFrameId" or "OriginFrameId" or "ParentFrameId" && OwnId(x.a.Expression, q));
    }
    static bool OwnId(ExpressionSyntax expression, Query q, int depth = 0)
    {
        if (depth > 2) return false;
        if (expression is ParenthesizedExpressionSyntax p) return OwnId(p.Expression, q, depth + 1);
        if (expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Id" } m)
        {
            var type = q.Model.GetTypeInfo(m.Expression).Type as INamedTypeSymbol;
            if (type?.Name == "ProgramSkillFrame") return true;
            // A native frame is not automatically this program's child. Multi-hop
            // receipt/stack ancestry remains uncertain unless separately established.
            return false;
        }
        if (expression is not IdentifierNameSyntax id) return false;
        var symbol = q.Model.GetSymbolInfo(id).Symbol;
        if (symbol is ILocalSymbol local && local.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is VariableDeclaratorSyntax { Initializer.Value: { } value }) return OwnId(value, q, depth + 1);
        if (symbol is not IParameterSymbol parameter || q.Callers.Count == 0) return false;
        return q.Callers.All(c => c.Call.ArgumentList.Arguments.Any(a =>
            c.Model.GetOperation(a) is IArgumentOperation argument && argument.Parameter?.Ordinal == parameter.Ordinal &&
            a.Expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Id" } frame && c.Model.GetTypeInfo(frame.Expression).Type?.Name == "ProgramSkillFrame"));
    }
    sealed class Query
    {
        public Query(InvocationExpressionSyntax node, SyntaxNode container, MethodDeclarationSyntax? method, string file, int start, string accessor, string? eventType)
        { Node = node; Container = container; Method = method; File = file; StartLine = start; Accessor = accessor; EventType = eventType; Line = node.GetLocation().GetLineSpan().StartLinePosition.Line + 1; MethodId = file + ":" + start + ":" + (method?.Identifier.ValueText ?? "property/initializer"); MethodLines = container.GetLocation().GetLineSpan().EndLinePosition.Line - start + 2; }
        public InvocationExpressionSyntax Node; public SyntaxNode Container; public MethodDeclarationSyntax? Method; public SemanticModel Model = null!; public IMethodSymbol? MethodSymbol;
        public string File, MethodId, Accessor, Callee = "", Scope = "", Category = "c", CDisposition = "uncertain"; public string? EventType; public int Line, StartLine, MethodLines; public bool Typed => EventType is not null; public bool ExcludedFromPrimary, Escaped, ProofContext;
        public List<ExpressionSyntax> Expressions = []; public HashSet<string> Aliases = []; public List<string> Reasons = [], OwnFrameEvidence = []; public List<CallContext> Callers = [];
    }
    sealed record CallContext(string File, int Line, string Method, string Expression, InvocationExpressionSyntax Call, SemanticModel Model);
}
