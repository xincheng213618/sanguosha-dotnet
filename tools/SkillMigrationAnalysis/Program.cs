using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var root = Path.GetFullPath(Option("--root") ?? FindRoot());
var historyMode = args.Contains("--history");
var invariantMode = args.Contains("--invariant-cleanup") || args.Contains("--invariant-metrics");
var output = Path.GetFullPath(Option("--out") ?? Path.Combine(root, ".artifacts", "benchmarks", invariantMode ? "character-invariants" : historyMode ? "step-receipts" : "skill-patterns"));
Directory.CreateDirectory(output);
var timer = Stopwatch.StartNew();
var json = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp12,
    preprocessorSymbols: ["DEBUG", "TRACE", "NET", "NET8_0", "NET8_0_OR_GREATER", "NETCOREAPP"]);
var characterRoot = Path.Combine(root, "src", "Characters");
var characterPaths = Directory.EnumerateFiles(characterRoot, "*.cs", SearchOption.AllDirectories)
    .Where(p => p.Split(Path.DirectorySeparatorChar).Contains("Runtime") && !IsBuildPath(p)).Order().ToArray();
var paths = args.Contains("--invariant-metrics") ? characterPaths : Directory.EnumerateFiles(Path.Combine(root, "src", "CardGame.Core"), "*.cs", SearchOption.AllDirectories)
    .Where(p => !IsBuildPath(p)).Concat(characterPaths).Order().ToArray();
Console.WriteLine($"Parsing {paths.Length} source-context files...");
var trees = paths.AsParallel().AsOrdered().Select(p => CSharpSyntaxTree.ParseText(File.ReadAllText(p), parseOptions, p, Encoding.UTF8)).ToArray();
if (args.Contains("--invariant-metrics")) { InvariantCleanupAnalysis.WriteMetrics(root, output, trees, json); return; }
if (invariantMode)
{
    InvariantCleanupAnalysis.Run(root, output, args, trees, CreateCompilation(trees, parseOptions), parseOptions, json);
    return;
}
if (historyMode)
{
    HistoryQueryAnalysis.Run(root, output, trees, CreateCompilation(trees, parseOptions), json);
    return;
}
if (args.Contains("--frame-invocations"))
{
    FrameInvocationAnalysis.Run(root, output, trees, CreateCompilation(trees, parseOptions), json);
    return;
}
var methods = trees.Where(t => t.FilePath.StartsWith(characterRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) &&
        !new[] { "Contracts", "DependencyAdapters", "StateContracts", "NativeOwners" }.Any(s => t.FilePath.Contains(s, StringComparison.OrdinalIgnoreCase)))
    .SelectMany(tree => tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
        .Where(m => m.Body is not null || m.ExpressionBody is not null).Select(m => new MethodInfo(root, m)))
    .OrderBy(m => m.File, StringComparer.Ordinal).ThenBy(m => m.StartLine).ToArray();
Write("methods-syntax.json", methods.Select(m => new
{
    m.Id, m.File, m.StartLine, m.BodyStartLine, m.BodyLines, m.MethodLines, m.Name, m.CharacterVersion,
    m.Attributes, m.Body, rawInvocations = m.Node.DescendantNodes().OfType<InvocationExpressionSyntax>().Select(i => i.Expression.ToString()).ToArray()
}));
Console.WriteLine($"Syntax report ready: {methods.Length} methods in {timer.Elapsed.TotalSeconds:F1}s.");
if (args.Contains("--syntax-only")) return;

var compilation = CreateCompilation(trees, parseOptions);
var models = new ConcurrentDictionary<SyntaxTree, SemanticModel>();
SemanticModel Model(SyntaxTree t) => models.GetOrAdd(t, t => compilation.GetSemanticModel(t));
var bySymbol = new Dictionary<ISymbol, MethodInfo>(SymbolEqualityComparer.Default);
foreach (var m in methods)
{
    m.Symbol = Model(m.Node.SyntaxTree).GetDeclaredSymbol(m.Node)!;
    bySymbol[m.Symbol.OriginalDefinition] = m;
}
var wrapperTargets = new Dictionary<ISymbol, IMethodSymbol>(SymbolEqualityComparer.Default);
IMethodSymbol Unwrap(IMethodSymbol symbol)
{
    symbol = (symbol.ReducedFrom ?? symbol).OriginalDefinition;
    if (symbol.ContainingNamespace.ToDisplayString() != "CardGame.Core.RuntimeEntries") return symbol;
    if (wrapperTargets.TryGetValue(symbol, out var saved)) return saved;
    var declaration = symbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as MethodDeclarationSyntax;
    if (declaration?.ExpressionBody?.Expression is InvocationExpressionSyntax forwarding &&
        Model(forwarding.SyntaxTree).GetSymbolInfo(forwarding).Symbol is IMethodSymbol target)
        return wrapperTargets[symbol] = (target.ReducedFrom ?? target).OriginalDefinition;
    return symbol;
}
var unresolvedCalls = 0;
foreach (var m in methods)
{
    foreach (var invocation in m.Node.DescendantNodes().OfType<InvocationExpressionSyntax>())
    {
        var bound = Model(invocation.SyntaxTree).GetSymbolInfo(invocation).Symbol as IMethodSymbol;
        if (bound is null) unresolvedCalls++;
        var target = bound is null ? null : Unwrap(bound);
        MethodInfo? helper = target is not null && bySymbol.TryGetValue(target, out var h) ? h : null;
        var identity = target is null ? "unresolved:" + MethodInfo.Scrub(invocation.Expression.ToString(), m.CharacterName) : CanonicalCall(target);
        m.Calls.Add(new(invocation, identity, ClassifyCall(target), helper));
    }
}
// Propagate known writes through analyzed character helpers. A method named PayFoo
// that only reads history is still a query; a short wrapper around a real mutation is not.
var callers = methods.ToDictionary(m => m, _ => new List<MethodInfo>());
foreach (var m in methods)
    foreach (var helper in m.Calls.Where(c => c.Helper is not null).Select(c => c.Helper!).Distinct()) callers[helper].Add(m);
var actions = new Queue<MethodInfo>();
foreach (var m in methods)
{
    m.HasAction = m.BodyNode.DescendantNodes().OfType<AssignmentExpressionSyntax>().Any(a =>
        a.Left is MemberAccessExpressionSyntax or ElementAccessExpressionSyntax && a.Parent is not InitializerExpressionSyntax) ||
        m.Calls.Any(c => c.Helper is null && c.Category == "action");
    if (m.HasAction) actions.Enqueue(m);
}
while (actions.TryDequeue(out var action))
    foreach (var caller in callers[action].Where(m => !m.HasAction)) { caller.HasAction = true; actions.Enqueue(caller); }
foreach (var m in methods)
{
    foreach (var call in m.Calls.Where(c => c.Helper is not null)) call.Category = call.Helper!.HasAction ? "action" : "query";
    m.Role = Role(m);
}
Console.WriteLine($"Call symbols resolved; {unresolvedCalls} unresolved calls. Normalizing...");
// A helper's own operators/member reads must survive exact grouping. Merely sharing
// its call list would conflate e.g. Hp > 0 with Hp == 0 through another partial.
foreach (var m in methods)
    foreach (var call in m.Calls)
        call.Normalized = call.Helper is null ? call.Identity : "local-helper:" + MethodInfo.Scrub(call.Identity, call.Helper.CharacterName);
foreach (var m in methods) m.HelperBodySignature = Hash(new BodyRenderer(m).Render(m.BodyNode));
foreach (var m in methods)
{
    foreach (var call in m.Calls)
    {
        call.CoarseNormalized = call.Helper is null ? call.Identity : "character-helper[" + call.Helper.Role + ";" +
            TypeShape(call.Helper.Symbol.ReturnType) + ";control=" + Hash(call.Helper.Control) + ";calls=" +
            Hash(string.Join(",", call.Helper.Calls.Select(c => c.Helper is null ? c.Identity : "nested-helper:" + MethodInfo.Scrub(c.Identity, c.Helper.CharacterName)))) + "]";
        call.Normalized = call.Helper is null ? call.Identity : call.CoarseNormalized + ";body=" + call.Helper.HelperBodySignature;
    }
    m.NormalizedCalls = m.Calls.Select(c => c.Normalized).ToArray();
    m.OperationCalls = m.Calls.Where(c => c.Category is "action" or "query").Select(c => c.Normalized).ToArray();
    var renderer = new BodyRenderer(m);
    m.NormalizedBody = renderer.Render(m.BodyNode);
    m.ExactSignature = Hash(m.Role + "|" + TypeShape(m.Symbol.ReturnType) + "|" + m.NormalizedBody);
    m.AccessedMembers = m.BodyNode.DescendantNodes().OfType<MemberAccessExpressionSyntax>()
        .Where(n => n.Parent is not InvocationExpressionSyntax { Expression: var e } || e != n)
        .Select(n => MethodInfo.Scrub(n.Name.Identifier.ValueText, m.CharacterName)).Distinct().Order().ToArray();
    // Coarse groups preserve operations and topology, but deliberately ignore leaf operands/constants.
    // They are review candidates; neither key establishes behavioral equivalence.
    m.CoarseSignature = Hash(m.Role + "|" + TypeShape(m.Symbol.ReturnType) + "|" + m.Control + "|" +
        string.Join("\n", m.Calls.Select(c => c.CoarseNormalized)) + "|" + string.Join(",", m.AccessedMembers));
    m.BehaviorFamilies = Families(m);
    m.ControlClasses = ControlClasses(m);
}
Write("methods.json", methods.Select(m => new
{
    m.Id, m.File, m.StartLine, m.BodyStartLine, m.BodyLines, m.MethodLines, m.Name, m.CharacterVersion, m.Role,
    returnType = m.Symbol.ReturnType.ToDisplayString(), parameters = m.Symbol.Parameters.Select(p => new { p.Ordinal, p.RefKind, type = p.Type.ToDisplayString() }),
    m.Attributes, m.Body, m.ExactSignature, m.CoarseSignature, m.Control, m.AccessedMembers, m.BehaviorFamilies, m.ControlClasses,
    normalizedCalls = m.Calls.Select(c => new { operation = c.Normalized, coarseOperation = c.CoarseNormalized, c.Category, original = c.Identity, helper = c.Helper?.Id }),
    normalizedBodyPreview = m.NormalizedBody[..Math.Min(1800, m.NormalizedBody.Length)]
}));
var exact = Patterns(methods, false).ToArray();
var coarse = Patterns(methods, true).ToArray();
var actionable = Patterns(methods.Where(m => m.Role is "execution" or "eligibility"), true).ToArray();
var behaviorRanges = methods.Where(m => m.Role is "execution" or "eligibility")
    .GroupBy(m => m.Role + "|" + string.Join(",", m.BehaviorFamilies) + "|" + string.Join(",", m.ControlClasses))
    .Select(g => new BehaviorPatternInfo(Hash(g.Key), g.Count(), g.Sum(m => m.BodyLines), g.Sum(m => m.MethodLines),
        g.Select(m => m.CharacterVersion).Distinct().Count(), g.First().Role, g.Select(m => m.ExactSignature).Distinct().Count(),
        g.First().BehaviorFamilies, g.First().ControlClasses,
        g.Take(3).Select(m => new Example(m.File, m.StartLine, m.Name, m.BodyLines)).ToArray(), g.Select(m => m.Id).ToArray()))
    .OrderByDescending(g => g.Count).ThenByDescending(g => g.BodyLines).ToArray();
var behaviorFamilies = behaviorRanges.Where(g => !g.Families.Contains("character-helper-flow") && !g.Families.Contains("predicate-or-data-construction")).ToArray();
var unclassifiedRanges = behaviorRanges.Where(g => g.Families.Contains("character-helper-flow") || g.Families.Contains("predicate-or-data-construction")).ToArray();
Write("behavior-families.json", new { classified = behaviorFamilies, unclassified = unclassifiedRanges });
Write("top25patterns.json", new
{
    exactRule = "Normalized complete body syntax; ordered shared callee identities, operators and literal values retained. Local/parameter names alpha-renamed; character names and skill identifiers normalized; message text ignored.",
    coarseRule = "Same role, return category, ordered callee identities (character helpers summarized by one-level shared calls/control), control topology/operators and accessed member names. Leaf operands and literal values may differ. Review candidates only.",
    behaviorFamilyRule = "Same execution/eligibility role, named action/query families and major control classes/registered boundary. Guard count, call count, literal values and argument bindings may differ. These broad non-equivalent candidate ranges require source review; full calls/control remain in methods.json.",
    exact = exact.Take(25), coarse = coarse.Take(25), executionAndEligibility = actionable.Take(25), behaviorFamilies = behaviorFamilies.Take(25), unclassifiedRanges = unclassifiedRanges.Take(10)
});
Write("summary.json", new
{
    generatedUtc = DateTime.UtcNow, sourceContextFiles = paths.Length, analyzedFiles = methods.Select(m => m.File).Distinct().Count(),
    methods = methods.Length, bodyLines = methods.Sum(m => m.BodyLines), unresolvedCalls,
    roles = methods.GroupBy(m => m.Role).ToDictionary(g => g.Key, g => new { methods = g.Count(), bodyLines = g.Sum(m => m.BodyLines) }),
    exactPatternCount = exact.Length, coarsePatternCount = coarse.Length, behaviorFamilyCount = behaviorFamilies.Length,
    unclassifiedBehaviorMethods = unclassifiedRanges.Sum(g => g.Count), repeatedExactPatterns = exact.Count(p => p.Count > 1), repeatedCoarsePatterns = coarse.Count(p => p.Count > 1),
    exclusions = new[] { "Contracts", "DependencyAdapters", "StateContracts", "NativeOwners" },
    limitations = new[] { "Patterns identify candidates, not equivalent behavior.", "Role/query/action labels are heuristics; original call identities and source bodies remain available.", "Character helper expansion is one level; frames, historical facts, ordering, costs and event identities need review.", "DEBUG C#12 compilation supplies symbols; no business assembly or full verification is built/run." },
    elapsedSeconds = timer.Elapsed.TotalSeconds
});
Console.WriteLine($"Reports ready: {output}; {methods.Length} methods, {coarse.Count(p => p.Count > 1)} repeated coarse patterns, {timer.Elapsed.TotalSeconds:F1}s.");

void Write(string name, object value) => File.WriteAllText(Path.Combine(output, name), JsonSerializer.Serialize(value, json), new UTF8Encoding(false));
string? Option(string name) { var i = Array.IndexOf(args, name); return i >= 0 && i + 1 < args.Length ? args[i + 1] : null; }
static string FindRoot()
{
    for (var d = new DirectoryInfo(Directory.GetCurrentDirectory()); d is not null; d = d.Parent)
        if (Directory.Exists(Path.Combine(d.FullName, "src", "Characters"))) return d.FullName;
    throw new InvalidOperationException("Run inside the Card checkout or pass --root <checkout>.");
}
bool IsBuildPath(string p) => Path.GetRelativePath(root, p).Split(Path.DirectorySeparatorChar).Any(x => x is "bin" or "obj" or ".artifacts");
static CSharpCompilation CreateCompilation(SyntaxTree[] trees, CSharpParseOptions options)
{
    var refs = Directory.GetDirectories(@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref", "8.*")
        .OrderByDescending(p => Version.Parse(Path.GetFileName(p))).First();
    return CSharpCompilation.Create("CardGame.Core", trees.Append(CSharpSyntaxTree.ParseText(
        "global using System;global using System.Collections.Generic;global using System.IO;global using System.Linq;global using System.Net.Http;global using System.Threading;global using System.Threading.Tasks;", options)),
        Directory.GetFiles(Path.Combine(refs, "ref", "net8.0"), "*.dll").Select(p => MetadataReference.CreateFromFile(p)),
        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
}
static string Hash(string text) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text)))[..20].ToLowerInvariant();
static string TypeShape(ITypeSymbol type) => type.SpecialType != SpecialType.None ? type.SpecialType.ToString() : type.TypeKind switch
{ TypeKind.Enum => "enum", TypeKind.Array => "array", TypeKind.Delegate => "delegate", TypeKind.Interface => "interface:" + type.Name, _ => type.IsValueType ? "value:" + type.Name : "reference:" + type.Name };
static string CanonicalCall(IMethodSymbol target)
{
    var owner = target.ContainingType;
    var kind = owner.ContainingNamespace.ToDisplayString();
    // Narrow kernel contracts all describe the same named engine entry. Keep overload parameter shapes.
    var identity = owner.Name.StartsWith("IKernel", StringComparison.Ordinal) ? "kernel." + target.Name : owner.ToDisplayString() + "." + target.Name;
    return identity + (target.Arity == 0 ? "" : "`" + target.Arity) + "(" + string.Join(",", target.Parameters.Select(p => p.RefKind + ":" + TypeShape(p.Type))) + ")";
}
static string ClassifyCall(IMethodSymbol? target)
{
    if (target is null) return "unresolved";
    var space = target.ContainingNamespace.ToDisplayString();
    if (!space.StartsWith("CardGame.Core", StringComparison.Ordinal)) return "library";
    if (Regex.IsMatch(target.Name, "^(Get|Has|Is|Can|Capture|Build|Find|Count|Matches|Valid|Qualify)") || target.Name.Contains("EventHistory", StringComparison.Ordinal)) return "query";
    if (Regex.IsMatch(target.Name, "^(Try)?(Draw|Move|Damage|Recover|Lose|Grant|Revoke|Change|Set|Issue|Publish|Advance|Replace|Push|Complete|Discard|Give|Obtain|Apply|Execute|Resolve|Begin|Continue|Answer|Record|Queue|Clear|Remove|Add|Consume|Pay|Resume|Drain|Finish|Return|Start|Offer|Update|Freeze|Request).*")) return "action";
    return "query";
}
static string Role(MethodInfo m)
{
    var name = m.Name;
    if (Regex.IsMatch(name, "^(Parse|Resources|Compile|Validate)") ||
        m.Attributes.Any(a => a.Contains("CatalogValidation", StringComparison.Ordinal) || a.Contains("Definition", StringComparison.Ordinal))) return "definition-validation";
    if (m.Attributes.Any(a => a.Contains("Legality", StringComparison.Ordinal) || a.Contains("TriggerProof", StringComparison.Ordinal) || a.Contains("SelectionEligibility", StringComparison.Ordinal) || a.Contains("DistanceRule", StringComparison.Ordinal))) return "eligibility";
    bool action = m.HasAction;
    bool registered = m.Attributes.Any(a => Regex.IsMatch(a, "Program(Answer|Continuation|RuleObserver|RuntimeHook|DamageHook|Operation)"));
    if (!action && (name.EndsWith("Source", StringComparison.Ordinal) || name is "Source" or "Name" or "Human" ||
        m.Calls.Any(c => c.Identity.Contains(".GetProgramBindingId(", StringComparison.Ordinal)))) return "source-metadata";
    if (!action && (Regex.IsMatch(name, "^(Choose|Ai|AI|Evaluate|KeepValue)") ||
        m.Symbol.ReturnType.Name == "PromptChoice" && m.Calls.Count == 1 && m.Calls[0].Identity.Contains("Enumerable.First", StringComparison.Ordinal))) return "strategy";
    if (name == "AllowsDeadTarget") return m.Calls.Count == 1 && m.Calls[0].Identity.Contains(".CanContinueAfterOwnerDeath(", StringComparison.Ordinal) ? "bridge" : "eligibility";
    if (!registered && m.Symbol.ContainingType.AllInterfaces.Any(i => i.Name == "ISkillProgramEffectHandler") && name == "Execute" &&
        (m.Node.ExpressionBody is not null || m.Node.Body?.Statements.Count == 1)) return "dispatch";
    if (m.Node.ExplicitInterfaceSpecifier is not null && m.Calls.Count == 1 &&
        (m.Node.ExpressionBody is not null || m.Node.Body?.Statements.Count == 1)) return "dispatch";
    if (m.Symbol.ReturnType.SpecialType == SpecialType.System_Boolean && !action) return "eligibility";
    if (!action && !registered && (m.Node.ExpressionBody?.Expression is InvocationExpressionSyntax or MemberAccessExpressionSyntax or LiteralExpressionSyntax ||
        m.Node.Body?.Statements is [ReturnStatementSyntax { Expression: InvocationExpressionSyntax or MemberAccessExpressionSyntax or LiteralExpressionSyntax }])) return "bridge";
    return "execution";
}
static string[] Families(MethodInfo m)
{
    var result = new HashSet<string>(StringComparer.Ordinal);
    var callInfos = m.Calls.Concat(m.Calls.Where(c => c.Helper is not null).SelectMany(c => c.Helper!.Calls)).ToArray();
    var calls = callInfos.Select(c => c.Identity).ToArray();
    foreach (var info in callInfos)
    {
        var call = info.Identity;
        var mutates = info.Category == "action";
        var end = call.IndexOf('(');
        var name = (end < 0 ? call : call[..end]).Split('.').Last();
        if (Regex.IsMatch(name, "Draw")) result.Add(mutates ? "draw" : "draw-query");
        if (Regex.IsMatch(name, "(Move|Discard|Give|Obtain|Take|CardMovement)")) result.Add(mutates ? "card-movement" : "card-movement-query");
        if (Regex.IsMatch(name, "(Recover|LoseHp|ChangeMaximumHp|HealthChange)")) result.Add(mutates ? "health-change" : "health-query");
        if (Regex.IsMatch(name, "(Damage|AttackAmount)")) result.Add(mutates ? "damage" : "damage-query");
        if (Regex.IsMatch(name, "(UseVirtual|UseBound|Commit.*Use|Begin.*Slash|Request.*Slash|UseSelectedCardsAs)")) result.Add("card-use");
        if (Regex.IsMatch(name, "(GetHand|HandCount|Equipment|OwnedCard|CardZone)")) result.Add("owned-card-query");
        if (Regex.IsMatch(name, "(HasRuntimeSkill|HasSkill|SkillBindingShard|EnabledSkill)")) result.Add("skill-presence-query");
        if (Regex.IsMatch(name, "(EventHistory|CompleteProgramEventHistory|SinceLastBoundary|History)")) result.Add("history-query");
        if (Regex.IsMatch(name, "(CombatDistance|AttackRange|EvaluateDistance)")) result.Add("distance-query");
        if (Regex.IsMatch(name, "(BooleanState|ParticipantMarker|MarkerCount|SkillChange|GrantSkills|RevokeSkills)")) result.Add(mutates ? "persistent-rule-state" : "persistent-rule-query");
        if (Regex.IsMatch(name, "(Random|NextInt|NextInclusive|Shuffle)")) result.Add("random");
        if (Regex.IsMatch(name, "(TryBeginProgramChild|TryBeginQueued|TryBeginCharacterState|TryBeginAdvancedSkillsChanged|TryBegin.*Window)")) result.Add("child-windows");
        if (Regex.IsMatch(name, "(FinishProgramSkill|CompleteProgramBinding|CompleteProgram)")) result.Add("program-completion");
        if (Regex.IsMatch(name, "(GetActiveProgramFrame|GetPausedProgramFrame|Find.*Frame|Require.*Frame|RuntimeTop|Push.*Frame|Replace.*Frame|PopResolutionFrame)")) result.Add("frame-cursor");
        if (Regex.IsMatch(name, "(AdvanceRulesAndPublishState|PublishState)")) result.Add("state-publication");
        if (Regex.IsMatch(name, "(QueueFact|QueueGameEvent|AppendPendingProgramEvent)")) result.Add("event-publication");
        if (Regex.IsMatch(name, "(Target|Select)") && !Regex.IsMatch(name, "(Damage|Proof|OwnerTarget|EventTarget|Enumerable)")) result.Add("target-or-card-selection");
    }
    if (m.AccessedMembers.Any(n => n is "Hp" or "MaxHp" or "HasHpValue" or "IsAlive" or "IsFaceDown" or "IsChained")) result.Add("public-player-state");
    if (m.Body.Contains("new PromptChoice", StringComparison.Ordinal) || m.Body.Contains("new PendingDecision", StringComparison.Ordinal) ||
        m.Symbol.ReturnType.ToDisplayString().Contains("PromptChoice", StringComparison.Ordinal) &&
        m.BodyNode.DescendantNodes().Any(n => n is ObjectCreationExpressionSyntax or ImplicitObjectCreationExpressionSyntax)) result.Add("choice-construction");
    if (m.BodyNode.DescendantNodes().OfType<AssignmentExpressionSyntax>().Any(a => a.Left.ToString().Contains("_pendingDecision", StringComparison.Ordinal))) result.Add("choice-publication");
    if (m.AccessedMembers.Any(n => n.Contains("Paid", StringComparison.Ordinal) || n.Contains("Payment", StringComparison.Ordinal))) result.Add("payment-state");
    if (result.Contains("history-query") && calls.Any(c => Regex.IsMatch(c, "Enumerable.(Count|Sum|Max|Min|Any)"))) result.Add("history-aggregation");
    if (result.Count == 0) result.Add(m.Calls.Any(c => c.Helper is not null) ? "character-helper-flow" : "predicate-or-data-construction");
    return result.Order().ToArray();
}
static string[] ControlClasses(MethodInfo m)
{
    var nodes = m.BodyNode.DescendantNodesAndSelf().ToArray();
    var result = new List<string>();
    if (nodes.Any(n => n is IfStatementSyntax or ConditionalExpressionSyntax)) result.Add("guarded-or-conditional");
    if (nodes.Any(n => n is ForEachStatementSyntax or ForEachVariableStatementSyntax)) result.Add("for-each");
    if (nodes.Any(n => n is ForStatementSyntax or WhileStatementSyntax or DoStatementSyntax)) result.Add("iteration");
    if (nodes.Any(n => n is SwitchStatementSyntax or SwitchExpressionSyntax)) result.Add("switch");
    if (nodes.Any(n => n is ThrowStatementSyntax or ThrowExpressionSyntax)) result.Add("throws");
    foreach (var (attribute, tag) in new[] { ("ProgramAnswer", "answer-boundary"), ("ProgramContinuation", "continuation-boundary"), ("ProgramRuleObserver", "observer-boundary"), ("ProgramTriggerLegality", "qualification-boundary"), ("ProgramTriggerProof", "proof-boundary"), ("ProgramRuntimeHook", "runtime-hook"), ("ProgramDamageHook", "damage-hook") })
        if (m.Attributes.Any(a => a.Contains(attribute, StringComparison.Ordinal))) result.Add(tag);
    if (result.Count == 0) result.Add("straight-line");
    return result.ToArray();
}
static IEnumerable<PatternInfo> Patterns(IEnumerable<MethodInfo> methods, bool coarse) => methods
    .GroupBy(m => coarse ? m.CoarseSignature : m.ExactSignature)
    .Select(g => new PatternInfo(g.Key, g.Count(), g.Sum(m => m.BodyLines), g.Sum(m => m.MethodLines),
        g.Select(m => m.CharacterVersion).Distinct().Count(), g.First().Role, g.Select(m => m.ExactSignature).Distinct().Count(),
        g.First().Calls.Select(c => coarse ? c.CoarseNormalized : c.Normalized).ToArray(), g.First().Control, g.First().AccessedMembers,
        g.Take(3).Select(m => new Example(m.File, m.StartLine, m.Name, m.BodyLines)).ToArray(), g.Select(m => m.Id).ToArray()))
    .OrderByDescending(g => g.Count).ThenByDescending(g => g.BodyLines).ThenBy(g => g.Signature, StringComparer.Ordinal);

sealed class MethodInfo
{
    public MethodInfo(string root, MethodDeclarationSyntax node)
    {
        Node = node; BodyNode = (SyntaxNode?)node.Body ?? node.ExpressionBody!.Expression;
        File = Path.GetRelativePath(root, node.SyntaxTree.FilePath).Replace('\\', '/');
        Name = node.Identifier.ValueText;
        var parts = File.Split('/'); CharacterName = parts[2]; CharacterVersion = parts[2] + "/" + parts[3];
        StartLine = node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line + 1;
        BodyStartLine = node.SyntaxTree.GetLineSpan(BodyNode.Span).StartLinePosition.Line + 1;
        BodyLines = node.SyntaxTree.GetLineSpan(TextSpan(BodyNode)).EndLinePosition.Line - BodyStartLine + 2;
        MethodLines = node.SyntaxTree.GetLineSpan(TextSpan(node)).EndLinePosition.Line - StartLine + 2;
        Id = File + ":" + StartLine + ":" + Name; Body = BodyNode.ToString();
        Attributes = node.AttributeLists.SelectMany(a => a.Attributes).Select(a => a.ToString()).ToArray();
        Control = string.Join(" ", BodyNode.DescendantNodesAndSelf().Where(IsControl).Select(n =>
            n.Ancestors().TakeWhile(a => a != BodyNode.Parent).Count(IsControl) + ":" + n.Kind() + (n is IfStatementSyntax i ? i.Else is null ? ":no-else" : ":else" : "")));
    }
    static Microsoft.CodeAnalysis.Text.TextSpan TextSpan(SyntaxNode n) => Microsoft.CodeAnalysis.Text.TextSpan.FromBounds(n.SpanStart, Math.Max(n.SpanStart, n.Span.End - 1));
    static bool IsControl(SyntaxNode n) => n is IfStatementSyntax or ElseClauseSyntax or SwitchStatementSyntax or SwitchExpressionSyntax or SwitchSectionSyntax or SwitchExpressionArmSyntax or
        ForStatementSyntax or ForEachStatementSyntax or ForEachVariableStatementSyntax or WhileStatementSyntax or DoStatementSyntax or TryStatementSyntax or CatchClauseSyntax or FinallyClauseSyntax or
        ReturnStatementSyntax or ThrowStatementSyntax or ThrowExpressionSyntax or BreakStatementSyntax or ContinueStatementSyntax or GotoStatementSyntax or YieldStatementSyntax or ConditionalExpressionSyntax or
        AwaitExpressionSyntax or LambdaExpressionSyntax or LocalFunctionStatementSyntax or AssignmentExpressionSyntax or BinaryExpressionSyntax or PrefixUnaryExpressionSyntax or PostfixUnaryExpressionSyntax ||
        n.IsKind(SyntaxKind.LogicalAndExpression) || n.IsKind(SyntaxKind.LogicalOrExpression) || n.IsKind(SyntaxKind.CoalesceExpression) || n.IsKind(SyntaxKind.LogicalNotExpression);
    public static string Scrub(string text, string character)
    {
        var pascal = string.Concat(character.Split('-').Select(s => s.Length == 0 ? "" : char.ToUpperInvariant(s[0]) + s[1..]));
        return text.Replace(pascal, "Character", StringComparison.OrdinalIgnoreCase).Replace(character, "character", StringComparison.OrdinalIgnoreCase);
    }
    public MethodDeclarationSyntax Node { get; }
    public SyntaxNode BodyNode { get; }
    public string Id { get; }
    public string File { get; }
    public string Name { get; }
    public string CharacterName { get; }
    public string CharacterVersion { get; }
    public int StartLine { get; }
    public int BodyStartLine { get; }
    public int BodyLines { get; }
    public int MethodLines { get; }
    public string Body { get; }
    public string[] Attributes { get; }
    public string Control { get; }
    public IMethodSymbol Symbol { get; set; } = null!;
    public List<CallInfo> Calls { get; } = [];
    public string Role { get; set; } = "";
    public bool HasAction { get; set; }
    public string[] NormalizedCalls { get; set; } = [];
    public string[] OperationCalls { get; set; } = [];
    public string[] AccessedMembers { get; set; } = [];
    public string[] BehaviorFamilies { get; set; } = [];
    public string[] ControlClasses { get; set; } = [];
    public string NormalizedBody { get; set; } = "";
    public string ExactSignature { get; set; } = "";
    public string CoarseSignature { get; set; } = "";
    public string HelperBodySignature { get; set; } = "";
}
sealed class CallInfo(InvocationExpressionSyntax node, string identity, string category, MethodInfo? helper)
{
    public InvocationExpressionSyntax Node { get; } = node;
    public string Identity { get; } = identity;
    public string Category { get; set; } = category;
    public MethodInfo? Helper { get; } = helper;
    public string Normalized { get; set; } = "";
    public string CoarseNormalized { get; set; } = "";
}
sealed class BodyRenderer
{
    readonly MethodInfo method;
    readonly Dictionary<string, string> variables = new(StringComparer.Ordinal);
    readonly Dictionary<(int Start, int Length), CallInfo> calls;
    public BodyRenderer(MethodInfo method)
    {
        this.method = method; calls = method.Calls.ToDictionary(c => (c.Node.SpanStart, c.Node.Span.Length));
        foreach (var p in method.Node.ParameterList.Parameters) variables.TryAdd(p.Identifier.ValueText, "p" + variables.Count);
        foreach (var n in method.BodyNode.DescendantNodes())
        {
            var name = n switch
            { VariableDeclaratorSyntax v => v.Identifier.ValueText, SingleVariableDesignationSyntax v => v.Identifier.ValueText, ParameterSyntax p => p.Identifier.ValueText, ForEachStatementSyntax f => f.Identifier.ValueText, CatchDeclarationSyntax c => c.Identifier.ValueText, _ => null };
            if (name is not null) variables.TryAdd(name, "v" + variables.Count);
        }
    }
    public string Render(SyntaxNode node)
    {
        if (node is InvocationExpressionSyntax invocation && calls.TryGetValue((node.SpanStart, node.Span.Length), out var call))
        {
            var receiver = invocation.Expression is MemberAccessExpressionSyntax access ? access.Expression : null;
            var receiverText = receiver is null ? "" : Render(receiver);
            // Dedicated dependencies differ in spelling between primary captures and remaining Entries.
            if (receiverText.StartsWith("dependency", StringComparison.Ordinal) || receiverText == "this") receiverText = "dependency";
            var generic = invocation.Expression switch { GenericNameSyntax g => g, MemberAccessExpressionSyntax { Name: GenericNameSyntax g } => g, _ => null };
            return "invoke[" + call.Normalized + "](" + receiverText + ";" + (generic is null ? "" : Render(generic.TypeArgumentList)) + ";" +
                string.Join(",", invocation.ArgumentList.Arguments.Select(Render)) + ")";
        }
        if (node is LiteralExpressionSyntax literal)
        {
            if (literal.IsKind(SyntaxKind.StringLiteralExpression))
            {
                var s = literal.Token.ValueText;
                if (Regex.IsMatch(s, "^[a-z][a-z0-9-]*:[a-z0-9-]+$")) return "skill-id";
                if (s.Length > 100 || s.Any(c => c > 127) || s.Contains(' ')) return "text";
                return "string[" + MethodInfo.Scrub(s, method.CharacterName) + "]";
            }
            return literal.Kind() + "[" + literal.Token.ValueText + "]";
        }
        if (node is ArgumentSyntax argument)
            return "argument[" + argument.NameColon?.Name.Identifier.ValueText + ":" + argument.RefKindKeyword.Text + "](" + Render(argument.Expression) + ")";
        return node.Kind() + "(" + string.Join(" ", node.ChildNodesAndTokens().Select(c => c.IsNode ? Render(c.AsNode()!) : Token(c.AsToken()))) + ")";
    }
    string Token(SyntaxToken token)
    {
        if (!token.IsKind(SyntaxKind.IdentifierToken)) return token.Text;
        var name = token.ValueText;
        if (variables.TryGetValue(name, out var replacement)) return replacement;
        if (name.StartsWith('_') && (name.EndsWith("Operations", StringComparison.Ordinal) || name.EndsWith("Access", StringComparison.Ordinal))) return "dependency";
        return MethodInfo.Scrub(name, method.CharacterName);
    }
}
sealed record Example(string File, int StartLine, string Name, int BodyLines);
sealed record PatternInfo(string Signature, int Count, int BodyLines, int MethodLines, int CharacterVersions, string Role,
    int ExactVariants, string[] NormalizedCalls, string Control, string[] AccessedMembers, Example[] Examples, string[] MethodIds);
sealed record BehaviorPatternInfo(string Signature, int Count, int BodyLines, int MethodLines, int CharacterVersions, string Role,
    int ExactVariants, string[] Families, string[] ControlClasses, Example[] Examples, string[] MethodIds);
