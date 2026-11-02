using System.Text.Json;

namespace CardGame.Core;

public enum ProgramExpressionType { Bool, Int, Participant, ParticipantSet, Fact, FactSet }

/// <summary>
/// Immutable typed expression read by <c>{"kind":"expression","expr":[...]}</c>
/// conditions. Evaluation is pure: it reads current public seat state through
/// <see cref="IProgramExpressionWorld"/> and never writes game state.
/// </summary>
public sealed class ProgramExpression
{
    internal ProgramExpression(string op, ProgramExpressionType type, IReadOnlyList<ProgramExpression> args,
        int literal = 0, string? name = null)
    {
        Op = op; Type = type; Args = Array.AsReadOnly(args.ToArray()); Literal = literal; Name = name;
    }

    public string Op { get; }
    public ProgramExpressionType Type { get; }
    public IReadOnlyList<ProgramExpression> Args { get; }
    public int Literal { get; }
    public string? Name { get; }

    internal bool Reads(string op) => Op == op || Args.Any(arg => arg.Reads(op));
    internal IEnumerable<ProgramExpression> Nodes() => new[] { this }.Concat(Args.SelectMany(arg => arg.Nodes()));
    internal bool StructurallyEquals(ProgramExpression other) => Op == other.Op && Type == other.Type &&
        Literal == other.Literal && Name == other.Name && Args.Count == other.Args.Count &&
        Args.Zip(other.Args).All(pair => pair.First.StructurallyEquals(pair.Second));
}

/// <summary>Read-only seat facts. Null means unknown or undefined.</summary>
internal interface IProgramExpressionWorld
{
    int? Participant(string name);
    IReadOnlyList<int>? ParticipantSet(string name);
    int? Stat(string stat, int seat);
    bool? Flag(string flag, int seat);
    int? BindingStat(string stat, string name) => null;
    int? Number(string name) => null;
    int? CardMarkCount(int seat, string mark, CardZoneKind zone) => null;
    int? HandPrintedSuitCount(int seat, Suit suit) => null;
    int? PublicPileCount(int seat, string skillId) => null;
    int? WindowStat(string stat, string participant) => null;
    IReadOnlyList<CardUseHistoryFact>? CardUses(string moment, string view) => null;
    CardUseHistoryFact? CurrentUse() => null;
}

internal static class ProgramExpressions
{
    internal static int? ReadWindowStat(SkillProgramTriggerFacts? facts, string stat, string participant, bool hasTarget)
    {
        if (facts is null) return null;
        return (stat, participant) switch
        {
            ("windowHp", "owner") when !facts.OwnerHpUndefined && facts.UnselectedOwnerHp is null => facts.CurrentHp,
            ("windowHp", "eventTarget") when hasTarget && !facts.EventTargetHpUndefined && facts.UnselectedEventTargetHp is null => facts.EventTargetHp,
            ("windowHandCount", "owner") => facts.CurrentHandCount,
            ("windowHandCount", "eventTarget") when hasTarget => facts.EventTargetHandCount,
            _ => null
        };
    }

    public const int MaximumDepth = 8;
    public const string Iterator = "it";
    public const string FactIterator = "use";
    public static readonly string[] Participants = ["turnOwner", "owner", "selectedTarget", "selectedFirst", "selectedSecond", "eventSource", "eventTarget"];
    public static readonly string[] Sets = ["living", "livingOthers"];
    private static readonly string[] Stats = ["hp", "maxHp", "lostHp", "handCount", "handLimit", "equipmentCount", "equipmentSuitCount"];
    private static readonly string[] Flags = ["alive", "hasHp", "faceDown", "chained", "ruleWounded", "inIssuedPhase", "inUpcomingTurnPrelude", "isTurnOwner", "inIssuedTurn", "inIssuedDying"];
    private static readonly string[] Comparisons = ["<", "<=", ">", ">=", "==", "!="];

    public static ProgramExpression Parse(JsonElement node, string path) =>
        Parse(node, path, ProgramExpressionType.Bool, 0, iteratorInScope: false);

    public static ProgramExpression ParseInt(JsonElement node, string path) =>
        Parse(node, path, ProgramExpressionType.Int, 0, iteratorInScope: false);

    public static ProgramExpression ParseParticipantSet(JsonElement node, string path) =>
        Parse(node, path, ProgramExpressionType.ParticipantSet, 0, iteratorInScope: false);

    private static ProgramExpression Parse(JsonElement node, string path, ProgramExpressionType? expected, int depth, bool iteratorInScope, bool factInScope = false)
    {
        if (depth > MaximumDepth) Fail(path, $"expression nesting exceeds {MaximumDepth}");
        var result = node.ValueKind switch
        {
            JsonValueKind.True or JsonValueKind.False =>
                new ProgramExpression("bool", ProgramExpressionType.Bool, [], node.GetBoolean() ? 1 : 0),
            JsonValueKind.Number when node.TryGetInt32(out var value) && value is >= -99 and <= 99 =>
                new ProgramExpression("int", ProgramExpressionType.Int, [], value),
            JsonValueKind.String => ParseName(node.GetString()!, path, iteratorInScope, factInScope),
            JsonValueKind.Array => ParseCall(node, path, depth, iteratorInScope, factInScope),
            _ => throw Failure(path, "must be a boolean, an integer from -99 to 99, a name or an [operator, ...] array")
        };
        if (expected is { } expectedType && result.Type != expectedType) Fail(path, $"expected {Camel(expectedType)} but found {Camel(result.Type)}");
        return result;
    }

    private static ProgramExpression ParseName(string name, string path, bool iteratorInScope, bool factInScope)
    {
        if (name == "target") name = "selectedTarget";
        if (name == Iterator)
        {
            if (!iteratorInScope) Fail(path, "'it' is only available inside a participant-set filter, all, any or count");
            return new(name, ProgramExpressionType.Participant, [], name: name);
        }
        if (name == FactIterator && !factInScope) Fail(path, "'use' is only available inside a fact-set filter, all, any or count");
        if (name is FactIterator or "currentUse") return new(name, ProgramExpressionType.Fact, [], name: name);
        if (Participants.Contains(name)) return new(name, ProgramExpressionType.Participant, [], name: name);
        if (Sets.Contains(name)) return new(name, ProgramExpressionType.ParticipantSet, [], name: name);
        throw Failure(path, $"unknown name '{name}'");
    }

    private static ProgramExpression ParseCall(JsonElement node, string path, int depth, bool iteratorInScope, bool factInScope)
    {
        var items = node.EnumerateArray().ToArray();
        if (items.Length == 0 || items[0].ValueKind != JsonValueKind.String) Fail(path, "an array must start with an operator name");
        var op = items[0].GetString()!;
        var args = items.Skip(1).ToArray();
        ProgramExpression Arg(int index, ProgramExpressionType? type, bool iterator = false, bool factIterator = false) =>
            Parse(args[index], $"{path}[{index + 1}]", type, depth + 1, iteratorInScope || iterator, factInScope || factIterator);
        void Arity(int minimum, int maximum)
        {
            if (args.Length < minimum || args.Length > maximum)
                Fail(path, minimum == maximum ? $"'{op}' requires {minimum} argument(s)" : $"'{op}' requires {minimum} to {maximum} arguments");
        }
        ProgramExpression[] Each(ProgramExpressionType type) => args.Select((_, index) => Arg(index, type)).ToArray();

        if (op is "and" or "or") { Arity(2, 8); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Bool)); }
        if (op == "not") { Arity(1, 1); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Bool)); }
        if (Comparisons.Contains(op)) { Arity(2, 2); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Int)); }
        if (op == "same") { Arity(2, 2); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Participant)); }
        if (Flags.Contains(op)) { Arity(1, 1); return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Participant)); }
        if (Stats.Contains(op)) { Arity(1, 1); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Participant)); }
        if (op is "windowHp" or "windowHandCount")
        {
            Arity(1, 1);
            var subject = Arg(0, ProgramExpressionType.Participant);
            if (subject.Name is not ("owner" or "eventTarget"))
                Fail(path, "window stats require the frozen owner or eventTarget");
            return new(op, ProgramExpressionType.Int, [subject]);
        }
        if (op == "publicPileCount")
        {
            Arity(2, 2);
            if (args[1].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[1].GetString()) || args[1].GetString()!.Length > 128)
                Fail(path, "publicPileCount requires a skill identifier");
            try { _ = new ContentId(args[1].GetString()!); }
            catch (ArgumentException) { throw Failure(path, "publicPileCount requires a skill identifier"); }
            return new(op, ProgramExpressionType.Int, [Arg(0, ProgramExpressionType.Participant)], name: args[1].GetString());
        }
        if (op == "cardMarkCount")
        {
            Arity(3, 3);
            if (args[1].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[1].GetString()) || args[1].GetString()!.Length > 128 ||
                args[2].ValueKind != JsonValueKind.String || args[2].GetString() is not ("hand" or "equipment"))
                Fail(path, "cardMarkCount requires a mark identifier and hand or equipment");
            return new(op, ProgramExpressionType.Int, [Arg(0, ProgramExpressionType.Participant)],
                literal: (int)(args[2].GetString() == "hand" ? CardZoneKind.Hand : CardZoneKind.Equipment), name: args[1].GetString());
        }
        if (op == "handPrintedSuitCount")
        {
            Arity(2, 2);
            if (args[1].ValueKind != JsonValueKind.String || args[1].GetString() is not ("none" or "spade" or "club" or "heart" or "diamond"))
                Fail(path, "handPrintedSuitCount requires a printed suit");
            return new(op, ProgramExpressionType.Int, [Arg(0, ProgramExpressionType.Participant)],
                literal: (int)Enum.Parse<Suit>(args[1].GetString()!, ignoreCase: true));
        }
        if (op is "+" or "min" or "max") { Arity(2, 8); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Int)); }
        if (op == "-") { Arity(2, 2); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Int)); }
        if (op == "floorDiv") { Arity(2, 2); return new(op, ProgramExpressionType.Int, Each(ProgramExpressionType.Int)); }
        if (op == "when")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Int, [Arg(0, ProgramExpressionType.Bool), Arg(1, ProgramExpressionType.Int)]);
        }
        if (op is "number" or "choiceNumber" or "boundCount" or "boundSuitCount" or "boundDistinctSuitCount" or "boundRank" or "boundIsEquipment")
        {
            Arity(1, 1);
            if (args[0].ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(args[0].GetString()) || args[0].GetString()!.Length > 128)
                Fail(path, "a binding reference requires a nonempty identifier of at most 128 characters");
            return new(op, op == "boundIsEquipment" ? ProgramExpressionType.Bool : ProgramExpressionType.Int, [], name: args[0].GetString());
        }
        if (op == "cardUses")
        {
            Arity(2, 2);
            if (args[0].ValueKind != JsonValueKind.String || args[0].GetString() is not ("window" or "now") ||
                args[1].ValueKind != JsonValueKind.String || args[1].GetString() is not ("latest" or "observations" or "confirmations"))
                Fail(path, "cardUses requires window or now, and latest, observations or confirmations");
            return new(op, ProgramExpressionType.FactSet, [], literal: args[0].GetString() == "now" ? 1 : 0, name: args[1].GetString());
        }
        if (op is "useActor" or "useDeclarer" or "useTargets" or "useConfirmedTargets" or "usePrimaryTargets" or "useEstablished" or "useIsResponse" or "useDeclared" or "useIsSelfNativeResponse")
        {
            Arity(1, 1);
            var type = op is "useTargets" or "useConfirmedTargets" or "usePrimaryTargets" ? ProgramExpressionType.ParticipantSet :
                op is "useEstablished" or "useIsResponse" or "useDeclared" or "useIsSelfNativeResponse" ? ProgramExpressionType.Bool : ProgramExpressionType.Participant;
            return new(op, type, [Arg(0, ProgramExpressionType.Fact)]);
        }
        if (op == "first")
        {
            Arity(1, 1);
            return new(op, ProgramExpressionType.Fact, [Arg(0, ProgramExpressionType.FactSet)]);
        }
        if (op == "sameUse")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Bool, Each(ProgramExpressionType.Fact));
        }
        if (op == "useKindIs")
        {
            Arity(2, 2);
            if (args[1].ValueKind != JsonValueKind.Array) Fail(path, "useKindIs requires an array of card kinds");
            var kinds = args[1].EnumerateArray().Select((kind, index) =>
                ParseFactEnum<CardKind>(kind, $"{path}[2][{index}]")).ToArray();
            if (kinds.Distinct().Count() != kinds.Length) Fail(path, "useKindIs contains duplicate card kinds");
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Fact),
                .. kinds.Select(kind => new ProgramExpression("int", ProgramExpressionType.Int, [], literal: (int)kind))]);
        }
        if (op == "usePhaseIs")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Fact)],
                literal: (int)ParseFactEnum<TurnPhase>(args[1], path + "[2]"));
        }
        if (op == "atOrBefore")
        {
            Arity(3, 3);
            if (args[2].ValueKind != JsonValueKind.String || args[2].GetString() != "accepted")
                Fail(path, "atOrBefore requires the accepted order");
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.Fact), Arg(1, ProgramExpressionType.Fact)], name: "accepted");
        }
        if (op == "targets")
        {
            Arity(1, 1);
            return new(op, ProgramExpressionType.ParticipantSet, [Arg(0, ProgramExpressionType.FactSet)]);
        }
        if (op == "contains")
        {
            Arity(2, 2);
            return new(op, ProgramExpressionType.Bool, [Arg(0, ProgramExpressionType.ParticipantSet), Arg(1, ProgramExpressionType.Participant)]);
        }
        if (op is "all" or "any" or "count" or "filter")
        {
            Arity(2, 2);
            var source = Arg(0, null);
            if (source.Type is not (ProgramExpressionType.ParticipantSet or ProgramExpressionType.FactSet))
                Fail(path, "all, any, count and filter require a participant set or a fact set");
            return new(op, op == "filter" ? source.Type : op == "count" ? ProgramExpressionType.Int : ProgramExpressionType.Bool,
                [source, Arg(1, ProgramExpressionType.Bool, iterator: source.Type == ProgramExpressionType.ParticipantSet,
                    factIterator: source.Type == ProgramExpressionType.FactSet)]);
        }
        throw Failure(path, $"unknown operator '{op}'");
    }

    /// <summary>Three-valued result: null when a read fact is unknown or undefined.</summary>
    public static bool? EvaluateBool(ProgramExpression e, IProgramExpressionWorld world, int? it = null, CardUseHistoryFact? use = null)
    {
        if (e.Op == "boundIsEquipment") return world.BindingStat(e.Op, e.Name!) is { } value ? value != 0 : null;
        switch (e.Op)
        {
            case "bool": return e.Literal != 0;
            case "not": return !EvaluateBool(e.Args[0], world, it, use);
            case "and":
            {
                bool? result = true;
                foreach (var arg in e.Args)
                {
                    var value = EvaluateBool(arg, world, it, use);
                    if (value == false) return false;
                    if (value is null) result = null;
                }
                return result;
            }
            case "or":
            {
                bool? result = false;
                foreach (var arg in e.Args)
                {
                    var value = EvaluateBool(arg, world, it, use);
                    if (value == true) return true;
                    if (value is null) result = null;
                }
                return result;
            }
            case "same":
                return EvaluateParticipant(e.Args[0], world, it, use) is { } left && EvaluateParticipant(e.Args[1], world, it, use) is { } right
                    ? left == right : null;
            case "contains":
                return EvaluateParticipantSet(e.Args[0], world, it, use) is { } members &&
                    EvaluateParticipant(e.Args[1], world, it, use) is { } member ? members.Contains(member) : null;
            case "sameUse":
                if (EvaluateFact(e.Args[0], world, it, use) is not { } first ||
                    EvaluateFact(e.Args[1], world, it, use) is not { } second) return null;
                if (first.NativeUseId is { } firstNative && second.NativeUseId is { } secondNative)
                    return firstNative == secondNative;
                return first.NativeUseId is null && second.NativeUseId is null &&
                    first.ActionId is { } firstAction && second.ActionId is { } secondAction ? firstAction == secondAction : null;
            case "useKindIs":
                return EvaluateFact(e.Args[0], world, it, use)?.DeclaredKind is { } kind
                    ? ProgramCardKindMatch.Contains(e.Args.Skip(1).Select(arg => (CardKind)arg.Literal).ToArray(), kind) : null;
            case "usePhaseIs":
                return EvaluateFact(e.Args[0], world, it, use)?.Phase is { } phase ? (int)phase == e.Literal : null;
            case "useEstablished" or "useIsResponse" or "useDeclared" or "useIsSelfNativeResponse":
                return EvaluateFact(e.Args[0], world, it, use) is { } fact
                    ? e.Op switch { "useEstablished" => fact.Established, "useDeclared" => fact.NativeUseId.HasValue,
                        "useIsSelfNativeResponse" => fact.IsSelfNativeResponseUse, _ => fact.Type == CardActionType.Response } : null;
            case "atOrBefore":
                return EvaluateFact(e.Args[0], world, it, use) is { } before && EvaluateFact(e.Args[1], world, it, use) is { } after
                    ? before.ActionId is { } beforeAction && after.ActionId is { } afterAction
                        ? beforeAction <= afterAction : before.DeclarationCursor <= after.DeclarationCursor : null;
            case "all" or "any":
            {
                if (EvaluatePredicates(e, world, it, use) is not { } values) return null;
                bool? result = e.Op == "all";
                foreach (var value in values)
                {
                    if (value == (e.Op == "any")) return value;
                    if (value is null) result = null;
                }
                return result;
            }
        }
        if (Comparisons.Contains(e.Op))
        {
            if (EvaluateInt(e.Args[0], world, it, use) is not { } left || EvaluateInt(e.Args[1], world, it, use) is not { } right) return null;
            return e.Op switch
            {
                "<" => left < right, "<=" => left <= right, ">" => left > right,
                ">=" => left >= right, "==" => left == right, _ => left != right
            };
        }
        if (Flags.Contains(e.Op))
            return EvaluateParticipant(e.Args[0], world, it, use) is { } seat ? world.Flag(e.Op, seat) : null;
        throw new InvalidOperationException($"Expression operator '{e.Op}' is not boolean.");
    }

    public static int? EvaluateInt(ProgramExpression e, IProgramExpressionWorld world, int? it = null, CardUseHistoryFact? use = null)
    {
        switch (e.Op)
        {
            case "int": return e.Literal;
            case "when": return EvaluateBool(e.Args[0], world, it, use) == true ? EvaluateInt(e.Args[1], world, it, use) : null;
            case "number": return world.Number(e.Name!);
            case "windowHp" or "windowHandCount": return world.WindowStat(e.Op, e.Args[0].Name!);
            case "cardMarkCount": return EvaluateParticipant(e.Args[0], world, it, use) is { } markedSeat
                ? world.CardMarkCount(markedSeat, e.Name!, (CardZoneKind)e.Literal) : null;
            case "handPrintedSuitCount": return EvaluateParticipant(e.Args[0], world, it, use) is { } handSeat
                ? world.HandPrintedSuitCount(handSeat, (Suit)e.Literal) : null;
            case "publicPileCount": return EvaluateParticipant(e.Args[0], world, it, use) is { } pileSeat
                ? world.PublicPileCount(pileSeat, e.Name!) : null;
            case "choiceNumber" or "boundCount" or "boundSuitCount" or "boundDistinctSuitCount" or "boundRank": return world.BindingStat(e.Op, e.Name!);
            case "-":
                return EvaluateInt(e.Args[0], world, it, use) is { } left && EvaluateInt(e.Args[1], world, it, use) is { } right ? left - right : null;
            case "floorDiv":
            {
                if (EvaluateInt(e.Args[0], world, it, use) is not { } dividend ||
                    EvaluateInt(e.Args[1], world, it, use) is not { } divisor || divisor == 0) return null;
                var quotient = dividend / (long)divisor;
                if (dividend % (long)divisor != 0 && (dividend < 0) != (divisor < 0)) quotient--;
                return quotient is >= int.MinValue and <= int.MaxValue ? (int)quotient : null;
            }
            case "+" or "min" or "max":
            {
                var values = e.Args.Select(arg => EvaluateInt(arg, world, it, use)).ToArray();
                if (values.Any(value => value is null)) return null;
                var numbers = values.Select(value => value!.Value);
                return e.Op == "+" ? numbers.Sum() : e.Op == "min" ? numbers.Min() : numbers.Max();
            }
            case "count":
            {
                if (EvaluatePredicates(e, world, it, use) is not { } values) return null;
                var count = 0;
                foreach (var value in values)
                    switch (value)
                    {
                        case null: return null;
                        case true: count++; break;
                    }
                return count;
            }
        }
        if (Stats.Contains(e.Op))
            return EvaluateParticipant(e.Args[0], world, it, use) is { } seat ? world.Stat(e.Op, seat) : null;
        throw new InvalidOperationException($"Expression operator '{e.Op}' is not an integer.");
    }

    public static IReadOnlyList<int>? EvaluateParticipantSet(ProgramExpression e, IProgramExpressionWorld world,
        int? it = null, CardUseHistoryFact? use = null)
    {
        switch (e.Op)
        {
            case "useTargets": return EvaluateFact(e.Args[0], world, it, use)?.Targets;
            case "useConfirmedTargets": return EvaluateFact(e.Args[0], world, it, use)?.ConfirmedTargets;
            case "usePrimaryTargets": return EvaluateFact(e.Args[0], world, it, use)?.PrimaryTargets;
            case "targets": return EvaluateFactSet(e.Args[0], world, it, use) is { } facts
                ? Array.AsReadOnly(facts.SelectMany(fact => fact.Targets).Distinct().ToArray()) : null;
            case "filter":
            {
                if (EvaluateParticipantSet(e.Args[0], world, it, use) is not { } seats) return null;
                var result = new List<int>();
                foreach (var seat in seats)
                {
                    var accepted = EvaluateBool(e.Args[1], world, seat, use);
                    if (accepted is null) return null;
                    if (accepted == true) result.Add(seat);
                }
                return Array.AsReadOnly(result.ToArray());
            }
            default: return world.ParticipantSet(e.Name!);
        }
    }

    private static IReadOnlyList<CardUseHistoryFact>? EvaluateFactSet(ProgramExpression e, IProgramExpressionWorld world,
        int? it, CardUseHistoryFact? use)
    {
        if (e.Op == "cardUses") return world.CardUses(e.Literal == 0 ? "window" : "now", e.Name!);
        if (e.Op != "filter") throw new InvalidOperationException($"Expression operator '{e.Op}' is not a fact set.");
        if (EvaluateFactSet(e.Args[0], world, it, use) is not { } facts) return null;
        var result = new List<CardUseHistoryFact>();
        foreach (var fact in facts)
        {
            var accepted = EvaluateBool(e.Args[1], world, it, fact);
            if (accepted is null) return null;
            if (accepted == true) result.Add(fact);
        }
        return Array.AsReadOnly(result.ToArray());
    }

    private static IEnumerable<bool?>? EvaluatePredicates(ProgramExpression e, IProgramExpressionWorld world,
        int? it, CardUseHistoryFact? use) => e.Args[0].Type == ProgramExpressionType.FactSet
        ? EvaluateFactSet(e.Args[0], world, it, use)?.Select(fact => EvaluateBool(e.Args[1], world, it, fact))
        : EvaluateParticipantSet(e.Args[0], world, it, use)?.Select(seat => EvaluateBool(e.Args[1], world, seat, use));

    private static CardUseHistoryFact? EvaluateFact(ProgramExpression e, IProgramExpressionWorld world,
        int? it, CardUseHistoryFact? use) => e.Op == "first"
        ? EvaluateFactSet(e.Args[0], world, it, use)?.FirstOrDefault()
        : e.Name == FactIterator ? use : world.CurrentUse();

    private static int? EvaluateParticipant(ProgramExpression e, IProgramExpressionWorld world, int? it, CardUseHistoryFact? use) =>
        e.Op is "useActor" or "useDeclarer" ? EvaluateFact(e.Args[0], world, it, use) is { } fact
            ? e.Op == "useActor" ? fact.ActorSeat : fact.DeclarerSeat : null :
        e.Name == Iterator ? it : world.Participant(e.Name!);

    private static T ParseFactEnum<T>(JsonElement node, string path) where T : struct, Enum
    {
        if (node.ValueKind == JsonValueKind.String && Enum.TryParse<T>(node.GetString(), true, out var value) &&
            Enum.GetName(value) is { } name && node.GetString() == char.ToLowerInvariant(name[0]) + name[1..])
            return value;
        throw Failure(path, $"must be a supported camelCase {typeof(T).Name}");
    }

    private static string Camel(ProgramExpressionType type) => type switch
    {
        ProgramExpressionType.Bool => "a boolean",
        ProgramExpressionType.Int => "an integer",
        ProgramExpressionType.Participant => "a participant",
        ProgramExpressionType.ParticipantSet => "a participant set",
        ProgramExpressionType.Fact => "a card-use fact",
        _ => "a card-use fact set"
    };

    private static void Fail(string path, string message) => throw Failure(path, message);
    private static InvalidOperationException Failure(string path, string message) =>
        new($"Invalid skill program at {path}: {message}.");
}

/// <summary>
/// AI estimate view: only the owner and the selected target are known; every
/// other participant or set is unknown, so expressions that need them are false.
/// </summary>
internal sealed class SkillContextExpressionWorld(PlayerSkillContext owner, PlayerSkillContext? selectedTarget,
    int? ownerEquipmentCount = null, Func<string, string, int?>? bindingStat = null,
    Func<string, int?>? number = null, SkillProgramTriggerFacts? windowFacts = null) : IProgramExpressionWorld
{
    public int? WindowStat(string stat, string participant) => ProgramExpressions.ReadWindowStat(windowFacts, stat, participant, selectedTarget is not null);
    public int? BindingStat(string stat, string name) => bindingStat?.Invoke(stat, name);
    public int? Number(string name) => number?.Invoke(name);
    private PlayerSkillContext? Context(int seat) =>
        seat == owner.Seat ? owner : selectedTarget?.Seat == seat ? selectedTarget : null;

    public int? Participant(string name) => name switch
    {
        "owner" => owner.Seat,
        "selectedTarget" => selectedTarget?.Seat,
        _ => null
    };

    public IReadOnlyList<int>? ParticipantSet(string name) => null;

    public int? Stat(string stat, int seat)
    {
        if (Context(seat) is not { } context) return null;
        return stat switch
        {
            "handCount" => context.HandCount,
            "handLimit" => context.HandLimit,
            "equipmentCount" => seat == owner.Seat ? ownerEquipmentCount : null,
            "maxHp" => context.MaxHp,
            "hp" => context.HpUndefined ? null : context.Hp,
            "lostHp" => context.HpUndefined ? null : context.MaxHp - context.Hp,
            _ => null
        };
    }

    public bool? Flag(string flag, int seat)
    {
        if (Context(seat) is not { } context) return null;
        return flag switch
        {
            "alive" => true,
            "hasHp" => !context.HpUndefined,
            "faceDown" => context.IsFaceDown,
            "chained" => context.IsChained,
            _ => null
        };
    }
}
