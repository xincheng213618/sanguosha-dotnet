using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

// Batch editor for redundant internal assertions. It never rewrites rule branches,
// registered continuation decisions, external input validation or required-value extraction.
internal static class InvariantCleanupAnalysis
{
    static readonly Regex Boundary = new("Restore|Replay|Checkpoint|Projection|Snapshot|Visibility|Deserialize|Serialize|Loader|JsonConverter", RegexOptions.IgnoreCase);
    static readonly Regex Input = new("Program(Answer|ChoiceAnswer|TriggerLegality|SelectionEligibility|CatalogValidation|Definition)|SubmittedUseValidation|CommandHandler|ContentValidation");
    static readonly Regex Writes = new("^(?:Try)?(?:Advance|Queue|Publish|Log|Write|Move|Draw|Discard|Give|Obtain|Lose|Recover|Damage|Set|Replace|Push|Pop|Clear|Remove|Add|Issue|Grant|Revoke|Consume|Pay|Cancel|Complete|Finish|Begin|Start|Request|Execute|Resolve|Answer|Resume|Continue|Drain|Record|Freeze|Commit|Prepare).*", RegexOptions.Compiled);
    static readonly HashSet<string> ReadCalls = ["ProgramEventHistory", "CompleteProgramEventHistory", "Read", "GetLocation", "GetProgramBindingId", "ActiveProgram", "GetSkill", "GetValueOrDefault", "ContainsKey", "TryGetValue", "ReferenceEquals", "Equals"];
    static readonly Regex ProofName = new("Paid|Proof|Invoice|Receipt|Valid|Exact|Original|Issued", RegexOptions.IgnoreCase);
    sealed record Edit(int Start, int Length, string Replacement, string Kind, int Line);
    sealed class FileState(string path, string text, SyntaxTree tree, string? version)
    {
        public string Path = path, Original = text, Current = text;
        public SyntaxTree Tree = tree;
        public string? Version = version;
        public List<Edit> Edits = [];
    }
    sealed record Candidate(string Version, string File, int Line, int Lines, string Method, string Kind,
        bool Mechanical, string Reason, string Preview);
    sealed record Auxiliary(string Version, string File, int Line, int Lines, string Kind, string Name, string Reason);

    public static void WriteMetrics(string root, string output, SyntaxTree[] trees, JsonSerializerOptions json)
    {
        var files = trees.Select(tree =>
        {
            var file = Path.GetRelativePath(root, tree.FilePath).Replace('\\', '/');
            var at = file.IndexOf("/Runtime/", StringComparison.OrdinalIgnoreCase);
            if (!file.StartsWith("src/Characters/", StringComparison.OrdinalIgnoreCase) || at < 0) return null;
            var text = tree.GetText();
            var syntax = tree.GetRoot();
            return new
            {
                version = file[..at], file,
                bytes = new FileInfo(tree.FilePath).Length,
                lines = text.Length == 0 ? 0 : text.Lines.Count - (text.Lines[^1].Span.Length == 0 ? 1 : 0),
                invalidOperationThrows = syntax.DescendantNodes().Count(node =>
                    node is ThrowStatementSyntax { Expression: ObjectCreationExpressionSyntax created } && IsInvalidOperation(created) ||
                    node is ThrowExpressionSyntax { Expression: ObjectCreationExpressionSyntax expression } && IsInvalidOperation(expression)),
                historyAccessors = syntax.DescendantNodes().OfType<InvocationExpressionSyntax>().Count(IsHistory),
                eventTypes = syntax.DescendantNodes().OfType<RecordDeclarationSyntax>().Count(record =>
                    record.BaseList?.Types.Any(type => type.Type switch
                    {
                        IdentifierNameSyntax name => name.Identifier.ValueText == "IGameEvent",
                        QualifiedNameSyntax name => name.Right.Identifier.ValueText == "IGameEvent",
                        AliasQualifiedNameSyntax name => name.Name.Identifier.ValueText == "IGameEvent",
                        _ => false
                    }) == true)
            };
        }).Where(file => file is not null).Select(file => file!).ToArray();
        var allRuntime = new { files = files.Length, lines = files.Sum(file => file.lines), bytes = files.Sum(file => file.bytes),
            invalidOperationThrows = files.Sum(file => file.invalidOperationThrows),
            historyAccessors = files.Sum(file => file.historyAccessors), eventTypes = files.Sum(file => file.eventTypes) };
        var result = new
        {
            generatedUtc = DateTime.UtcNow,
            scope = "src/Characters/**/Runtime/**/*.cs; AST throw statements/expressions, history invocations and IGameEvent records",
            allRuntime,
            perVersion = files.GroupBy(file => file.version).OrderBy(group => group.Key, StringComparer.Ordinal).Select(group => new
            {
                version = group.Key, files = group.Count(), lines = group.Sum(file => file.lines), bytes = group.Sum(file => file.bytes),
                invalidOperationThrows = group.Sum(file => file.invalidOperationThrows),
                historyAccessors = group.Sum(file => file.historyAccessors), eventTypes = group.Sum(file => file.eventTypes)
            }).ToArray()
        };
        File.WriteAllText(Path.Combine(output, "metrics.json"), JsonSerializer.Serialize(result, json), new UTF8Encoding(false));
        Console.WriteLine("Runtime AST metrics: " + JsonSerializer.Serialize(allRuntime) + "; " + Path.Combine(output, "metrics.json"));
    }

    public static void Run(string root, string output, string[] args, SyntaxTree[] trees, CSharpCompilation compilation,
        CSharpParseOptions options, JsonSerializerOptions json)
    {
        var timer = Stopwatch.StartNew();
        string Rel(string path) => Path.GetRelativePath(root, path).Replace('\\', '/');
        string? Version(string file)
        {
            var path = Rel(file); var at = path.IndexOf("/Runtime/", StringComparison.OrdinalIgnoreCase);
            return path.StartsWith("src/Characters/", StringComparison.OrdinalIgnoreCase) && at >= 0 ? path[..at] : null;
        }
        var states = trees.Select(t => new FileState(t.FilePath, t.GetText().ToString(), t, Version(t.FilePath))).ToDictionary(s => s.Path, StringComparer.OrdinalIgnoreCase);
        // Reference removal is deliberately stricter than symbol binding: any source/test
        // token or reflection-name string with this identifier keeps the declaration.
        foreach (var folder in new[] { "src", "tests" })
        foreach (var path in Directory.EnumerateFiles(Path.Combine(root, folder), "*.cs", SearchOption.AllDirectories)
            .Where(p => !p.Split(Path.DirectorySeparatorChar).Any(s => s is "bin" or "obj" or ".artifacts")))
            if (!states.ContainsKey(path))
            {
                var text = File.ReadAllText(path);
                states[path] = new(path, text, CSharpSyntaxTree.ParseText(text, options, path), Version(path));
            }
        var runtime = states.Values.Where(s => s.Version is not null).OrderBy(s => s.Path, StringComparer.Ordinal).ToArray();
        var versions = runtime.Select(s => s.Version!).Distinct().Order(StringComparer.Ordinal).ToArray();
        var versionsFile = Arg(args, "--versions-file");
        var selected = versionsFile is null ? versions.ToHashSet(StringComparer.OrdinalIgnoreCase) : ReadVersions(versionsFile, root);
        var unknown = selected.Except(versions, StringComparer.OrdinalIgnoreCase).ToArray();
        if (unknown.Length != 0) throw new InvalidOperationException("Unknown Runtime version directories: " + string.Join(", ", unknown));
        var apply = args.Contains("--apply");
        if (apply && versionsFile is null) throw new InvalidOperationException("--apply requires --versions-file; choose a reviewed batch manifest.");
        var models = new Dictionary<SyntaxTree, SemanticModel>();
        SemanticModel Model(SyntaxTree tree) => models.TryGetValue(tree, out var m) ? m : models[tree] = compilation.GetSemanticModel(tree);
        var purity = new Dictionary<ISymbol, bool>(SymbolEqualityComparer.Default);
        var invariantPurity = new Dictionary<ISymbol, bool>(SymbolEqualityComparer.Default);
        var active = new HashSet<ISymbol>(SymbolEqualityComparer.Default);
        bool Pure(SyntaxNode node, bool invariant = false)
        {
            foreach (var n in node.DescendantNodesAndSelf())
            {
                if (invariant && (n is ThrowStatementSyntax { Expression: not ObjectCreationExpressionSyntax } ||
                    n is ThrowStatementSyntax { Expression: ObjectCreationExpressionSyntax error } && !IsInvalidOperation(error) ||
                    n is ThrowExpressionSyntax { Expression: not ObjectCreationExpressionSyntax } ||
                    n is ThrowExpressionSyntax { Expression: ObjectCreationExpressionSyntax errorExpression } && !IsInvalidOperation(errorExpression))) return false;
                if (n is AwaitExpressionSyntax or AnonymousMethodExpressionSyntax ||
                    !invariant && n is ThrowExpressionSyntax or ThrowStatementSyntax ||
                    n is PrefixUnaryExpressionSyntax p && p.Kind() is SyntaxKind.PreIncrementExpression or SyntaxKind.PreDecrementExpression ||
                    n is PostfixUnaryExpressionSyntax q && q.Kind() is SyntaxKind.PostIncrementExpression or SyntaxKind.PostDecrementExpression ||
                    n is ArgumentSyntax a && !a.RefKindKeyword.IsKind(SyntaxKind.None)) return false;
                if (n is AssignmentExpressionSyntax assignment)
                {
                    var assigned = Model(node.SyntaxTree).GetSymbolInfo(assignment.Left).Symbol;
                    if (assigned is not ILocalSymbol and not IParameterSymbol || assignment.Kind() != SyntaxKind.SimpleAssignmentExpression) return false;
                }
                if (n is InvocationExpressionSyntax call)
                {
                    var symbol = Model(node.SyntaxTree).GetSymbolInfo(call).Symbol as IMethodSymbol;
                    if (symbol is null || symbol.ReturnsVoid && !invariant || symbol.Parameters.Any(p => p.RefKind != RefKind.None)) return false;
                    var target = (symbol.ReducedFrom ?? symbol).OriginalDefinition;
                    var ns = target.ContainingNamespace.ToDisplayString();
                    var frameworkRead = ns == "System.Linq" || target.ContainingType.SpecialType == SpecialType.System_String ||
                        target.ContainingType.ToDisplayString() is "System.Math" or "System.MathF" ||
                        ns.StartsWith("System.Collections", StringComparison.Ordinal) &&
                        target.Name is "Contains" or "ContainsKey" or "ContainsValue" or "Find" or "FindAll" or "FindIndex" or "IndexOf" or "LastIndexOf" or "GetValueOrDefault" or "AsReadOnly";
                    if (frameworkRead && !symbol.ReturnsVoid && !Writes.IsMatch(target.Name)) continue;
                    if (ReadCalls.Contains(target.Name)) continue;
                    if (Writes.IsMatch(target.Name)) return false;
                    var cache = invariant ? invariantPurity : purity;
                    if (!cache.TryGetValue(target, out var pure))
                    {
                        if (active.Count >= 8 || !active.Add(target)) return false;
                        var declaration = target.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax;
                        pure = declaration is not null && declaration.AttributeLists.Count == 0 &&
                            !declaration.Modifiers.Any(SyntaxKind.AsyncKeyword) &&
                            (declaration.Body is { } body ? Pure(body, invariant) : declaration.ExpressionBody is { } expression && Pure(expression.Expression, invariant));
                        active.Remove(target); cache[target] = pure;
                    }
                    if (!pure) return false;
                }
            }
            return true;
        }
        var candidates = new List<Candidate>();
        var auxiliary = new List<Auxiliary>();
        var removedRegisteredInvariants = new List<Auxiliary>();
        var deletedGuardCallees = new HashSet<string>(StringComparer.Ordinal);
        var eligibleHelperKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var originalMethods = runtime.SelectMany(f => f.Tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>())
            .GroupBy(m => HelperKey(m.SyntaxTree.FilePath, m.Identifier.ValueText), StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() == 1).ToDictionary(g => g.Key, g => g.Single(), StringComparer.OrdinalIgnoreCase);
        void Remember(IMethodSymbol target)
        {
            if (target.ReturnType.SpecialType != SpecialType.System_Boolean) return;
            deletedGuardCallees.Add(target.Name);
            foreach (var reference in target.DeclaringSyntaxReferences)
                eligibleHelperKeys.Add(HelperKey(reference.SyntaxTree.FilePath, target.Name));
        }
        foreach (var file in runtime)
        {
            var treeRoot = file.Tree.GetRoot();
            var retiredAssertions = new HashSet<MethodDeclarationSyntax>();
            foreach (var method in treeRoot.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {
                var attributes = method.AttributeLists.SelectMany(a => a.Attributes).ToArray();
                var nativeValidation = attributes.Length == 1 && attributes[0].Name.ToString() is "NativeUseValidation" or "NativeUseValidationAttribute";
                var publishAssertion = attributes.Length == 1 && method.Identifier.ValueText.StartsWith("Assert", StringComparison.Ordinal) &&
                    attributes[0].Name.ToString() is "ProgramRuleObserver" or "ProgramRuleObserverAttribute" && attributes[0].ToString().Contains("BeforeStatePublish", StringComparison.Ordinal);
                if (!nativeValidation && !publishAssertion) continue;
                var reason = BoundaryReason(file, method, method);
                if (reason is null && (method.ReturnType.ToString() != "void" || method.Body is null || !Pure(method.Body, true))) reason = "registered-method-has-write-or-non-invariant-call";
                var kind = nativeValidation ? "native-completed-use-invariant" : "before-publish-invariant-sweep";
                candidates.Add(new(file.Version!, Rel(file.Path), Line(method), Lines(method), method.Identifier.ValueText, kind, reason is null, reason ?? "pure-registered-invariant-only", Preview(method.ToString())));
                if (reason is null)
                {
                    retiredAssertions.Add(method);
                    if (!selected.Contains(file.Version!)) continue;
                    file.Edits.Add(RemoveMember(file.Current, method, kind));
                    removedRegisteredInvariants.Add(new(file.Version!, Rel(file.Path), Line(method), Lines(method), kind, method.Identifier.ValueText, "pure-internal-invariant-registration"));
                    foreach (var call in method.DescendantNodes().OfType<InvocationExpressionSyntax>())
                        if (Model(file.Tree).GetSymbolInfo(call).Symbol is IMethodSymbol target) Remember(target);
                }
            }
            foreach (var guard in treeRoot.DescendantNodes().OfType<IfStatementSyntax>())
            {
                if (guard.Ancestors().OfType<MethodDeclarationSyntax>().Any(retiredAssertions.Contains)) continue;
                if (guard.Else is not null) continue;
                var statement = guard.Statement is BlockSyntax { Statements: [var only] } ? only : guard.Statement;
                if (statement is not ThrowStatementSyntax { Expression: ObjectCreationExpressionSyntax exception }) continue;
                var method = Enclosing(guard);
                var reason = BoundaryReason(file, guard, method);
                if (reason is null && !IsInvalidOperation(exception))
                    reason = "exception-is-input-or-other-contract";
                if (reason is null && guard.Condition.DescendantNodesAndSelf().OfType<VariableDesignationSyntax>()
                    .OfType<SingleVariableDesignationSyntax>().Any(v => Escapes(v.Identifier.ValueText, guard, method))) reason = "pattern-variable-used-after-guard";
                if (reason is null && !Pure(guard.Condition)) reason = "condition-has-write-or-unresolved-impure-call";
                var candidate = new Candidate(file.Version!, Rel(file.Path), Line(guard), Lines(guard), MethodName(method), "if-throw", reason is null,
                    reason ?? "internal-read-only-invariant-throw", Preview(guard.ToString()));
                candidates.Add(candidate);
                if (reason is null && selected.Contains(file.Version!))
                {
                    file.Edits.Add(RemoveStatement(file.Current, guard, "if-throw"));
                    foreach (var call in guard.Condition.DescendantNodesAndSelf().OfType<InvocationExpressionSyntax>())
                        if (Model(file.Tree).GetSymbolInfo(call).Symbol is IMethodSymbol target) Remember(target);
                }
            }
            foreach (var coalesce in treeRoot.DescendantNodes().OfType<BinaryExpressionSyntax>()
                .Where(b => b.IsKind(SyntaxKind.CoalesceExpression) && b.Right is ThrowExpressionSyntax))
            {
                var method = Enclosing(coalesce);
                candidates.Add(new(file.Version!, Rel(file.Path), Line(coalesce), Lines(coalesce), MethodName(method), "coalesce-throw", false,
                    BoundaryReason(file, coalesce, method) ?? "required-value-extraction-retained", Preview(coalesce.ToString())));
            }
            foreach (var method in treeRoot.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {
                var reads = method.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(IsHistory).ToArray();
                var proof = ProofName.IsMatch(method.Identifier.ValueText) || method.AttributeLists.ToString().Contains("ProgramContinuation", StringComparison.Ordinal);
                if (reads.Length == 0 && !proof) continue;
                var own = method.ParameterList.Parameters.Where(p => p.Type?.ToString() == "ProgramSkillFrame").Any(p =>
                    reads.Any(q => Regex.IsMatch(HistoryPipeline(q).ToString(), @"\b(?:FrameId|OwnerFrameId)\s*==\s*" + Regex.Escape(p.Identifier.ValueText) + @"\.Id\b")));
                auxiliary.Add(new(file.Version!, Rel(file.Path), Line(method), Lines(method),
                    own ? "self-frame-history-or-proof" : reads.Length > 0 ? "engine-fact-ledger-query" : "proof-or-continuation-name",
                    method.Identifier.ValueText, method.AttributeLists.Count > 0 ? "registered-rule-entry-retained" : "review-only-no-name-based-deletion"));
            }
        }
        Console.WriteLine($"Guard inventory ready: {candidates.Count} candidates, {candidates.Count(c => c.Mechanical)} mechanical. Resolving dead guard helpers...");
        foreach (var file in runtime) ApplyEdits(file);

        // Only helpers that were actually used by a removed guard enter this queue.
        // An identifier anywhere in src/tests (including an attribute/nameof/string)
        // prevents deletion; registered hooks and public/interface methods always stay.
        var removedHelpers = new List<Auxiliary>();
        var retainedHelpers = new List<Auxiliary>();
        var helperPool = runtime.Where(f => selected.Contains(f.Version!)).SelectMany(f => f.Tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(m => eligibleHelperKeys.Contains(HelperKey(f.Path, m.Identifier.ValueText))).Select(m => (File: f, Method: m))).ToArray();
        var removedNames = new HashSet<string>(StringComparer.Ordinal);
        bool changed;
        do
        {
            changed = false;
            var referenceCounts = new Dictionary<string, int>(StringComparer.Ordinal);
            var reflectionNames = new HashSet<string>(StringComparer.Ordinal);
            foreach (var file in states.Values)
            foreach (var token in file.Tree.GetRoot().DescendantTokens())
            {
                if (deletedGuardCallees.Contains(token.ValueText)) referenceCounts[token.ValueText] = referenceCounts.GetValueOrDefault(token.ValueText) + 1;
                if (token.IsKind(SyntaxKind.StringLiteralToken))
                    foreach (Match word in Regex.Matches(token.ValueText, @"\b[A-Za-z_]\w*\b"))
                        if (deletedGuardCallees.Contains(word.Value)) reflectionNames.Add(word.Value);
            }
            foreach (var file in runtime.Where(f => selected.Contains(f.Version!)))
            foreach (var method in file.Tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where(m => eligibleHelperKeys.Contains(HelperKey(file.Path, m.Identifier.ValueText))).ToArray())
            {
                if (method.ReturnType.ToString() != "bool" || method.AttributeLists.Count > 0 || method.ExplicitInterfaceSpecifier is not null ||
                    method.Modifiers.Any(t => t.IsKind(SyntaxKind.PublicKeyword) || t.IsKind(SyntaxKind.ProtectedKeyword) || t.IsKind(SyntaxKind.OverrideKeyword)) ||
                    BoundaryReason(file, method, method) is not null ||
                    referenceCounts.GetValueOrDefault(method.Identifier.ValueText) > method.DescendantTokens().Count(t => t.ValueText == method.Identifier.ValueText) ||
                    reflectionNames.Contains(method.Identifier.ValueText)) continue;
                var record = new Auxiliary(file.Version!, Rel(file.Path), Line(method), Lines(method), "dead-guard-only-bool-helper", method.Identifier.ValueText, "no-src-or-test-reference-after-guard-removal");
                removedHelpers.Add(record); removedNames.Add(method.Identifier.ValueText);
                if (originalMethods.TryGetValue(HelperKey(file.Path, method.Identifier.ValueText), out var originalMethod))
                    foreach (var call in originalMethod.DescendantNodes().OfType<InvocationExpressionSyntax>())
                        if (Model(originalMethod.SyntaxTree).GetSymbolInfo(call).Symbol is IMethodSymbol target) Remember(target);
                file.Edits.Add(RemoveMember(file.Current, method, "dead-proof-helper"));
                changed = true;
            }
            foreach (var file in runtime) ApplyEdits(file);
        } while (changed);
        Console.WriteLine($"Removed {removedHelpers.Count} dead guard helpers and {removedRegisteredInvariants.Count} registered invariant methods in memory. Checking self-only events...");
        foreach (var (file, method) in helperPool.Where(p => !removedNames.Contains(p.Method.Identifier.ValueText)).DistinctBy(p => p.File.Path + ":" + p.Method.SpanStart))
            retainedHelpers.Add(new(file.Version!, Rel(file.Path), Line(method), Lines(method), "guard-helper-retained", method.Identifier.ValueText,
                method.AttributeLists.Count > 0 ? "registered-entry-or-attribute" : "other-src-test-reference-or-non-bool-contract"));

        var eventInventory = new List<Auxiliary>();
        var removedEvents = new List<Auxiliary>();
        var eventNames = runtime.Where(f => selected.Contains(f.Version!)).SelectMany(f => f.Tree.GetRoot().DescendantNodes().OfType<RecordDeclarationSyntax>())
            .Where(r => r.BaseList?.Types.Any(t => t.Type.ToString() == "IGameEvent") == true).Select(r => r.Identifier.ValueText).ToHashSet(StringComparer.Ordinal);
        var eventReferences = new Dictionary<string, List<(FileState File, SyntaxToken Token)>>(StringComparer.Ordinal);
        var affectedEventNames = new HashSet<string>(StringComparer.Ordinal);
        foreach (var edited in states.Values.Where(f => f.Current != f.Original))
        {
            var beforeReferences = CSharpSyntaxTree.ParseText(edited.Original, options).GetRoot().DescendantTokens().Where(t => eventNames.Contains(t.ValueText)).GroupBy(t => t.ValueText)
                .ToDictionary(g => g.Key, g => g.Count(), StringComparer.Ordinal);
            var afterReferences = edited.Tree.GetRoot().DescendantTokens().Where(t => eventNames.Contains(t.ValueText)).GroupBy(t => t.ValueText)
                .ToDictionary(g => g.Key, g => g.Count(), StringComparer.Ordinal);
            foreach (var pair in beforeReferences.Where(p => p.Value > afterReferences.GetValueOrDefault(p.Key))) affectedEventNames.Add(pair.Key);
        }
        foreach (var source in states.Values)
        foreach (var token in source.Tree.GetRoot().DescendantTokens())
        {
            if (eventNames.Contains(token.ValueText))
            {
                if (!eventReferences.TryGetValue(token.ValueText, out var list)) eventReferences[token.ValueText] = list = [];
                list.Add((source, token));
            }
            if (token.IsKind(SyntaxKind.StringLiteralToken))
                foreach (Match word in Regex.Matches(token.ValueText, @"\b[A-Za-z_]\w*\b"))
                    if (eventNames.Contains(word.Value))
                    {
                        if (!eventReferences.TryGetValue(word.Value, out var list)) eventReferences[word.Value] = list = [];
                        list.Add((source, token));
                    }
        }
        foreach (var file in runtime.Where(f => selected.Contains(f.Version!)))
        foreach (var declaration in file.Tree.GetRoot().DescendantNodes().OfType<RecordDeclarationSyntax>()
            .Where(r => r.BaseList?.Types.Any(t => t.Type.ToString() == "IGameEvent") == true).ToArray())
        {
            var name = declaration.Identifier.ValueText;
            var references = (eventReferences.GetValueOrDefault(name) ?? []).Where(r => r.File != file || !declaration.FullSpan.Contains(r.Token.Span)).ToArray();
            var publications = new List<(FileState File, ExpressionStatementSyntax Statement)>();
            var reason = references.Length == 0 ? "no-remaining-src-test-consumer" : "only-independent-local-publications";
            foreach (var reference in references)
            {
                var statement = reference.Token.Parent?.AncestorsAndSelf().OfType<ExpressionStatementSyntax>().FirstOrDefault();
                if (reference.File.Version != file.Version || statement?.Expression is not InvocationExpressionSyntax call ||
                    CallName(call) != "AdvanceEventRulesAndQueueFact" || call.ArgumentList.Arguments.Count != 1 ||
                    call.ArgumentList.Arguments[0].Expression is not ObjectCreationExpressionSyntax creation || creation.Type.ToString() != name ||
                    creation.ArgumentList?.Arguments.Any(a => a.Expression.DescendantNodesAndSelf().Any(n => n is InvocationExpressionSyntax or AssignmentExpressionSyntax or AwaitExpressionSyntax ||
                        n is PrefixUnaryExpressionSyntax pre && pre.Kind() is SyntaxKind.PreIncrementExpression or SyntaxKind.PreDecrementExpression ||
                        n is PostfixUnaryExpressionSyntax post && post.Kind() is SyntaxKind.PostIncrementExpression or SyntaxKind.PostDecrementExpression)) == true)
                { reason = "other-consumer-or-publication-side-effect"; break; }
                publications.Add((reference.File, statement));
            }
            var item = new Auxiliary(file.Version!, Rel(file.Path), Line(declaration), Lines(declaration), "character-event", name, reason);
            eventInventory.Add(item);
            // Do not remove pre-existing unreferenced event declarations incidentally.
            // Require this batch to have removed a reference from one of its source files.
            var touched = affectedEventNames.Contains(name);
            if (reason == "other-consumer-or-publication-side-effect" || !touched || declaration.Modifiers.Any(SyntaxKind.PartialKeyword)) continue;
            foreach (var (publisher, statement) in publications.DistinctBy(p => p.File.Path + ":" + p.Statement.SpanStart))
                publisher.Edits.Add(RemoveStatement(publisher.Current, statement, "dead-self-event-publication"));
            file.Edits.Add(RemoveMember(file.Current, declaration, "dead-self-event"));
            removedEvents.Add(item);
        }
        foreach (var file in runtime) ApplyEdits(file);
        foreach (var file in runtime.Where(f => f.Current != f.Original))
        {
            var newErrors = file.Tree.GetDiagnostics().Count(d => d.Severity == DiagnosticSeverity.Error);
            var oldErrors = CSharpSyntaxTree.ParseText(file.Original, options).GetDiagnostics().Count(d => d.Severity == DiagnosticSeverity.Error);
            if (newErrors > oldErrors) throw new InvalidOperationException("Rewritten syntax has new errors: " + Rel(file.Path));
        }
        object Metrics(IEnumerable<FileState> files, bool after) => new
        {
            files = files.Count(), lines = files.Sum(f => TextLines(after ? f.Current : f.Original)), bytes = files.Sum(f => Encoding.UTF8.GetByteCount(after ? f.Current : f.Original)),
            invalidOperationThrows = files.Sum(f => CSharpSyntaxTree.ParseText(after ? f.Current : f.Original, options).GetRoot().DescendantNodes().Count(n =>
                n is ThrowStatementSyntax { Expression: ObjectCreationExpressionSyntax created } && IsInvalidOperation(created) ||
                n is ThrowExpressionSyntax { Expression: ObjectCreationExpressionSyntax createdExpression } && IsInvalidOperation(createdExpression))),
            historyAccessors = files.Sum(f => CSharpSyntaxTree.ParseText(after ? f.Current : f.Original, options).GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>().Count(IsHistory)),
            eventTypes = files.Sum(f => CSharpSyntaxTree.ParseText(after ? f.Current : f.Original, options).GetRoot().DescendantNodes().OfType<RecordDeclarationSyntax>().Count(r => r.BaseList?.Types.Any(t => t.Type.ToString() == "IGameEvent") == true))
        };
        void Write(string name, object value) => System.IO.File.WriteAllText(Path.Combine(output, name), JsonSerializer.Serialize(value, json), new UTF8Encoding(false));
        Directory.CreateDirectory(Path.Combine(output, "batches"));
        var verification = InvariantVerificationIndex.Build(root, versions);
        Write("verification-index.json", verification);
        var batches = versions.Chunk(50).Select((batch, index) => new { id = index + 1, file = $"batches/batch-{index + 1:000}.txt", versions = batch,
            mechanicalGuards = candidates.Count(c => c.Mechanical && batch.Contains(c.Version)), guardLines = candidates.Where(c => c.Mechanical && batch.Contains(c.Version)).Sum(c => c.Lines),
            coreFilters = verification.Matches.Where(m => batch.Contains(m.Version)).SelectMany(m => m.CoreFilters).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray(),
            unmatchedVersions = verification.UnmatchedVersions.Where(batch.Contains).ToArray() }).ToArray();
        foreach (var batch in batches) System.IO.File.WriteAllLines(Path.Combine(output, batch.file), batch.versions, new UTF8Encoding(false));
        Write("manifest.json", new { generatedUtc = DateTime.UtcNow, scope = "All src/Characters/**/Runtime C#; no historical four-directory exclusions", batchSize = 50, versions = versions.Length, batches });
        Write("candidates.json", candidates);
        Write("auxiliary.json", new { candidates = auxiliary, removedHelpers, retainedHelpers, removedRegisteredInvariants, events = eventInventory, removedEvents });
        Write("changes.json", runtime.Where(f => f.Current != f.Original).Select(f => new { file = Rel(f.Path), f.Version,
            beforeSha256 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(f.Original))).ToLowerInvariant(),
            removedLines = TextLines(f.Original) - TextLines(f.Current), removedBytes = Encoding.UTF8.GetByteCount(f.Original) - Encoding.UTF8.GetByteCount(f.Current) }));
        Write("summary.json", new { generatedUtc = DateTime.UtcNow, mode = apply ? "apply" : "dry-run", versionsFile, scannedVersions = versions.Length, selectedVersions = selected.Count,
            candidates = candidates.Count, mechanicalGuards = candidates.Count(c => c.Mechanical), selectedMechanicalGuards = candidates.Count(c => c.Mechanical && selected.Contains(c.Version)),
            retainedReasons = candidates.Where(c => !c.Mechanical).GroupBy(c => c.Reason).OrderByDescending(g => g.Count()).Select(g => new { reason = g.Key, count = g.Count() }),
            removedHelpers = removedHelpers.Count, removedEvents = removedEvents.Count, removedRegisteredInvariants = removedRegisteredInvariants.Count, changedFiles = runtime.Count(f => f.Current != f.Original),
            before = Metrics(runtime.Where(f => selected.Contains(f.Version!)), false), after = Metrics(runtime.Where(f => selected.Contains(f.Version!)), true),
            allRuntimeBefore = Metrics(runtime, false), elapsedSeconds = timer.Elapsed.TotalSeconds,
            limitations = new[] { "Syntax/semantic read-only helper screening is a mechanical invariant classification, not per-skill equivalence proof.", "All registered hooks/rule qualifications, external input/definition/replay/projection boundaries and required-value coalescing extractions remain.", "Dead helpers are only reached from deleted guards; any remaining src/tests identifier or reflection-name string keeps them. Self-events require only standalone local publications and no consumers.", "Dry-run computes proposed changes in memory. Apply requires an explicit version list and checks source has not changed. No business build or tests are run." } });
        if (apply)
        {
            var changedFiles = runtime.Where(f => f.Current != f.Original).ToArray();
            foreach (var file in changedFiles)
                if (System.IO.File.ReadAllText(file.Path) != file.Original) throw new InvalidOperationException("Source changed during analysis: " + Rel(file.Path));
            foreach (var file in changedFiles)
            {
                var originalBytes = System.IO.File.ReadAllBytes(file.Path);
                var bom = originalBytes.Length >= 3 && originalBytes[0] == 0xef && originalBytes[1] == 0xbb && originalBytes[2] == 0xbf;
                System.IO.File.WriteAllText(file.Path, file.Current, new UTF8Encoding(bom));
            }
        }
        Console.WriteLine($"Invariant {(apply ? "cleanup applied" : "dry-run ready")}: {versions.Length} versions; {candidates.Count(c => c.Mechanical)} mechanical guards; selected {selected.Count} versions, {runtime.Count(f => f.Current != f.Original)} changed files, -{runtime.Sum(f => TextLines(f.Original) - TextLines(f.Current))} lines; helpers {removedHelpers.Count}, events {removedEvents.Count}; {timer.Elapsed.TotalSeconds:F1}s. {output}");

        void ApplyEdits(FileState file)
        {
            if (file.Edits.Count == 0) return;
            var unique = file.Edits.DistinctBy(e => (e.Start, e.Length)).OrderByDescending(e => e.Start).ToArray();
            var previous = file.Current.Length;
            foreach (var edit in unique)
            {
                if (edit.Start + edit.Length > previous) throw new InvalidOperationException("Overlapping cleanup edits: " + Rel(file.Path));
                file.Current = file.Current.Remove(edit.Start, edit.Length).Insert(edit.Start, edit.Replacement); previous = edit.Start;
            }
            file.Edits.Clear(); file.Tree = CSharpSyntaxTree.ParseText(file.Current, options, file.Path);
        }
    }
    static SyntaxNode? Enclosing(SyntaxNode node) => node.AncestorsAndSelf().FirstOrDefault(n => n is MethodDeclarationSyntax or ConstructorDeclarationSyntax or PropertyDeclarationSyntax or AccessorDeclarationSyntax);
    static string HelperKey(string path, string method) => path + "::" + method;
    static bool IsInvalidOperation(ObjectCreationExpressionSyntax exception) => exception.Type.ToString() is "InvalidOperationException" or "System.InvalidOperationException" or "global::System.InvalidOperationException";
    static string MethodName(SyntaxNode? n) => n switch { MethodDeclarationSyntax m => m.Identifier.ValueText, ConstructorDeclarationSyntax c => c.Identifier.ValueText, PropertyDeclarationSyntax p => p.Identifier.ValueText, _ => "accessor/initializer" };
    static string? BoundaryReason(FileState file, SyntaxNode node, SyntaxNode? method)
    {
        if (Boundary.IsMatch(file.Path) || Boundary.IsMatch(MethodName(method))) return "restore-replay-loader-projection-boundary";
        if (method is null) return "initializer-or-unknown-entry";
        var ancestors = node.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().ToArray();
        if (ancestors.Any(a => a.AttributeLists.SelectMany(l => l.Attributes).Any(at => Input.IsMatch(at.ToString())))) return "external-input-qualification-or-definition-registration";
        if (method is MethodDeclarationSyntax m)
        {
            var parameters = m.ParameterList.Parameters.ToString();
            if (Regex.IsMatch(parameters, @"\b(?:JsonElement|Utf8JsonReader|SkillProgramDefinition|SkillProgramActivation|SkillProgramTrigger|PromptChoice|GameCommand|PlayerCommand|CommandRequest|PendingDecision)\b") ||
                m.ExplicitInterfaceSpecifier?.ToString().Contains("Validation", StringComparison.OrdinalIgnoreCase) == true ||
                Regex.IsMatch(m.Identifier.ValueText, "^(?:Parse|Validate|Compile|Resources|CheckDefinition)") && Regex.IsMatch(parameters, @"\b(?:SkillProgram|path|definition|JsonElement)\b")) return "external-input-or-definition-validation";
            if (Regex.IsMatch(m.Identifier.ValueText, "^(?:Answer|HandleCommand|ValidateCommand|ApplyCommand|Submit|CommitCommand)")) return "external-command-or-answer-entry";
        }
        return null;
    }
    static bool Escapes(string name, IfStatementSyntax guard, SyntaxNode? scope) => scope is not null && scope.DescendantNodes().OfType<IdentifierNameSyntax>().Any(n => n.SpanStart > guard.Span.End && n.Identifier.ValueText == name);
    static bool HasOtherReference(IEnumerable<FileState> files, FileState owner, MethodDeclarationSyntax method, string name) => files.Any(f => f.Tree.GetRoot().DescendantTokens().Any(t =>
        (t.ValueText == name || t.IsKind(SyntaxKind.StringLiteralToken) && t.ValueText.Contains(name, StringComparison.Ordinal)) &&
        !(f == owner && method.FullSpan.Contains(t.Span))));
    static bool IdentifierOccurs(string text, string name) => Regex.IsMatch(text, @"\b" + Regex.Escape(name) + @"\b");
    static string CallName(InvocationExpressionSyntax call) => call.Expression switch { MemberAccessExpressionSyntax m => m.Name.Identifier.ValueText, SimpleNameSyntax s => s.Identifier.ValueText, _ => "" };
    static bool IsHistory(InvocationExpressionSyntax call) => CallName(call) is "ProgramEventHistory" or "CompleteProgramEventHistory";
    static ExpressionSyntax HistoryPipeline(ExpressionSyntax expression)
    {
        while (expression.Parent is MemberAccessExpressionSyntax member && member.Expression == expression)
            expression = member.Parent is InvocationExpressionSyntax call ? call : member;
        return expression;
    }
    static int Line(SyntaxNode n) => n.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
    static int Lines(SyntaxNode n) => n.GetLocation().GetLineSpan() is var span ? span.EndLinePosition.Line - span.StartLinePosition.Line + 1 : 0;
    static int TextLines(string text) => text.Length == 0 ? 0 : SourceText.From(text).Lines.Count - (text.EndsWith('\n') || text.EndsWith('\r') ? 1 : 0);
    static string Preview(string text) { var compact = Regex.Replace(text, @"\s+", " "); return compact[..Math.Min(compact.Length, 800)]; }
    static Edit RemoveStatement(string text, StatementSyntax statement, string kind) => statement.Parent is BlockSyntax or SwitchSectionSyntax ? RemoveMember(text, statement, kind) : new(statement.SpanStart, statement.Span.Length, ";", kind, Line(statement));
    static Edit RemoveMember(string text, SyntaxNode node, string kind)
    {
        var start = node.SpanStart; var end = node.Span.End;
        var lineStart = text.LastIndexOf('\n', Math.Max(0, start - 1)) + 1;
        var lineEnd = text.IndexOf('\n', end); if (lineEnd < 0) lineEnd = text.Length;
        // Preserve adjacent comments/trivia and compressed neighboring statements.
        if (string.IsNullOrWhiteSpace(text[lineStart..start]) && string.IsNullOrWhiteSpace(text[end..lineEnd])) { start = lineStart; end = Math.Min(lineEnd + 1, text.Length); }
        return new(start, end - start, "", kind, Line(node));
    }
    static string? Arg(string[] args, string name) { var at = Array.IndexOf(args, name); return at >= 0 && at + 1 < args.Length ? args[at + 1] : null; }
    static HashSet<string> ReadVersions(string path, string root)
    {
        var text = System.IO.File.ReadAllText(path);
        var lines = text.TrimStart().StartsWith('[') ? JsonSerializer.Deserialize<string[]>(text)! : text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        return lines.Where(s => !s.TrimStart().StartsWith('#')).Select(s =>
        {
            var normalized = s.Trim().Replace('\\', '/').TrimEnd('/');
            if (Path.IsPathRooted(normalized)) normalized = Path.GetRelativePath(root, normalized).Replace('\\', '/');
            if (normalized.EndsWith("/Runtime", StringComparison.OrdinalIgnoreCase)) normalized = normalized[..^8];
            return normalized;
        }).Where(s => s.Length > 0).ToHashSet(StringComparer.OrdinalIgnoreCase);
    }
}
